using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels
{
    public class LoginView
    {
        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "請輸入會員帳號")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "會員帳號必須介於6-30個字之間")]
        public string Account { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "請輸入會員密碼")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "會員密碼必須介於8-20個字之間")]
        public string Password { get; set; }
    }
}