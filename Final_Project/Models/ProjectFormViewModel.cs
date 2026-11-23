using System.ComponentModel.DataAnnotations;
namespace Final_Project.Models;

public class ProjectFormViewModel
{
    public int ProjectId { get; set; }
    [Required] public int CourseId { get; set; }
    [Required, StringLength(150)] public string Title { get; set; } = "";
    [StringLength(1000)] public string? Description { get; set; }
    [StringLength(500)] public string? VideoUrl { get; set; }
    [Display(Name = "Image URLs (one per line)")] public string ImageUrls { get; set; } = "";
}
