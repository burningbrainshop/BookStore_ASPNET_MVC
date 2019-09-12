using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "請輸入帳號")]
        [Remote("CheckAccount", "Manager", ErrorMessage = "此帳號已被註冊了，請重新輸入帳號")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "帳號必須介於5-30個字之間")]
        public string Account { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "密碼必須大於8個字")]
        public string Password { get; set; }

        public DateTime LastLoginOn { get; set; }

        public virtual List<ManagerInRole> InRoleList { get; set; }
    }
}