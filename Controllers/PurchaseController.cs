using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Infrastruture;
using BookStore.Services;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    [CustomAuthorize]
    public class PurchaseController : Controller
    {
        PurchaseService purchaseService = new PurchaseService();
        ProductService productService = new ProductService();
        private static int _page = 1;

        public ActionResult PurchaseTitleIndex()
        {
            IList<PurchaseTitleView> datas = purchaseService.GetTitleList();

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(datas.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            return View(datas.OrderByDescending(p => p.PurchaseId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList());
        }

        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("PurchaseTitleIndex");
        }

        public ActionResult PurchaseTitleCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PurchaseTitleCreate([Bind(Include = "ProviderId, StockedOn")]PurchaseTitle info)
        {
            purchaseService.InsertPurchaseTitle(info);
            return RedirectToAction("PurchaseTitleIndex");
        }

        public ActionResult PurchaseTitleEdit(PurchaseTitle purchaseId)
        {
            return View(purchaseId);
        }

        [HttpPost]
        public ActionResult PurchaseTitleEdit(int PurchaseId, [Bind(Exclude = "CreatedOn")]PurchaseTitle info)
        {
            purchaseService.EditPurchaseTitle(info);
            return RedirectToAction("PurchaseTitleIndex");
        }

        public ActionResult PurchaseDelete(int purchaseId)
        {
            if (Request.IsAjaxRequest())
            {
                if (purchaseService.DeletePurchase(purchaseId))
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
                else
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        public ActionResult PurchaseDetail(int purchaseId)
        {
            var data = purchaseService.GetPurchaseDetails(purchaseId);

            var categoryList = productService.GetCategoryByProvider(data.ProviderId);           
            SelectList categorySelect = new SelectList(categoryList, "CategoryId", "CategoryName");
            ViewBag.CategorySelectList = categorySelect;

            SelectList productSelect = new SelectList("", "ProductId", "ProductName");
            ViewBag.ProductSelectList = productSelect;

            return View(data);
        }

        public ActionResult GetDetails(int purchaseId)
        {
            //string result = (purchaseService.GetDetails(purchaseId).Count() != null) ? "TRUE" : "FALSE";
            return Json(purchaseService.GetDetails(purchaseId).Count(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PurchaseItemCreate(FormCollection info)
        {
            purchaseService.InsertItems(Convert.ToInt32(info["PurchaseId"]), Convert.ToInt32(info["ProductId"]), Convert.ToInt32(info["Cost"]), Convert.ToInt32(info["Quantity"]));
            return RedirectToAction("PurchaseDetail", new { purchaseId = Convert.ToInt32(info["PurchaseId"]) });
        }

        public ActionResult PurchaseItemDelete(int purchaseDetailId)
        {
            if (Request.IsAjaxRequest())
            {
                purchaseService.DeleteItems(purchaseDetailId);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        public ActionResult PurchaseItemEdit(int purchaseId, int purchaseDetailId)
        {
            var data = purchaseService.GetPurchaseDetails(purchaseId);
            ViewBag.EditId = purchaseDetailId;
            return View(data);
        }

        [HttpPost]
        public ActionResult PurchaseItemEdit(FormCollection info)
        {
            purchaseService.EditItems(Convert.ToInt32(info["item.Id"]), Convert.ToInt32(info["item.Cost"]), Convert.ToInt32(info["item.Quantity"]));
            return RedirectToAction("PurchaseDetail", new { purchaseId = info["PurchaseId"]});
        }

        public ActionResult GetProductsByCategory(int providerId, int categoryId)
        {
            var datas = productService.GetProductsByCategory(providerId, categoryId);
            return Json(datas.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCost(int productId)
        {
            var data = productService.GetById(productId);
            return Json(data.Cost, JsonRequestBehavior.AllowGet);
        }
    }
}
