using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Harmonizer.Model
{
    public class User 
    {
        [Key]
        public int UserId {  get; set; }
        public string? Google_id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Picture { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? Last_login { get; set; }

        [Required]
        public string Role { get; set; }

        // Navigation property
        public ICollection<TaskEnitity> Tasks { get; set; }
    }
}
