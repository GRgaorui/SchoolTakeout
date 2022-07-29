using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolTakeout.Tool;
using System.Data.SqlClient;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Text;

namespace SchoolTakeout.Controllers
{
    public class DeliveryController : Controller
    {
        public ActionResult RobOrder()
        {
            ViewData["Account"]=Request["id"];
            return View();
        }

        public ActionResult Distribution()
        {
            ViewData["Account"] = Request["id"];

            return View();
        }

        public ActionResult Success()
        {
            ViewData["Account"] = Request["id"];

            return View();
        }

        public ActionResult Mine()
        {
            ViewData["Account"] = Request["id"];
            return View();
        }

        public ActionResult MyOrder()
        {
            ViewData["Account"] = Request["id"];
            return View();
        }

        public void OrderResult(string num)
        {
            SqlDB A = new SqlDB();
            A.InsertRow(num);
            A.DeleteRow(num);
        }
        public ActionResult Login()
        {

            return View();
        }
        public int Rob(string OrderID, string UserAccount, string ShopAccount,string DeliveryAccount)
        {
            //订单信息存入消息队列
            OrderInfoQueue B = new OrderInfoQueue();
            return B.WaitRobedOrderToCook(OrderID, UserAccount, ShopAccount, DeliveryAccount);           
        }
        public string PushOrderToDeliver()
        {
            OrderInfoQueue B = new OrderInfoQueue();
            return B.PushConfirmedOrderToDelivery();
        }
        /*修改表的状态*/
        public void UpdataOrderState(string dtable1, string ctable2, string Ordernum)
        {
            SqlDB A = new SqlDB();
            A.UpdateState(dtable1, ctable2, Ordernum);
        }

        public bool LoginAccoount(string account)
        {
            SqlDB A = new SqlDB();
            return A.SelectAccount(account, "Delivery");
        }
        public bool LoginPassword(string account, string password)
        {
            SqlDB A = new SqlDB();
            return A.JudePassword(account, password, "Delivery");
        }
        //按日期查询我的订单
        public string Select(string start, string end, string number)
        {
            SqlDB A = new SqlDB();
            SqlDataReader read = A.ReadOrderCount(start, end, number);
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