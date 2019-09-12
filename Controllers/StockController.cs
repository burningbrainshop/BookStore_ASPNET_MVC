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
    public class StockController : Controller
    {
        PurchaseService purchaseService = new PurchaseService();
        ProductService productService = new ProductService();
        StockService stockService = new StockService();
        private static int _page = 1;

        public ActionResult StockTitleIndex()
        {
            IList<StockTitleView> datas = stockService.GetTitleList();

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(datas.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            return View(datas.OrderByDescending(s => s.StockId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList());
        }

        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("PurchaseTitleIndex");
        }

        public ActionResult StockTitleEdit(StockTitle stockId)
        {
            return View(stockId);
        }

        [HttpPost]
        public ActionResult StockTitleEdit(int StockId, [Bind(Exclude = "PurchaseId")]StockTitle info)
        {
            stockService.EditStockTitle(info);
            return RedirectToAction("StockTitleIndex");
        }

        public ActionResult StockTitleDelete(int stockId)
        {
            if (Request.IsAjaxRequest())
            {
                stockService.DeleteStock(stockId);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.MethodNotAllowed);
        }

        public ActionResult StockDetail(int stockId)
        {
            var data = stockService.GetStockDetails(stockId);
            return View(data);
        }

        public ActionResult StockItemEdit(int stockId, int stockDetailId)
        {
            var data = stockService.GetStockDetails(stockId);
            ViewBag.EditId = stockDetailId;
            return View(data);
        }

        [HttpPost]
        public ActionResult StockItemEdit(FormCollection info)
        {
            stockService.EditItems(Convert.ToInt32(info["item.Id"]), Convert.ToInt32(info["item.Quantity"]), Convert.ToInt32(info["item.ReturnQuantity"]));
            return RedirectToAction("StockDetail", new { stockId = info["StockId"] });
        }

        public ActionResult ConvertPurchaseToStock(int purchaseId)
        {
            if (Request.IsAjaxRequest())
            {
                stockService.ConvertPurchaseToStock(purchaseId);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}
