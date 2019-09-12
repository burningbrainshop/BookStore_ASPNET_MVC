using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Repository;

namespace BookStore.Models
{
    public class StockTitle : Entity
    {
        [Key]
        public int StockId { get; set; } 
        [ForeignKey("purchaseInfo")]
        public int WithPurchaseId { get; set; }

        [Required(ErrorMessage = "請選擇供應商名稱")]
        public int ProviderId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "金額必須大於0")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "請輸入進貨日期")]
        public string InStockedOn { get; set; }

        public PurchaseTitle purchaseInfo { get; set; }
        public virtual Provider providerInfo { get; set; }
        public virtual List<StockDetail> itemDetail { get; set; }
    }
}