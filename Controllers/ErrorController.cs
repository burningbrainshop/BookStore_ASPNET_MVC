using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Infrastruture;

namespace BookStore.Controllers
{
    [CustomAuthorize]
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ForbidPage()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}
