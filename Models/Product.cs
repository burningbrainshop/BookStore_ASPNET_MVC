using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Repository;

namespace BookStore.Models
{
    public class Product : Entity
    {
        [Key]
        [DisplayName("商品編號")]
        public int ProductId { get; set; }

        [DisplayName("類別代碼")]
        [Required(ErrorMessage = "請選擇商品類別")]
        public int CategoryId { get; set; }

        [DisplayName("供應商代碼")]
        [Required(ErrorMessage = "請選擇供應商名稱")]
        public int ProviderId { get; set; }

        [DisplayName("商品名稱")]
        [Required(ErrorMessage = "請輸入商品名稱")]
        public string Name { get; set; }

        [DisplayName("商品說明")]
        public string Description { get; set; }

        [DisplayName("商品成本")]
        [Required(ErrorMessage = "請輸入商品成本")]
        public int Cost { get; set; }

        [DisplayName("商品售價")]
        [Required(ErrorMessage = "請輸入商品售價")]
        public int Price { get; set; }

        [DisplayName("庫存量")]
        public int Stock { get; set; }

        [DisplayName("標準庫存量")]
        [Required(ErrorMessage = "請輸入標準庫存量")]
        public int StandardStock { get; set; }

        [DisplayName("銷售量")]
        public int SaledQuantity { get; set; }

        [DisplayName("商品上架")]
        [Required(ErrorMessage = "請選擇商品是否上架")]
        public string OnSale { get; set; }

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageType { get; set; }

        [DisplayName("資料建立時間")]
        public DateTime CreatedOn { get; set; }

        public virtual CategorySetting categoryInfo { get; set; }
    }
}