using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations.Schema;

namespace DefenceSimulator3.Models
{
    public class Threat
    {
        [Key]
        public int ThreatId { get; set; }
        [Required]
        [Display(Name ="מקור האיום")]
        public int OriginId { get; set; }

        public Origin? Origin { get; set; }
        // New property: Time when the threat was launched
        public DateTime LaunchTime { get; set; }
        [Display(Name = "כמות")]
        public int Amount { get; set; }

        public int? Success { get; set; } = 0;
        public int? Fail { get; set; } = 0;
        [Display(Name = "סטטוס")]
        public Enums.ThreatStatus Status { get; set; }
        // Foreign Key for Weapon
        [Display(Name = "סוג האיום")]
        public int WeaponId { get; set; }
        // Navigation property for Weapon
        public Weapon? Weapon { get; set; }

        [NotMapped]
        [DisplayName("Time To Impact")]
        public int? TimeToImpact
        {
            get
            {
                if (Weapon == null || Origin == null)
                {
                    return null;
                }



                // Convert speed from km/h to km/s
                double speedKmh = (double)Weapon.Speed;
                double speedKms = speedKmh / 3600;

                // Distance in kilometers
                double distanceKm = (double)Origin.Distance;

                // Time to impact in seconds
                double timeToImpactSeconds = distanceKm / speedKms;
                return (int)Math.Round(timeToImpactSeconds);
            }
        }
        public int? TimeToImpact1 { get; set; } = -1;

    }
}
