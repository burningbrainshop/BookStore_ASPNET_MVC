using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels
{
    public class ManagerLoginView
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "密碼必須介於5-20個字之間")]
        public string Account { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼必須介於8-20個字之間")]
        public string Password { get; set; }
    }
}