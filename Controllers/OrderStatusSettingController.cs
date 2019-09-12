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
    public class OrderStatusSettingController : Controller
    {
        SettingService service = new SettingService("OrderStatusSetting");
        private static int _page = 1;

        public ActionResult OrderStatusSettingIndex()
        {
            return View();
        }

        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("OrderStatusSettingIndex");
        }

        [ChildActionOnly]
        public ActionResult OrderStatusSettingDataList()
        {
            return PartialView(DataList());
        }

        [HttpPost]
        public ActionResult OrderStatusSettingCreate([Bind(Include = "StatusName")]OrderStatusSetting info)
        {
            if (ModelState.IsValid)
            {
                service.InsertSetting(info);
                return PartialView("OrderStatusSettingDataList", DataList());
            }
            else
                return RedirectToAction("OrderStatusSettingIndex");
        }

        public ActionResult OrderStatusSettingEdit(OrderStatusSetting statusId)
        {
            return View(statusId);
        }

        [HttpPost]
        public ActionResult OrderStatusSettingEdit(int StatusId, [Bind(Include = "StatusName")]OrderStatusSetting info)
        {
            if (ModelState.IsValid)
            {
                info.StatusId = StatusId;
                service.EditSetting(info);
                return RedirectToAction("OrderStatusSettingIndex");
            }
            else
            {
                info.StatusId = StatusId;
                return View(info);
            }
        }

        protected IList<OrderStatusSetting> DataList()
        {
            List<OrderStatusSetting> data = Enumerable.Cast<object>(service.GetAll()).Cast<OrderStatusSetting>().ToList();

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            return data.OrderByDescending(x => x.StatusId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList();
        }
    }
}
