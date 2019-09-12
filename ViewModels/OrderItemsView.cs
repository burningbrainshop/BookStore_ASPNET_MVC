using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Models;

namespace BookStore.ViewModels
{
    public class OrderItemsView
    {
        public Product singleProduct { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; } 
    }
}