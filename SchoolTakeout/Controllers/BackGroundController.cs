using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolTakeout.Tool;
using System.Data.SqlClient;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using System.IO;

namespace SchoolTakeout.Controllers
{
    public class BackGroundController : Controller
    {
        public ActionResult BackGroundProcessingOrder()
        {
            return View();
        }
        public ActionResult BackGroundProcessingNotDistribution()
        {
            return View();
        }
        public ActionResult Profits()
        {
            return View();
        }
        public ActionResult FullOrder()
        {
            return View();
        }
        public ActionResult BackGroundLogin()
        {

            return View();
        }
        //后台查找出所有商家没有处理的订单
        public string ProcessingOrder()
        {
            SqlDB A = new SqlDB();
            SqlDataReader ReadFullShop = A.ReferFullMerchant();
            List<List<string>> show = new List<List<string>>();
            while (ReadFullShop.Read())
            {
                List<string> arry = new List<string>();
                for (int i = 0; i < ReadFullShop.FieldCount; i++)
                {
                    arry.Add(ReadFullShop[i].ToString());
                }
                show.Add(arry);
            }
            A.CloseConnection();
            SqlDB B = new SqlDB();
            List<List<string>> UntreatedOrder = new List<List<string>>();
            for (var i = 0; i < show.Count; i++)
            {
                SqlDataReader ReadOneShopUntreatedOrder = B.ReferUntreatedOrder(show[i][1]);
                while (ReadOneShopUntreatedOrder.Read())
                {
                    List<string> arr = new List<string>();
                    for (int k = 0; k < ReadOneShopUntreatedOrder.FieldCount; k++)
                    {
                        arr.Add(ReadOneShopUntreatedOrder[k].ToString());
                    }
                    UntreatedOrder.Add(arr);
                }
            }
            B.CloseConnection();
            DataContractJsonSerializer json = new DataContractJsonSerializer(UntreatedOrder.GetType());
            string szJson = "";
            using (MemoryStream stream = new MemoryStream()) //序列化
            {
                json.WriteObject(stream, UntreatedOrder);
                szJson = Encoding.UTF8.GetString(stream.ToArray());
            }
            return szJson;
        }
        //后台替商家接单
        public string Accept(string OrderID,string UserAccount,string ShopAccount)
        {
            OrderInfoQueue A = new OrderInfoQueue();
            A.WaitConfirmedOrderToDelivery(OrderID, UserAccount, ShopAccount);
            return "SucceedAccept";
        }
        //返回所有商家
        public string ReferFullMerchant()
        {
            SqlDB A = new SqlDB();
            SqlDataReader read = A.ReferFullMerchant();
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
        //返回已选择的商家的订单
        public string ReferCheckedMerchantsOrder(string Check)
        {
            SqlDB A = new SqlDB();
            SqlDataReader read = A.ReferCheckedMerchantsOrder(Check);
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
        //获得没有分配的单子
        public string NotDistributionOrder()
        {
            SqlDB A = new SqlDB();
            SqlDataReader read = A.NotDistributionOrder();
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
        //获取所有的配送员
        public string Delivery()
        {
            SqlDB A = new SqlDB();
            SqlDataReader read = A.Delivery();
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
        //后台登陆验证账号
        public bool LoginAccoount(string account)
        {
            SqlDB A = new SqlDB();
            return A.VerifyAccount(account);
        }
        //后台登陆验证密码
        public bool LoginPassword(string account, string password)
        {
            SqlDB A = new SqlDB();
            return A.VerifyPassword(account, password);
        }
    }
}