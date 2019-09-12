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
    public class ProductController : Controller
    {
        ProductService productService = new ProductService();
        SettingService settingService = new SettingService("CategorySetting");
        ProviderService providerService = new ProviderService();
        private static int _page = 1;

        public ActionResult ProductIndex(string CategoryId = "", string ProviderId = "", string OnSale = "", string TimeValue = "")
        {
            IList<Product> data = productService.GetAll(CategoryId, ProviderId, OnSale, TimeValue);

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            List<SelectListItem> categories = new List<SelectListItem>();
            var categoryDatas = settingService.GetAll();
            SelectList categoryList = new SelectList(categoryDatas, "CategoryId", "CategoryName", CategoryId);
            ViewData["categoryList"] = categoryList;

            List<SelectListItem> providers = new List<SelectListItem>();
            var providerDatas = providerService.GetAll();
            SelectList providerList = new SelectList(providerDatas, "ProviderId", "Name", ProviderId);
            ViewData["providerList"] = providerList;

            List<SelectListItem> onsales = new List<SelectListItem>();
            if (OnSale == "Y") { onsales.Add(new SelectListItem { Text = "上架", Value = "Y", Selected = true }); } else { onsales.Add(new SelectListItem { Text = "上架", Value = "Y" }); }
            if (OnSale == "Y") { onsales.Add(new SelectListItem { Text = "下架", Value = "N", Selected = true }); } else { onsales.Add(new SelectListItem { Text = "下架", Value = "N" }); }
            ViewData["onsaleList"] = onsales;

            List<SelectListItem> timeValue = new List<SelectListItem>();
            if (TimeValue == "1") { timeValue.Add(new SelectListItem { Text = "最近三個月商品", Value = "1", Selected = true }); } else { timeValue.Add(new SelectListItem { Text = "最近三個月商品", Value = "1" }); }
            if (TimeValue == "2") { timeValue.Add(new SelectListItem { Text = "最近六個月商品", Value = "2", Selected = true }); } else { timeValue.Add(new SelectListItem { Text = "最近六個月商品", Value = "2" }); }
            if (TimeValue == "3") { timeValue.Add(new SelectListItem { Text = "最近一年商品", Value = "3", Selected = true }); } else { timeValue.Add(new SelectListItem { Text = "最近一年商品", Value = "3" }); }
            if (TimeValue == "4") { timeValue.Add(new SelectListItem { Text = "最近二年商品", Value = "4", Selected = true }); } else { timeValue.Add(new SelectListItem { Text = "最近二年商品", Value = "4" }); }
            ViewData["timevalueList"] = timeValue;

            return View(data.OrderByDescending(x => x.ProductId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList());
        }

        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("ProductIndex");
        }

        public ActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductCreate([Bind(Exclude = "ProductId, CreatedOn")]Product info)
        {
            if (ModelState.IsValid)
            {
                _page = 1;
                productService.InsertProduct(info);
                return RedirectToAction("ProductIndex");
            }
            else
                return View(info);
        }

        public ActionResult ProductEdit(Product productId)
        {
            return View(productId);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductEdit(int ProductId, [Bind(Exclude = "Stock, SaledQuantity, ImageData, ImageType, CreatedOn")]Product info, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                string noupdateImg = Request.Form["noupdateImg"];
                info.ProductId = ProductId;
                productService.EditProduct(info, imageFile, noupdateImg);
                return View(productService.GetById(ProductId));
            }
            else
            {
                info.ProductId = ProductId;
                return View(info);
            }
        }

        public ActionResult ProductDetail(Product productId)
        {
            return View(productId);
        }

        [AllowAnonymous]
        public ActionResult GetImage(int productId)
        {
            var data = productService.GetById(productId);
            if (data != null && data.ImageType != null)
            {
                return File(data.ImageData, data.ImageType);
            }
            else
                return null;
        }

        [AllowAnonymous]
        public ActionResult ProductByKey(string term)
        {
            var datas = productService.GetProductByKey(term).Select(p => p.Name).ToList();
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult SearchProducts(string term)
        {
            var datas = productService.GetProductByKey(term).ToList();
            return Json(datas, JsonRequestBehavior.AllowGet);
        }
    }
}
