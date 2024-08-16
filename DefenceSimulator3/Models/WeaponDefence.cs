using System.ComponentModel.DataAnnotations;

namespace DefenceSimulator3.Models
{
    public class WeaponDefence
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="סוג נשק")]
        public string Name { get; set; }
        public int Speed { get; set; }
        [Display(Name = "כמות נשק")]
        public int Amount { get; set; }
    }
}
