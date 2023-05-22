﻿namespace WebShop.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public long ArticleId { get; set; }
        public Article Article { get; set; }
        public long OrderId { get; set; }
        public Order Order { get; set; }
    }
}
