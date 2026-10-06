using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(author => author.Id);
        builder.Property(author => author.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(author => author.LastName).HasMaxLength(100).IsRequired();
    }
}

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(book => book.Id);
        builder.Property(book => book.Title).HasMaxLength(200).IsRequired();
        builder.Property(book => book.ISBN).HasMaxLength(20).IsRequired();
        builder.HasIndex(book => book.ISBN).IsUnique();

        builder
            .HasOne(book => book.Author)
            .WithMany(author => author.Books)
            .HasForeignKey(book => book.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(book => book.Category)
            .WithMany(category => category.Books)
            .HasForeignKey(book => book.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class BookCopyConfiguration : IEntityTypeConfiguration<BookCopy>
{
    public void Configure(EntityTypeBuilder<BookCopy> builder)
    {
        builder.HasKey(copy => copy.Id);
        builder
            .HasOne(copy => copy.Book)
            .WithMany(book => book.Copies)
            .HasForeignKey(copy => copy.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
{
    public void Configure(EntityTypeBuilder<BorrowRecord> builder)
    {
        builder.HasKey(record => record.Id);
        builder
            .HasOne(record => record.BookCopy)
            .WithMany(copy => copy.BorrowRecords)
            .HasForeignKey(record => record.BookCopyId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(record => record.User)
            .WithMany()
            .HasForeignKey(record => record.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);
        builder.Property(category => category.Name).HasMaxLength(100).IsRequired();
        builder.HasIndex(category => category.Name).IsUnique();
    }
}

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(payment => payment.Id);
        builder.Property(payment => payment.Amount).HasPrecision(18, 2);
        builder
            .HasOne(payment => payment.BorrowRecord)
            .WithMany(record => record.Payments)
            .HasForeignKey(payment => payment.BorrowRecordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(user => user.LastName).HasMaxLength(100).IsRequired();
        builder.Property(user => user.Email).HasMaxLength(255).IsRequired();
        builder.Property(user => user.PhoneNumber).HasMaxLength(30).IsRequired();
        builder.Property(user => user.PasswordHash).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasIndex(user => user.PhoneNumber).IsUnique();
        builder
            .HasDiscriminator<string>("UserType")
            .HasValue<Member>("Member")
            .HasValue<Librarian>("Librarian")
            .HasValue<Admin>("Admin");
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(role => role.Id);
        builder.Property(role => role.RoleName).HasMaxLength(100).IsRequired();
        builder.Ignore(role => role.Users);
    }
}
