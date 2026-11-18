using Portfolio.Data;
using Portfolio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers;

public class ProjectImagesController : Controller
{
    private readonly ProjectImageRepository _images;

    public ProjectImagesController(ProjectImageRepository images)
    {
        _images = images;
    }

    // GET /ProjectImages : the list of project images.
    public async Task<IActionResult> Index()
    {
        var images = await _images.GetAllAsync();

        return View(images);
    }

    // GET /ProjectImages/Create : an empty form.
    public IActionResult Create()
    {
        return View(new ProjectImage());
    }

    // POST /ProjectImages/Create : the form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectImage image)
    {
        if (!ModelState.IsValid)
        {
            return View(image);
        }

        await _images.AddAsync(image);

        return RedirectToAction(nameof(Index));
    }

    // GET /ProjectImages/Edit/5 : the form, filled with image 5.
    public async Task<IActionResult> Edit(long id)
    {
        var image = await _images.GetByIdAsync(id);

        if (image == null)
        {
            return NotFound();
        }

        return View(image);
    }

    // POST /ProjectImages/Edit/5 : the changed form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProjectImage image)
    {
        if (!ModelState.IsValid)
        {
            return View(image);
        }

        await _images.UpdateAsync(image);

        return RedirectToAction(nameof(Index));
    }

    // GET /ProjectImages/Delete/5 : ask "are you sure?"
    public async Task<IActionResult> Delete(long id)
    {
        var image = await _images.GetByIdAsync(id);

        if (image == null)
        {
            return NotFound();
        }

        return View(image);
    }

    // POST /ProjectImages/Delete/5 : the delete was confirmed.
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        await _images.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}