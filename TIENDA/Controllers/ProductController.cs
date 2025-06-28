using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TIENDA.Data;
using TIENDA.Models;
using TIENDA.Models.Repositories;

namespace TIENDA.Controllers;
[Route("Product")]
public class ProductController : Controller
{
    private readonly ProductRepository _productRepository;
    private readonly ApplicationDbContext _context;

    public ProductController(ProductRepository productRepository, ApplicationDbContext context)
    {
        _productRepository = productRepository;
        _context = context;
    }
    
    
    

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProductsAsync();
        return View(products);
    }
    
    
[HttpGet("ExportToExcel")]
public async Task<IActionResult> ExportToExcel()
{
    using (var workbook = new XLWorkbook())
    {
        var worksheet = workbook.Worksheets.Add("Productos");

        // Configurar el estilo general de la hoja
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.FontSize = 12;

        // Agregar título
        worksheet.Cell("A1").Value = "CATÁLOGO DE PRODUCTOS";
        var titleRange = worksheet.Range("A1:H1");
        titleRange.Merge();
        titleRange.Style
            .Font.SetBold(true)
            .Font.SetFontSize(20)
            .Fill.SetBackgroundColor(XLColor.FromHtml("#1e40af"))
            .Font.SetFontColor(XLColor.White)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
            .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        worksheet.Row(1).Height = 35;

        // Agregar fecha de exportación
        worksheet.Cell("A2").Value = $"Fecha de exportación: {DateTime.Now:dd/MM/yyyy HH:mm}";
        var dateRange = worksheet.Range("A2:H2");
        dateRange.Merge();
        dateRange.Style
            .Font.SetItalic(true)
            .Font.SetFontSize(11)
            .Font.SetFontColor(XLColor.Gray)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Row(2).Height = 20;

        // Espacio entre fecha y tabla
        worksheet.Row(3).Height = 10;

        // Agregar encabezados de columnas (fila 4)
        string[] headers = { "Nombre", "Precio", "Categoría", "Stock", "Orgánico", "Origen", "Calificación", "Reseñas" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(4, i + 1);
            cell.Value = headers[i];
            cell.Style
                .Font.SetBold(true)
                .Font.SetFontSize(13)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#475569"))
                .Font.SetFontColor(XLColor.White)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin);
        }
        worksheet.Row(4).Height = 30;

        // Obtener y escribir datos
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

        int row = 5;
        foreach (var product in products)
        {
            worksheet.Row(row).Height = 25;
            var rowColor = row % 2 == 0 ? XLColor.FromHtml("#f8fafc") : XLColor.FromHtml("#f1f5f9");

            // Configurar el estilo de toda la fila
            var rowRange = worksheet.Range(row, 1, row, 8);
            rowRange.Style
                .Fill.SetBackgroundColor(rowColor)
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorder(XLBorderStyleValues.Thin)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            // Nombre
            var nameCell = worksheet.Cell(row, 1);
            nameCell.Value = product.Name;
            nameCell.Style
                .Alignment.SetWrapText(true)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            // Precio
            var priceCell = worksheet.Cell(row, 2);
            priceCell.Value = product.Price;
            priceCell.Style
                .NumberFormat.SetFormat("€#,##0.00")
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right)
                .Alignment.SetWrapText(true);

            // Categoría
            var categoryCell = worksheet.Cell(row, 3);
            categoryCell.Value = product.Category?.Name;
            categoryCell.Style
                .Alignment.SetWrapText(true)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            // Stock
            var stockCell = worksheet.Cell(row, 4);
            stockCell.Value = product.InStock ? "Sí" : "No";
            stockCell.Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetWrapText(true)
                .Font.SetFontColor(product.InStock ? XLColor.FromHtml("#15803d") : XLColor.FromHtml("#be123c"));

            // Orgánico
            var organicCell = worksheet.Cell(row, 5);
            organicCell.Value = product.Organic ? "Sí" : "No";
            organicCell.Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetWrapText(true);

            // Origen
            var originCell = worksheet.Cell(row, 6);
            originCell.Value = product.Origin;
            originCell.Style
                .Alignment.SetWrapText(true)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

            // Calificación
            var ratingCell = worksheet.Cell(row, 7);
            ratingCell.Value = product.Rating;
            ratingCell.Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetWrapText(true);

            // Reseñas
            var reviewsCell = worksheet.Cell(row, 8);
            reviewsCell.Value = product.Reviews;
            reviewsCell.Style
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetWrapText(true);

            row++;
        }

        // Establecer anchos de columna
        int[] columnWidths = { 40, 15, 25, 12, 12, 25, 15, 12 };
        for (int i = 0; i < columnWidths.Length; i++)
        {
            var column = worksheet.Column(i + 1);
            column.Width = columnWidths[i];
            column.AdjustToContents(minWidth: columnWidths[i], maxWidth: 60);
        }

        // Agregar filtros a los encabezados
        worksheet.Range(4, 1, row - 1, 8).SetAutoFilter();

        // Congelar panel de encabezados
        worksheet.SheetView.FreezeRows(4);

        // Agregar pie de página con espacio
        row += 1; // Agregar una fila de espacio
        var footerRow = row + 1;
        worksheet.Cell(footerRow, 1).Value = $"Total de productos: {products.Count}";
        var footerRange = worksheet.Range(footerRow, 1, footerRow, 8);
        footerRange.Merge();
        footerRange.Style
            .Font.SetBold(true)
            .Font.SetFontSize(12)
            .Font.SetFontColor(XLColor.FromHtml("#475569"))
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Row(footerRow).Height = 25;

        // Proteger la hoja permitiendo filtros
        worksheet.Protect()
            .AllowElement(XLSheetProtectionElements.AutoFilter)
            .AllowElement(XLSheetProtectionElements.Sort);

        // Ajustar la vista de la hoja
        worksheet.SheetView.ZoomScale = 100;
        worksheet.SheetView.View = XLSheetViewOptions.Normal;

        // Generar el archivo
        using (var stream = new MemoryStream())
        {
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Productos_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }
    }
}

    [HttpGet("Create")]
    public IActionResult Create()
    {
        // Preparar lista de categorías para el dropdown
        var categories = _context.Categories.ToList();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {

        ModelState.Remove("Category");
        if (ModelState.IsValid)
        {
            try
            {
                await _productRepository.CreateProductAsync(product);
                TempData["Success"] = "Producto creado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ha ocurrido un error al crear el producto.");
            }
        }
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, Product product)
    {
        long categoryId = product.CategoryId;
        ModelState.Remove("Category");
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                product.CategoryId = categoryId;
                await _productRepository.UpdateProductAsync(product);
                TempData["Success"] = "Producto actualizado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception e)
            {
                ModelState.AddModelError($"{e}", "Ha ocurrido un error al actualizar el producto.");
            }
        }

        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        try
        {
            await _productRepository.DeleteProductAsync(id);
            TempData["Success"] = "Producto eliminado exitosamente.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Ha ocurrido un error al eliminar el producto.";
        }
        return RedirectToAction("Index");
    }

    // Métodos adicionales útiles
    [HttpGet("Category/{categoryId}")]
    public async Task<IActionResult> ByCategory(long categoryId)
    {
        var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
        return View("Index", products);
    }

    [HttpGet("Featured")]
    public async Task<IActionResult> Featured()
    {
        var products = await _productRepository.GetFeaturedProductsAsync();
        return View("Index", products);
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return RedirectToAction("Index");

        var products = await _productRepository.SearchProductsAsync(query);
        ViewBag.SearchQuery = query;
        return View("Index", products);
    }
}
