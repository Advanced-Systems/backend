using System.ComponentModel.DataAnnotations;

namespace AdvancedSystems.Backend.Configuration;

public record AppSettings
{
    [Required]
    public double DefaultApiVersion { get; init; }
}