using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers;

public class ProjectController : Controller
{
    private readonly ProjectRepository _projects;

    public ProjectController(ProjectRepository projects)
    {
        _projects = projects;
    }

    // GET /Projects : the list of projects.
    public async Task<IActionResult> Index()
    {
        var projects = await _projects.GetAllAsync();

        return View(projects);
    }

    // GET /Projects/Create : an empty form.
    public IActionResult Create()
    {
        return View(new Project());
    }

    // POST /Projects/Create : the form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {
        if (!ModelState.IsValid)
        {
            return View(project);
        }

        await _projects.AddAsync(project);

        return RedirectToAction(nameof(Index));
    }

    // GET /Projects/Edit/5 : the form, filled with project 5.
    public async Task<IActionResult> Edit(long id)
    {
        var project = await _projects.GetByIdAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    // POST /Projects/Edit/5 : the changed form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Project project)
    {
        if (!ModelState.IsValid)
        {
            return View(project);
        }

        await _projects.UpdateAsync(project);

        return RedirectToAction(nameof(Index));
    }

    // GET /Projects/Delete/5 : ask "are you sure?"
    public async Task<IActionResult> Delete(long id)
    {
        var project = await _projects.GetByIdAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    // POST /Projects/Delete/5 : the delete was confirmed.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        await _projects.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}