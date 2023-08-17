using ProniaMVC.ViewModels.Basket;

namespace ProniaMVC.ViewModels
{
    public class HeaderVM
    {
        public IDictionary<string, string> Settings { get; set; }
        public BasketVM Basket { get; set; }
    }
}
