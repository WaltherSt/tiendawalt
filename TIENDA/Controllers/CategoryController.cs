
using Microsoft.AspNetCore.Mvc;
using TIENDA.Models;
using TIENDA.Models.Repositories;

namespace TIENDA.Controllers;

public class CategoryController : Controller
{
    private readonly CategoryRepository _repository;

    public CategoryController(CategoryRepository repository)
    {
        _repository = repository;
    }

    // GET: Category
    public async Task<IActionResult> Index()
    {
        var categories = await _repository.GetAllCategoriesAsync();
        return View(categories);
    }

    // GET: Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,Color,Image")] Category category)
    {
        if (ModelState.IsValid)
        {
            try
            {
                category.ProductCount = 0; // Default value
                await _repository.CreateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine(ex.ToString());
                // Add the error to the model state to display to the user
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
            }
        }
        return View(category);
    }

    // GET: Category/Edit/5
    public async Task<IActionResult> Edit(long id)
    {
        var category = await _repository.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,Color,Image,ProductCount")] Category category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _repository.UpdateCategoryAsync(category);
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // GET: Category/Delete/5
    public async Task<IActionResult> Delete(long id)
    {
        var category = await _repository.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: Category/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        await _repository.DeleteCategoryAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
