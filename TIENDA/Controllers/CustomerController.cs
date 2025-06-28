
using Microsoft.AspNetCore.Mvc;
using TIENDA.Models;
using TIENDA.Models.Repositories;

namespace TIENDA.Controllers;

public class CustomerController : Controller
{
    private readonly CustomerRepository _repository;

    public CustomerController(CustomerRepository repository)
    {
        _repository = repository;
    }

    // GET: Customer
    public async Task<IActionResult> Index()
    {
        var customers = await _repository.GetAllCustomersAsync();
        return View(customers);
    }

    // GET: Customer/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Customer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,Address,City,Country,PostalCode,RegistrationDate")] Customer customer)
    {
        if (ModelState.IsValid)
        {
            customer.RegistrationDate = DateTime.Now; // Set registration date
            await _repository.CreateCustomerAsync(customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customer/Edit/5
    public async Task<IActionResult> Edit(long id)
    {
        var customer = await _repository.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return View(customer);
    }

    // POST: Customer/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,Address,City,Country,PostalCode,RegistrationDate")] Customer customer)
    {
        if (id != customer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _repository.UpdateCustomerAsync(customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customer/Delete/5
    public async Task<IActionResult> Delete(long id)
    {
        var customer = await _repository.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    // POST: Customer/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        await _repository.DeleteCustomerAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
