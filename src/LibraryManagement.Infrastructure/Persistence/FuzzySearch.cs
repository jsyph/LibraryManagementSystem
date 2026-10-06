using System.Globalization;
using System.Text;

namespace LibraryManagement.Infrastructure.Persistence;

/// <summary>
/// Ranks already-loaded entities against a user's text query using tokenization
/// and Levenshtein similarity. This is an in-memory search helper; database
/// repositories should load a suitable candidate set before calling it.
/// </summary>
internal static class FuzzySearch
{
    /// <summary>
    /// Scores each entity by comparing every query token with the closest token
    /// in the entity's searchable fields, removes results below the minimum
    /// score, and returns the remaining entities from most to least relevant.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being searched.</typeparam>
    /// <param name="source">The candidate entities to rank.</param>
    /// <param name="searchText">The user's search text. It may contain multiple words or be null.</param>
    /// <param name="fields">
    /// A selector returning the text fields that should participate in the search.
    /// Fields such as ISBN, email, and phone numbers should be excluded when they
    /// require exact matching.
    /// </param>
    /// <param name="minimumScore">
    /// The lowest average similarity score an entity may have and still be returned.
    /// A score of 1 is an exact match; the default is 0.55.
    /// </param>
    /// <returns>Entities ranked by descending fuzzy-match score.</returns>
    public static IEnumerable<TEntity> Rank<TEntity>(
        IEnumerable<TEntity> source,
        string? searchText,
        Func<TEntity, IEnumerable<string?>> fields,
        double minimumScore = 0.55
    )
    {
        var queryTokens = Tokens(searchText).ToArray();
        /// <summary>
        /// Gives each query token its best match among the entity's searchable tokens,
        /// then averages those best-match scores. This makes every word in the query
        /// contribute to the entity's final relevance score.
        /// </summary>
        if (queryTokens.Length == 0)
        {
            return source;
        }

        return source
            .Select(entity =>
            {
                var searchableTokens = fields(entity).SelectMany(Tokens).ToArray();
                var score = Score(queryTokens, searchableTokens);
                return (entity, score);
            })
            .Where(result => result.score >= minimumScore)
            .OrderByDescending(result => result.score)
            .Select(result => result.entity)
            .ToList();
    }

    private static double Score(
        IReadOnlyCollection<string> queryTokens,
        IReadOnlyCollection<string> searchableTokens
    )
    {
        if (searchableTokens.Count == 0)
        {
            return 0;
        }

        return queryTokens.Average(queryToken =>
            searchableTokens.Max(token => Similarity(queryToken, token))
        );
    }

    /// <summary>
    /// Converts two normalized tokens into a similarity score between 0 and 1.
    /// Exact matches score 1, containment scores 0.95, and other matches are
    /// calculated from their Levenshtein edit distance.
    /// </summary>
    private static double Similarity(string left, string right)
    {
        if (left == right)
        {
            return 1;
        }

        if (
            left.Contains(right, StringComparison.Ordinal)
            || right.Contains(left, StringComparison.Ordinal)
        )
        {
            return 0.95;
        }

        var distance = LevenshteinDistance(left, right);
        return 1d - (double)distance / Math.Max(left.Length, right.Length);
    }

    /// <summary>
    /// Normalizes text by removing accents and punctuation, lowercasing letters,
    /// and splitting the value into searchable words.
    /// </summary>
    private static IEnumerable<string> Tokens(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var character in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(
                    char.IsLetterOrDigit(character) ? char.ToLowerInvariant(character) : ' '
                );
            }
        }

        return builder
            .ToString()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    /// <summary>
    /// Calculates the minimum number of single-character insertions, deletions,
    /// or substitutions needed to transform one token into another.
    /// </summary>
    private static int LevenshteinDistance(string left, string right)
    {
        var previous = Enumerable.Range(0, right.Length + 1).ToArray();
        var current = new int[right.Length + 1];

        for (var leftIndex = 1; leftIndex <= left.Length; leftIndex++)
        {
            current[0] = leftIndex;

            for (var rightIndex = 1; rightIndex <= right.Length; rightIndex++)
            {
                var substitutionCost = left[leftIndex - 1] == right[rightIndex - 1] ? 0 : 1;
                current[rightIndex] = Math.Min(
                    Math.Min(current[rightIndex - 1] + 1, previous[rightIndex] + 1),
                    previous[rightIndex - 1] + substitutionCost
                );
            }

            (previous, current) = (current, previous);
        }

        return previous[right.Length];
    }
}
