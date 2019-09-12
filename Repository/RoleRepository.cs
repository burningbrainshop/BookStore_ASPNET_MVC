using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Infrastruture;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Repository
{
    public class RoleRepository : Repository<Role>
    {
        public Role GetByName(string roleName)
        {
            return _table.Where(r => r.Name.Equals(roleName)).FirstOrDefault();
        }

        public RoleDetailsView GetDetail(int roleId)
        {
            var role = GetById(roleId);

            _context.Entry(role)
                .Collection(mr => mr.ManagerInList);

            _context.Entry(role)
                .Collection(mr => mr.InApplicationList);

            RoleDetailsView roleDetail = new RoleDetailsView();
            roleDetail.RoleId = role.RoleId;
            roleDetail.Name = role.Name;

            if (role.ManagerInList.Count > 0)
            {
                foreach (var item in role.ManagerInList)
                {
                    _context.Entry(item).Reference(m => m.singleManager).Load();
                    roleDetail.ManagerList.Add(item.singleManager);
                }
            }

            if (role.InApplicationList.Count > 0)
            {
                int count = role.InApplicationList.Count;
                foreach(var value in Enum.GetValues(typeof(ControllerName)))
                {
                    string isChecked = "N";
                    string controllerListItem = Enum.GetName(typeof(ControllerList), value);
                    foreach (var app in role.InApplicationList)
                    {
                        if (app.AppId.Equals(Convert.ToInt32(value)))
                            isChecked = "Y";
                    }
                    roleDetail.ControllerList.Add(new ControllerListInfo { ControllerName = ((ControllerName)value).ToString(), ControllerListName = controllerListItem, ControllerListValue = (int)value, Checked = isChecked });
                }
            }
            else
            {
                foreach (var name in Enum.GetNames(typeof(ControllerName)))
                {
                    string controllerListItem = Enum.GetName(typeof(ControllerList), Enum.Parse(typeof(ControllerName), name));
                    roleDetail.ControllerList.Add(new ControllerListInfo { ControllerName = name, ControllerListName = controllerListItem, ControllerListValue = (int)Enum.Parse(typeof(ControllerName), name), Checked = "N" });
                }
            }

            return roleDetail;
        }
    }
}