using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models;

public class ProjectImage
{
    public int ImageId { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [Required, StringLength(500)]
    [RegularExpression(
        @"^https?://.+\.(jpg|jpeg|png|gif|webp)(\?.*)?$",
        ErrorMessage = "Enter a valid image URL.")]
    public string ImageUrl { get; set; } = "";

    public Project? Project { get; set; }
}