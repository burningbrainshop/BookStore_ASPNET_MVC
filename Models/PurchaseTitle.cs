using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Repository;

namespace BookStore.Models
{
    public class PurchaseTitle : Entity
    {        
        [Key]
        public int PurchaseId { get; set; }
        
        [Required(ErrorMessage =  "請選擇供應商名稱")]
        public int ProviderId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "金額必須大於0")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "請輸入交貨日期")]
        public string StockedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual List<StockTitle> stockInfo { get; set; }
        public virtual Provider providerInfo { get; set; }
        public virtual List<PurchaseDetail> itemDetail { get; set; }
    }
}