using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TIENDA.Models;

public class Product
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    [StringLength(1000)]
    public string Description { get; set; }
    
    public bool InStock { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
    public bool Organic { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Origin { get; set; }
    
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public double Rating { get; set; }
    
    [Required]
    public int Reviews { get; set; }
    
    [Required]
    [StringLength(1000)] // Assuming image URLs can be long
    public string ImageUrl { get; set; }

    [Required]
    public long CategoryId { get; set; } 


    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }  // Agregada la propiedad de navegación



    

}