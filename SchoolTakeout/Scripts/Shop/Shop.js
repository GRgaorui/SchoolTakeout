
function AddOrder(OrderNumber, OrderTime, OrderType, OrderDetail,
    CustomerAddress, CustomerNumber,CustomerName, OrderID,UserAccount,ShopAccount) {
    var btnDisplay, btnCSSClass;
    switch (OrderType) {
        case "烹":
            btnDisplay = "</div><div>正在制作中</div>";
            btnCSSClass = "btn5";
            break;
        case "送":
            btnDisplay = "</div><div>正在配送中</div>";
            btnCSSClass = "btn2";
            break;
        case "完":
            btnDisplay = "</div>";
            btnCSSClass = "btn3";
            break;
        case "新":
            btnDisplay = "<div class=\"OCancel Obtn\" onclick=\"CancelOrder(this.parentNode.parentNode); \">取消</div><div class=\"OAccept Obtn\" onclick=\"AcceptOrder(this.parentNode.parentNode," + OrderID + "," + UserAccount + "," + ShopAccount + ");\">接受</div></div></div>";
            btnCSSClass = "btn4";
            break;
    }

    var text = "<div class=\"col-xs-12 \"><table style=\"width:100%;\" onclick=\"ToggleDetail(this)\" ><tr><td class=\"ONum\"><div>#"
        + OrderNumber + "</div></td><td class=\"OInfo\"><div>订单号:"
        + OrderID + "</div></td><td class=\"BtnInfo "
        + btnCSSClass + "\"><div>"
        + OrderType + "</div></td></tr></table><div id=\"Odetail\"><div class=\"Food\">商品信息:"
        + OrderDetail + "</div><div class=\"Cadds\">用户地址:"
        + CustomerAddress + "</div><div class=\"Cname\">用户姓名:"
        + CustomerName + "</div><div class=\"CNum\">联系电话:"
        + CustomerNumber + "</div><div class=\"CTime\">下单时间:"
        + OrderTime + "</div>"
        + btnDisplay;
    $("#OrderFormR").append(text);
}
function ToggleDetail(obj) {
    //obj = $(obj).parent();
    if (obj.isOpen !== true) {
        $(obj).next("#Odetail").attr("style", "display:block;margin-bottom:60px;");
        obj.isOpen = true;
    }
    else {
        $(obj).next("#Odetail").attr("style", " ");
        obj.isOpen = false;
    }
}
function AcceptOrder(obj,OrderID,UserAccount,ShopAccount)
{
    RemoveOder(obj);
    alert("You Accept the order");
    $.ajax({
        type: "post",
        url: "/ShopManage/Confirm",
        data: { "OrderID": OrderID, "UserAccount": UserAccount, "ShopAccount": ShopAccount},
        dataType: 'JSON',
        async: false,
        success: function (result) {
            if (result == 1) {
                alert("已推送配送员");
            }
            else if (result == 0) {
                alert("已推送配送员失败");
            }
        }
    });
}
function CancelOrder(obj) {
    RemoveOder(obj);
    alert("You Cancel the order");
}
function AddOrdertest(tp)
{
    AddOrder('1', "2017-10-25 14:00", tp,"SuperChikenHourse x1 $52.50", "1栋1001", "小明明明", "15366486648");
}
function RemoveOder(obj) {
    $(obj).remove();
}
//ShopMnage
function ToggleOnSale(obj)//测试函数，发布删除
    {
        var d = $(obj).children("div");
        if (d.attr("class") === "BtnOn"){
            d.attr("class", "BtnOff");
        }
        else {
            d.attr("class", "BtnOn");
        }
        
    }
function EditDetail(obj)
{
    if (obj.dataset.foodid != null) {
        foodEditDetail.style = "display:block;";
        $(foodEditDetail).find(".Fname").val($(obj).children("div").eq(0).text());
        $(foodEditDetail).find(".Fprice").val($(obj).children("div").eq(1).text().replace("单价：¥", ""));
        $(foodEditDetail).find(".FID").val(obj.dataset.foodid);
    }
    else
    {
        foodEditDetail.style = "display:block;";
    }
}
function AddNewFood(Name,id,price,pakprice,pic,fclass,isonSale)
{
    if (isonSale) {
        isonSale = 'BtnOn';
    }
    else
    {
        isonSale = 'BtnOff';
    }
    var text = "<div class=\"col-xs-12 \"><table style=\"width:100%;\"><tr><td class=\"WFoodPic\" style=\"background-image:url(" + pic + ");background-size:100% 100%;\"></td><td class=\"Introduce\" onclick=\"EditDetail(this)\" data-foodid=\"" + id + "\"><div>" + Name + "</div><div>单价：¥" + price + "</div></td><td class=\"FoodToggleBtn\" onclick=\"ToggleOnSale(this)\"><div class=\"" + isonSale + "\"><div></div></div></td></tr></table></div> "
    $(FoodEditM).append(text);
}
//FoodEdit
function Switch() {
    if (shopstate) {
        shopstate = 0;
    } else {
        shopstate = 1;
    }
    $.ajax({
        type: "post",
        url: "/ShopManage/ChangeShopState",
        data: { "State": shopstate, "Account": account },
        dataType: 'JSON',
        async: false,
        success: function (result) {
            //alert(result);
        }
    });
    window.location.reload();
}