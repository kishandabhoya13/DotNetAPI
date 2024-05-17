using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assignment_API.Models.Dto
{
    public class CourseUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Course Name")]
        [StringLength(50)]
        [Column(TypeName = "character varying")]
        public string Name { get; set; } = null!;
    }
}
