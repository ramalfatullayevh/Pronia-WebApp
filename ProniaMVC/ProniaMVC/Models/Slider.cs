using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaMVC.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float DiscountPercent { get; set; }
        public string ImgUrl { get; set; }
        public int Order { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }


    }
}
