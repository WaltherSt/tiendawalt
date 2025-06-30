
using System.Text.Json;
using TIENDA.Models.ViewModels;

namespace TIENDA.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (item == null)
            {
                Items.Add(new CartItem
                {
                    ProductId = (int)product.Id,
                    ProductName = product.Name,
                    Price = (decimal)product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public decimal GetTotal()
        {
            return Items.Sum(i => i.Total);
        }
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
}
