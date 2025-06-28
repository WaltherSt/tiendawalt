using Microsoft.EntityFrameworkCore;
using TIENDA.Models;

namespace TIENDA.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        // Asegurarse de que la base de datos esté creada
        

        // Si ya hay categorías, no inicializar
        if (context.Categories.Any())
        {
            return;   // DB has been seeded
        }

        var categories = new Category[]
        {
            new Category { Name = "Electrónica", Description = "Dispositivos electrónicos", Color = "#FF0000", Image = "https://example.com/electronica.jpg", ProductCount = 0 },
            new Category { Name = "Ropa", Description = "Vestimenta y accesorios", Color = "#00FF00", Image = "https://example.com/ropa.jpg", ProductCount = 0 },
            new Category { Name = "Hogar", Description = "Artículos para el hogar", Color = "#0000FF", Image = "https://example.com/hogar.jpg", ProductCount = 0 },
            new Category { Name = "Libros", Description = "Novelas, ciencia ficción, etc.", Color = "#FFFF00", Image = "https://example.com/libros.jpg", ProductCount = 0 },
            new Category { Name = "Deportes", Description = "Artículos deportivos", Color = "#00FFFF", Image = "https://example.com/deportes.jpg", ProductCount = 0 },
            new Category { Name = "Juguetes", Description = "Juguetes para niños y adultos", Color = "#FF00FF", Image = "https://example.com/juguetes.jpg", ProductCount = 0 },
            new Category { Name = "Alimentos", Description = "Productos comestibles", Color = "#C0C0C0", Image = "https://example.com/alimentos.jpg", ProductCount = 0 },
            new Category { Name = "Belleza", Description = "Productos de cuidado personal", Color = "#800000", Image = "https://example.com/belleza.jpg", ProductCount = 0 },
            new Category { Name = "Automotriz", Description = "Accesorios para vehículos", Color = "#808000", Image = "https://example.com/automotriz.jpg", ProductCount = 0 },
            new Category { Name = "Jardinería", Description = "Herramientas y plantas", Color = "#008000", Image = "https://example.com/jardineria.jpg", ProductCount = 0 }
        };

        foreach (Category c in categories)
        {
            context.Categories.Add(c);
        }
        await context.SaveChangesAsync();

        var products = new Product[]
        {
            new Product { Name = "Laptop", Description = "Potente laptop para trabajo y juegos", Price = 1200.00, CategoryId = categories[0].Id, InStock = true, Organic = false, Origin = "China", Rating = 4.5, Reviews = 150, ImageUrl = "https://picsum.photos/seed/laptop/400/300" },
            new Product { Name = "Camiseta", Description = "Camiseta de algodón suave", Price = 25.00, CategoryId = categories[1].Id, InStock = true, Organic = true, Origin = "India", Rating = 4.0, Reviews = 200, ImageUrl = "https://picsum.photos/seed/camiseta/400/300" },
            new Product { Name = "Sartén", Description = "Sartén antiadherente de 28cm", Price = 40.00, CategoryId = categories[2].Id, InStock = true, Organic = false, Origin = "Alemania", Rating = 4.8, Reviews = 90, ImageUrl = "https://picsum.photos/seed/sarten/400/300" },
            new Product { Name = "Libro de C#", Description = "Aprende C# desde cero", Price = 35.50, CategoryId = categories[3].Id, InStock = true, Organic = false, Origin = "USA", Rating = 4.2, Reviews = 80, ImageUrl = "https://picsum.photos/seed/libro/400/300" },
            new Product { Name = "Balón de Fútbol", Description = "Balón oficial de la liga", Price = 50.00, CategoryId = categories[4].Id, InStock = true, Organic = false, Origin = "Brasil", Rating = 4.7, Reviews = 120, ImageUrl = "https://picsum.photos/seed/balon/400/300" },
            new Product { Name = "Set de Construcción", Description = "Juego de bloques para niños", Price = 60.00, CategoryId = categories[5].Id, InStock = true, Organic = false, Origin = "Dinamarca", Rating = 4.9, Reviews = 300, ImageUrl = "https://picsum.photos/seed/juguetes/400/300" },
            new Product { Name = "Café Orgánico", Description = "Granos de café 100% orgánicos", Price = 15.00, CategoryId = categories[6].Id, InStock = true, Organic = true, Origin = "Colombia", Rating = 4.6, Reviews = 90, ImageUrl = "https://picsum.photos/seed/cafe/400/300" },
            new Product { Name = "Crema Hidratante", Description = "Crema facial para todo tipo de piel", Price = 20.00, CategoryId = categories[7].Id, InStock = true, Organic = false, Origin = "Francia", Rating = 4.3, Reviews = 180, ImageUrl = "https://picsum.photos/seed/crema/400/300" },
            new Product { Name = "Aceite de Motor", Description = "Aceite sintético para motor", Price = 45.00, CategoryId = categories[8].Id, InStock = true, Organic = false, Origin = "USA", Rating = 4.1, Reviews = 70, ImageUrl = "https://picsum.photos/seed/aceite/400/300" },
            new Product { Name = "Tijeras de Podar", Description = "Tijeras ergonómicas para jardinería", Price = 30.00, CategoryId = categories[9].Id, InStock = true, Organic = false, Origin = "Japón", Rating = 4.4, Reviews = 50, ImageUrl = "https://picsum.photos/seed/tijeras/400/300" }
        };

        foreach (Product p in products)
        {
            context.Products.Add(p);
        }
        await context.SaveChangesAsync();

        // Actualizar ProductCount en categorías
        foreach (var category in categories)
        {
            category.ProductCount = context.Products.Count(p => p.CategoryId == category.Id);
            context.Categories.Update(category);
        }
        await context.SaveChangesAsync();

        // Añadir datos de clientes
        var customers = new Customer[]
        {
            new Customer { FirstName = "Juan", LastName = "Perez", Email = "juan.perez@example.com", PhoneNumber = "111-222-3333", Address = "Calle Falsa 123", City = "Ciudad", Country = "País", PostalCode = "12345", RegistrationDate = DateTime.Now.AddDays(-30) },
            new Customer { FirstName = "Maria", LastName = "Gomez", Email = "maria.gomez@example.com", PhoneNumber = "444-555-6666", Address = "Avenida Siempre Viva 742", City = "Ciudad", Country = "País", PostalCode = "54321", RegistrationDate = DateTime.Now.AddDays(-60) },
            new Customer { FirstName = "Carlos", LastName = "Ruiz", Email = "carlos.ruiz@example.com", PhoneNumber = "777-888-9999", Address = "Blvd. de los Sueños 1", City = "Ciudad", Country = "País", PostalCode = "67890", RegistrationDate = DateTime.Now.AddDays(-90) },
            new Customer { FirstName = "Laura", LastName = "Diaz", Email = "laura.diaz@example.com", PhoneNumber = "123-456-7890", Address = "Plaza Mayor 5", City = "Ciudad", Country = "País", PostalCode = "10001", RegistrationDate = DateTime.Now.AddDays(-10) },
            new Customer { FirstName = "Pedro", LastName = "Sanchez", Email = "pedro.sanchez@example.com", PhoneNumber = "987-654-3210", Address = "Calle del Sol 20", City = "Ciudad", Country = "País", PostalCode = "20002", RegistrationDate = DateTime.Now.AddDays(-25) },
            new Customer { FirstName = "Ana", LastName = "Lopez", Email = "ana.lopez@example.com", PhoneNumber = "555-123-4567", Address = "Ronda de Noche 3", City = "Ciudad", Country = "País", PostalCode = "30003", RegistrationDate = DateTime.Now.AddDays(-40) },
            new Customer { FirstName = "Miguel", LastName = "Fernandez", Email = "miguel.fernandez@example.com", PhoneNumber = "222-333-4444", Address = "Paseo de la Luna 8", City = "Ciudad", Country = "País", PostalCode = "40004", RegistrationDate = DateTime.Now.AddDays(-55) },
            new Customer { FirstName = "Sofia", LastName = "Martinez", Email = "sofia.martinez@example.com", PhoneNumber = "666-777-8888", Address = "Camino Real 15", City = "Ciudad", Country = "País", PostalCode = "50005", RegistrationDate = DateTime.Now.AddDays(-70) },
            new Customer { FirstName = "Javier", LastName = "Garcia", Email = "javier.garcia@example.com", PhoneNumber = "333-444-5555", Address = "Avenida de la Paz 10", City = "Ciudad", Country = "País", PostalCode = "60006", RegistrationDate = DateTime.Now.AddDays(-85) },
            new Customer { FirstName = "Elena", LastName = "Rodriguez", Email = "elena.rodriguez@example.com", PhoneNumber = "999-000-1111", Address = "Callejón del Gato 7", City = "Ciudad", Country = "País", PostalCode = "70007", RegistrationDate = DateTime.Now.AddDays(-5) }
        };

        foreach (Customer cust in customers)
        {
            context.Customers.Add(cust);
        }
        await context.SaveChangesAsync();

        // Añadir datos de pedidos
        var orders = new Order[]
        {
            new Order { CustomerId = customers[0].Id, OrderDate = DateTime.Now.AddDays(-20), TotalAmount = 1225.00m, Status = "Completado" },
            new Order { CustomerId = customers[1].Id, OrderDate = DateTime.Now.AddDays(-45), TotalAmount = 75.00m, Status = "Pendiente" },
            new Order { CustomerId = customers[2].Id, OrderDate = DateTime.Now.AddDays(-70), TotalAmount = 40.00m, Status = "En Proceso" },
            new Order { CustomerId = customers[3].Id, OrderDate = DateTime.Now.AddDays(-8), TotalAmount = 35.50m, Status = "Completado" },
            new Order { CustomerId = customers[4].Id, OrderDate = DateTime.Now.AddDays(-20), TotalAmount = 50.00m, Status = "Pendiente" },
            new Order { CustomerId = customers[5].Id, OrderDate = DateTime.Now.AddDays(-35), TotalAmount = 60.00m, Status = "Completado" },
            new Order { CustomerId = customers[6].Id, OrderDate = DateTime.Now.AddDays(-50), TotalAmount = 15.00m, Status = "En Proceso" },
            new Order { CustomerId = customers[7].Id, OrderDate = DateTime.Now.AddDays(-65), TotalAmount = 20.00m, Status = "Completado" },
            new Order { CustomerId = customers[8].Id, OrderDate = DateTime.Now.AddDays(-80), TotalAmount = 45.00m, Status = "Pendiente" },
            new Order { CustomerId = customers[9].Id, OrderDate = DateTime.Now.AddDays(-3), TotalAmount = 30.00m, Status = "En Proceso" }
        };

        foreach (Order o in orders)
        {
            context.Orders.Add(o);
        }
        await context.SaveChangesAsync();

        // Añadir datos de detalles de pedidos
        var orderDetails = new OrderDetail[]
        {
            new OrderDetail { OrderId = orders[0].Id, ProductId = products[0].Id, Quantity = 1, UnitPrice = (decimal)products[0].Price },
            new OrderDetail { OrderId = orders[0].Id, ProductId = products[1].Id, Quantity = 1, UnitPrice = (decimal)products[1].Price },
            new OrderDetail { OrderId = orders[1].Id, ProductId = products[1].Id, Quantity = 3, UnitPrice = (decimal)products[1].Price },
            new OrderDetail { OrderId = orders[2].Id, ProductId = products[2].Id, Quantity = 1, UnitPrice = (decimal)products[2].Price },
            new OrderDetail { OrderId = orders[3].Id, ProductId = products[3].Id, Quantity = 1, UnitPrice = (decimal)products[3].Price },
            new OrderDetail { OrderId = orders[4].Id, ProductId = products[4].Id, Quantity = 1, UnitPrice = (decimal)products[4].Price },
            new OrderDetail { OrderId = orders[5].Id, ProductId = products[5].Id, Quantity = 1, UnitPrice = (decimal)products[5].Price },
            new OrderDetail { OrderId = orders[6].Id, ProductId = products[6].Id, Quantity = 1, UnitPrice = (decimal)products[6].Price },
            new OrderDetail { OrderId = orders[7].Id, ProductId = products[7].Id, Quantity = 1, UnitPrice = (decimal)products[7].Price },
            new OrderDetail { OrderId = orders[8].Id, ProductId = products[8].Id, Quantity = 1, UnitPrice = (decimal)products[8].Price },
            new OrderDetail { OrderId = orders[9].Id, ProductId = products[9].Id, Quantity = 1, UnitPrice = (decimal)products[9].Price }
        };

        foreach (OrderDetail od in orderDetails)
        {
            context.OrderDetails.Add(od);
        }
        await context.SaveChangesAsync();
    }
}