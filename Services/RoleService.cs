using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Services
{
    public class RoleService
    {
        private RoleRepository _roleRepository;
        private IRepository<ManagerInRole> _inRoleRepository;
        private IRepository<RoleInApplication> _inApplicationRepository;

        public RoleService()
        {
            _roleRepository = new RoleRepository();
            _inRoleRepository = new Repository<ManagerInRole>();
            _inApplicationRepository = new Repository<RoleInApplication>();
        }

        public IList<Role> GetAll()
        {
            return _roleRepository.GetAll().ToList();
        }

        public Role GetById(int id)
        {
            return _roleRepository.GetById(id);
        }

        public bool CheckData(int roleId)
        {
            var data = _inRoleRepository.SearchFor(mr => mr.RoleId == roleId).FirstOrDefault();
            return (data == null) ? true : false;
        }

        public RoleDetailsView GetRoleDetail(int roleId)
        {
            var roleDetail = _roleRepository.GetDetail(roleId);
            return roleDetail;
        }

        public void InsertRole(Role entity)
        {
            _roleRepository.Insert(entity);
        }

        public void EditRole(Role entity)
        {
            _roleRepository.Edit(entity);
        }

        public void DeleteRole(int roleId)
        {
            var entity = _roleRepository.GetById(roleId);
            _roleRepository.Delete(entity);
            DeleteRoleInApplication(roleId);
        }

        public void JoinRole(int managerId, int roleId, string status)
        { 
            switch(status)
            {
                case "add":
                    ManagerInRole newRole = new ManagerInRole();
                    newRole.ManagerId = managerId;
                    newRole.RoleId = roleId;
                    _inRoleRepository.Insert(newRole);
                    break;
                default:
                    var role = _inRoleRepository.SearchFor(mr => mr.ManagerId == managerId && mr.RoleId == roleId).FirstOrDefault();
                    _inRoleRepository.Delete(role);
                    break;
            }
        }

        public void UpdateRoleInApplication(string applicationValues, int roleId)
        {
            DeleteRoleInApplication(roleId);
            if (!string.IsNullOrEmpty(applicationValues))
            {
                string[] valueArray = applicationValues.Split(new char[] { ',' });
                for (int i = 0; i < valueArray.Length - 1; i++)
                {
                    RoleInApplication inApp = new RoleInApplication();
                    inApp.RoleId = roleId;
                    inApp.AppId = Convert.ToInt32(valueArray[i]);
                    _inApplicationRepository.Insert(inApp);
                }
            }
        }

        private void DeleteRoleInApplication(int roleId)
        {
            var entities = _inApplicationRepository.SearchFor(a => a.RoleId.Equals(roleId));
            _inApplicationRepository.DeleteAll(entities);
        }
    }
}