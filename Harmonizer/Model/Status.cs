using System.ComponentModel.DataAnnotations;

namespace Harmonizer.Model
{
    public class Status
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; } 

         
        public ICollection<TaskEnitity> Tasks { get; set; }
    }
}
