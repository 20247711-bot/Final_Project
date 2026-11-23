using System.ComponentModel.DataAnnotations;
namespace Final_Project.Models;

public class ProjectImage
{
    public int ImageId { get; set; }
    [Required] public int ProjectId { get; set; }
    [Required, StringLength(500), Url] public string ImageUrl { get; set; } = "";
    public Project? Project { get; set; }
}
