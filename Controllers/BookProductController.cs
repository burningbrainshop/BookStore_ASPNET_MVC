using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Infrastruture;
using BookStore.Models;
using BookStore.Services;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    [CustomAuthorize(Roles = "Member")]
    public class BookProductController : Controller
    {
        BookProductService bookService = new BookProductService();
        MemberService memberService = new MemberService();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetCategory()
        {
            var result = bookService.GetCategory().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult ProductsListByCategory(int categoryId)
        {
            var result = bookService.ProductsListByCategory(categoryId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetProduct(int productId)
        {
            var result = bookService.GetProduct(productId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNextBuyProducts()
        {
            if (User.Identity.IsAuthenticated)
            {
                var memberId = memberService.GetByAccount(User.Identity.Name).MemberId;
                var datas = bookService.GetNextBuyProducts(memberId);
                return Json(datas.ToList(), JsonRequestBehavior.AllowGet);
            }
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
        }

        public ActionResult AddNextBuy(int productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var memberId = memberService.GetByAccount(User.Identity.Name).MemberId;
                bookService.InsertNextBuyProducts(memberId, productId);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
            }
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
        }

        public ActionResult DeleteNextBuy(int productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var memberId = memberService.GetByAccount(User.Identity.Name).MemberId;
                bookService.DeleteNextBuyProducts(memberId, productId);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
            }
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
        }
    }
}
