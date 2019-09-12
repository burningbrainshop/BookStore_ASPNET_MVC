using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Infrastruture;
using BookStore.Services;
using BookStore.Models;

namespace BookStore.Controllers
{
    [CustomAuthorize]
    public class RoleController : Controller
    {
        RoleService service = new RoleService();
        private static int _page = 1;

        public ActionResult RoleIndex()
        {
            IList<Role> data = service.GetAll();

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            return View(data.OrderByDescending(x => x.RoleId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList());
        }

        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("RoleIndex");
        }

        public ActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RoleCreate([Bind(Exclude = "RoleId")]Role info)
        {
            service.InsertRole(info);
            return RedirectToAction("RoleIndex");
        }

        public ActionResult RoleEdit(int roleId)
        {
            var data = service.GetById(roleId);
            return View(data);
        }

        [HttpPost]
        public ActionResult RoleEdit(Role info)
        {
            service.EditRole(info);
            return RedirectToAction("RoleIndex");
        }

        public ActionResult RoleDelete(int roleId)
        {
            bool result = service.CheckData(roleId);
            if (result)
            {
                service.DeleteRole(roleId);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden, "請先至[內容]將此角色下的所有帳號刪除");
        }

        public ActionResult RoleDetail(int roleId)
        {
            var datas = service.GetRoleDetail(roleId);
            return View(datas);
        }

        public ActionResult RoleJoin(int managerId, int roleId, string status, string operation)
        {
            service.JoinRole(managerId, roleId, status);
            if (operation.Equals("Role"))
                return RedirectToAction("RoleDetail", "Role", new { roleId = roleId });
            else
                return RedirectToAction("ManagerDetail", "Manager", new { managerId = managerId });
        }

        public ActionResult UpdateRoleInApplication(FormCollection form)
        {
            string str = GetApplicationValue(form);
            service.UpdateRoleInApplication(str, Convert.ToInt32(form["RoleId"]));
            
            return RedirectToAction("RoleDetail", new { RoleId = Convert.ToInt32(form["RoleId"]) });
        }

        private string GetApplicationValue(FormCollection form)
        {
            string values = "";
            if (form.Count > 1)
            {
                for (int i = 1; i < form.Count; i++)
                {
                    if (form[i] != "false")
                        values += form[i].Substring(0, 1) + ",";
                }
            }
            return values;
        }
    }
}
