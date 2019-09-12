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
    public class OrderController : Controller
    {
        OrderService orderService = new OrderService();
        MemberService memberService = new MemberService();
        private static int _page = 1;

        List<Cart> Carts
        {
            get
            {
                if (Session["Carts"] == null)
                {
                    Session["Carts"] = new List<Cart>();
                }
                return (Session["Carts"] as List<Cart>);
            }

            set { Session["Carts"] = value; }
        }

        [AllowAnonymous]
        public ActionResult GetPage(int curPage = 1)
        {
            _page = curPage;
            return RedirectToAction("ProductIndex");
        }

        [AllowAnonymous]
        public ActionResult OrderCreate()
        {
            if (User.IsInRole("Member") && User.Identity.IsAuthenticated)
            {
                var member = memberService.GetByAccount(User.Identity.Name);
                List<Cart> datas = Session["Carts"] as List<Cart>;
                if (datas.Count() > 0)
                {                    
                    int orderId = orderService.InsertOrder(datas, member);
                    return RedirectToAction("OrderList", new { orderId = orderId });
                }
                return RedirectToAction("Index", "Cart");
            }
            return RedirectToAction("Login", "Member");
        }

        [AllowAnonymous]
        public ActionResult OrderList(int orderId)
        {
            if (User.IsInRole("Member") && User.Identity.IsAuthenticated)
            {                
                if (orderId.Equals(0))
                    return RedirectToAction("Index", "Cart");
                else
                {
                    var member = memberService.GetByAccount(User.Identity.Name);
                    ViewBag.OrderId = orderId;
                    return View(member);
                }
            }
            else
                return RedirectToAction("Login", "Member");
        }

        public ActionResult OrderIndex(int timeValue = 1, int keepTimeValue = 0)
        {
            if (keepTimeValue > 0) { timeValue = keepTimeValue; }

            IList<OrderTitleView> data = orderService.GetOrderTitleList(timeValue);

            Pager setPage = new Pager();
            setPage.NowPage = _page;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            List<SelectListItem> lists = new List<SelectListItem>();
            if (timeValue == 1) { lists.Add(new SelectListItem { Text = "最近三個月內訂單", Value = "1", Selected = true }); } else { lists.Add(new SelectListItem { Text = "最近三個月內訂單", Value = "1" }); }
            if (timeValue == 2) { lists.Add(new SelectListItem { Text = "最近六個月內訂單", Value = "2", Selected = true }); } else { lists.Add(new SelectListItem { Text = "最近六個月內訂單", Value = "2" }); }
            if (timeValue == 3) { lists.Add(new SelectListItem { Text = "最近一年內訂單", Value = "3", Selected = true }); } else { lists.Add(new SelectListItem { Text = "最近一年內訂單", Value = "3" }); }
            ViewData["ordertime"] = lists;
            ViewData["keepTimeValue"] = timeValue;
            
            return View(data.OrderByDescending(x => x.OrderId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList());        
        }

        public ActionResult OrderStatusEdit(FormCollection form)
        {
            orderService.OrderStatusEdit(Convert.ToInt32(form["OrderId"]), Convert.ToInt32(form["StatusId"]));
            if (form["getpage"] == "detailpage")
                return RedirectToAction("OrderDetails", "Order", new { orderId = Convert.ToInt32(form["OrderId"]) });
            else
                return RedirectToAction("OrderIndex", "Order", new { keepTimeValue = Convert.ToInt32(form["keepTimeValue"])  });
        }

        public ActionResult OrderDetails(int orderId)
        {
            var data = orderService.GetOrderDetails(orderId);
            return View(data);
        }
    }
}
