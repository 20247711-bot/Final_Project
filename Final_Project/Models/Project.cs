using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models;

public class Project
{
    public int ProjectId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = "";

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(500)]
    [RegularExpression(
        @"^(https?://)?(www\.)?(youtube\.com/watch\?v=|youtu\.be/|youtube\.com/shorts/)[A-Za-z0-9_-]{11}.*$",
        ErrorMessage = "Enter a valid YouTube video URL.")]
    public string? VideoUrl { get; set; }

    public Course? Course { get; set; }

    public List<ProjectImage> Images { get; set; } = new();

    [Display(Name = "Image URLs")]
    public string ImageUrlsInput { get; set; } = "";
}