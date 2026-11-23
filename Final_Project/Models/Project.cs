using System.ComponentModel.DataAnnotations;
namespace Final_Project.Models;

public class Project
{
    public int ProjectId { get; set; }
    [Required] public int CourseId { get; set; }
    [Required, StringLength(150)] public string Title { get; set; } = "";
    [StringLength(1000)] public string? Description { get; set; }
    [StringLength(500)] public string? VideoUrl { get; set; }
    public Course? Course { get; set; }
    public List<ProjectImage> Images { get; set; } = new();
}
