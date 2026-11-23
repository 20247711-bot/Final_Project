using Final_Project.Data;
using Final_Project.Models;
using Final_Project.Security;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers;

[AuthorizeAdmin]
public class CoursesController : Controller
{
    private readonly CourseRepository _courses;

    public CoursesController(CourseRepository courses)
    {
        _courses = courses;
    }

    // GET /Courses : the list of courses.
    public async Task<IActionResult> Index()
    {
        var courses = await _courses.GetAllAsync();

        return View(courses);
    }

    // GET /Courses/Create : an empty form.
    public IActionResult Create()
    {
        return View(new Course());
    }

    // POST /Courses/Create : the form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        if (!ModelState.IsValid)
        {
            return View(course);
        }

        await _courses.AddAsync(course);

        return RedirectToAction(nameof(Index));
    }

    // GET /Courses/Edit/5 : the form, filled with course 5.
    public async Task<IActionResult> Edit(long id)
    {
        var course = await _courses.GetByIdAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    // POST /Courses/Edit/5 : the changed form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Course course)
    {
        if (!ModelState.IsValid)
        {
            return View(course);
        }

        await _courses.UpdateAsync(course);

        return RedirectToAction(nameof(Index));
    }

    // GET /Courses/Delete/5 : ask "are you sure?"
    public async Task<IActionResult> Delete(long id)
    {
        var course = await _courses.GetByIdAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    // POST /Courses/Delete/5 : the delete was confirmed.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        await _courses.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}