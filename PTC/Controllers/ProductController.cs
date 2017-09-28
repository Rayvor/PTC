using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTC.ViewModelLayer;

namespace PTC.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
        {
            ProductViewModel vm = new ProductViewModel();
            vm.HandleRequest();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Product(ProductViewModel vm)
        {
            // Handle action by user
            vm.HandleRequest();
            // Rebind controls
            ModelState.Clear();

            return View(vm);
        }
    }
}