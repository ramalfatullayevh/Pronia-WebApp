namespace ProniaMVC.Models
{
    public class ProductInformation
    {
        public int Id { get; set; }
        public string Information { get; set; }
        public string AboutReturnRequst { get; set; }
        public string Guarantee { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
