using System.ComponentModel.DataAnnotations;

namespace DefenceSimulator3.Models
{
    public class Origin
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        
    }
}
