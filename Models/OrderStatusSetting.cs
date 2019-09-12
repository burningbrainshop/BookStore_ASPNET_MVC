using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Repository;

namespace BookStore.Models
{
    public class OrderStatusSetting : Entity
    {
        [Key]
        [DisplayName("訂單狀態代碼")]
        public int StatusId { get; set; }

        [DisplayName("訂單狀態名稱")]
        [Required(ErrorMessage = "請輸入訂單狀態名稱")]
        [StringLength(10, ErrorMessage = "訂單狀態名稱不可超過10個字")]
        public string StatusName { get; set; }
    }
}