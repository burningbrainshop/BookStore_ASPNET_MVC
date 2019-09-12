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
    public class ProviderSettingController : Controller
    {
        SettingService service = new SettingService("ProviderSetting");
        private static int _page = 1;

        public ActionResult ProviderSettingIndex()
        {
            return View();
        }

        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("ProviderSettingIndex");
        }

        [ChildActionOnly]
        public ActionResult ProviderSettingDataList()
        {
            return PartialView(DataList());
        }

        [HttpPost]
        public ActionResult ProviderSettingCreate([Bind(Include = "ProviderCodeName")]ProviderSetting info)
        {
            if (ModelState.IsValid)
            {
                service.InsertSetting(info);
                return PartialView("ProviderSettingDataList", DataList());
            }
            else
                return RedirectToAction("ProviderSettingIndex");
        }

        public ActionResult ProviderSettingEdit(ProviderSetting providerId)
        {
            return View(providerId);
        }

        [HttpPost]
        public ActionResult ProviderSettingEdit(int ProviderId, [Bind(Include = "ProviderCodeName")]ProviderSetting info)
        {
            if (ModelState.IsValid)
            {
                info.ProviderCodeId = ProviderId;
                service.EditSetting(info);
                return RedirectToAction("ProviderSettingIndex");
            }
            else
            {
                info.ProviderCodeId = ProviderId;
                return View(info);
            }
        }

        protected IList<ProviderSetting> DataList()
        {
            List<ProviderSetting> data = Enumerable.Cast<object>(service.GetAll()).Cast<ProviderSetting>().ToList();

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            return data.OrderByDescending(x => x.ProviderCodeId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList();
        }
    }
}
