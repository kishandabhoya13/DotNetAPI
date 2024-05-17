using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Assignment_API.Models.Dto
{
    public class StudentDataModelDto
    {
        public int StudentId { get; set; }

        [RegularExpression("^[a-zA-Z]{1,50}$", ErrorMessage = "Only Alphabets are allowed")]
        [StringLength(50), Required(ErrorMessage = "CourseName is Required")]
        public string? Course { get; set; }

        [Required(ErrorMessage = "Please Enter Course")]
        public int CourseId { get; set; } = 0;

        [StringLength(50), Required(ErrorMessage = "FirstName is Required")]
        [RegularExpression("^[a-zA-Z]{1,50}$", ErrorMessage = "Only Alphabets are allowed")]
        public string? FirstName { get; set; }

        [RegularExpression("^[a-zA-Z]{1,50}$", ErrorMessage = "Only Alphabets are allowed")]
        [StringLength(50), Required(ErrorMessage = "LastName is Required")]
        public string? LastName { get; set; }

        [RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage = "Enter Correct Email")]
        [StringLength(50), Required(ErrorMessage = "Email is Required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "DateOfBith is Required")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please Select Gender")]
        public int Gender { get; set; }

        public string? GenderName { get; set; } = null;

        [Required(ErrorMessage = "Please Select Grade")]
        public int Grade { get; set; }

        public string? GradeName { get; set; } = null;

        public int Age => DateTime.Now.Year - DateOfBirth?.Year ?? 0;

    }
}
