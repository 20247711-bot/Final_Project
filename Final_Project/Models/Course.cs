using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models;

public class Course
{
    public int CourseId { get; set; }

    [Required, StringLength(20)]
    [RegularExpression(@"^[A-Z0-9-]{2,20}$",
        ErrorMessage = "Use 2-20 uppercase letters, numbers, or hyphens.")]
    public string CourseCode { get; set; } = "";

    [Required, StringLength(120)]
    public string CourseName { get; set; } = "";

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(20)]
    public string? CourseYear { get; set; }

    public List<Project> Projects { get; set; } = new();
}