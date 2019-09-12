using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels
{
    public class ChangePasswordView
    {
        public int ManagerId { get; set; }
        public string Account { get; set; }

        [Required(ErrorMessage = "請輸入變更密碼")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼必須介於8-20個字之間")]
        public string Password { get; set; }

        [Required(ErrorMessage = "請輸入確認密碼")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼必須介於8-20個字之間")]
        [System.Web.Mvc.Compare("Password",ErrorMessage = "兩次密碼輸入不同，請重新輸入")]
        public string CheckPwd { get; set; }

        public DateTime LastLoginOn { get; set; }
    }
}