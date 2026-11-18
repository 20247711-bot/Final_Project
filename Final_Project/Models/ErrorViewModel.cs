using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models;

public class ProjectFormViewModel
{
    public int ProjectId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Required, StringLength(150)]
    public string ProjectTitle { get; set; } = "";

    [Required, StringLength(2000)]
    public string Description { get; set; } = "";

    [StringLength(500)]
    [RegularExpression(@"^(https?://)?(www\.)?(youtube\.com/watch\?v=|youtu\.be/|youtube\.com/shorts/)[A-Za-z0-9_-]{11}(&.*)?$",
        ErrorMessage = "Enter a valid YouTube video URL.")]
    public string? VideoUrl { get; set; }

    public DateOnly? CompletedOn { get; set; }

    [Display(Name = "Image URLs (one per line)")]
    public string ImageUrls { get; set; } = "";
}
