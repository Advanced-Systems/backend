using System.ComponentModel.DataAnnotations;

namespace AdvancedSystems.Backend.Options;

public class AppSettings
{
    [Required]
    public required double DefaultApiVersion { get; init; }
}