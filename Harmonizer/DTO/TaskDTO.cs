using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Harmonizer.DTO
{
    public class TaskDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title length cannot exceed 100 characters.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description length cannot exceed 500 characters.")]
        public string? Description { get; set; }

       
        public DateTime? DueDate { get; set; }

        // UserId of the person assigned to the task (must be an existing user in the system)
        [Required]
        public int UserId { get; set; }

        // StatusId for the task (must be an existing status in the system)
        [Required]
        public int StatusId { get; set; }
    }
}
