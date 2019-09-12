using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Models;

namespace BookStore.ViewModels
{
    public class ManagerDetailAndRolesListView
    {
        public ManagerDetailAndRolesListView()
        {
            RoleList = new List<RolesView>();
        }

        public int ManagerId { get; set; }
        public string Account { get; set; }
        public DateTime LastLoginOn { get; set; }
        public List<RolesView> RoleList { get; set; }
    }
}