using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication3.pages.main
{
    public partial class main : System.Web.UI.Page
    {
        #region Private Members
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static int _prepareTime = 0;
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
            }

            this.searchButton.ServerClick += new System.EventHandler(this.SearchButton_Clicked);
            this.signUpButton.ServerClick += new System.EventHandler(this.SignUpButton_Clicked);
            this.signInButton.ServerClick += new System.EventHandler(this.SignInButton_Clicked);
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion InitializeComponent

        #endregion Ctor

        #region Public Methods

        #region SignInButton_Clicked
        public void SignInButton_Clicked(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string cookUserName = this.cookUserName.Value;
                string cookPassword = this.cookPassword.Value;
                string isUserExist = "";

                MySqlCommand cmd = new MySqlCommand("SELECT EXISTS(SELECT * FROM `database`.cooks WHERE cookUserName = '" + cookUserName + "' AND cookPassword = '" + cookPassword + "')", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    isUserExist = reader.GetString(0);
                }
                reader.Close();

                if (isUserExist == "1")
                {
                    string cookId = "";

                    cmd = new MySqlCommand("SELECT cookId FROM `database`.cooks WHERE cookUserName = '" + cookUserName + "' AND cookPassword = '" + cookPassword + "';", conn);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cookId = reader.GetString(0);
                    }
                    reader.Close();
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                    Session["cookUserName"] = cookUserName;
                    Session["cookId"] = cookId;
                    Response.Redirect("../cookPage/cookPage.aspx");
                    //Server.Transfer("../cookPage/cookPage.aspx"); // If we dont want to change url address
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('שם משתמש / סיסמא שגויים. אנא נסה שנית');", true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("main.aspx: signInButton_Clicked(): " + ex);
            }
        }
        #endregion SignInButton_Clicked

        #region SignUpButton_Clicked
        public void SignUpButton_Clicked(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('בקרוב!');", true);
        }
        #endregion SignUpButton_Clicked

        #region SearchButton_Clicked
        public void SearchButton_Clicked(object sender, EventArgs e)
        {
            string searchVal = searchBar.Value;

            if (String.IsNullOrEmpty(searchVal))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('שדה החיפוש ריק');", true);
            }
            else
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                MySqlConnection conn2 = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    MySqlCommand cmd = new MySqlCommand("SELECT dishId, dishName, dishStyle, dishPrice, dishImage, dishRanking, cookId  FROM `database`.dishes WHERE dishName LIKE " + "'%" + searchVal + "%'" + " OR " + "dishStyle LIKE " + "'%" + searchVal + "%'" + ";", conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    MySqlDataReader reader2;
                    string dishId = "";
                    string dishName = "";
                    string dishCookId = "";
                    string dishCookName = "";
                    string dishStyle = "";
                    string dishPrice = "";
                    byte[] dishImgByteArray;
                    string dishImgBase64 = "";
                    int dishRanking = 0;

                    string newDishObject = "";
                    dishesObjects.InnerHtml = "";
                    dishesObjects.Attributes["style"] = "visibility: visible";
                    int dishIndex = 0;

                    while (reader.Read())
                    {
                        dishIndex++;
                        dishId = reader.GetString(0).ToString();
                        dishName = reader.GetString(1).ToString();
                        dishCookId = reader.GetString(6).ToString();

                        if (conn2.State == ConnectionState.Closed)
                        {
                            conn2.Open();
                        }

                        cmd = new MySqlCommand("SELECT cookName FROM `database`.cooks WHERE cookId=" + dishCookId + ";", conn2);
                        reader2 = cmd.ExecuteReader();
                        while (reader2.Read())
                        {
                            dishCookName = reader2.GetString(0).ToString();
                        }
                        reader2.Close();
                        if (conn2.State == ConnectionState.Open)
                        {
                            conn2.Close();
                        }

                        dishStyle = reader.GetString(2).ToString();
                        dishPrice = reader.GetString(3).ToString();
                        dishImgByteArray = (byte[])reader.GetValue(4);
                        dishImgBase64 = Convert.ToBase64String(dishImgByteArray, 0, dishImgByteArray.Length);
                        dishRanking = reader.GetInt32(5);

                        newDishObject = @"<div id= 'media_" + dishIndex.ToString() + @"'class='media'>
                                        <div class='rating' data-role='rating' data-static='true' data-size='medium' data-score-title='דירוג: ' data-value='" + dishRanking + @"'></div>
                                        <div class='media-body' dir='rtl'>
                                            <label>שם מנה: <span id = 'dishName_" + dishIndex.ToString() + @"'> " + dishName + @"</span><span> (מזהה: </span><span id = 'dishId_" + dishIndex.ToString() + @"'> " + dishId + @"</span><span>)</span></label><br/>
                                            <label>בשלן: <span  id = 'dishCookName_" + dishIndex.ToString() + @"'>" + dishCookName + @"</span><span> (מזהה: </span><span id = 'dishCookId_" + dishIndex.ToString() + @"'> " + dishCookId + @"</span><span>)</span></label><br/>
                                            <label>סגנון בישול: <span id = 'dishStyle_" + dishIndex.ToString() + @"'>" + dishStyle + @"</span></label><br/>
                                            <label id = 'dishPrice_" + dishIndex.ToString() + @"'>מחיר: <span class = 'dishPrice'>" + dishPrice + @"</span> ש'ח</label>
                                        </div>
                                        <div class='media-middle media-left order_li'>
                                            <img id = 'dishImg_" + dishIndex.ToString() + @"'class='dishImg' src='data: image / png; base64, " + dishImgBase64 + @"'alt='...' />
                                        </div>
                                        <div class='container'>
	                                        <div class='count-input space-bottom'>
                                                <a class='incr-btn' data-action='decrease' href='#'>–</a>
                                                <input id = 'dishQuantity_" + dishIndex.ToString() + @"'class='quantity' type='text' name='quantity' value='0' readonly='readonly'/>
                                                <a class='incr-btn' data-action='increase' href='#'>&plus;</a>
                                                <label>:כמות</label>
                                                <label>ש'ח </label>
                                                <label id = 'dishTotalPrice_" + dishIndex.ToString() + @"'class='dishTotalPrice' readonly='readonly'>0</label>
                                                <div class='priceForOne' style='visibility:hidden'>100</div>
                                            </div>
                                        </div>
                                        <button type='button' id = 'checkIfCookAvailable_" + dishIndex.ToString() + @"'class='checkIfCookAvailable' data-toggle='modal' style='color: white;background-color: #595959;'>המשך לתשלום</button>
                                        <hr />
                                    </div>";

                        dishesObjects.Style.Add("max-height", "380px");
                        dishesObjects.InnerHtml += newDishObject;
                    }
                    reader.Close();
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("main.aspx: searchButton_Clicked(): " + ex);
                }
            }
        }
        #endregion SearchButton_Clicked

        #region CheckIfCookAvailable
        [System.Web.Services.WebMethod]
        public static bool CheckIfCookAvailable(string dishId, string dishCookId, string dishQuantity)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                List<string> dishIdsList = new List<string>();
                List<Step[]> dishesList = new List<Step[]>();
                List<int> quantityOfEachDishList = new List<int>();

                // Add current dish/dishes details from DB to quantityOfEachDishList and create dishIdsList for next step
                MySqlCommand cmd = new MySqlCommand("SELECT dishId, dishQuantity FROM `database`.orders WHERE cookId=" + dishCookId + ";", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dishIdsList.Add(reader.GetString(0).ToString());
                    quantityOfEachDishList.Add(reader.GetInt32(1));
                }
                reader.Close();

                // Add current dish/dishes details from DB to dishesList and quantityOfEachDishList
                for (int i = 0; i < dishIdsList.Count; i++)
                {
                    cmd = new MySqlCommand("SELECT stepNum, stepName, stepTime, stepType, isNeedAttension, dependOnSteps FROM `database`.steps WHERE dishId=" + dishIdsList[i] + " ORDER BY stepNum;", conn);
                    reader = cmd.ExecuteReader();
                    List<Step> dish = new List<Step>();
                    while (reader.Read())
                    {
                        int stepNum = reader.GetInt32(0);
                        string stepName = reader.GetString(1);
                        int stepTime = reader.GetInt32(2);
                        string stepType = reader.GetString(3);
                        bool isNeedAttension = bool.Parse(reader.GetString(4));
                        List<int> dependOnSages = reader.GetString(5).Split(',').Select(Int32.Parse).ToList();

                        Step step = new Step(stepNum, stepName, stepTime, stepType, isNeedAttension, dependOnSages, false, false);
                        dish.Add(step);
                    }
                    dishesList.Add(dish.ToArray());
                    reader.Close();
                }

                // Add dish details from web to dishesList and quantityOfEachDishList
                cmd = new MySqlCommand("SELECT stepNum, stepName, stepTime, stepType, isNeedAttension, dependOnSteps FROM `database`.steps WHERE dishId=" + dishId + " ORDER BY stepNum;", conn);
                reader = cmd.ExecuteReader();
                List<Step> dishFromWeb = new List<Step>();
                while (reader.Read())
                {
                    int stepNum = reader.GetInt32(0);
                    string stepName = reader.GetString(1);
                    int stepTime = reader.GetInt32(2);
                    string stepType = reader.GetString(3);
                    bool isNeedAttension = bool.Parse(reader.GetString(4));
                    List<int> dependOnSages = reader.GetString(5).Split(',').Select(Int32.Parse).ToList();

                    Step step = new Step(stepNum, stepName, stepTime, stepType, isNeedAttension, dependOnSages, false, false);
                    dishFromWeb.Add(step);
                }
                quantityOfEachDishList.Add(Int32.Parse(dishQuantity));
                dishesList.Add(dishFromWeb.ToArray());
                reader.Close();

                // Calculate prepareTime for dish/dishes from DB + dish from web
                int prepareTime = CheckPrepareOrderTIme(dishesList, quantityOfEachDishList);

                // Check if cook available ( need to be: cookDayTimeLimit > prepareTime )
                cmd = new MySqlCommand("SELECT cookDayTimeLimit FROM `database`.cooks WHERE cookId=" + dishCookId + ";", conn);
                reader = cmd.ExecuteReader();
                int cookDayTimeLimit = 0;
                while (reader.Read())
                {
                    cookDayTimeLimit = reader.GetInt32(0);
                }
                reader.Close();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                if (cookDayTimeLimit > prepareTime)
                {
                    _prepareTime = prepareTime;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("main.aspx: CheckIfCookAvailable(): " + ex);
                return false;
            }
        }
        #endregion CheckIfCookAvailable

        #region ContinueToPayButton
        [System.Web.Services.WebMethod]
        public static bool ContinueToPayButton(string dishId, string dishCookId, string dishQuantity, string customerName, string customerMail, string customerAddress, string notesToCook)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Find current cookDayTimeLimit 
                MySqlCommand cmd = new MySqlCommand("SELECT cookDayTimeLimit FROM `database`.cooks WHERE cookId= " + dishCookId + ";", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                int cookDayTimeLimit = 0;

                while (reader.Read())
                {
                    cookDayTimeLimit = Int32.Parse(reader.GetString(0));
                }
                reader.Close();

                // Find last orderId
                cmd = new MySqlCommand("SELECT COALESCE(MAX(orderId),0) FROM `database`.orders;", conn);
                reader = cmd.ExecuteReader();
                int maxOrderIdVal = 0;

                while (reader.Read())
                {
                    var x = reader.GetString(0);
                    maxOrderIdVal = Int32.Parse(reader.GetString(0));
                }
                reader.Close();

                // Insert new Order to orders table
                cmd = new MySqlCommand("INSERT INTO `database`.orders (`orderId`, `dishQuantity`, `customerName`, `customerMail`, `customerAddress`, `notesToCook`, `dishId`, `cookId`) VALUES('" + (maxOrderIdVal + 1).ToString() + "', '" + dishQuantity + "', '" + customerName + "', '" + customerMail + "', '" + customerAddress + "', '" + notesToCook + "' ,'" + dishId + "', '" + dishCookId + "');", conn);
                reader = cmd.ExecuteReader();
                reader.Close();

                // Update cookDaytimeLeft in cooks table
                int newCookDayTimeLeft = cookDayTimeLimit - _prepareTime;
                cmd = new MySqlCommand("UPDATE `database`.cooks SET `cookDaytimeLeft`= '" + newCookDayTimeLeft + "' WHERE `cookId`= '" + dishCookId + "';", conn);
                reader = cmd.ExecuteReader();
                reader.Close();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("main.aspx: ContinueToPayButton(): " + ex);
                return false;
            }
        }
        #endregion ContinueToPayButton

        #endregion Public Methods

        #region Private Methods

        #region CheckPrepareOrderTIme
        private static int CheckPrepareOrderTIme(List<Step[]> dishesList, List<int> quantityOfEachDishList)
        {
            //For Any Dish, Calculate Stage Time For Any Stage By Linear/Exponential
            for (int i = 0; i < dishesList.Count(); i++)
            {
                for (int j = 0; j < dishesList[i].Count(); j++)
                {
                    if (dishesList[i][j].StepType == "exponential")
                    {
                        dishesList[i][j].StepTime *= quantityOfEachDishList[i];
                    }
                }
            }

            //For Any Dish, ReCalculate StageNum and Dependencies Of Each Stage
            int stageCount = dishesList[0].Count();
            for (int i = 1; i < dishesList.Count(); i++) // Iterate Over All Dishes
            {
                for (int j = 0; j < dishesList[i].Count(); j++) // Iterate Over All Stages In Each Dish
                {
                    dishesList[i][j].StepNum += stageCount;
                    for (int k = 0; k < dishesList[i][j].DependOnSteps.Count(); k++) // Iterate Over All The Dependencies In Each Stage
                    {
                        if (dishesList[i][j].DependOnSteps[k] != 0)
                        {
                            dishesList[i][j].DependOnSteps[k] += dishesList[i - 1].Count();
                        }
                    }
                }
                stageCount += dishesList[i].Count();
            }

            // Merge All Dishes To 1 Order
            Step[] order = dishesList.SelectMany(a => a).ToArray();

            // Calculate Worst Time
            int worstTime = 0;
            for (int i = 0; i < order.Count(); i++)
            {
                worstTime += order[i].StepTime;
            }

            // Calculate Actual Time
            int prepareTime = 0;
            bool isWorkFinished = false;
            bool isOccupiedByParallelStage = false;
            bool isNeedToChangeOccupationFlag = false;
            for (int i = 0; i < worstTime; i++)
            {
                if (isNeedToChangeOccupationFlag == true)
                {
                    isOccupiedByParallelStage = false;
                }
                if (isWorkFinished)
                {
                    bool isExit = true;
                    for (int j = 0; j < order.Count(); j++)
                    {
                        if (order[j].IsStepFinished == false)
                        {
                            isExit = false;
                            break;
                        }
                    }

                    if (isExit)
                    {
                        prepareTime--;
                        break;
                    }
                }
                prepareTime++;
                for (int j = 0; j < order.Count(); j++)
                {
                    if (order[j].StepTime == 0)
                    {
                        if (order[j].IsStepFinished == false)
                        {
                            order[j].IsInProgress = false;
                            order[j].IsStepFinished = true;
                        }
                        isWorkFinished = true;
                        continue;
                    }
                    else if (order[j].StepTime != 0)
                    {
                        isWorkFinished = false;

                        // Find Dependence Stages (If Stage Can Start)
                        bool isStageCanStartOrContinue = false;
                        int dependenceStage = 0;
                        for (int k = 0; k < order[j].DependOnSteps.Count(); k++)
                        {
                            dependenceStage = order[j].DependOnSteps[k];
                            if ((dependenceStage != 0) && order[order[j].DependOnSteps[k] - 1].IsStepFinished == false)
                            {
                                isStageCanStartOrContinue = false;
                                break;
                            }
                            else
                            {
                                isStageCanStartOrContinue = true;
                            }
                        }

                        // Stage Cannot Start - Not All His Dependencies Has Finished
                        if (isStageCanStartOrContinue == false)
                        {
                            continue;
                        }

                        // Stage Cannot Start
                        if (order[j].IsNeedAttension == true && isOccupiedByParallelStage == true && order[j].IsInProgress == false)
                        {
                            continue;
                        }

                        // Stage Can Continue
                        else if (order[j].IsNeedAttension == true && isOccupiedByParallelStage == true && order[j].IsInProgress == true)
                        {
                            order[j].IsInProgress = true;
                            isOccupiedByParallelStage = true;
                            order[j].StepTime--;
                            if (order[j].StepTime == 0)
                            {
                                isNeedToChangeOccupationFlag = true;
                            }
                            else
                            {
                                isNeedToChangeOccupationFlag = false;
                            }
                            continue;
                        }

                        // Stage Can Start
                        else if (order[j].IsNeedAttension == true && isOccupiedByParallelStage == false)
                        {
                            order[j].IsInProgress = true;
                            isOccupiedByParallelStage = true;
                            order[j].StepTime--;
                            continue;
                        }

                        // Stage Can Start/Continue
                        else if (order[j].IsNeedAttension == false)
                        {
                            order[j].IsInProgress = true;
                            order[j].StepTime--;
                            continue;
                        }
                    }
                }
            }
            return prepareTime;
        }
        #endregion CheckPrepareOrderTIme

        #endregion Private Methods
    }
}