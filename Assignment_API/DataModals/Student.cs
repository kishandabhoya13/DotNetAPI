using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment_API.DataModals;

[Table("Student")]
public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    public int CourseId { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    public int Gender { get; set; }

    [StringLength(50)]
    public string? Course { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? DateOfBirth { get; set; }

    public int? Grade { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Students")]
    public virtual Course CourseNavigation { get; set; } = null!;
}
