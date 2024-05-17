﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.Models.Dto
{
    public class CourseModelDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter Course Name")]
        [StringLength(50)]
        [Column(TypeName = "character varying")]
        public string Name { get; set; } = null!;
    }
}
