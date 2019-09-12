using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Models;

namespace BookStore.ViewModels
{
    public class RoleDetailsView
    {
        public RoleDetailsView()
        {
            ManagerList = new List<Manager>();
            ControllerList = new List<ControllerListInfo>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }

        public List<Manager> ManagerList { get; set; }
        public List<ControllerListInfo> ControllerList { get; set; }
    }
}