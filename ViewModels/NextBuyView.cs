using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.ViewModels
{
    public class NextBuyView
    {
        public int nextId { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }
        public int productPrice { get; set; }
    }
}