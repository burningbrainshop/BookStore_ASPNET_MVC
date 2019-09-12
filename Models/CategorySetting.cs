using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Repository;

namespace BookStore.Models
{
    public class CategorySetting : Entity
    {
        [Key]
        [DisplayName("產品類別代碼")]
        public int CategoryId { get; set; }

        [DisplayName("產品類別名稱")]
        [Required(ErrorMessage = "請輸入產品類別名稱")]
        [StringLength(10, ErrorMessage = "產品類別名稱不可超過10個字")]
        public string CategoryName { get; set; }
    }
}