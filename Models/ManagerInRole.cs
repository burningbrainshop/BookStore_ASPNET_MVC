using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class ManagerInRole
    {
        [Key]
        public int Id { get; set; }
        public int ManagerId { get; set; }
        public int RoleId { get; set; }

        public virtual Manager singleManager { get; set; }
    }
}