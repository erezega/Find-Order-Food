<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" EnableEventValidation="false" Inherits="WebApplication3.pages.main.main" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title>Main Window</title>

    <link href="main.css" rel="stylesheet" type="text/css" />

    <link href="../../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../../css/bootstrap-theme.min.css" rel="stylesheet"/>

    <link href="../../css/metro.min.css" rel="stylesheet"/>
    <link href="../../css/metro-icons.min.css" rel="stylesheet"/>
    <link href="../../css/metro-colors.min.css" rel="stylesheet"/>
    <link href="../../css/metro-responsive.min.css" rel="stylesheet"/>
    <link href="../../css/metro-rtl.min.css" rel="stylesheet"/>
    <link href="../../css/metro-schemes.min.css" rel="stylesheet"/>
  
    <script src="../../js/jquery-3.1.1.min.js"></script>
    <script src="../../js/bootstrap.min.js"></script>
    <script src="../../js/metro.min.js"></script>

</head>
<body>
<form runat="server">
    <nav id="navbar" class="navbar navbar-inverse navbar-custom">
        <div class="container-fluid">
            <ul class="nav navbar-nav navbar-left">
                <li class="dropdown">
                    <a href="#" data-toggle="dropdown"><span class="mif mif-user" style="font-size: 17px"></span> התחבר<strong class="caret"></strong></a>
                    <div class="dropdown-menu">
                        <div id="signInMenu">
                            <input type="text" id="cookUserName" runat="server" name="cookUserName" dir="rtl" placeholder="שם משתמש" style="margin-bottom: 15px;" />
                            <input type="password" id="cookPassword" runat="server" name="cookPassword"  dir="rtl" placeholder="סיסמא" style="margin-bottom: 15px;" />
                            <input type="submit" id="signInButton" class="button" value="כניסה" runat="server" />
                        </div>
                    </div>
                </li>
                <li class="dropdown"><a href="#" id="signUpButton" runat="server"><span class="mif mif-user-plus" style="font-size: 16px"></span> הרשמה</a><ul class="dropdown-menu"></ul></li>
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li><a href="#">אודות</a></li>
                <li><a href="#">כתבו לנו</a></li>
                <li><a class="navbar-brand" href="../main/main.aspx"><img class="websiteImg" src="../../img/logo.PNG" alt='' /></a></li>
            </ul>
        </div>
    </nav>

    <br/><br/><br/><br/>

    <div class="input-group">
        <span class="input-group-btn">
        <button id="searchButton" class="btn btn-secondary" runat="server"><span class="glyphicon glyphicon-search"/></button>
        </span>
        <input type="text" id="searchBar" runat="server" dir="rtl" placeholder="חפש מנה..."/>
    </div>

    <br/>

    <div id="dishesObjects" style="visibility:hidden" runat="server">

    </div>

    <div class="container">
          <!-- Modal -->
          <div class="modal fade" id="myModal" role="dialog">
            <div class="modal-dialog">
    
              <!-- Modal content-->
              <div class="modal-content">
                <div class="modal-header">
                  <button type="button" class="close" data-dismiss="modal">&times;</button>
                  <h4 id="orderDate" class="modal-title"></h4>
                </div>
                <div class="modal-body">
                    <div id="orderSummary" class="media">
                        <div class="media-body" dir="rtl">
                            <label>שם מנה: <span id ="summaryDishName"></span><span> (מזהה: </span><span id ="summaryDishId"></span><span>)</span></label><br/>
                            <label>בשלן: <span id ="summaryDishCookName"></span><span> (מזהה: </span><span id ="summaryDishCookId"></span><span>)</span></label><br/>
                            <label>כמות: <span id ="summaryDishQuantity"></span></label><br/>
                        </div>
                        <div class="media-middle media-left order_li">
                            <img id ="summaryDishImg" src="" alt="..." />
                        </div>
                        <br />
                        <div dir="rtl">
                            <label>אופן אספקה: </label>   
                               <input type="radio" id="shippingRadioOn" class="input-inline" name="optradio" checked="checked" style="margin-top: -3px; margin-right: 10px;width:20px;" /><label>שליח</label>
                               <input type="radio" id="shippingRadioOff" class="input-inline" name="optradio" style="margin-top: -3px; margin-right: 10px;width:20px;" /><label>איסוף עצמי</label>
                        </div>
                        <br />
                        <div dir="rtl">
                            <label style="width:120px">שם מלא:</label> 
                            <textarea id="customerName" rows="1" cols="50"></textarea>
                        </div>
                        <br />
                        <div dir="rtl">
                            <label style="width:120px">כתובת מייל:</label>
                            <textarea id="customerMail" rows="1" cols="50"></textarea>
                        </div>
                        <br />
                        <div dir="rtl">
                            <label style="width:120px">כתובת:</label>
                            <textarea id="customerAddress" rows="1" cols="50"></textarea>
                        </div>
                        <br />
                            <div dir="rtl">
                            <label style="width:120px">הערות לבשלן:</label>
                            <textarea id="notesToCook" rows="4" cols="50"></textarea>
                        </div>
                        <br />
                        <label>מחיר הזמנה: <span id ="summaryDishTotalPrice"></span><span> ש'ח</span></label><br />
                        <label>דמי משלוח: <span id ="shippingPriceLabel"></span><span> ש'ח</span></label><br />
                        <label style="font-size:20px;" >סה"כ לתשלום: <span id ="summaryOrderTotalPrice"></span><span> ש'ח</span></label>
                </div>
                <div class="modal-footer">
                  <button type='button' id="continueToPayButton" class="continueToPayButton" style="width:120px; height: 40px; margin-right:40%;">!הזמן</button>
                </div>
              </div>
            </div>
          </div> 
    </div>

</div>
</form>
</body>
</html>

<script>
    // Increment Button Action
    $(".incr-btn").on("click", function (e) {
        var $button = $(this);
        var oldValue = $button.parent().find('.quantity').val();
        var oldPriceValue = $button.parent().find('.dishTotalPrice').text();
        var priceForOne = $button.parent().parent().parent().find('.dishPrice').text();

        $button.parent().find('.incr-btn[data-action="decrease"]').removeClass('inactive');
        if ($button.data('action') == "increase") {
            var newVal = parseFloat(oldValue) + 1;
            var newPriceVal = parseFloat(oldPriceValue) + parseFloat(priceForOne);
            $button.parent().find('.dishTotalPrice').text(newPriceVal);
        } else {
            // Don't allow decrementing below 1
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
                var newPriceVal = parseFloat(oldPriceValue) - parseFloat(priceForOne);
                $button.parent().find('.dishTotalPrice').text(newPriceVal);
            } else {
                newVal = 0;
                $button.addClass('inactive');
            }
        }
        $button.parent().find('.quantity').val(newVal);
        e.preventDefault();
    });

    $(".checkIfCookAvailable").on("click", function (e) {
        var $button = $(this);
        var buttonText = $button.attr('id');

        //var text1 = $("#media_1").children().eq(0).text();
        //var text2 = $("#media_1").children().children().children().text();

        var index = buttonText.substr(buttonText.indexOf('_') + 1, 1);

        var now = new Date();

        var month = now.getMonth() + 1;
        var day = now.getDate();

        var orderDate = (('' + day).length < 2 ? '0' : '') + day + '/' +
             (('' + month).length < 2 ? '0' : '') + month + '/' +
             now.getFullYear();
            
        var dishName = $("#dishName_" + index).text();
        var dishId = $("#dishId_" + index).text();
        var dishCookName = $("#dishCookName_" + index).text();
        var dishCookId = $("#dishCookId_" + index).text();
        var dishQuantity = $("#dishQuantity_" + index).val();
        if (parseInt(dishQuantity) == 0)
        {
            alert("לא נבחרה כמות של מנה");
            return;
        }
        var dishImgValue = $("#dishImg_" + index).attr('src');
        var dishTotalPrice = $("#dishTotalPrice_" + index).text();

        var parametersToSend = {};
        parametersToSend.dishId = $.trim(dishId);
        parametersToSend.dishCookId = $.trim(dishCookId);
        parametersToSend.dishQuantity = $.trim(dishQuantity);

        $.ajax({
            type: "POST",
            url: "main.aspx/CheckIfCookAvailable",
            async: false,
            data: JSON.stringify(parametersToSend),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                if (r.d == true) {
                    $("#orderDate").text(orderDate);
                    $("#summaryDishName").text(dishName);
                    $("#summaryDishId").text(dishId);
                    $("#summaryDishCookName").text(dishCookName);
                    $("#summaryDishCookId").text(dishCookId);
                    $("#summaryDishQuantity").text(dishQuantity);
                    $("#summaryDishImg").attr("src", dishImgValue);
                    $("#summaryDishTotalPrice").text(dishTotalPrice);

                    // shippingOn is always checked by default
                    var $radioButtons = $('input:radio[id=shippingRadioOn]');
                    if ($radioButtons.is(':checked') === false) {
                        $radioButtons.filter('[id=shippingRadioOn]').prop('checked', true);
                    }

                    $("#shippingPriceLabel").text('15');
                    $("#summaryOrderTotalPrice").text(parseInt($("#summaryDishTotalPrice").text()) + parseInt($("#shippingPriceLabel").text()));
                    $button.attr('data-target', '#myModal');
                }
                else {
                    alert("הבשלן לא יכול לקבל הזמנה זו - אנא נסו להזמין כמות קטנה יותר או נסו במועד מאוחר יותר");
                    return;
                }
            }
        });
    });

    $('#shippingRadioOn').click(function () {
        $("#shippingPriceLabel").text('15');
        $("#summaryOrderTotalPrice").text(parseInt($("#summaryDishTotalPrice").text()) + parseInt($("#shippingPriceLabel").text()));
    });

    $('#shippingRadioOff').click(function () {
        $("#shippingPriceLabel").text('0');
        $("#summaryOrderTotalPrice").text(parseInt($("#summaryDishTotalPrice").text()) + parseInt($("#shippingPriceLabel").text()));
    });

    $("#continueToPayButton").on("click", function (e) {
        var dishId = $("#summaryDishId").text();
        var dishCookId = $("#summaryDishCookId").text();
        var dishQuantity = $("#summaryDishQuantity").text();
        var customerName = $("#customerName").val();
        var customerMail = $("#customerMail").val();
        var customerAddress = $("#customerAddress").val();
        var notesToCook = $("#notesToCook").val();

        var parametersToSend = {};
        parametersToSend.dishId = $.trim(dishId);
        parametersToSend.dishCookId = $.trim(dishCookId);
        parametersToSend.dishQuantity = $.trim(dishQuantity);
        parametersToSend.customerName = $.trim(customerName);
        parametersToSend.customerMail = $.trim(customerMail);
        parametersToSend.customerAddress = $.trim(customerAddress);
        parametersToSend.notesToCook = $.trim(notesToCook);

        $.ajax({
            type: "POST",
            url: "main.aspx/ContinueToPayButton",
            async: false,
            data: JSON.stringify(parametersToSend),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                if (r.d == true) {
                    alert("ההזמנה נוספה בהצלחה");
                    $('.websiteImg').click();
                }
                else {
                    alert("קיימת בעיה בהוספת ההזמנה - אנא נסו במועד מאוחר יותר");                
                    return;
                }
            }
        });
    });
</script>