using System.ComponentModel.DataAnnotations;

namespace Assignment.Models.Dto
{
    public class StudentCreateDto
    {

        [RegularExpression("^[a-zA-Z]{1,50}$", ErrorMessage = "Only Alphabets are allowed")]
        [StringLength(50), Required(ErrorMessage = "CourseName is Required")]
        public string? CourseName { get; set; }

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
        public int GenderId { get; set; }

        [Required(ErrorMessage = "Please Select Grade")]
        public int GradeId { get; set; }
    }
}
