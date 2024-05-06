namespace AdvancedSystems.Backend.Models;

public record Book
{
    public required int Id { get; set; }

    public required string Author { get; set; }

    public required string Title { get; set; }
}