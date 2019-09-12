using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookStore.Services;
using BookStore.Repository;
using BookStore.Models;
using System.Web.Routing;

namespace BookStore.Infrastruture
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                return false;

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            if (Roles.Contains("Member"))
            {
                return base.AuthorizeCore(httpContext);
            }

            string controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            bool isCheck = false;

            if (controllerName.Equals("Error"))
            {
                isCheck = true;
            }
            else
            {
                int controllerNumber = GetControllerNumber(controllerName);
                string applicationList = GetInApplicationNumber();
                if (applicationList.IndexOf("," + controllerNumber + ",", 0) == -1)
                    isCheck = false;
                else
                    isCheck = true;
            }
            return isCheck;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //base.HandleUnauthorizedRequest(filterContext);
                if (filterContext.HttpContext.User.IsInRole("Member"))
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "BookProduct", action = "Index" }));
                else
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Manager", action = "Welcome" }));
            }
            else
            {
                if (GetControllerNumber(filterContext.RouteData.Values["controller"].ToString()) == -1)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Member", action = "Login" }));
                else
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Manager", action = "Login" }));
            
            }
        }

        private int GetControllerNumber(string controllerName)
        {
            foreach (var name in Enum.GetNames(typeof(ControllerList)))
            {
                if (name == controllerName)
                {
                    return (int)Enum.Parse(typeof(ControllerList), name);
                }
            }
            return -1;
        }

        private string GetInApplicationNumber()
        {
            IRepository<Role> _roleRepository = (IRepository<Role>)ServiceLocator.Resolve(typeof(Repository<Role>));
            
            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string[] roles = ticket.UserData.Split(new char[] {','});
            string[] rolesNumber = new string[roles.Length];
            for (int i = 0; i < roles.Length-1; i++)
            {
                if (roles[i] != "")
                {
                    string roleName = roles[i];
                    Role result = _roleRepository.SearchFor(r => r.Name.Equals(roleName)).FirstOrDefault();
                    rolesNumber[i] = result.RoleId.ToString();
                }
            }
            return GetInApplicationNumberByRole(rolesNumber);
        }

        private string GetInApplicationNumberByRole(string[] rolesNumber)
        {
            IRepository<RoleInApplication> _inApplicationRepository = (IRepository<RoleInApplication>)ServiceLocator.Resolve(typeof(Repository<RoleInApplication>));
            string appStr = ",";
            for (int i = 0; i < rolesNumber.Length; i++ )
            {
                if (rolesNumber[i] != null)
                {
                    int roleId = Convert.ToInt32(rolesNumber[i]);
                    List<RoleInApplication> result = _inApplicationRepository.SearchFor(r => r.RoleId.Equals(roleId)).ToList();
                    foreach (var item in result)
                    {
                        appStr += item.AppId.ToString() + ",";
                    }
                }
            }
            return appStr;
        }
    }
}