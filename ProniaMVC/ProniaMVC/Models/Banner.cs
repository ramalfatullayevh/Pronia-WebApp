using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaMVC.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public string PrimaryTitle { get; set; }
        public string SecondTitle { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
