
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TIENDA.Models;

public class OrderDetail
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order Order { get; set; }

    [Required]
    public long ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }
}
