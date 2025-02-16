using System.ComponentModel.DataAnnotations;

namespace LibraryReservationManagementSystem.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Author { get; set; }

    public required string ISBN { get; set; }

    public required StatusEnum Status { get; set; }


    public enum StatusEnum
    {
        Available,
        Unavailable
    }


}