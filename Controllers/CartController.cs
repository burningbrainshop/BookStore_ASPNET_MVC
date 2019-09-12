using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.Services;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        ProductService service = new ProductService();

        List<Cart> Carts 
        {
            get {
                if (Session["Carts"] == null)
                {
                    Session["Carts"] = new List<Cart>();
                }
                return (Session["Carts"] as List<Cart>);
            }

            set { Session["Carts"] = value; }
        }

        public ActionResult Add(int productId, int quantity = 1)
        {
            var product = service.GetById(productId);
            if (product == null)
                return HttpNotFound();

            var existCart = this.Carts.FirstOrDefault(c => c.productCart.ProductId == productId);
            if (existCart == null)
            {
                this.Carts.Add(new Cart() { productCart = product, quantity = 1 });
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCart()
        {
            return Json(this.Carts.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int productId, int quantity = 1)
        { 
            var existCart = this.Carts.FirstOrDefault(c => c.productCart.ProductId == productId);
            if (existCart != null)
            {
                if (quantity.Equals(0))
                    this.Carts.Remove(existCart);
                else
                    existCart.quantity = quantity;
            }
            return RedirectToAction("Index");
        }
    }
}
