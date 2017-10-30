using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication3.pages.cookPage
{
    public partial class cookPage : System.Web.UI.Page
    {
        #region Private Members
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string _cookId = "";
        #endregion Private Members

        #region Ctor

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion Page_Load

        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }
        #endregion OnInit

        #region InitializeComponent
        private void InitializeComponent()
        {
            if (!Page.IsPostBack)
            {
                cookUsernameLabel.InnerText += " " + Session["cookUserName"].ToString();
                _cookId = Session["cookId"].ToString();
                //cookUsernameLabel.InnerText += " " + "danS";
                //_cookId = "1";
            }

            this.showCookOrdersButton.ServerClick += new System.EventHandler(this.ShowCookOrdersButton_Clicked);
            this.showCookMenuButton.ServerClick += new System.EventHandler(this.ShowCookMenuButton_Clicked);
            this.showCookWorkingDayButton.ServerClick += new System.EventHandler(this.ShowCookWorkingDayButton_Clicked);
            this.deleteCookOrdersButton.ServerClick += new System.EventHandler(this.DeleteCookOrdersButton_Clicked);
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion InitializeComponent

        #endregion Ctor

        #region Public Methods

        #region ShowCookOrdersButton_Clicked
        public void ShowCookOrdersButton_Clicked(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            MySqlConnection conn2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                MySqlCommand cmd = new MySqlCommand("SELECT dishId, dishQuantity, customerName, customerAddress, customerMail FROM `database`.orders WHERE cookId=" + "'" + _cookId + "'" + ";", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                MySqlDataReader reader2;

                string dishId = "";
                string dishName = "";
                byte[] dishImgByteArray;
                string dishImgBase64 = "";
                string dishQuantity = "";
                string customerName = "";
                string customerAddress = "";
                string customerMail = "";

                string newCookOrderObject = "";
                cookObjects.InnerHtml = "";
                cookObjects.Attributes["style"] = "visibility: visible";
                int dishIndex = 0;

                while (reader.Read())
                {
                    dishIndex++;
                    dishId = reader.GetString(0);
                    dishQuantity = reader.GetInt32(1).ToString();
                    customerName = reader.GetString(2);
                    customerAddress = reader.GetString(3);
                    customerMail = reader.GetString(4);

                    if (conn2.State == ConnectionState.Closed)
                    {
                        conn2.Open();
                    }

                    cmd = new MySqlCommand("SELECT dishName, dishImage FROM `database`.dishes WHERE dishId=" + "'" + dishId + "'" + ";", conn2);
                    reader2 = cmd.ExecuteReader();

                    while (reader2.Read())
                    {
                        dishName = reader2.GetString(0);
                        dishImgByteArray = (byte[])reader2.GetValue(1);
                        dishImgBase64 = Convert.ToBase64String(dishImgByteArray, 0, dishImgByteArray.Length);
                    }
                    reader2.Close();

                    if (conn2.State == ConnectionState.Open)
                    {
                        conn2.Close();
                    }

                    newCookOrderObject = @"<div id= 'media_" + dishIndex.ToString() + @"'class='media'>
                                                <div class='media-body' dir='rtl'>
                                                    <label>שם מנה: <span id = 'dishName_" + dishIndex.ToString() + @"'> " + dishName + @"</span><span> (מזהה: </span><span id = 'dishId_" + dishIndex.ToString() + @"'> " + dishId + @"</span><span>)</span></label><br/>
                                                    <label>כמות: <span id = 'dishQuantity_" + dishIndex.ToString() + @"'> " + dishQuantity + @"</span></label><br/>
                                                    <label>שם המזמין: <span id = 'customerName_" + dishIndex.ToString() + @"'> " + customerName + @"</span></label><br/>
                                                    <label>כתובת המזמין: <span id = 'customerAddress" + dishIndex.ToString() + @"'> " + customerAddress + @"</span></label><br/>
                                                    <label>מייל המזמין: <span id = 'customerMail" + dishIndex.ToString() + @"'> " + customerMail + @"</span></label><br/>
                                                </div>
                                                <div class='media-middle media-left order_li'>
                                                    <img id = 'dishImg_" + dishIndex.ToString() + @"'class='dishImg' src='data: image / png; base64, " + dishImgBase64 + @"'alt='...' />
                                                </div>
                                                <hr />
                                            </div>";

                    cookObjects.Style.Add("max-height", "420px");
                    cookObjects.Style.Add("overflow-y", "scroll");
                    cookObjects.InnerHtml += newCookOrderObject;
                }
                reader.Close();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("cookPage.aspx: showCookOrdersButton_Clicked(): " + ex);
            }
        }
        #endregion ShowCookOrdersButton_Clicked

        #region ShowCookMenuButton_Clicked
        public void ShowCookMenuButton_Clicked(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                MySqlCommand cmd = new MySqlCommand("SELECT dishId, dishName, dishStyle, dishPrice, dishImage, dishRanking FROM `database`.dishes WHERE cookId=" + "'" + _cookId + "'" + ";", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                string dishId = "";
                string dishName = "";
                string dishStyle = "";
                string dishPrice = "";
                byte[] dishImgByteArray;
                string dishImgBase64 = "";
                string dishRanking = "";

                string newCookDishObject = "";
                cookObjects.InnerHtml = "";
                cookObjects.Attributes["style"] = "visibility: visible";
                int dishIndex = 0;

                while (reader.Read())
                {
                    dishIndex++;
                    dishId = reader.GetString(0);
                    dishName = reader.GetString(1);
                    dishStyle = reader.GetString(2);
                    dishPrice = reader.GetString(3);
                    dishImgByteArray = (byte[])reader.GetValue(4);
                    dishImgBase64 = Convert.ToBase64String(dishImgByteArray, 0, dishImgByteArray.Length);
                    dishRanking = reader.GetInt32(5).ToString();

                    newCookDishObject = @"<div id= 'media_" + dishIndex.ToString() + @"'class='media'>
                                        <div class='rating' data-role='rating' data-static='true' data-size='medium' data-score-title='דירוג: ' data-value='" + dishRanking + @"'></div>
                                        <div class='media-body' dir='rtl'>
                                            <label>שם מנה: <span id = 'dishName_" + dishIndex.ToString() + @"'> " + dishName + @"</span><span> (מזהה: </span><span id = 'dishId_" + dishIndex.ToString() + @"'> " + dishId + @"</span><span>)</span></label><br/>
                                            <label>סגנון בישול: <span id = 'dishStyle_" + dishIndex.ToString() + @"'>" + dishStyle + @"</span></label><br/>
                                            <label id = 'dishPrice_" + dishIndex.ToString() + @"'>מחיר: <span class = 'dishPrice'>" + dishPrice + @"</span> ש'ח</label>
                                        </div>
                                        <div class='media-middle media-left order_li'>
                                            <img id = 'dishImg_" + dishIndex.ToString() + @"'class='dishImg' src='data: image / png; base64, " + dishImgBase64 + @"'alt='...' />
                                        </div>
                                        <hr />
                                    </div>";

                    cookObjects.Style.Add("max-height", "420px");
                    cookObjects.Style.Add("overflow-y", "scroll");
                    cookObjects.InnerHtml += newCookDishObject;
                }
                reader.Close();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("cookPage.aspx: showCookMenuButton_Clicked(): " + ex);
            }
        }
        #endregion ShowCookMenuButton_Clicked

        #region ShowCookWorkingDayButton_Clicked
        public void ShowCookWorkingDayButton_Clicked(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                MySqlCommand cmd = new MySqlCommand("SELECT cookDayTimeLimit, cookDayTimeLeft FROM `database`.cooks WHERE cookId=" + "'" + _cookId + "'" + ";", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                string cookDayTimeLimit = "";
                string cookDayTimeLeft = "";
                float ratio = 0;

                string newCookWorkingDayObject = "";
                cookObjects.InnerHtml = "";
                cookObjects.Attributes["style"] = "visibility: visible";

                while (reader.Read())
                {
                    cookDayTimeLimit = reader.GetInt32(0).ToString();
                    cookDayTimeLeft = reader.GetInt32(1).ToString();

                    ratio = 100 - ((float.Parse(cookDayTimeLeft) / float.Parse(cookDayTimeLimit)) * 100);

                    newCookWorkingDayObject = @"<br /><br />
                                                <div class='progressBootstrap'>
                                                   <div class='progress-barBootstrap progress-bar-stripedBootstrap activeBootstrap' role='progressbar' aria-valuenow='0' aria-valuemin='0' aria-valuemax='100' style='width:" + ratio + @"%;color:black;'>" + ratio + @"%</div>
                                                </div>
                                                <br />
                                                <label style='margin-left:20%';>(נותרו " + cookDayTimeLeft + @" דקות למכסת יום העבודה (מתוך " + cookDayTimeLimit + @" דקות" + @"</label>
                                                <br /><br /><br />";

                    cookObjects.Style.Add("padding-left", "5px");
                    cookObjects.Style.Add("padding-right", "5px");
                    cookObjects.Style.Add("padding-down", "5px");
                    cookObjects.InnerHtml += newCookWorkingDayObject;
                }
                reader.Close();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("cookPage.aspx: showCookWorkingDayButton_Clicked(): " + ex);
            }
        }
        #endregion ShowCookWorkingDayButton_Clicked

        #region DeleteCookOrdersButton_Clicked
        public void DeleteCookOrdersButton_Clicked(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cookObjects.InnerHtml = "";
                cookObjects.Attributes["style"] = "visibility: hidden";

                MySqlCommand cmd = new MySqlCommand("DELETE FROM `database`.orders WHERE cookId=" + "'" + _cookId + "';", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Close();

                cmd = new MySqlCommand("UPDATE `database`.cooks SET `cookDaytimeLeft`= `cookDaytimeLimit` WHERE `cookId`= '" + _cookId + "';", conn);
                reader = cmd.ExecuteReader();
                reader.Close();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('כל ההזמנות של הבשלן נמחקו');", true);
            }
            catch (Exception ex)
            {
                _logger.Error("cookPage.aspx: DeleteCookOrdersButton_Clicked(): " + ex);
            }
        }
        #endregion DeleteCookOrdersButton_Clicked

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods
    }
}