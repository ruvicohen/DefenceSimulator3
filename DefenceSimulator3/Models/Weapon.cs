using System.ComponentModel.DataAnnotations;

namespace DefenceSimulator3.Models
{
    public class Weapon
    {
        [Key]
        public int WeaponId { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }
        
        public int WeaponDefenceId { get; set; }
        public WeaponDefence? WeaponDefence { get; set; }


    }
}
