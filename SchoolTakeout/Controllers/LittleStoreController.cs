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
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Net;
using System.Security.Cryptography;
using System.Threading;

namespace SchoolTakeout.Controllers
{
    public class LittleStoreController : Controller
    {
        static bool acctoke = true;
        static Access_Token acc = new Access_Token();
        // GET: LittleStore
        public ActionResult Shop()
        {
            string gettokenurl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxea100f196527f08e&secret=a1d8f5cc7934bea763d67876db6f988a&code=" + Request["code"] + "&grant_type=authorization_code ";
            //generate http request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(gettokenurl);
            //use GET method to get url's html
            req.Method = "GET";
            //use request to get response
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            //otherwise will return messy code
            //  Encoding htmlEncoding = Encoding.GetEncoding(htmlCharset);
            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
            //read out the returned html
            string respHtml = sr.ReadToEnd();
            //上边为读取json数据，下边就是解析了
            //传说中的反序列化
            //另外，为了方便我在model里新建了一个Access_Token实体
            abc j2 = new JavaScriptSerializer().Deserialize<abc>(respHtml);
            SqlDB A = new SqlDB();
            SqlDataReader read = A.GetUser("123");
            bool NewUsers = true;
            while(read.Read())
            {
                ViewData["Name"] = read[0].ToString();
                ViewData["Address"] = read[1].ToString();
                ViewData["Telephone"] = read[2].ToString();
                ViewData["Account"] = read[3].ToString();
                NewUsers = false;
            }
            A.CloseConnection();
            if (!NewUsers)
            {
                return View();
            }
            else
            {
                ViewData["WeChatOpenid"] = "123";
                return View("Imfo");
            }         
        }

        public class abc
        {
            public string access_token;
            public string expires_in;
            public string refresh_token;
            public string openid;
            public string scope;
        }
        public ActionResult Mine()
        {
            ViewData["UserAccount"] = Request["UserAccount"];
            string UserAccount = Request["UserAccount"];
            SqlDB A = new SqlDB();
            SqlDataReader read = A.GetUserInfo(UserAccount);
            while(read.Read())
            {
                ViewData["UserName"] = read[0];
                ViewData["UserAddress"] = read[1];
                ViewData["UserPhone"] = read[2];
            }
            return View();
        }
        public ActionResult Order()
        {
            ViewData["UserAccount"] = Request["UserAccount"];
            string UserAccount = Request["UserAccount"];
            SqlDB A = new SqlDB();
            SqlDataReader read = A.GetUserInfo(UserAccount);
            while (read.Read())
            {
                ViewData["UserName"] = read[0];
                ViewData["UserAddress"] = read[1];
                ViewData["UserPhone"] = read[2];
            }
            return View();
        }
        public ActionResult NowOrder()
        {
            ViewData["ShopID"] = Request["ShopID"];
            ViewData["ShopName"] = Request["ShopName"];
            ViewData["ShopAddress"] = Request["ShopAddress"];
            ViewData["UserAccount"] = Request["UserAccount"];
            string UserAccount = Request["UserAccount"];
            SqlDB A = new SqlDB();
            SqlDataReader read = A.GetUserInfo(UserAccount);
            while (read.Read())
            {
                ViewData["UserName"] = read[0];
                ViewData["UserAddress"] = read[1];
                ViewData["UserPhone"] = read[2];
            }
            ViewData["FoodMoney"] = Request["FoodMoney"];
            ViewData["PackingMoney"] = Request["PackingMoney"];
            ViewData["DistributionPrice"] = Request["DistributionPrice"];
            ViewData["TotalPrice"] = Request["TotalPrice"];
            ViewData["OrderNumber"] = Request["OrderNumber"];
            ViewData["FoodList"] = Request["FoodList"];
            ViewData["NumList"] = Request["NumList"];
            ViewData["NameList"] = Request["NameList"];
            ViewData["PriceList"] = Request["PriceList"];
            return View();
        }
        public ActionResult Food()
        {
            ViewData["ShopID"] = Request["ShopID"];
            ViewData["ShopName"] = Request["ShopName"];
            ViewData["ShopAddress"] = Request["ShopAddress"];
            ViewData["DistributionPrice"] = Request["DistributionPrice"];
            ViewData["UserAccount"] = Request["UserAccount"];
            string UserAccount = Request["UserAccount"];
            SqlDB A = new SqlDB();
            SqlDataReader read = A.GetUserInfo(UserAccount);
            while (read.Read())
            {
                ViewData["UserName"] = read[0];
                ViewData["UserAddress"] = read[1];
                ViewData["UserPhone"] = read[2];
            }
            return View();
        }

        public ActionResult Imfo()
        {
            return View();
        }
        public string GetTypeFood(string Type, string ShopAccount)
        {
            SqlDB A = new SqlDB();
            string szJson = "";
            SqlDataReader read = A.GetThisTypeFood(ShopAccount, Type);

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
            using (MemoryStream stream = new MemoryStream()) //序列化
            {
                json.WriteObject(stream, show);
                szJson = Encoding.UTF8.GetString(stream.ToArray());

            }
            return szJson;
        }

        public string GetOrderFoodList(string ShopAccount, string FoodIDList, string NumList, string UserAccount, string DistributionPrice )
        {
            string[] foodIDList = Regex.Split(FoodIDList, "WT", RegexOptions.IgnoreCase);
            string[] numList = Regex.Split(NumList, "WT", RegexOptions.IgnoreCase);
            float Foodmomey = 0;
            float Packingmoney = 0;
            float TotalMoney = 0;
            SqlDB A = new SqlDB();
            for (int i = 0; i < foodIDList.Length; i++)
            {
                float[] Price = A.GetFoodPrice(ShopAccount, foodIDList[i]);
                Foodmomey += Price[0]* float.Parse(numList[i]);
                Packingmoney += Price[1] * float.Parse(numList[i]);
            }
            TotalMoney = Foodmomey + Packingmoney + float.Parse(DistributionPrice);
            return Foodmomey.ToString("0.00") + "WT" + Packingmoney.ToString("0.00") + "WT" + TotalMoney.ToString("0.00") + "WT" + UserAccount + DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds.ToString().Replace(".", "");
        }

        public string CreatOrder(string OrderID, string UserAccount, string ShopAccount, string ShopInfo, string FoodInfo, string FoodMoney, string PackingMoney, string DistributionPrice, string TotalPrice, string ClientInfo,string Remark, string RealtimeState, string EndState, string Time)
        {
            SqlDB A = new SqlDB();
            //订单信息插入第一步执行的表
            A.InsertFirstOrder(OrderID, UserAccount, ShopAccount, ShopInfo, FoodInfo, FoodMoney, PackingMoney, DistributionPrice, TotalPrice, ClientInfo,Remark,RealtimeState, "0", Time);
            //订单信息插入用户的表 (未写)
            A.InsertFirstOrder(OrderID, UserAccount, ShopAccount, ShopInfo, FoodInfo, FoodMoney, PackingMoney, DistributionPrice, TotalPrice, ClientInfo, Remark, RealtimeState,"0", Time, "userorder" + UserAccount);
            OrderInfoQueue B = new OrderInfoQueue();
            B.WaitOrderToShop(OrderID, UserAccount, ShopAccount);
            return "1";
        }


        /// <summary>
        /// 定义Token，与微信公共平台上的Token保持一致
        /// </summary>
        private const string Token = "StupidMe";

        /// <summary>
        /// 验证签名，检验是否是从微信服务器上发出的请求
        /// </summary>
        /// <param name="model">请求参数模型 Model</param>
        /// <returns>是否验证通过</returns>
        private bool CheckSignature(Model.FormatModel.WeChatRequestModel model)
        {
            string signature, timestamp, nonce, tempStr;
            //获取请求来的参数
            signature = model.signature;
            timestamp = model.timestamp;
            nonce = model.nonce;
            //创建数组，将 Token, timestamp, nonce 三个参数加入数组
            string[] array = { Token, timestamp, nonce };
            //进行排序
            Array.Sort(array);
            //拼接为一个字符串
            tempStr = String.Join("", array);
            //对字符串进行 SHA1加密
            tempStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tempStr, "SHA1").ToLower();
            //判断signature 是否正确
            if (tempStr.Equals(signature))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void loopacctoken()
        {
            while (true)
            {
                acc = get_acctoken();
                Thread.Sleep((acc.expires_in - 300) * 1000);
            }
        }

        public void Valid(Model.FormatModel.WeChatRequestModel model)
        {
            if(acctoke)
            {
                Thread thread = new Thread(loopacctoken);
                thread.Start();
                acctoke = false;
            }
            creat_mymenu();
            //获取请求来的 echostr 参数
            string echoStr = model.echostr;
            //通过验证
            if (CheckSignature(model))
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    //将随机生成的 echostr 参数 原样输出
                    Response.Write(echoStr);
                    //截止输出流
                    Response.End();
                }
            }
        }
        public Access_Token get_acctoken()
        {
            string gettokenurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxea100f196527f08e&secret=a1d8f5cc7934bea763d67876db6f988a";
            //generate http request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(gettokenurl);
            //use GET method to get url's html
            req.Method = "GET";
            //use request to get response
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            //otherwise will return messy code
            //  Encoding htmlEncoding = Encoding.GetEncoding(htmlCharset);
            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
            //read out the returned html
            string respHtml = sr.ReadToEnd();
            //上边为读取json数据，下边就是解析了
            //传说中的反序列化
            //另外，为了方便我在model里新建了一个Access_Token实体
            Access_Token j2 = new JavaScriptSerializer().Deserialize<Access_Token>(respHtml);
            //acctoken是一个静态变量，全局的就是。
            return j2;

        }


        public class Access_Token
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }

        }


        public string creat_mymenu()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + acc.access_token;
            string data = "{\"button\":[{\"type\":\"view\",\"name\":\"订餐\",\"url\":\"https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxea100f196527f08e&redirect_uri=http%3a%2f%2f120.78.136.154%2fLittleStore%2fShop&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect\"}]}";
            System.Net.HttpWebRequest httpWebRequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
            httpWebRequest.Method = "POST";


            byte[] postBytes = Encoding.UTF8.GetBytes(data);
            //httpWebRequest.ContentType = "text/xml";
            httpWebRequest.ContentType = "application/json; charset=utf-8";//
            httpWebRequest.ContentLength = Encoding.UTF8.GetByteCount(data);//strJson为json字符串
            Stream stream = httpWebRequest.GetRequestStream();
            stream.Write(postBytes, 0, postBytes.Length);
            stream.Close();//发送完毕，接受返回值
            var response = httpWebRequest.GetResponse();

            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);

            String responseString = streamRead.ReadToEnd();
            Write(responseString);
            return responseString;


        }

        public void Write(string text)
        {
            FileStream fs = new FileStream("E:\\log.txt", FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(text);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        public int GetUserImfo(string Name, string Phone, string Address,string Openid)
        {
            try
            {
                SqlDB A = new SqlDB();
                int UesrCount = A.GetUserAccount();
                A.InsertUsers(Name, UesrCount.ToString(), Openid, Address, Phone);
                A.CreateShopORDeliveryORUsersOrder("userorder" + UesrCount);
                ViewData["Account"] = UesrCount;
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public string GetNewName(string UserAccount, string NewName)
        {
            string result;
            SqlDB A = new SqlDB();
            if (A.ChangeUserName(UserAccount, NewName))
            {
                result = "1";
                return result;
            }
            else {
                result = "0";
                return result;
            }
        }
        public string GetNewPhone(string UserAccount, string NewPhone)
        {
            string result;
            SqlDB A = new SqlDB();
            if (A.ChangeUserPhone(UserAccount, NewPhone))
            {
                result = "1";
                return result;
            }
            else
            {
                result = "0";
                return result;
            }
        }
        public string GetNewAddress(string UserAccount, string NewAddress)
        {
            string result;
            SqlDB A = new SqlDB();
            if (A.ChangeUserAddress(UserAccount, NewAddress))
            {
                result = "1";
                return result;
            }
            else
            {
                result = "0";
                return result;
            }
        }
    }
}