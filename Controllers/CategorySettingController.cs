using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Infrastruture;
using BookStore.Services;
using BookStore.Models;
using BookStore.Repository;

namespace BookStore.Controllers
{
    [CustomAuthorize]
    public class CategorySettingController : Controller
    {
        SettingService service = new SettingService("CategorySetting");
        private static int _page = 1;

        //SettingService service = new SettingService(); //For UnitTest

        public ActionResult CategorySettingIndex()
        {
            return View();
        }

        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("CategorySettingIndex");
        }

        [ChildActionOnly]
        public ActionResult CategorySettingDataList()
        {
            return PartialView(DataList());
        }

        [HttpPost]
        public ActionResult CategorySettingCreate([Bind(Include = "CategoryName")]CategorySetting info)
        {
            if (ModelState.IsValid)
            {
                service.InsertSetting(info);
                return PartialView("CategorySettingDataList", DataList());
            }
            else
                return RedirectToAction("CategorySettingIndex");
        }

        public ActionResult CategorySettingEdit(CategorySetting categoryId)
        {
            return View(categoryId);
        }

        [HttpPost]
        public ActionResult CategorySettingEdit(int CategoryId, [Bind(Include = "CategoryName")]CategorySetting info)
        {
            if (ModelState.IsValid)
            {
                info.CategoryId = CategoryId;
                service.EditSetting(info);
                return RedirectToAction("CategorySettingIndex");
            }
            else
            {
                info.CategoryId = CategoryId;
                return View(info);
            }
        }

        protected IList<CategorySetting> DataList()
        {
            List<CategorySetting> data = Enumerable.Cast<object>(service.GetAll()).Cast<CategorySetting>().ToList();

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            return data.OrderByDescending(x => x.CategoryId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList();
        }
    }
}
