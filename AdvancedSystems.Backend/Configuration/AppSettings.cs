using System.ComponentModel.DataAnnotations;

namespace AdvancedSystems.Backend.Configuration;

public class AppSettings
{
    [Required]
    public required double DefaultApiVersion { get; init; }
}