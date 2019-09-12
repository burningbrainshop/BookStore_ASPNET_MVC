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
    public class ProviderController : Controller
    {
        ProviderService service = new ProviderService();
        private static int _page = 1;

        public ActionResult ProviderIndex()
        {
            IList<Provider> data = service.GetAll();

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            return View(data.OrderByDescending(x => x.ProviderId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList());
        }

        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("ProviderIndex");
        }

        public ActionResult ProviderCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProviderCreate([Bind(Exclude = "ProviderId, CreatedOn")]Provider info)
        {
            if (ModelState.IsValid)
            {
                _page = 1;
                service.InsertProvider(info);
                return RedirectToAction("ProviderIndex");
            }
            else
                return View(info);
        }

        public ActionResult ProviderEdit(Provider providerId)
        {
            return View(providerId);
        }

        [HttpPost]
        public ActionResult ProviderEdit(int providerId, [Bind(Exclude = "CreatedOn")]Provider info)
        {
            if (ModelState.IsValid)
            {
                info.ProviderId = providerId;
                service.EditProvider(info);
                return RedirectToAction("ProviderIndex");
            }
            else
            {
                info.ProviderId = providerId;
                return View(info);
            }
        }

        public ActionResult ProviderDetail(Provider providerId)
        {
            return View(providerId);
        }
    }
}
