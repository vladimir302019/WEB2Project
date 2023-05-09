namespace WebShop.Models
{
    public class Order
    {
        public long Id { get; set; }
        public List<Article> Articles { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public bool Confirmed { get; set; }
        public bool Approved { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
