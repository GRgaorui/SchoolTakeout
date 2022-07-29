using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolTakeout.Tool;
using System.IO;
using System.Data.SqlClient;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;

namespace SchoolTakeout.Controllers
{
    public class ShopManageController : Controller
    {
        public ActionResult OrderMain()
        {
            //ViewData["Account"] = "666666";
            string Account=Request["Account"];
            ViewData["Account"] = Account;
            OrderInfoQueue B = new OrderInfoQueue();
            B.PushOrderToShop(Account);
            return View();
        }
        public ActionResult distribution()
        {
            ViewData["Account"] = Request["Account"];
            return View();
        }
        public ActionResult Set()
        {
            ViewData["Account"] = Request["Account"];
            return View();
        }
        public ActionResult Completed()
        {
            ViewData["Account"] = Request["Account"];
            return View();
        }
        public ActionResult Login()
        {
            ViewData["Account"] = Request["Account"];
            return View();
        }
        public ActionResult FoodEdit()
        {
            ViewData["Account"] = Request["Account"];
            return View();
        }
        public ActionResult ShopSet()
        {
            ViewData["Account"] = Request["Account"];
            return View();
        }

        public ActionResult OrderHistory()
        {
            ViewData["Account"] = Request["Account"];
            return View();
        }

        public int Updata(string Account)
        {
            OrderInfoQueue B = new OrderInfoQueue();
            return B.PushOrderToShopNum(Account);
        }
        public int Confirm(string OrderID, string UserAccount, string ShopAccount)
        {
           //订单信息存入消息队列
            OrderInfoQueue B = new OrderInfoQueue();
            B.WaitConfirmedOrderToDelivery(OrderID, UserAccount, ShopAccount);
            return 1;
        }
        public bool LoginAccoount(string account)
        {
            SqlDB A = new SqlDB();
            return A.SelectAccount(account, "Shop");
        }
        public bool LoginPassword(string account, string password)
        {
            SqlDB A = new SqlDB();
            return A.JudePassword(account, password, "Shop");
        }
        public int ChangeShopState(string State, string Account)
        {
            SqlDB A = new SqlDB();
            if (A.ChangeShopState(State, Account))
            {
                return 1;
            }
            else return 0;
        }
        public string OneShopOrderHistory(string OldTime, string Time, string Account)
        {
            SqlDB A = new SqlDB();
            SqlDataReader read = A.GetOneShopOrderInaTime(OldTime, Time, Account);
            List<List<string>> show = new List<List<string>>();
            while (read.Read())
            {
                List<string> arry = new List<string>();
                for (int i = 0; i < read.FieldCount; i++)
                {
                    arry.Add(read[i].ToString());
                }
                show.Add(arry);
            }
            A.CloseConnection();
            DataContractJsonSerializer json = new DataContractJsonSerializer(show.GetType());
            string szJson = "";
            using (MemoryStream stream = new MemoryStream()) //序列化
            {
                json.WriteObject(stream, show);
                szJson = Encoding.UTF8.GetString(stream.ToArray());
            }
            return szJson;
        }
    }
}