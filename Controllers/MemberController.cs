using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookStore.Infrastruture;
using BookStore.Services;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    [CustomAuthorize(Roles = "Member")]
    public class MemberController : Controller
    {
        BookProductService bookService = new BookProductService();
        MemberService memberService = new MemberService();
        OrderService orderService = new OrderService();
        private static int _indexPage = 1;
        private static int _nextPage = 1;

        public ActionResult Index()
        {
            var data = memberService.GetByAccount(User.Identity.Name);           
            return View(data);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.IsInRole("Member") && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "BookProduct");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RegisterCreate([Bind(Exclude = "MemberId, AuthCode, CreatedOn")]Member info)
        {
            if(!memberService.CheckAccount(info.Account))
                ModelState.AddModelError("Account", "您輸入的帳號已經有人註冊過了!");

            if (ModelState.IsValid)
            {
                info.AuthCode = Guid.NewGuid().ToString();
                UriBuilder validateUrl = new UriBuilder(Request.Url)
                {
                    Path = Url.Action("EmailValidation", "Member", new { userName = info.Account, authCode = info.AuthCode })
                };
                memberService.InsertMember(info, validateUrl.ToString().Replace("%3F", "?"));
                return View("RegisterComplete");
            }
            return View("Register");
        }

        [AllowAnonymous]
        public ActionResult EmailValidation(string userName, string authCode)
        {
            bool result = memberService.CheckAccount(userName);
            if (result)
            {
                TempData["RegisterResult"] = "NoAccount";
                return View("RegisterState");
            }

            result = memberService.EmailValidation(userName, authCode);
            if (result)
            {
                TempData["RegisterResult"] = "CheckOk";
                return View("RegisterState");
            }
            else
            {
                TempData["RegisterResult"] = "Checked";
                return View("RegisterState");
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.IsInRole("Member") && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "BookProduct");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginView info)
        {
            ViewData["Error"] = "* 您輸入的帳號或密碼不正確";
            if (memberService.CheckMember(info.Account, info.Password))
            {                
                var data = memberService.GetByAccount(info.Account);
                if (data.AuthCode != null)
                {
                    ViewData["Error"] = "* 您尚未通過會員驗證,請收信並點擊會員驗證連結!";
                    return View(info);
                }
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, info.Account, DateTime.Now, DateTime.Now.AddMinutes(30), false, "Member", FormsAuthentication.FormsCookiePath);
                string encryTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryTicket));
                return RedirectToAction("Index", "BookProduct");
            }       
            else
                return View(info);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "BookProduct");
        }

        public ActionResult GetPasswordChangeHtml()
        {
            string html = System.IO.File.ReadAllText(Server.MapPath("~/Views/Member/PasswordChangeHtml.html"));
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword(string newpwd)
        {
            var data = memberService.GetByAccount(User.Identity.Name);
            data.Password = newpwd;
            memberService.EditMemberPassword(data);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        public ActionResult CheckUserLogin()
        {
            if (User.Identity.IsAuthenticated)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            else
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
        }

        public ActionResult NextBuyIndex()
        {
            var memberId = memberService.GetByAccount(User.Identity.Name).MemberId;
            var datas = bookService.GetNextBuyProducts(memberId);

            Pager nextPage = new Pager();
            nextPage.NowPage = _nextPage;
            nextPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(datas.Count) / nextPage.PageSize));
            nextPage.ValidatePage();
            ViewBag.nextPaging = nextPage;

            return View(datas.OrderByDescending(x => x.productId).Skip((nextPage.NowPage - 1) * nextPage.PageSize).Take(nextPage.PageSize).ToList());
        }

        public ActionResult MemberOrderIndex(int timeValue = 1)
        {
            var memberId = memberService.GetByAccount(User.Identity.Name).MemberId;
            IList<OrderTitleView> data = orderService.GetOrderTitleList(timeValue, memberId);

            Pager setPage = new Pager();
            setPage.NowPage = _indexPage;
            setPage.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count) / setPage.PageSize));
            setPage.ValidatePage();
            ViewBag.Paging = setPage;

            List<SelectListItem> lists = new List<SelectListItem>();
            lists.Add(new SelectListItem { Text = "最近三個月內訂單", Value = "1" });
            lists.Add(new SelectListItem { Text = "最近六個月內訂單", Value = "2" });
            lists.Add(new SelectListItem { Text = "最近一年內訂單", Value = "3" });
            ViewData["ordertime"] = lists;

            return View(data.OrderByDescending(x => x.OrderId).Skip((setPage.NowPage - 1) * setPage.PageSize).Take(setPage.PageSize).ToList());
        }

        public ActionResult GetIndexPage(int icurPage = 1)
        {
            _indexPage = icurPage;
            return RedirectToAction("MemberOrderIndex");
        }

        public ActionResult OrderDetails(int orderId)
        { 
            var data = orderService.GetOrderDetails(orderId);
            return View(data);
        }

        public ActionResult GetNextPage(int ncurPage = 1)
        {
            _nextPage = ncurPage;
            return RedirectToAction("NextBuyIndex");
        }
    }
}
