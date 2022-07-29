using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolTakeout.Controllers
{
    public class DataSelectController : Controller
    {
        // GET: DataSelect
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShopOrderSelect()
        {
            return View();
        }
    }
}