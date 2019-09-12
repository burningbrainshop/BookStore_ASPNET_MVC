using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.ViewModels
{
    public class StockTitleView
    {
        public int StockId { get; set; }
        public int PurchaseId { get; set; }
        public string ProviderName { get; set; }
        public int Amount { get; set; }
        public string InStockedOn { get; set; }
    }
}