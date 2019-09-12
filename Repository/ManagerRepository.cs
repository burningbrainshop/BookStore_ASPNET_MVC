using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Repository
{
    public class ManagerRepository : Repository<Manager>
    {
        public Manager GetByAccount(string account)
        {
            return _table.Where(m => m.Account.Equals(account)).FirstOrDefault();
        }

        public ManagerDetailAndRolesListView GetDetail(int managerId, List<Role> roles)
        {
            var manager = GetById(managerId);

            _context.Entry(manager)
                .Collection(mr => mr.InRoleList);

            ManagerDetailAndRolesListView managerAndRoles = new ManagerDetailAndRolesListView();
            managerAndRoles.ManagerId = manager.ManagerId;
            managerAndRoles.Account = manager.Account;
            managerAndRoles.LastLoginOn = manager.LastLoginOn;

            foreach (var each in roles)
            {
                bool flag = false;
                foreach (var item in manager.InRoleList)
                {
                    if (each.RoleId.Equals(item.RoleId))
                    {
                        flag = true;
                    }
                }
                if (flag)
                    managerAndRoles.RoleList.Add(new RolesView { RoleId = each.RoleId, Name = each.Name, Join = "Y" });
                else
                    managerAndRoles.RoleList.Add(new RolesView { RoleId = each.RoleId, Name = each.Name, Join = "N" });
            }
            return managerAndRoles;
        }
    }
}