using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Models;

namespace BookStore.ViewModels
{
    public class StockDetailView
    {
        public StockDetailView()
        {
            productItems = new List<ProductItemView>();
        }

        public int StockId { get; set; }
        public int PurchaseId { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public int Amount { get; set; }
        public string InStockedOn { get; set; }
        public List<ProductItemView> productItems { get; set; }
    }
}