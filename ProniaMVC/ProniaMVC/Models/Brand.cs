using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaMVC.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
