using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Repository;

namespace BookStore.Models
{
    public class Member
    {
        [Key]
        [DisplayName("會員代碼")]
        public int MemberId { get; set; }

        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "請輸入會員帳號")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "會員帳號必須介於6-30個字之間")]
        public string Account { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "請輸入會員密碼")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "會員密碼必須介於8-20個字之間")]
        public string Password { get; set; }

        [DisplayName("會員姓名")]
        [Required(ErrorMessage = "請輸入會員姓名")]
        [StringLength(30, ErrorMessage = "會員姓名不可超過30個字")]
        public string Name { get; set; }

        [DisplayName("會員生日")]
        [Required(ErrorMessage = "請輸入會員生日")]
        public string Birthday { get; set; }

        [DisplayName("電子郵件")]
        [Required(ErrorMessage = "請輸入電子郵件帳號")]
        [EmailAddress(ErrorMessage = "電子郵件帳號格式不正確")]
        public string Email { get; set; }

        [DisplayName("聯絡電話")]
        [Required(ErrorMessage = "請輸入聯絡電話")]
        [StringLength(30, ErrorMessage = "聯絡電話不可超過30個字")]
        public string PhoneNumber { get; set; }

        public string AuthCode { get; set; }

        [DisplayName("加入會員時間")]
        public DateTime CreatedOn { get; set; }
    }
}