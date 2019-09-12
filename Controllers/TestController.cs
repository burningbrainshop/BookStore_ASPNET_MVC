using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class TestController : Controller
    {
        BookStoreContext _Context = new BookStoreContext();

        public ActionResult ManagerInRoleTest()
        {
            /* 方法一: Eager Loading(配合Manager Model)
            var query = _Context.ManagerInRoles
                        .Include(mr => mr.RoleDetail)
                        .Where(mr => mr.ManagerId == 1);

            return View(query.ToList());
             * */


            // 方法二: Explicit Loading(配合ManagerInRole Model)
            /*
            var manager = (from m in _Context.Managers
                           where m.ManagerId == 1
                           select m).FirstOrDefault();

            _Context.Entry(manager)
                .Collection(m => m.InRoleList)
                .Load();


            List<ManagerInRole> MIR = new List<ManagerInRole>();
            foreach(var item in manager.InRoleList)
            {
                _Context.Entry(item).Reference(mr => mr.RoleDetail).Load();  //因為RoleDetail是virtual屬性，必須用Reference()取用
                MIR.Add(new ManagerInRole { ManagerId = item.ManagerId, RoleId = item.RoleId, ManagerDetail = manager, RoleDetail = item.RoleDetail });
            }
            
            return View(MIR.ToList());
             * */
            var roles = from r in _Context.Roles
                        orderby r.RoleId
                        select r;

            var manager = (from m in _Context.Managers
                           where m.ManagerId == 1
                           select m).FirstOrDefault();

            _Context.Entry(manager)
                .Collection(mr => mr.InRoleList)
                .Load();

            ManagerDetailAndRolesListView managerAndRoles = new ManagerDetailAndRolesListView();
            managerAndRoles.ManagerId = manager.ManagerId;
            managerAndRoles.Account = manager.Account;
            /*
            foreach(var each in roles)
            {
                //Role roleAdded = new Role();
                bool flag = false;
                foreach(var item in manager.InRoleList)
                {
                    _Context.Entry(item).Reference(mr => mr.RoleDetail).Load();                    
                    if (each.RoleId.Equals(item.RoleDetail.RoleId))
                    {
                        flag = true;
                    }
                }
                if (flag)
                    managerAndRoles.RoleList.Add(new Role { RoleId = each.RoleId, Name = each.Name });
                else
                    managerAndRoles.RoleList.Add(new Role { RoleId = each.RoleId, Name = "NONE" });
            }
            */
            return View(managerAndRoles);
        }
    }
}
