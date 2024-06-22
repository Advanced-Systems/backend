namespace AdvancedSystems.Backend.Models.API;

public record Book
{
    public required int Id { get; set; }

    public required string Author { get; set; }

    public required string Title { get; set; }
}