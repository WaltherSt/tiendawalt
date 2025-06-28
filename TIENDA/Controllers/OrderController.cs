
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TIENDA.Models;
using TIENDA.Models.Repositories;

namespace TIENDA.Controllers;

public class OrderController : Controller
{
    private readonly OrderRepository _orderRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly ProductRepository _productRepository;

    public OrderController(OrderRepository orderRepository, CustomerRepository customerRepository, ProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
    }

    // GET: Order
    public async Task<IActionResult> Index()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        return View(orders);
    }

    // GET: Order/Details/5
    public async Task<IActionResult> Details(long id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    // GET: Order/Create
    public async Task<IActionResult> Create()
    {
        ViewData["CustomerId"] = new SelectList(await _customerRepository.GetAllCustomersAsync(), "Id", "Email");
        ViewData["ProductId"] = new SelectList(await _productRepository.GetAllProductsAsync(), "Id", "Name");
        return View();
    }

    // POST: Order/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CustomerId,TotalAmount,Status")] Order order)
    {
        if (ModelState.IsValid)
        {
            await _orderRepository.CreateOrderAsync(order);
            return RedirectToAction(nameof(Index));
        }
        ViewData["CustomerId"] = new SelectList(await _customerRepository.GetAllCustomersAsync(), "Id", "Email", order.CustomerId);
        return View(order);
    }

    // GET: Order/Edit/5
    public async Task<IActionResult> Edit(long id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        ViewData["CustomerId"] = new SelectList(await _customerRepository.GetAllCustomersAsync(), "Id", "Email", order.CustomerId);
        ViewData["ProductId"] = new SelectList(await _productRepository.GetAllProductsAsync(), "Id", "Name");
        return View(order);
    }

    // POST: Order/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, [Bind("Id,CustomerId,OrderDate,TotalAmount,Status")] Order order)
    {
        if (id != order.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _orderRepository.UpdateOrderAsync(order);
            return RedirectToAction(nameof(Index));
        }
        ViewData["CustomerId"] = new SelectList(await _customerRepository.GetAllCustomersAsync(), "Id", "Email", order.CustomerId);
        return View(order);
    }

    // GET: Order/Delete/5
    public async Task<IActionResult> Delete(long id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: Order/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        await _orderRepository.DeleteOrderAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // POST: Order/AddDetail
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddDetail([Bind("OrderId,ProductId,Quantity,UnitPrice")] OrderDetail orderDetail)
    {
        if (ModelState.IsValid)
        {
            await _orderRepository.AddOrderDetailAsync(orderDetail);
            return RedirectToAction(nameof(Details), new { id = orderDetail.OrderId });
        }
        // If there's an error, you might want to redirect back to the order details page
        // or handle it differently. For simplicity, redirecting to Index.
        return RedirectToAction(nameof(Index));
    }
}
