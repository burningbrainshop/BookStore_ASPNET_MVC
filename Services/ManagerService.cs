using System;
using System.Security;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using BookStore.Infrastruture;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;


namespace BookStore.Services
{
    public class ManagerService
    {
        private ManagerRepository _managerRepository;
        private RoleRepository _roleRepository;
        private Repository<ManagerInRole> _inRoleRepository;
        private Repository<RoleInApplication> _inApplicationRepository;

        public ManagerService()
        {
            _managerRepository = new ManagerRepository();
            _roleRepository = new RoleRepository();
            _inRoleRepository = new Repository<ManagerInRole>();
            _inApplicationRepository = new Repository<RoleInApplication>();
        }

        public bool CheckAccount(string account)
        {
            var result = _managerRepository.GetByAccount(account);
            return (result == null) ? true : false;
        }

        public string HashPassword(string password)
        {
            string crypPwd = null;
            string salt = "8LJn9a03ow20NP918GSNoqvac32hfMRI34";
            string saltPwd = string.Concat(salt, password);

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] pwdByte = Encoding.Default.GetBytes(saltPwd);
            byte[] hashPwd = sha1.ComputeHash(pwdByte);
            for (int i = 0; i < hashPwd.Length; i++)
            {
                crypPwd += hashPwd[i].ToString("x2");
            }
            return crypPwd;
        } 

        public bool CheckManager(ManagerLoginView entity)
        {            
            if (entity.Account.Equals("Admin"))
            {
                var result1 = _managerRepository.GetByAccount(entity.Account);
                if (result1 == null)
                {
                    Manager manager = new Manager();
                    manager.Account = entity.Account;
                    manager.Password = entity.Password;
                    InsertManager(manager);
                    
                    Role role = new Role();
                    var result2 = _roleRepository.GetByName("Administrator");
                    if (result2 == null)
                    {
                        role.Name = "Administrator";
                        _roleRepository.Insert(role);
                    }
                    else
                    {
                        role.RoleId = result2.RoleId;
                        role.Name = result2.Name;
                    }
                     
                    var result3 = _inRoleRepository.SearchFor(m => m.ManagerId == manager.ManagerId && m.RoleId == role.RoleId).FirstOrDefault();
                    if (result3 == null)
                    {
                        ManagerInRole managerInRole = new ManagerInRole();
                        managerInRole.ManagerId = manager.ManagerId;
                        managerInRole.RoleId = role.RoleId;
                        _inRoleRepository.Insert(managerInRole);
                    }
                    
                    foreach(var value in Enum.GetValues(typeof(ControllerName)))
                    {
                        RoleInApplication roleInApplication = new RoleInApplication();
                        roleInApplication.RoleId = role.RoleId;
                        roleInApplication.AppId = Convert.ToInt32(value);
                        _inApplicationRepository.Insert(roleInApplication);
                    }
                    return true;
                }
            }
            string pwd = HashPassword(entity.Password);
            var data = _managerRepository.SearchFor(m => m.Account.Equals(entity.Account) && m.Password.Equals(pwd)).FirstOrDefault();
            return (data == null) ? false : true;
        }
        
        public string GetRoles(string account)
        {
            string roles = "";
            
            Manager manager = _managerRepository.GetByAccount(account);
            IQueryable<ManagerInRole> lists = _inRoleRepository.SearchFor(m => m.ManagerId.Equals(manager.ManagerId));
            foreach(var item in lists)
            {
                var role = _roleRepository.GetById(item.RoleId);
                roles += role.Name + ",";
            }
            return roles;
        }

        public List<Manager> GetAll()
        {
            return _managerRepository.GetAll().ToList();
        }

        public Manager GetById(int id)
        {
            return _managerRepository.GetById(id);
        }

        public ChangePasswordView GetDataForChangePassword(int managerId)
        {
            Manager manager = GetById(managerId);
            ChangePasswordView data = new ChangePasswordView();
            data.ManagerId = manager.ManagerId;
            data.Account = manager.Account;
            data.LastLoginOn = manager.LastLoginOn;
            return data;
        }

        public void InsertManager(Manager entity)
        {
            entity.Password = HashPassword(entity.Password);
            entity.LastLoginOn = DateTime.Now;
            _managerRepository.Insert(entity);
        }

        public void DeleteManager(int managerId)
        {
            var entity = _managerRepository.GetById(managerId);
            _managerRepository.Delete(entity);
        }

        public void EditManager(ChangePasswordView entity)
        {
            var data = _managerRepository.GetById(entity.ManagerId);
            data.Password = HashPassword(entity.Password);
            _managerRepository.Edit(data);
        }

        public ManagerDetailAndRolesListView DetailManager(int managerId)
        {
            List<Role> roles = _roleRepository.GetAll().ToList();
            var managerRoles = _managerRepository.GetDetail(managerId, roles);
            return managerRoles;
        }

        public void UpdateLoginTime(string account)
        {
            var data = _managerRepository.GetByAccount(account);
            data.LastLoginOn = DateTime.Now;
            _managerRepository.Edit(data);
        }
    }
}