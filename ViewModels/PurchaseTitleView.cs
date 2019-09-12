using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.ViewModels
{
    public class PurchaseTitleView
    {
        public int PurchaseId { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public int Amount { get; set; }
        public string StockedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string StockCheck { get; set; }
    }
}