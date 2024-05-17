using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment_API.DataModals;

[Table("Course")]
public partial class Course
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [InverseProperty("CourseNavigation")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
