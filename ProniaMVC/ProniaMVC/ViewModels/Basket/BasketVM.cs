namespace ProniaMVC.ViewModels.Basket
{
    public class BasketVM
    {
        public ICollection<FlowerBasketItemVM> Flowers { get; set; }
        public double TotalPrice { get; set; }
    }
}
