<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cookPage.aspx.cs" EnableEventValidation="false" Inherits="WebApplication3.pages.cookPage.cookPage" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <title>Cook Page</title>

    <link href="cookPage.css" rel="stylesheet" type="text/css" />

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
            <li><a id="cookUsernameLabel" runat="server">שלום</a></li>
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li><a href="#">אודות</a></li>
                <li><a href="#">כתבו לנו</a></li>
                <li><a class="navbar-brand" href="../main/main.aspx"><img class="websiteImg" src="../../img/logo.PNG" alt='' /></a></li>
            </ul>
        </div>
    </nav>

    <br/><br/><br/>

    <ul id="menu" class="t-menu">
        <li><a href="#" id="deleteCookOrdersButton" runat="server"><span class="icon mif-bin"></span>איפוס כל ההזמנות שלי</a></li>
        <li id="reviewDropdownToggle"><a href="#" class="dropdown-toggle"><span class="icon mif-bubbles"></span> מה אומרים עלי</a>
            <ul class="dropdown-menu" data-role="dropdown">
                <li><a href="#"><span class="icon mif-table"></span> מנות</a></li>            
                <br />
                <li><a href="#"><span class="icon mif-user"></span>עלי</a></li>
            </ul>
        </li>
        <li><a href="#" id="showCookWorkingDayButton" runat="server"><span class="icon mif-play"></span>יום העבודה שלי</a></li>
        <li><a href="#"><span class="icon mif-plus"></span> הוסף מנה</a></li>
        <li><a href="#"><span class="icon mif-profile"></span> הפרטים שלי</a></li>
        <li><a href="#" id="showCookMenuButton" runat="server"><span class="icon mif-spoon-fork"></span>התפריט שלי</a></li>
        <li><a href="#" id="showCookOrdersButton" runat="server"><span class="icon mif-list2"></span> ההזמנות שלי</a></li>
    </ul>

    <br/><br/><br/>
    <div id="cookObjects" style="visibility:hidden" runat="server">

     </div>

</form>
</body>
</html>
