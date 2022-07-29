using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolTakeout.Tool;
using System.IO;

namespace SchoolTakeout.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult ShopManage()
        {
            return View();
        }

        public int ChangeShopState(string State,string Account)
        {
            SqlDB A = new SqlDB();
            if (A.ChangeShopState(State, Account))
            {
                return 1;
            }
            else return 0;

        }

        public int ChangeWechatState(string State, string Account)
        {
            SqlDB A = new SqlDB();
            if (A.ChangeWechatState(State, Account))
            {
                return 1;
            }
            else return 0;
        }

        public ActionResult FoodManage()
        {
            string account = Request["account"];
            ViewData["Account"] =  account;
            return View();
        }

        public int ChangeFoodState(string State, string Account,string ID)
        {
            SqlDB A = new SqlDB();
            if (A.ChangeFoodState(State, Account,ID))
            {
                return 1;
            }
            else return 0;
        }

        public ActionResult CreateFood()
        {
            string account = Request["account"];
            ViewData["Account"] = account;
            return View();
        }
        public int PresentFood()
        {
            HttpPostedFileBase img = Request.Files["Img"];
            string name = Request.Form["Name"];
            string type = Request.Form["Type"];
            string introduce = Request.Form["Introduce"];
            string price = Request.Form["Price"];
            string packingPrice = Request.Form["PackingPrice"];
            string foodState = Request.Form["FoodState"];
            if (foodState == "上架")
            {
                foodState = "1";
            }
            else foodState = "0";
            string account = Request.Form["Account"]; ;
            // string path = @"E:\Image";
            string path = Server.MapPath("/Resource/Food/Image");
            string ShopImgPath = path + @"\" + account;
            if (!Directory.Exists(ShopImgPath))
                Directory.CreateDirectory(ShopImgPath);
            //保存文件的物理路径
            int ImgNum = Directory.GetFileSystemEntries(ShopImgPath).Length;
            string saveFile = ShopImgPath + @"\" + ImgNum + Path.GetExtension(img.FileName);
            string oppositePath = "/Resource/Food/Image" + @"/" + account + @"/" + ImgNum + Path.GetExtension(img.FileName);
            //保存图片到服务器
            try
            {
                img.SaveAs(saveFile);
                SqlDB A = new SqlDB();
                if (A.InsertFood(account, name, type, price, packingPrice, foodState, introduce, oppositePath))
                    return 1;
                else return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}