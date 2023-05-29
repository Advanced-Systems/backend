using System.ComponentModel.DataAnnotations;

namespace AdvancedSystems.Backend.Models;

public class Book
{
    [Required]
    public int Id { set; get; }

    [Required]
    public string? Author { get; set; }

    [Required]
    public string? Title { get; set; }
}
