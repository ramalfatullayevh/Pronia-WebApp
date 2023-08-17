using ProniaMVC.Models;

namespace ProniaMVC.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<ShippingArea> ShippingAreas { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Color> Colors { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<CreateProductVM> CreateProductVMs { get; set; }
    }
}
