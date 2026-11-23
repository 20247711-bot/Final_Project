using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Final_Project.Controllers;

public class HomeController : Controller
{
    private readonly ProjectRepository _projects; private readonly CourseRepository _courses;
    public HomeController(ProjectRepository projects, CourseRepository courses) { _projects = projects; _courses = courses; }
    public async Task<IActionResult> Index() => View(new PortfolioViewModel { Projects = await _projects.GetAllAsync(), Courses = await _courses.GetAllAsync() });
    public IActionResult Privacy() => View();
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
