using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "請輸入角色名稱")]
        public string Name { get; set; }

        public virtual List<ManagerInRole> ManagerInList { get; set; }
        public virtual List<RoleInApplication> InApplicationList { get; set; }
    }
}