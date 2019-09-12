using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Repository;

namespace BookStore.Models
{
    public class Provider : Entity
    {
        [Key]
        [DisplayName("供應商代碼")]
        public int ProviderId { get; set; }

        [DisplayName("供應商類別代碼")]
        [Required(ErrorMessage = "請選擇供應商類別")]
        public int ProviderCodeId { get; set; }

        [DisplayName("供應商名稱")]
        [Required(ErrorMessage = "請輸入供應商名稱")]
        public string Name { get; set; }

        [DisplayName("聯絡人")]
        [Required(ErrorMessage = "請輸入聯絡人姓名")]
        [StringLength(30, ErrorMessage = "聯絡人姓名不可超過30個字")]
        public string Contactor { get; set; }

        [DisplayName("電子郵件")]
        [Required(ErrorMessage = "請輸入電子郵件")]
        [EmailAddress(ErrorMessage = "電子郵件格式不正確")]
        public string Email { get; set; }

        [DisplayName("聯絡電話")]
        [Required(ErrorMessage = "請輸入聯絡電話")]
        public string PhoneNumber { get; set; }

        [DisplayName("折扣數")]
        [Required(ErrorMessage = "請輸入折扣數")]
        public int Discount { get; set; }

        [DisplayName("成為供應商")]
        [Required(ErrorMessage = "請選擇是否成為供應商")]
        public string Service { get; set; }

        [DisplayName("資料建立時間")]
        public DateTime CreatedOn { get; set; }
    }
}