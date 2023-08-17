using System.ComponentModel.DataAnnotations;

namespace ProniaMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double CostPrice { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double SellPrice { get; set; }
        public string Description { get; set; }
        [Range(0,100)]
        public float Discount { get; set; }
        public string SKU { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ProductColor>? ProductColors { get; set; }
        public ICollection<ProductSize>? ProductSizes { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; }
        public ProductInformation? ProductInformation { get; set; }
    }
}
