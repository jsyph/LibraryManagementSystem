namespace LibraryManagement.Domain.Enums;

public enum BookStatus
{
    Available = 1,   // In library and ready to borrow
    Borrowed = 2,    // Currently checked out by member
    Lost = 3,        // Missing or lost
    Maintenance = 4  // Damaged/undergoing repair
}