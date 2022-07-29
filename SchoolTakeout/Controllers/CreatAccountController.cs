using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolTakeout.Tool;
using System.IO;

namespace SchoolTakeout.Controllers
{
    public class CreatAccountController : Controller
    {
        // GET: CreatAccount
        public ActionResult CreatShopAccount()
        {
            return View();
        }
        public ActionResult CreatDeliveryAccount()
        {
            return View();
        }
        public int creatAccountShop()
        {
            string Name = Request.Form["Name"];
            string Address = Request.Form["Address"];
            string DistributionPrice = Request.Form["DistributionPrice"];
            string Account = Request.Form["Account"];
            string Password = Request.Form["Password"];
            HttpPostedFileBase img = Request.Files["Img"];

            string path = Server.MapPath("/Resource/Shop/Image");
            string ShopImgPath = path + @"\" + Account;
            if (!Directory.Exists(ShopImgPath))
                Directory.CreateDirectory(ShopImgPath);
            //保存文件的物理路径
            int ImgNum = Directory.GetFileSystemEntries(ShopImgPath).Length;
            string saveFile = ShopImgPath + @"\" + ImgNum + Path.GetExtension(img.FileName);
            string oppositePath = "/Resource/Shop/Image" + @"/" + Account + @"/" + ImgNum + Path.GetExtension(img.FileName);
            //保存图片到服务器
            try
            {
                img.SaveAs(saveFile);
                SqlDB A = new SqlDB();
                if (A.InsertShop(Name, Account, Password, Address, oppositePath, DistributionPrice))
                {
                    string text = "cuitfood" + Account;
                    A.CreateShopFood(text);
                    text = "cuitorder" + Account;
                    A.CreateShopORDeliveryORUsersOrder(text);
                    return 1;
                }
                else return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public void creatAccountDelivery(string name, string account, string password)
        {
            SqlDB A = new SqlDB();
            A.InsertDelivery(name, account, password);
            string text = "deliveryorder" + account;
            A.CreateShopORDeliveryORUsersOrder(text);
        }
      
    }
}