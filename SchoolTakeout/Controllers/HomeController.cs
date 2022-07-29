using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolTakeout.Tool;
using System.Threading;
using System.Data.SqlClient;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using System.IO;

namespace SchoolTakeout.Controllers
{
    public class HomeController : Controller
    {
        static int ab = 5;
        public ActionResult Index()
        {
            ViewBag.Message = ab;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public int Order(string OrderID, string UserAccount, string ShopAccount, string ShopInfo, string FoodInfo, string Price, string PackingPrice, string ClientInfo, string RealtimeState, string EndState, string Time)
        {
            //SqlDB A = new SqlDB();
            ////订单信息插入第一步执行的表
            //A.InsertFirstOrder(OrderID, UserAccount, ShopAccount, ShopInfo, FoodInfo, Price, PackingPrice, ClientInfo, RealtimeState, EndState, Time);
            ////订单信息插入用户的表 (未写)
            //A.InsertFirstOrder(OrderID, UserAccount, ShopAccount, ShopInfo, FoodInfo, Price, PackingPrice, ClientInfo, RealtimeState, EndState, Time, "userorder" + UserAccount);
            //OrderInfoQueue B = new OrderInfoQueue();
            //B.WaitOrderToShop(OrderID, UserAccount, ShopAccount);
            return 1;
        }
        public int Updata(string Account)
        {
            OrderInfoQueue B = new OrderInfoQueue();
            return B.PushOrderToShop(Account);
        }
        public bool LoginAccount(string account)
        {
            SqlDB A = new SqlDB();
            return A.SelectAccount(account, "Manage");
        }
        public bool LoginPassword(string account,string password)
        {
            SqlDB A = new SqlDB();
            return A.JudePassword(account, password, "Manage");
        }
    }
}