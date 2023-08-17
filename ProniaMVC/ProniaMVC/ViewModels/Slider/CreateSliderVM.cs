using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaMVC.ViewModels
{
    public class CreateSliderVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float DiscountPercent { get; set; }
        [Range(1,15)]
        public int Order { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
