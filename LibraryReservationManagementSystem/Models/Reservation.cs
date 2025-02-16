using System.ComponentModel.DataAnnotations;

namespace LibraryReservationManagementSystem.Models;

public class Reservation
{
    [Key]
    public int Id { get; set; }

    public required Customer Customer { get; set; }

    public required Book Book { get; set; }

    public required DateTime ReservationDate { get; set; }

    public required DateTime ExpirationDate { get; set; }
}