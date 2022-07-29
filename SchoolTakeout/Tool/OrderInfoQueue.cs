using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;

namespace SchoolTakeout.Tool
{
    public class OrderInfoQueue
    {
        class OrderInfo
        {
            public string OrderID;
            public string UserAccount;
            public string ShopAccount;
        }
        static bool FirstOpen = true;
        static List<OrderInfo> FirstOrder = new List<OrderInfo>();//下单未推送商家
        static List<OrderInfo> SecondOrder = new List<OrderInfo>();//推送商家未确认
        static List<OrderInfo> ThirdOrder = new List<OrderInfo>();//确认已推送配送员
        private static readonly object First = new object();
        public OrderInfoQueue()
        {
            if(FirstOpen)
            {
                SqlDB A = new SqlDB();
                SqlDataReader read = A.GetOrderInfo("FirstOrder");
                while (read.Read())
                {
                    OrderInfo a = new OrderInfo();
                    a.OrderID = read[0].ToString();
                    a.UserAccount = read[1].ToString();
                    a.ShopAccount = read[2].ToString();
                    FirstOrder.Add(a);
                }
                A.CloseConnection();
                read = A.GetOrderInfo("SecondOrder");
                while (read.Read())
                {
                    OrderInfo a = new OrderInfo();
                    a.OrderID = read[0].ToString();
                    a.UserAccount = read[1].ToString();
                    a.ShopAccount = read[2].ToString();
                    FirstOrder.Add(a);
                }
                A.CloseConnection();
                read = A.GetOrderInfo("ThirdOrder");
                while (read.Read())
                {
                    OrderInfo a = new OrderInfo();
                    a.OrderID = read[0].ToString();
                    a.UserAccount = read[1].ToString();
                    a.ShopAccount = read[2].ToString();
                    ThirdOrder.Add(a);
                }
                A.CloseConnection();
                FirstOpen = false;
            }
        }     
        public int WaitOrderToShop(string OrderID, string UserAccout, string ShopAccount)//订单状态为用户已下单,储存在消息对列，等待推送商家
        {
        
            lock (First)
            {

                OrderInfo a = new OrderInfo();
                a.OrderID = OrderID;
                a.UserAccount = UserAccout;
                a.ShopAccount = ShopAccount;
                FirstOrder.Add(a);
                return 0;
            }
        }
        public int PushOrderToShop(string account)//更改订单状态为已推送商家，商家未确认,等待商家确认
        {
            lock (First)
            {
                int num = 0;
                if (FirstOrder.Count > 0)
                {
                   
                    for (int i =0 ; i <= FirstOrder.Count - 1; i++)
                    {
                        if (FirstOrder[i].ShopAccount == account)
                        {
                            num++;
                            SecondOrder.Add(FirstOrder[i]);
                            FirstOrder.RemoveAt(i);
                            break;
                        }
                    }
                    if (num > 0)
                    {
                        SqlDB A = new SqlDB();

                        {
                            A.ChangeOrderState("FirstOrder", "SecondOrder", "cuitorder" + account, account);//更改为已推送，未确认
                        }

                    }
                }
                    return num;
            }
        }

        public int PushOrderToShopNum(string account)//更改订单状态为已推送商家，商家未确认,等待商家确认
        {
            lock (First)
            {
                int num = 0;
                if (FirstOrder.Count > 0)
                {

                    for (int i = 0; i <= FirstOrder.Count - 1; i++)
                    {
                        if (FirstOrder[i].ShopAccount == account)
                        {
                            num++;
                            break;
                        }
                    }
                }
                return num;
            }
        }

        public int WaitConfirmedOrderToDelivery(string OrderID, string UserAccount, string ShopAccount)//更改订单状态为商家已确认,等待推送给配送员
        {
            lock(First)
            {
                int num = 0;
                if (SecondOrder.Count > 0)
                {

                    for (int i = SecondOrder.Count - 1; i >= 0; i--)
                    {
                        if (SecondOrder[i].ShopAccount == ShopAccount && SecondOrder[i].OrderID == OrderID && SecondOrder[i].UserAccount == UserAccount)
                        {
                            num++;
                            ThirdOrder.Add(SecondOrder[i]);
                            SecondOrder.RemoveAt(i);
                        }
                    }
                    if (num > 0)
                    {
                        SqlDB A = new SqlDB();

                        {
                            A.RobORConfrimChangeOrderState("SecondOrder", "ThirdOrder", OrderID, UserAccount, ShopAccount);//
                            A.ChangeShopOrderState(ShopAccount, UserAccount, OrderID, "2");
                        }
                    }
                }
                return num;
            }
        }
        public string PushConfirmedOrderToDelivery()//推送给配送员
        {
            lock (First)
            {
                string returnInfo = null;
                int num = 0;
                for (int i = ThirdOrder.Count - 1; i >= 0; i--)
                {
                    returnInfo = returnInfo + "+-+" + ThirdOrder[i].OrderID;
                    num++;
                }
                return returnInfo + "+-+" + num.ToString();
            }
        }

        public int WaitRobedOrderToCook(string OrderID, string UserAccount, string ShopAccount, string DeliveryAccount)//等待取货
        {
            lock (First)
            {
                int num = 0;
                SqlDB A = new SqlDB();
                if (ThirdOrder.Count > 0)
                {

                    for (int i = ThirdOrder.Count - 1; i >= 0; i--)
                    {
                        if (ThirdOrder[i].ShopAccount == ShopAccount && ThirdOrder[i].OrderID == OrderID && ThirdOrder[i].UserAccount == UserAccount)
                        {
                            num++;
                            ThirdOrder.RemoveAt(i);
                            A.Delete("ThirdOrder", "Deliveryorder" + DeliveryAccount, OrderID, UserAccount, ShopAccount);//
                            A.ChangeShopOrderState(ShopAccount, UserAccount, OrderID, "4");
                        }
                    }
                }
                return num;
            }
        }
    }
}