using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using TIENDA.Data;

namespace TIENDA.Models.Repositories;

public class ProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(long id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> GetProductsByCategoryIdAsync(long categoryId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task CreateProductAsync(Product product)
    {
        // Verificar si la categoría existe
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == product.CategoryId);
            
        if (!categoryExists)
        {
            throw new InvalidOperationException($"La categoría con ID {product.CategoryId} no existe.");
        }

        try
        {
            await _context.Products.AddAsync(product);
        
            // Actualizar el contador de productos en la categoría
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category != null)
            {
                category.ProductCount++;
                _context.Categories.Update(category);
            }
        
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al crear el producto: {ex.Message}");
        }
    }


    public async Task UpdateProductAsync(Product product)
    {
        var existingProduct = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        if (existingProduct == null)
        {
            throw new InvalidOperationException($"El producto con ID {product.Id} no existe.");
        }

        // Si la categoría cambió, actualizar los contadores
        if (existingProduct.CategoryId != product.CategoryId)
        {
            var oldCategory = await _context.Categories.FindAsync(existingProduct.CategoryId);
            var newCategory = await _context.Categories.FindAsync(product.CategoryId);

            if (oldCategory != null)
                oldCategory.ProductCount--;
            if (newCategory != null)
                newCategory.ProductCount++;
        }

        // Actualizar las propiedades
        _context.Entry(existingProduct).CurrentValues.SetValues(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(long id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            throw new InvalidOperationException($"El producto con ID {id} no existe.");
        }

        // Decrementar el contador de productos en la categoría
        var category = await _context.Categories.FindAsync(product.CategoryId);
        if (category != null)
        {
            category.ProductCount--;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    // Métodos adicionales útiles
    public async Task<List<Product>> GetFeaturedProductsAsync(int count = 6)
    {
        return await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.Rating)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<Product>> SearchProductsAsync(string searchTerm)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Name.Contains(searchTerm) || 
                       p.Description.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task<List<Product>> GetOrganicProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Organic)
            .ToListAsync();
    }

}