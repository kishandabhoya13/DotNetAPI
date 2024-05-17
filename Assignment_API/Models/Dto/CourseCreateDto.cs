using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assignment_API.Models.Dto
{
    public class CourseCreateDto
    {

        [Required(ErrorMessage = "Enter Course Name")]
        [StringLength(50)]
        [Column(TypeName = "character varying")]
        public string Name { get; set; } = null!;
    }
}
