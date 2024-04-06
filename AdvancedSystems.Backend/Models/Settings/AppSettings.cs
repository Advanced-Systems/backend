using System.ComponentModel.DataAnnotations;

namespace AdvancedSystems.Backend.Configuration.Settings
{
    public record AppSettings
    {
        [Required]
        public double DefaultApiVersion { get; set; }
    }
}
