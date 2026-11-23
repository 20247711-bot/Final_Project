using Final_Project.Data;
using Final_Project.Models;
using Final_Project.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Final_Project.Controllers;

[AuthorizeAdmin]
public class ProjectsController : Controller
{
    private readonly ProjectRepository _projects; 
    private readonly CourseRepository _courses;
    public ProjectsController(ProjectRepository projects, CourseRepository courses) { 
        _projects = projects; _courses = courses; 
    }
    private async Task LoadCourses(int? selected = null) => ViewBag.Courses = new SelectList(await _courses.GetAllAsync(), "CourseId", "CourseName", selected);
    public async Task<IActionResult> Index() => View(await _projects.GetAllAsync());
    [HttpGet] public async Task<IActionResult> Create() { 
        await LoadCourses(); return View(new ProjectFormViewModel()); 
    }
    [HttpPost, ValidateAntiForgeryToken] 
    public async Task<IActionResult> Create(ProjectFormViewModel model) { 
        if (!ModelState.IsValid) { 
            await LoadCourses(model.CourseId);
            return View(model); 
        } 

        await _projects.AddAsync(new Project { CourseId = model.CourseId, Title = model.Title, Description = model.Description, VideoUrl = model.VideoUrl }, ParseUrls(model.ImageUrls)); TempData["Success"] = "Project created."; return RedirectToAction(nameof(Index)); }
    [HttpGet] public async Task<IActionResult> Edit(long id) { var p = await _projects.GetByIdAsync(id); if (p == null) return NotFound(); await LoadCourses(p.CourseId); return View(new ProjectFormViewModel { ProjectId = p.ProjectId, CourseId = p.CourseId, Title = p.Title, Description = p.Description, VideoUrl = p.VideoUrl, ImageUrls = string.Join(Environment.NewLine, p.Images.Select(x => x.ImageUrl)) }); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(ProjectFormViewModel model) { if (!ModelState.IsValid) { await LoadCourses(model.CourseId); return View(model); } await _projects.UpdateAsync(new Project { ProjectId = model.ProjectId, CourseId = model.CourseId, Title = model.Title, Description = model.Description, VideoUrl = model.VideoUrl }, ParseUrls(model.ImageUrls)); TempData["Success"] = "Project updated."; return RedirectToAction(nameof(Index)); }
    [HttpGet] public async Task<IActionResult> Delete(long id) { var p = await _projects.GetByIdAsync(id); return p == null ? NotFound() : View(p); }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(long id) { await _projects.DeleteAsync(id); TempData["Success"] = "Project deleted."; return RedirectToAction(nameof(Index)); }
    private static List<string> ParseUrls(string? raw) => (raw ?? "").Split(new[] { '\r', '\n', ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Where(x => Uri.TryCreate(x, UriKind.Absolute, out var u) && (u.Scheme == "http" || u.Scheme == "https")).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
}
