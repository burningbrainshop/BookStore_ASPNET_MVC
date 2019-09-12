using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookStore.Infrastruture;
using BookStore.Services;
using BookStore.Models;
using BookStore.ViewModels;
using System.Data.Entity.Infrastructure;

namespace BookStore.Controllers
{
    [CustomAuthorize]
    public class ManagerController : Controller
    {
        ManagerService service = new ManagerService();
        private static int _page = 1;

        public ActionResult ManagerIndex()
        {
            IList<Manager> datas = service.GetAll();

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(datas.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            return View(datas.OrderByDescending(m => m.ManagerId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList());
        }

        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("ManagerIndex");
        }

        public JsonResult CheckAccount([Bind(Include = "Account, Password")]Manager info)
        {
            bool result = service.CheckAccount(info.Account);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (!User.IsInRole("Member") && User.Identity.IsAuthenticated)
                return RedirectToAction("CategorySettingIndex", "CategorySetting");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(ManagerLoginView info)
        {
            var result = service.CheckManager(info);
            if (result)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, info.Account, DateTime.Now, DateTime.Now.AddMinutes(30), false, service.GetRoles(info.Account), FormsAuthentication.FormsCookiePath);
                string encryTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryTicket));
                return RedirectToAction("Welcome", "Manager");
            }
            else
            {
                ViewBag.LoginError = "帳號或密碼有誤，請重新輸入";
                return View(info);
            }
        }

        [AllowAnonymous]
        public ActionResult Welcome()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            service.UpdateLoginTime(User.Identity.Name);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Manager");
        }

        public ActionResult ManagerCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ManagerCreate([Bind(Exclude = "ManagerId, LastLoginOn")]Manager info)
        {            
            if (ModelState.IsValid)
            {
                service.InsertManager(info);
                return RedirectToAction("ManagerIndex");
            }
            else
                return View(info);
        }

        public ActionResult ManagerDelete(int managerId)
        {
            service.DeleteManager(managerId);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        public ActionResult ManagerEdit(int managerId)
        {
            var data = service.GetDataForChangePassword(managerId);
            return View(data);
        }

        [HttpPost]
        public ActionResult ManagerEdit([Bind(Exclude = "LastLoginOn")]ChangePasswordView info)
        {
            service.EditManager(info);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
        }

        public ActionResult ManagerDetail(int managerId)
        {
            var data = service.DetailManager(managerId);
            return View(data);
        }
    }
}
