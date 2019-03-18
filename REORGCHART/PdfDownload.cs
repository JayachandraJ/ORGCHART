using ceTe.DynamicPDF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using REORGCHART.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Configuration;

namespace REORGCHART
{
    public class PdfDownload
    {

        [DataContract]
        public class PageObjectInf
        {
            [DataMember]
            public string Id { get; set; }
            [DataMember]
            public int Col { get; set; }
            [DataMember]
            public int Row { get; set; }
            [DataMember]
            public int CurPageNo { get; set; }
            [DataMember]
            public int PageNo { get; set; }

            public PageObjectInf(string pId, int pCol, int pRow, int pCurPageNo, int pPageNo)
            {
                Id = pId;
                Col = pCol;
                Row = pRow;
                CurPageNo = pCurPageNo;
                PageNo = pPageNo;
            }

        }

        // Org chart Information
        [DataContract]
        public class ObjectInf
        {
            [DataMember]
            public string Id { get; set; }
            [DataMember]
            public string Title { get; set; }
            [DataMember]
            public string PId { get; set; }
            [DataMember]
            public string Level { get; set; }
            [DataMember]
            public int Row { get; set; }
            [DataMember]
            public int Col { get; set; }
            [DataMember]
            public int Width { get; set; }
            [DataMember]
            public int Height { get; set; }
            [DataMember]
            public int Owidth { get; set; }
            [DataMember]
            public int Oheight { get; set; }
            [DataMember]
            public string NextLevelFlag { get; set; }
            [DataMember]
            public string GrayColourFlag { get; set; }
            [DataMember]
            public string DottedLineFlag { get; set; }
            [DataMember]
            public string ShowFullBox { get; set; }
            [DataMember]
            public string Language { get; set; }
            [DataMember]
            public string SortNo { get; set; }
            [DataMember]
            public string PositionFlag { get; set; }
            [DataMember]
            public string ColorFlag { get; set; }
            [DataMember]
            public string BackColor { get; set; }
            [DataMember]
            public string Flag { get; set; }
            [DataMember]
            public string PDFLevel { get; set; }
            [DataMember]
            public string GDDBID { get; set; }

            public ObjectInf()
            {
            }

            public ObjectInf(string pId, string pTitle, string pPId,
                             string pLevel, int pRow, int pCol,
                             int pWidth, int pHeight, int pOwidth, int pOheight,
                             string pNLF, string pGCF, string pDLF, string pSFB,
                             string pLN, string pSortNo, string pPositionFlag,
                             string pColorFlag, string pBackColor,
                             string pFlag, string pPDFLevel, string pGDDBID)
            {
                Id = pId;
                Title = pTitle;
                PId = pPId;
                Level = pLevel;
                Row = pRow;
                Col = pCol;
                Width = pWidth;
                Height = pHeight;
                Owidth = pOwidth;
                Oheight = pOheight;
                NextLevelFlag = pNLF;
                GrayColourFlag = pGCF;
                DottedLineFlag = pDLF;
                ShowFullBox = pSFB;
                Language = pLN;
                SortNo = pSortNo;
                PositionFlag = pPositionFlag;
                ColorFlag = pColorFlag;
                BackColor = pBackColor;
                Flag = pFlag;
                PDFLevel = pPDFLevel;
                GDDBID = pGDDBID;
            }
        }

        [DataContract]
        public class AllObjectInf
        {
            [DataMember]
            public string Id { get; set; }
            [DataMember]
            public string OprId { get; set; }
            [DataMember]
            public string LevelInfo { get; set; }
            [DataMember]
            public string LevelFlag { get; set; }
            [DataMember]
            public int Row { get; set; }
            [DataMember]
            public int Col { get; set; }
            [DataMember]
            public int Height { get; set; }
            [DataMember]
            public int Page { get; set; }
            [DataMember]
            public int ColIndex { get; set; }
            [DataMember]
            public string LevelType { get; set; }
            [DataMember]
            public int ItemCount { get; set; }
            [DataMember]
            public string LevelEnd { get; set; }

            public AllObjectInf(string pId, string pOprId, string pLevelInfo, string pLevelFlag,
                                int pRow, int pCol, int pHeight, int pPage,
                                int pColIndex, string pLevelType, int pItemCount, string pLevelEnd)
            {
                Id = pId;
                OprId = pOprId;
                LevelInfo = pLevelInfo;
                LevelFlag = pLevelFlag;
                Row = pRow;
                Col = pCol;
                Height = pHeight;
                Page = pPage;
                ColIndex = pColIndex;
                LevelType = pLevelType;
                ItemCount = pItemCount;
                LevelEnd = pLevelEnd;
            }
        }

        [DataContract]
        public class ORG_CONFIG_INFO
        {
            [DataMember]
            public string LEVEL_ID { get; set; }
            [DataMember]
            public string BOX_HEIGHT { get; set; }
            [DataMember]
            public string TEMPLATE_URL { get; set; }
            [DataMember]
            public string VIEW_NAME { get; set; }
            [DataMember]
            public string VIEW_TYPE { get; set; }
            [DataMember]
            public string BOX_FILE { get; set; }
            [DataMember]
            public string VIEW_DEFAULT { get; set; }
            [DataMember]
            public string LINE_COLOR { get; set; }
            [DataMember]
            public string LINE_WIDTH { get; set; }

            public ORG_CONFIG_INFO(string json)
            {
                JToken jUser = JObject.Parse(json.Substring(1, json.Length - 2));
                LEVEL_ID = (string)jUser["LEVEL_ID"];
                BOX_HEIGHT = (string)jUser["BOX_HEIGHT"];
                TEMPLATE_URL = (string)jUser["TEMPLATE_URL"];
                VIEW_NAME = (string)jUser["VIEW_NAME"];
                VIEW_TYPE = (string)jUser["VIEW_TYPE"];
                BOX_FILE = (string)jUser["BOX_FILE"];
                VIEW_DEFAULT = (string)jUser["VIEW_DEFAULT"];
                LINE_COLOR = (string)jUser["LINE_COLOR"];
                LINE_WIDTH = (string)jUser["LINE_WIDTH"];
            }

            public ORG_CONFIG_INFO(string pLEVEL_ID, string pBOX_HEIGHT, string pTEMPLATE_URL,
                                   string pVIEW_NAME, string pVIEW_TYPE, string pBOX_FILE,
                                   string pVIEW_DEFAULT, string pLINE_COLOR, string pLINE_WIDTH)
            {
                LEVEL_ID = pLEVEL_ID;
                BOX_HEIGHT = pBOX_HEIGHT;
                TEMPLATE_URL = pTEMPLATE_URL;
                VIEW_NAME = pVIEW_NAME;
                VIEW_TYPE = pVIEW_TYPE;
                BOX_FILE = pBOX_FILE;
                VIEW_DEFAULT = pVIEW_DEFAULT;
                LINE_COLOR = pLINE_COLOR;
                LINE_WIDTH = pLINE_WIDTH;
            }
        }

        [DataContract]
        public class LEVEL_CONFIG_INFO
        {
            [DataMember]
            public string ID { get; set; }
            [DataMember]
            public string FIELD_CAPTION { get; set; }
            [DataMember]
            public string FIELD_NAME { get; set; }
            [DataMember]
            public string FIELD_ROW { get; set; }
            [DataMember]
            public string FIELD_ROW_TYPE { get; set; }
            [DataMember]
            public string FIELD_COL { get; set; }
            [DataMember]
            public string FIELD_COL_TYPE { get; set; }
            [DataMember]
            public string WRAP { get; set; }
            [DataMember]
            public string FONT_NAME { get; set; }
            [DataMember]
            public string FONT_SIZE { get; set; }
            [DataMember]
            public string FONT_COLOR { get; set; }
            [DataMember]
            public string FONT_STYLE { get; set; }
            [DataMember]
            public string FONT_FLOAT { get; set; }
            [DataMember]
            public string ACTIVE_IND { get; set; }
            [DataMember]
            public string TABLE_IND { get; set; }
            [DataMember]
            public string FIELD_WIDTH { get; set; }
            [DataMember]
            public string ADJUSTMENT { get; set; }
            [DataMember]
            public string SAMPLE_DATA { get; set; }


            public LEVEL_CONFIG_INFO(string pID, string pFIELD_CAPTION, string pFIELD_NAME,
                                     string pFIELD_ROW, string pFIELD_ROW_TYPE,
                                     string pFIELD_COL, string pFIELD_COL_TYPE,
                                     string pWRAP,
                                     string pFONT_NAME, string pFONT_SIZE, string pFONT_COLOR, string pFONT_STYLE, string pFONT_FLOAT,
                                     string pACTIVE_IND, string pTABLE_IND, string pFIELD_WIDTH, string pADJUSTMENT, string pSAMPLE_DATA)
            {
                ID = pID;
                FIELD_CAPTION = pFIELD_CAPTION;
                FIELD_NAME = pFIELD_NAME;
                FIELD_ROW = pFIELD_ROW;
                FIELD_ROW_TYPE = pFIELD_ROW_TYPE;
                FIELD_COL = pFIELD_COL;
                FIELD_COL_TYPE = pFIELD_COL_TYPE;
                WRAP = pWRAP;
                FONT_NAME = pFONT_NAME;
                FONT_SIZE = pFONT_SIZE;
                FONT_COLOR = pFONT_COLOR;
                FONT_STYLE = pFONT_STYLE;
                FONT_FLOAT = pFONT_FLOAT;
                ACTIVE_IND = pACTIVE_IND;
                TABLE_IND = pTABLE_IND;
                FIELD_WIDTH = pFIELD_WIDTH;
                ADJUSTMENT = pADJUSTMENT;
                SAMPLE_DATA = pSAMPLE_DATA;
            }
        }

        [DataContract]
        public class RECT_OBJ
        {
            [DataMember]
            public int Col { get; set; }
            [DataMember]
            public int Row { get; set; }
            [DataMember]
            public int Width { get; set; }
            [DataMember]
            public int Height { get; set; }
        }



        public class LevelInfo
        {
            string LevelUpto = "", Height = "", TemplateURL = "", strOutput = "HTML", sParentLevelNo = "", sNextLevelNo = "", sAllPDF = "N";
            string[] Suppress = new string[10];
            string[] LevelInf, LevelIds;
            List<int> PageSizeNos = new List<int>();
            int CurPage = 0, MaxPage = 0, PDFPage = 0, Original_Height = 100, Original_Height_10 = 0, Adjustment_Height = 0, FieldCount = 11, Output_Height = 901, iTotalPage = 0;
            ceTe.DynamicPDF.Document MyDocument = null;
            ceTe.DynamicPDF.Page[] MyPage = new ceTe.DynamicPDF.Page[100000];
            ceTe.DynamicPDF.Page[] MyAllPage = new ceTe.DynamicPDF.Page[100000];
            ceTe.DynamicPDF.PageElements.Image MyImage = null;
            ceTe.DynamicPDF.PageElements.Rectangle MyRect = null;
            ceTe.DynamicPDF.PageElements.Rectangle MyLabelRect = null;
            ceTe.DynamicPDF.PageElements.Line MyLine = null;
            ceTe.DynamicPDF.PageElements.Label MyLabel = null;
            ceTe.DynamicPDF.PageElements.Circle MyCircle = null;
            ceTe.DynamicPDF.PageElements.Link MyLink = null;

            ObjectInf PreviousObj = null;
            Bitmap BmpURL = null, BmpUpArrow = null, BmpDnArrow = null;

            List<PageObjectInf> thePageObjectInf = new List<PageObjectInf>();

            List<ObjectInf> lstObjLevel0 = new List<ObjectInf>();
            List<ObjectInf> lstObjLevel1 = new List<ObjectInf>();
            List<ObjectInf> lstObjLevel2 = new List<ObjectInf>();
            List<ObjectInf> lstObjLevel = new List<ObjectInf>();

            // Places the IDs in the List for Creating All Level PDF and PPT
            List<string> lstID = new List<string>();
            List<string> lstLevel = new List<string>();
            List<string> pageLevel = new List<string>();
            List<string> lstPageNo = new List<string>();


            // Variable used when BRD object created.
            RECT_OBJ RectPPT = new RECT_OBJ();

            // Create font and brush for Connector.
            System.Drawing.Font drawFont = new System.Drawing.Font("Calibri", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            SolidBrush drawBrush = new SolidBrush(System.Drawing.Color.Black);

            // Create font and brush for Label.
            System.Drawing.Font drawFontText = new System.Drawing.Font("Calibri", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            SolidBrush drawBrushText = new SolidBrush(System.Drawing.Color.Black);

            // Create point for upper-left corner of drawing.
            PointF drawPoint = new PointF(150.0F, 150.0F);

            // Black pen to draw line
            Pen blackPen = new Pen(System.Drawing.Color.DarkGray, 1);

            Bitmap[] ImageOut = new Bitmap[10000];
            Graphics[] GraphicImg = new Graphics[10000];

            Bitmap[] ImagePIC = new Bitmap[10000];
            Graphics[] GraphicPIC = new Graphics[10000];

            string OPR_LEVEL_ID = WebConfigurationManager.AppSettings["OPR_LEVEL_ID"];
            string OPR_PARENT_ID = WebConfigurationManager.AppSettings["OPR_PARENT_ID"];
            string OPR_SEARCH_FIELD = WebConfigurationManager.AppSettings["OPR_SEARCH_FIELD"];
            string LGL_LEVEL_ID = WebConfigurationManager.AppSettings["LGL_LEVEL_ID"];
            string LGL_PARENT_ID = WebConfigurationManager.AppSettings["LGL_PARENT_ID"];
            string SUPPRESS_FIELDS = WebConfigurationManager.AppSettings["SUPPRESS_FIELDS"];

            public string Level { get; set; }
            public string PreviousLevel { get; set; }
            public string View { get; set; }
            public string FlagFM { get; set; }
            public string Country { get; set; }
            public string LineColor { get; set; }
            public string LineWidth { get; set; }
            public string LevelFlag { get; set; }
            public string LevelDate { get; set; }
            public string Language { get; set; }
            public int LevelCount { get; set; }
            public string TableHTML { get; set; }
            public string JsonFieldWidth { get; set; }
            public string JsonFieldInfo { get; set; }

            DataTable dtFieldInformation = null, dtFieldActive = null;

            // Constructor
            public LevelInfo(string LI, string PLI, string Vtype, string CountryName)
            {
                Level = LI;
                PreviousLevel = PLI;
                View = Vtype;
                Country = CountryName;
                LevelFlag = "N";
                LevelDate = "";
                LevelCount = 6;
                Language = "EN";

                Common csobj = new Common();
                dtFieldInformation = csobj.SQLReturnDataTable("SELECT DISTINCT * FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + HttpContext.Current.Session["VIEW"].ToString() + "'");
                dtFieldActive = csobj.SQLReturnDataTable("SELECT DISTINCT  * FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + HttpContext.Current.Session["VIEW"].ToString() + "' AND ACTIVE_IND='Y'");
            }

            // Constructor
            public LevelInfo(string LI, string PLI, string Vtype, string CountryName, string LvlFlag, string LvlDate, int LvlCount, string Lang, string FM)
            {
                Level = LI;
                PreviousLevel = PLI;
                View = Vtype;
                Country = CountryName;
                LevelFlag = LvlFlag;
                LevelDate = LvlDate;
                LevelCount = LvlCount;
                Language = Lang;
                FlagFM = FM;

                Common csobj = new Common();
                dtFieldInformation = csobj.SQLReturnDataTable("SELECT * FROM REORG_LEVEL_CONFIG_INFO WHERE VIEW_ID='VIEW_DEFAULT'");
                dtFieldActive = csobj.SQLReturnDataTable("SELECT * FROM REORG_LEVEL_CONFIG_INFO WHERE VIEW_ID='VIEW_DEFAULT' AND ACTIVE_IND='Y'");
            }

            // Constructor
            public LevelInfo()
            {
                Common csobj = new Common();
                dtFieldInformation = csobj.SQLReturnDataTable("SELECT * FROM REORG_LEVEL_CONFIG_INFO WHERE VIEW_ID='VIEW_DEFAULT'");
                dtFieldActive = csobj.SQLReturnDataTable("SELECT * FROM REORG_LEVEL_CONFIG_INFO WHERE VIEW_ID='VIEW_DEFAULT'");
                LevelFlag = "N";
                LevelDate = "";
                LevelCount = 6;
            }

            // Check for ID in ID Collection
            public bool CheckIDExistence(string ID, string IDs)
            {
                string[] Arr_IDS = IDs.Split(';');

                if (Arr_IDS.Count() >= 1)
                {
                    for (int Idx = 0; Idx <= Arr_IDS.Length - 1; Idx++)
                    {
                        if (ID == Arr_IDS[Idx]) return true;
                    }
                }

                return false;
            }

            // Gets the Position ID
            public string getID(string IdInfo)
            {
                string OperationalId = "", SearchName = "";

                Common csobj = new Common();
                DataTable dtLevel = csobj.SQLReturnDataTable("SELECT TOP 1 " + OPR_LEVEL_ID + ", " + OPR_SEARCH_FIELD + " FROM LEVEL_INFO WHERE " + OPR_SEARCH_FIELD + " LIKE '%" + IdInfo + "%'");
                if (dtLevel.Rows.Count >= 1)
                {
                    OperationalId = dtLevel.Rows[0][OPR_LEVEL_ID].ToString();
                    SearchName = dtLevel.Rows[0][OPR_SEARCH_FIELD].ToString();
                }
                else OperationalId = "NODATA";

                return OperationalId;
            }

            // Distinct Menu Information
            public string MenuInfo()
            {
                // Show View
                string LastRefresh = "";

                string ViewInfo = "", OperationalId = "", SearchName = "", GROUP_ID = "", USER_GROUP = "", UID = HttpContext.Current.Session["USERID"].ToString();
                string ViewName = "VIEW_DEFAULT", DisplayLevel = "Show upto level one", DisplayBoxes = "Four boxes";
                if (HttpContext.Current.Session["VIEW"] != null)
                    ViewName = HttpContext.Current.Session["VIEW"].ToString();

                Common csobj = new Common();
                DataTable dtLevel = csobj.SQLReturnDataTable("SELECT TOP 1 " + OPR_LEVEL_ID + ", " + OPR_SEARCH_FIELD + ",REFRESHDT FROM LEVEL_INFO WHERE " + OPR_PARENT_ID + "='-1'");
                if (dtLevel.Rows.Count >= 1)
                {
                    OperationalId = dtLevel.Rows[0][OPR_LEVEL_ID].ToString();
                    SearchName = dtLevel.Rows[0][OPR_SEARCH_FIELD].ToString();
                    LastRefresh = Convert.ToDateTime(dtLevel.Rows[0]["REFRESHDT"]).AddDays(1).ToString("dd-MM-yyyy");

                }



                DataTable dtVI = csobj.SQLReturnDataTable("SELECT DISTINCT VIEW_ID, GROUP_IDS, VIEW_PUBLIC FROM VIEW_INFO " +
                                                                    "WHERE ((VIEW_PUBLIC='N' AND UID='" + UID + "') OR UID='DEFAULT')");
                ViewInfo += "<ul class='custommainmenu'>";
                ViewInfo += "<label><b>View Template</b></label>";
                ViewInfo += "<select class='country' id='selViewInfo'  name='selViewInfo'>";
                if (dtVI.Rows.Count >= 1)
                {
                    foreach (DataRow itemVI in dtVI.Rows)
                    {
                        ViewInfo += "<option value='" + itemVI["VIEW_ID"].ToString() + "' selected='selected'>" + itemVI["VIEW_ID"].ToString() + "</option>";
                    }
                }

                // Gets the User View information
                DataTable dtUG = csobj.SQLReturnDataTable("SELECT DISTINCT GID, USER_GRP_IDS FROM USER_GROUP");
                if (dtUG.Rows.Count >= 1)
                {
                    foreach (DataRow itemUG in dtUG.Rows)
                    {
                        GROUP_ID = itemUG["GID"].ToString();
                        USER_GROUP = itemUG["USER_GRP_IDS"].ToString();

                        if (CheckIDExistence(UID, USER_GROUP))
                        {
                            dtVI = csobj.SQLReturnDataTable("SELECT DISTINCT VIEW_ID, GROUP_IDS, VIEW_PUBLIC FROM VIEW_INFO WHERE VIEW_PUBLIC='Y' AND UID<>'DEFAULT'");
                            if (dtVI.Rows.Count >= 1)
                            {
                                foreach (DataRow itemVI in dtVI.Rows)
                                {
                                    if (CheckIDExistence(GROUP_ID, itemVI["GROUP_IDS"].ToString()))
                                    {
                                        ViewInfo += "<option value='" + itemVI["VIEW_ID"].ToString() + "'>" + itemVI["VIEW_ID"].ToString() + "</option>";
                                    }
                                }
                            }
                        }
                    }
                }
                ViewInfo += "</select>";

                // Language
                string Language = "<label><b>Language</b></label>", LanguageName = "EN";
                Language += "<select class='language' id='selLanguageInfo'  name='selLanguageInfo'>";

                DataTable dtLanguage = csobj.SQLReturnDataTable("SELECT LANGUAGE_ID, LANGUAGE_NAME FROM LANGUAGE_INFO");
                if (dtLanguage.Rows.Count >= 1)
                {
                    if (dtLanguage.Rows.Count >= 1)
                    {
                        foreach (DataRow drLanguage in dtLanguage.Rows)
                        {
                            Language += "<option value='" + drLanguage["LANGUAGE_ID"].ToString() + "'>" + drLanguage["LANGUAGE_NAME"].ToString() + "</option>";
                        }
                    }
                }
                Language += "</select>";

                // Legal View
                int CountryCount = 0;
                string Country = "<select class='country' id='selCountryInfo'  name='selCountryInfo' onchange='SelectSubMenu(this)'>", CountryName = "";
                DataTable dtCountry = csobj.SQLReturnDataTable("SELECT COUNTRY_NAME FROM LEGAL_COUNTRY_INFO ORDER BY COUNTRY_NAME");
                if (dtCountry.Rows.Count >= 1)
                {
                    if (dtCountry.Rows.Count >= 1)
                    {
                        foreach (DataRow drCountry in dtCountry.Rows)
                        {
                            if (CountryCount == 0) CountryName = "";
                            Country += "<option value='" + drCountry["COUNTRY_NAME"].ToString() + "'>" + drCountry["COUNTRY_NAME"].ToString() + "</option>";
                            CountryCount++;
                        }
                    }
                }
                Country += "</select>";


                // Operation View
                string OprString = "<ul  class='custommainmenu'>";
                OprString += "<li class='search'>";
                OprString += "<input id='txtSearchField' name='txtSearchField' type='text' runat='server' value='" + SearchName + "' style='width:100%' />";
                OprString += "<input id='txtSearchInfo' name='txtSearchInfo' type='text' runat='server' value='" + OperationalId + "' style='Display:none' />";
                OprString += "</li>";
                OprString += "<li class='caption'>Press F1 key for search[to select Position ID] and press enter key to get Operational chart.</li>";
                OprString += "</ul>";

                // Search 
                string Search = "<ul  class='custommainmenu'><li class='caption'>Shows the Org chart using search window</li></ul>";

                // Date Range
                //DateTime fixtureDate = DateTime.Parse(DateTime.Now.ToString(), new CultureInfo("en-US"));
                DateTime fixtureDate = DateTime.Parse(HttpContext.Current.Session["LEVEL_DATE"].ToString(), new CultureInfo("en-GB"));
                string DateRange = "<ul class='custommainmenu'>";
                DateRange += "<li class='search'>";
                DateRange += "<div><input id='txtDateInfo' name='txtDateInfo' type='text' value='" + fixtureDate.ToString("dd-MM-yyyy") + "' onchange=\"$('#txtDate').val(this.value);ShowDateChange();\" style='width:100%' /></div>";
                DateRange += "</li>";
                DateRange += "</ul>";

                // Functional Manager
                string FunctionalManager = "<label><b>Functional Reporting Line</b></label>";
                FunctionalManager += "<ul  class='custommainmenu'>";
                FunctionalManager += "<li class='submenu'><input type='radio' id='rdoFMShow' name='FunctionalManager' value='Show' checked='checked' style='display:inline;'/><label class='labelInf' for='rdoFMShow' >Show</label><li>";
                FunctionalManager += "<li class='submenu'><input type='radio' id='rdoFMHide' name='FunctionalManager' value='Hide' style='display:inline;'/><label class='labelInf' for='rdoFMHide'>Hide</label></li>";
                FunctionalManager += "</ul>";

                // Level Upto
                string DisplayUpto = "<label><b>Display Upto</b></label>";
                DisplayUpto += "<ul  class='custommainmenu'>";
                DisplayUpto += "<li class='submenu'><input type='radio' id='rdoLevelOne' name='DisplayUpto' value='Show upto level one' style='display:inline;'/><label class='labelInf' for='rdoLevelOne' >Show upto level one</label><li>";
                DisplayUpto += "<li class='submenu'><input type='radio' id='rdoLevelTwo' name='DisplayUpto' value='Show upto level two' style='display:inline;'/><label class='labelInf' for='rdoLevelTwo'>Show upto level two</label></li>";
                DisplayUpto += "</ul>";

                // Level Change
                string DisplayChange = "<label><b>Display Changes</b></label>";
                DisplayChange += "<ul  class='custommainmenu'>";
                DisplayChange += "<li class='submenu'><input type='radio' id='rdoFourBoxes' name='DisplayBoxes' value='Four boxes' style='display:inline;'/><label class='labelInf' for='rdoFourBoxes' >Four boxes</label></li>";
                DisplayChange += "<li class='submenu'><input type='radio' id='rdoSixBoxes' name='DisplayBoxes' value='Six boxes' style='display:inline;'/><label class='labelInf' for='rdoSixBoxes' >Six boxes</label></li>";
                DisplayChange += "</ul>";

                // Settings
                string Settings = "<ul  class='custommainmenu'>";
                Settings += "<li class='caption'>To change configuration settings of different views</li>";
                Settings += "</ul>";

                // Export 
                string Export = "<ul  class='custommainmenu'>";
                Export += "<li class='submenu'><a href='javascript:void(0);'><span onclick=\"SelectSubMenu(this)\">Current Level to PDF</span></a></li>";
                Export += "<li class='submenu'><a href='javascript:void(0);'><span onclick=\"SelectSubMenu(this)\">All Levels to PDF</span></a></li>";
                Export += "<li class='submenu'><a href='javascript:void(0);'><span onclick=\"SelectSubMenu(this)\">Current Level to PPT</span></a></li>";
                Export += "<li class='submenu'><a href='javascript:void(0);'><span onclick=\"SelectSubMenu(this)\">All Levels to PPT</span></a></li>";
                Export += "</ul>";

                // Chart Table
                string ChartTable = "<ul  class='custommainmenu'>";
                ChartTable += "<li class='caption'>Shows the Org chart table for the current views</li>";
                ChartTable += "</ul>";

                Dictionary<string, string> MenuInfo = new Dictionary<string, string>
            {
                { "ViewInfo", ViewInfo }, 
                { "ViewName", ViewName }, 
                { "LanguageInfo", Language },
                { "LanguageName", LanguageName },
                { "LegalInfo", Country },
                { "CountryName", CountryName },
                { "OperationalId", OperationalId },
                { "OprInfo", OprString },
                { "SearchInfo", Search },
                { "FunctionalManager", FunctionalManager },
                { "FM", "" },
                { "DateRange", DateRange },
                { "DisplayUpto", DisplayUpto },
                { "DisplayLevel", DisplayLevel},
                { "DisplayChange", DisplayChange },
                { "DisplayBoxes", DisplayBoxes },
                { "SettingInfo", Settings },
                { "ExportInfo", Export },
                { "ChartTable", ChartTable },
                { "LastRefresh", LastRefresh }
                
            };

                return JsonConvert.SerializeObject(MenuInfo, Formatting.Indented);
            }

            // Distinct View Information
            public string ViewInfo()
            {
                string VID = "", GROUP_ID = "", USER_GROUP = "", UID = HttpContext.Current.Session["USERID"].ToString();

                Common csobj = new Common();
                DataTable dtVI = csobj.SQLReturnDataTable("SELECT DISTINCT VIEW_ID, GROUP_IDS, VIEW_PUBLIC FROM VIEW_INFO " +
                                                                    "WHERE ((VIEW_PUBLIC='N' AND UID='" + UID + "') OR UID='DEFAULT')");
                if (dtVI.Rows.Count >= 1)
                {
                    foreach (DataRow item in dtVI.Rows)
                    {
                        if (HttpContext.Current.Session["VIEW"].ToString() == item["VIEW_ID"].ToString())
                            VID += "<option value='" + item["VIEW_ID"].ToString() + "' selected>" + item["VIEW_ID"].ToString() + "</option>";
                        else
                            VID += "<option value='" + item["VIEW_ID"].ToString() + "'>" + item["VIEW_ID"].ToString() + "</option>";
                    }
                }

                // Gets the User View information
                DataTable dtUG = csobj.SQLReturnDataTable("SELECT DISTINCT GID, USER_GRP_IDS FROM USER_GROUP");
                if (dtUG.Rows.Count >= 1)
                {
                    foreach (DataRow itemUG in dtUG.Rows)
                    {
                        GROUP_ID = itemUG["GID"].ToString();
                        USER_GROUP = itemUG["USER_GRP_IDS"].ToString();

                        if (CheckIDExistence(UID, USER_GROUP))
                        {
                            dtVI = csobj.SQLReturnDataTable("SELECT DISTINCT VIEW_ID, GROUP_IDS, VIEW_PUBLIC FROM VIEW_INFO WHERE VIEW_PUBLIC='Y' AND UID<>'DEFAULT'");
                            if (dtVI.Rows.Count >= 1)
                            {
                                foreach (DataRow itemVI in dtVI.Rows)
                                {
                                    if (CheckIDExistence(GROUP_ID, itemVI["GROUP_IDS"].ToString()))
                                    {
                                        VID += "<option value='" + itemVI["VIEW_ID"].ToString() + "'>" + itemVI["VIEW_ID"].ToString() + "</option>";
                                    }
                                }
                            }
                        }
                    }
                }

                return VID;
            }

            // Deletes the Config information
            public string DeleteConfigInfo(string View)
            {
                SqlTransaction transaction = null;
                try
                {
                    Common csobj = new Common();
                    using (SqlConnection SqlCon = new SqlConnection(csobj.getConnStr()))
                    {
                        SqlCon.Open();

                        // Start a local transaction.
                        transaction = SqlCon.BeginTransaction("OrgChartTransaction");  // Begins Dashboard transaction

                        // LEVEL_CONFIG_INFO table
                        string Query = "DELETE FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + View + "'";
                        csobj.ExecuteTransactionQuery(Query, transaction, SqlCon);

                        // ORG_CONFIG_INFO table
                        Query = "DELETE FROM ORG_CONFIG_INFO WHERE VIEW_ID='" + View + "' AND " +
                                    "(FIELD_NAME IN ('LEVEL', 'HEIGHT', 'TEMPLATE', 'LINE_COLOR', 'LINE_WIDTH'))";
                        csobj.ExecuteTransactionQuery(Query, transaction, SqlCon);

                        // VIEW_INFO table
                        Query = "DELETE FROM VIEW_INFO WHERE VIEW_ID='" + View + "'";
                        csobj.ExecuteTransactionQuery(Query, transaction, SqlCon);

                        // Attempt to commit the transaction.
                        transaction.Commit();

                        return "Success";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction. 
                    try
                    {
                        if (transaction != null) transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred 
                        // on the server that would cause the rollback to fail, such as 
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }

                return "Fail";

            }

            // Save Config Information
            public string SaveConfigInfo(string ConfigFI, string ConfigLVL)
            {
                SqlTransaction transaction = null;
                try
                {
                    List<LEVEL_CONFIG_INFO> LstConfigFI = (List<LEVEL_CONFIG_INFO>)JsonConvert.DeserializeObject(ConfigFI, typeof(List<LEVEL_CONFIG_INFO>));
                    ORG_CONFIG_INFO CfgLVL = new ORG_CONFIG_INFO(ConfigLVL);

                    Common csobj = new Common();
                    using (SqlConnection SqlCon = new SqlConnection(csobj.getConnStr()))
                    {
                        SqlCon.Open();

                        // Start a local transaction.
                        transaction = SqlCon.BeginTransaction("OrgChartTransaction");  // Begins Dashboard transaction

                        // LEVEL_CONFIG_INFO table
                        string[] CONFIG_INFO = SUPPRESS_FIELDS.Split(',');
                        string SUP_FIELDS = "";
                        for (int Idx = 0; Idx <= CONFIG_INFO.Length - 1; Idx++)
                            SUP_FIELDS += ",\'" + CONFIG_INFO[Idx] + "\'";
                        string Query = "DELETE FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + CfgLVL.VIEW_NAME + "' AND FIELD_NAME NOT IN (" + SUP_FIELDS.Substring(1) + ")";
                        csobj.ExecuteTransactionQuery(Query, transaction, SqlCon);

                        Query = "";
                        foreach (LEVEL_CONFIG_INFO Obj in LstConfigFI)
                        {
                            Query += "INSERT INTO LEVEL_CONFIG_INFO (" +
                                            "VIEW_ID, FIELD_CAPTION, FIELD_NAME, FIELD_ROW, FIELD_ROW_TYPE, " +
                                            "FIELD_COL, FIELD_COL_TYPE, WRAP, FONT_NAME, FONT_SIZE, FONT_COLOR, " +
                                            "FONT_STYLE, FONT_FLOAT, ACTIVE_IND, TABLE_IND, FIELD_WIDTH, ADJUSTMENT, SAMPLE_DATA " +
                                            ") VALUES ('" + CfgLVL.VIEW_NAME + "', '" +
                                                          Obj.FIELD_CAPTION + "', '" +
                                                          Obj.FIELD_NAME + "', '" +
                                                          Obj.FIELD_ROW + "', '" +
                                                          Obj.FIELD_ROW_TYPE + "', '" +
                                                          Obj.FIELD_COL + "', '" +
                                                          Obj.FIELD_COL_TYPE + "', '" +
                                                          Obj.WRAP + "', '" +
                                                          Obj.FONT_NAME + "', '" +
                                                          Obj.FONT_SIZE + "', '" +
                                                          Obj.FONT_COLOR + "', '" +
                                                          Obj.FONT_STYLE + "', '" +
                                                          Obj.FONT_FLOAT + "', '" +
                                                          Obj.ACTIVE_IND + "', '" +
                                                          Obj.TABLE_IND + "', '" +
                                                          Obj.FIELD_WIDTH + "', '" +
                                                          Obj.ADJUSTMENT + "', '" +
                                                          Obj.SAMPLE_DATA + "')";
                        }
                        SqlCommand SqlCmd = new SqlCommand(Query, SqlCon);
                        SqlCmd.Transaction = transaction;
                        SqlCmd.ExecuteNonQuery();

                        // ORG_CONFIG_INFO table
                        Query = "DELETE FROM ORG_CONFIG_INFO WHERE VIEW_ID='" + CfgLVL.VIEW_NAME + "' AND " +
                                    "(FIELD_NAME IN ('LEVEL', 'HEIGHT', 'TEMPLATE', 'LINECOLOR', 'LINEWIDTH'))";
                        csobj.ExecuteTransactionQuery(Query, transaction, SqlCon);

                        Query = "INSERT INTO ORG_CONFIG_INFO(VIEW_ID, FIELD_NAME, FIELD_VALUE) " +
                                     "SELECT '" + CfgLVL.VIEW_NAME + "', 'LEVEL','" + CfgLVL.LEVEL_ID + "' UNION ALL " +
                                     "SELECT '" + CfgLVL.VIEW_NAME + "', 'HEIGHT','" + CfgLVL.BOX_HEIGHT + "' UNION ALL " +
                                     "SELECT '" + CfgLVL.VIEW_NAME + "', 'LINECOLOR','" + CfgLVL.LINE_COLOR + "' UNION ALL " +
                                     "SELECT '" + CfgLVL.VIEW_NAME + "', 'LINEWIDTH','" + CfgLVL.LINE_WIDTH + "' UNION ALL " +
                                     "SELECT '" + CfgLVL.VIEW_NAME + "', 'TEMPLATE','" + CfgLVL.TEMPLATE_URL + "'";

                        SqlCmd = new SqlCommand(Query, SqlCon);
                        SqlCmd.Transaction = transaction;
                        SqlCmd.ExecuteNonQuery();

                        // VIEW Info table
                        Query = "DELETE FROM VIEW_INFO WHERE VIEW_ID='" + CfgLVL.VIEW_NAME + "'";
                        csobj.ExecuteTransactionQuery(Query, transaction, SqlCon);

                        Query = "INSERT INTO VIEW_INFO(VIEW_ID, UID, GROUP_IDS, VIEW_PUBLIC) " +
                                     "SELECT '" + CfgLVL.VIEW_NAME + "', '" + HttpContext.Current.Session["USERID"].ToString() + "', '" + HttpContext.Current.Session["GROUP"].ToString() + "', '" + CfgLVL.VIEW_TYPE + "'";
                        csobj.ExecuteTransactionQuery(Query, transaction, SqlCon);

                        // USER_INFO table[Updating Default View]
                        string[] sViewDefault = CfgLVL.VIEW_DEFAULT.Split(':');
                        Query = "UPDATE USER_INFO SET VIEW_DEFAULT='" + sViewDefault[0] + "' WHERE UID='" + HttpContext.Current.Session["USERID"].ToString() + "'";
                        csobj.ExecuteTransactionQuery(Query, transaction, SqlCon);

                        SqlCmd = new SqlCommand(Query, SqlCon);
                        SqlCmd.Transaction = transaction;
                        SqlCmd.ExecuteNonQuery();

                        // Attempt to commit the transaction.
                        transaction.Commit();

                        return "Success";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction. 
                    try
                    {
                        if (transaction != null) transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred 
                        // on the server that would cause the rollback to fail, such as 
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }

                return "Fail";
            }

            // Config Information
            public string SearchConfigInfo(string ViewName)
            {
                string FieldValue = "", FieldName = "";
                Dictionary<string, string> LevelInfo = new Dictionary<string, string>();

                Common csobj = new Common();

                // Gets the JSON data for Field information
                // Accumulates the Field information that exists
                DataTable dtTableFields = csobj.SQLReturnDataTable("SELECT * FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + ViewName + "'");
                foreach (DataRow drFI in dtTableFields.Rows)
                {
                    FieldValue = "";
                    FieldName += "," + drFI["FIELD_NAME"].ToString();
                    DataTable dtFieldInfo = csobj.SQLReturnDataTable("SELECT DISTINCT " + drFI["FIELD_NAME"].ToString() + " FROM LEVEL_INFO");
                    foreach (DataRow drField in dtFieldInfo.Rows)
                    {
                        FieldValue += "," + drField[drFI["FIELD_NAME"].ToString()].ToString();
                    }
                    LevelInfo.Add(drFI["FIELD_NAME"].ToString(), FieldValue.Substring(1));
                }
                LevelInfo.Add("FIELD_NAME", FieldName.Substring(1));

                return JsonConvert.SerializeObject(LevelInfo, Formatting.Indented);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="MobilityDate"></param>
            /// <param name="PersonnelNumber"></param>
            /// <returns></returns>
            /// 

            public string GetJson(string txtPositionID,
                                    string txtNextPositionID,
                                    string txtOrgUnit,
                                    string txtNextLevelOrg,
                                    string txtGDDBID,
                                    string txtFullName,
                                    string txtFirstName,
                                    string txtLastName,
                                    string txtMiddleName,
                                    string txtNickName,
                                    string txtSiteCode,
                                    string txtDivision,
                                    string txtPersonID,
                                    string view)
            {
                string search = txtPositionID + txtNextPositionID + txtGDDBID + txtOrgUnit + txtNextLevelOrg + txtFullName + txtFirstName + txtLastName + txtMiddleName + txtNickName + txtSiteCode + txtDivision + txtPersonID;
                if (search != "")
                {
                    string LevelDate = DateTime.Now.ToString("dd-MM-yyyy");



                    string keyDate = LevelDate.Substring(6, 4) + "-" + LevelDate.Substring(3, 2) + "-" + LevelDate.Substring(0, 2);


                    //SqlQry = "SELECT DISTINCT a.*, b.OLEVEL,c.*, ISNULL(d.orgunitText,'') OrgunitText, ISNULL(e.positiontitle,'') positiontitle " +
                    //                " FROM LEVEL_INFO a JOIN LEVEL_NOR_SOC c ON a." + OPR_LEVEL_ID + "=c.LEVEL_ID AND a." +
                    //                OPR_PARENT_ID + "=c.PARENT_ID  " +
                    //                " JOIN TEXT_ORG d on a.orgunit = d.orgunit " +
                    //                " JOIN TEXT_POS e on a.PositionID = e.PositionID " +
                    //                " , (SELECT '" + ShowLevel + "' OID, '-1' NextPositionID,  '0' OLEVEL " +
                    //                " UNION ALL SELECT DISTINCT " + OPR_LEVEL_ID + " OID, li.NextPositionID,  '1' OLEVEL FROM LEVEL_INFO LI WHERE " +
                    //                OPR_PARENT_ID + "='" + ShowLevel + "' AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "'" +
                    //                " ) b " +
                    //                " WHERE a." + OPR_LEVEL_ID + "=b.OID AND a.LANGUAGE_SELECTED = '" + Language + "' AND c.KEY_DATE='" + keyDate + "'" +
                    //                " AND A.END_DATE >= '" + keyDate + "'  AND A.START_DATE <= '" + keyDate + "' AND " +
                    //                " d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate +
                    //                "' AND d.LANGUAGE_SELECTED = '" + Language + "' " +
                    //                " AND e.END_DATE >= '" + keyDate + "'  AND e.START_DATE <= '" + keyDate +
                    //                "' AND e.LANGUAGE_SELECTED = '" + Language + "' " +
                    //                "  AND (( b.NextPositionID = a.NextPositionID )  or ( b.NextPositionID = '-1' )) ";

                    search = search.Replace("'", "''");
                    Common csobj = new Common();
                    DataTable dt = null;
                    if (view == "Operational View")
                    {
                        string sWHERE = "";
                        string sSQL = " select  distinct top 100 isnull(A.POSITIONID, '') PositionId, " +
                                        " isnull(A.NextPositionId, '') NextPositionId," +
                                        " isnull(A.ORGUNIT, '') OrgUnitID, " +
                                        " isnull(A.NextLevelOrg, '') NextLevelOrg, " +
                                        " isnull(F.FULLNAME, '') FULLNAME, " +
                                        " isnull(F.FIRSTNAME, '') FIRSTNAME, " +
                                        " isnull(A.LASTNAME, '') LASTNAME, " +
                            //" isnull(A.MIDDLENAME, '') MIDDLENAME, " +
                            //" isnull(A.NICKNAME, '') NICKNAME, " +
                                        " isnull(A.SITECODE, '') SITECODE, " +
                                        " isnull(a.DIVISION, '') DIVISION, " +
                                        " isnull(a.GDDBID, '') GDDBID, " +
                                        " isnull(a.PERSONID, '') PERSONID, " +
                                        " isnull(a.Flag, '') Flag, " +
                                        " (CASE WHEN isnull(a.DOTTED_LINE_FLAG, 'N')='N'THEN 'Operational Reporting' ELSE 'Functional Reporting' END) DOTTED_LINE_FLAG, " +
                                        " '' AS COUNTRYNAME, " +
                                        " substring(isnull(a.LEGALENTITY, '  '), 1, 2) COUNTRYCODE " +
                                        " from [REORG_LEVEL_INFO] a LEFT JOIN TEXT_EMP F ON a.PERSONID = f.personid, (SELECT DISTINCT POSITIONID  FROM PositionManagement) pm" +
                                        " where (";
                        if (txtSiteCode != "") sWHERE += " and a.SITECODE like '%" + txtSiteCode + "%'";
                        if (txtDivision != "") sWHERE += " and a.DIVISION like '%" + txtDivision + "%'";
                        if (txtFullName != "") sWHERE += " and a.FIRSTNAME like '%" + txtFullName.Replace("\\'", "''") + "%'";
                        if (txtFirstName != "") sWHERE += " and F.FIRSTNAME like '%" + txtFirstName.Replace("\\'", "''") + "%'";
                        if (txtOrgUnit != "") sWHERE += " and a.ORGUNIT like '%" + txtOrgUnit.Replace("\\'", "''") + "%'";
                        if (txtNextLevelOrg != "") sWHERE += " and a.NEXTLEVELORG like '%" + txtNextLevelOrg + "%'";
                        if (txtPositionID != "") sWHERE += " and a.Positionid like '%" + txtPositionID + "%'";
                        if (txtNextPositionID != "") sWHERE += " and a.NextPositionID like '%" + txtNextPositionID + "%'";
                        if (txtLastName != "") sWHERE += " and a.LASTNAME like '%" + txtLastName.Replace("\\'", "''") + "%'";
                        if (txtMiddleName != "") sWHERE += " and a.MIDDLENAME like '%" + txtMiddleName.Replace("\\'", "''") + "%'";
                        if (txtNickName != "") sWHERE += " and a.NICKNAME like '%" + txtNickName.Replace("\\'", "''") + "%'";
                        if (txtPersonID != "") sWHERE += " and a.PERSONID like '%" + txtPersonID + "%'";
                        if (txtGDDBID != "") sWHERE += " and  a.GDDBID like '%" + txtGDDBID + "%'";
                        sSQL += sWHERE.Substring(5) + ") AND a.START_DATE <= '" + keyDate + "' AND a.END_DATE >= '" + keyDate + "' AND a.POSITIONID NOT IN (SELECT DISTINCT POSITIONID  FROM PositionManagement)";
                        dt = csobj.SQLReturnDataTable(sSQL);
                    }
                    else if (view == "Legal View")
                    {
                        string sWHERE = "";
                        string sSQL = "select distinct TOP 100 isnull(A.POSITIONID, '') PositionId, " +
                                        " isnull(A.NextPositionId, '') NextPositionId," +
                                        " isnull(A.ORGUNIT, '') OrgUnitID, " +
                                        " isnull(A.NextLevelOrg, '') NextLevelOrg, " +
                                        " isnull(F.FULLNAME, '') FULLNAME, " +
                                        " isnull(F.FIRSTNAME, '') FIRSTNAME, " +
                                        " isnull(A.LASTNAME, '') LASTNAME, " +
                                        " isnull(A.MIDDLENAME, '') MIDDLENAME, " +
                                        " isnull(A.NICKNAME, '') NICKNAME, " +
                                        " isnull(A.SITECODE, '') SITECODE, " +
                                        " isnull(A.DIVISION, '') DIVISION, " +
                                        " isnull(A.GDDBID, '') GDDBID, " +
                                        " isnull(A.PERSONID, '') PERSONID, " +
                                        " isnull(a.Flag, '') Flag, " +
                                        " ISNULL((SELECT COUNTRY_NAME FROM LEGAL_COUNTRY_INFO WHERE COUNTRY_ID=a.ORGUNIT),'') COUNTRYNAME " +
                                        " from [LEGAL_INFO] a LEFT JOIN TEXT_EMP F ON a.PERSONID = f.personid, (SELECT DISTINCT POSITIONID  FROM PositionManagement) pm" +
                                        " where ( ";
                        if (txtSiteCode != "") sWHERE += " and a.SITECODE like '%" + txtSiteCode + "%'";
                        if (txtPersonID != "") sWHERE += " and a.PERSONID like '%" + txtPersonID + "%'";
                        if (txtDivision != "") sWHERE += " and a.DIVISION like '%" + txtDivision + "%'";
                        if (txtFullName != "") sWHERE += " and a.FIRSTNAME like '%" + txtFullName.Replace("\\'", "''") + "%'";
                        if (txtFirstName != "") sWHERE += " and F.FIRSTNAME like '%" + txtFirstName.Replace("\\'", "''") + "%'";
                        if (txtMiddleName != "") sWHERE += " and a.MIDDLENAME like '%" + txtMiddleName.Replace("\\'", "''") + "%'";
                        if (txtNickName != "") sWHERE += " and a.NICKNAME like '%" + txtNickName.Replace("\\'", "''") + "%'";
                        if (txtOrgUnit != "") sWHERE += " and a.ORGUNIT like '%" + txtOrgUnit.Replace("\\'", "''") + "%'";
                        if (txtNextLevelOrg != "") sWHERE += " and a.NEXTLEVELORG like '%" + txtNextLevelOrg + "%'";
                        if (txtPositionID != "") sWHERE += " and a.Positionid like '%" + txtPositionID + "%'";
                        if (txtNextPositionID != "") sWHERE += " and a.NextPositionID like '%" + txtNextPositionID + "%'";
                        if (txtLastName != "") sWHERE += " and a.LASTNAME like '%" + txtLastName.Replace("\\'", "''") + "%'";
                        if (txtGDDBID != "") sWHERE += " and a.GDDBID like '%" + txtGDDBID + "%'";
                        sSQL += sWHERE.Substring(5) + ") AND A.START_DATE <= '" + keyDate + "' AND A.END_DATE >= '" + keyDate + "' AND a.POSITIONID NOT IN (SELECT DISTINCT POSITIONID  FROM PositionManagement)";
                        dt = csobj.SQLReturnDataTable(sSQL);
                    }

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new

                    System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows =
                      new List<Dictionary<string, object>>();
                    Dictionary<string, object> row = null;

                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName.Trim(), dr[col]);
                        }
                        rows.Add(row);
                    }
                    return serializer.Serialize(rows);
                }
                return "";
            }

            public string GetPositionJson()
            {
                string LevelDate = HttpContext.Current.Session["LEVEL_DATE"].ToString();
                string keyDate = LevelDate.Substring(6, 4) + "-" + LevelDate.Substring(3, 2) + "-" + LevelDate.Substring(0, 2);

                Common csobj = new Common();
                DataTable dt = null;
                string sSQL = "select distinct isnull(A.POSITIONID, '') POSITIONID, " +
                                            " isnull(A.NextPositionId, '') NextPositionId," +
                                            " isnull(A.FIRSTNAME, '') FIRSTNAME, " +
                                            " isnull(A.LASTNAME, '') LASTNAME, " +
                                            " isnull(A.MIDDLENAME, '') MIDDLENAME, " +
                                            " isnull(A.NICKNAME, '') NICKNAME, " +
                                            " isnull(d.ORGUNITTEXT, '') ORGUNITTEXT, " +
                                            " isnull(A.SITECODE, '') SITECODE, " +
                                            " isnull(a.DIVISION, '') DIVISION, " +
                                            " isnull(a.GDDBID, '') GDDBID, " +
                                            " isnull(a.PERSONID, '') PERSONID, " +
                                            " (CASE WHEN isnull(a.DOTTED_LINE_FLAG, 'N')='N'THEN 'Operational Reporting' ELSE 'Functional Reporting' END) DOTTED_LINE_FLAG, " +
                                            " '' AS COUNTRYNAME, " +
                                            " '' AS DeleteRecord, " +
                                            " substring(isnull(a.LEGALENTITY, '  '), 1, 2) COUNTRYCODE " +
                                            " from [REORG_LEVEL_INFO] a " +
                                            " JOIN TEXT_ORG d on a.orgunit = d.orgunit " +
                                            " JOIN PositionManagement e on a.POSITIONID = e.POSITIONID " +
                                            " where  a.START_DATE <= '" + keyDate + "' AND a.END_DATE >= '" + keyDate + "'" +
                                            " AND d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate + "'";
                //string sSQL = "select isnull(A.POSITIONID, '') POSITIONID, " +
                //                " isnull(A.NextPositionId, '') NextPositionId," +
                //                " isnull(A.FIRSTNAME, '') FIRSTNAME, " +
                //                " isnull(A.LASTNAME, '') LASTNAME, " +
                //                " isnull(A.MIDDLENAME, '') MIDDLENAME, " +
                //                " isnull(A.NICKNAME, '') NICKNAME, " +
                //                " isnull(d.ORGUNITTEXT, '') ORGUNITTEXT, " +
                //                " isnull(A.SITECODE, '') SITECODE, " +
                //                " isnull(a.DIVISION, '') DIVISION, " +
                //                " isnull(a.GDDBID, '') GDDBID, " +
                //                " isnull(a.PERSONID, '') PERSONID, " +
                //                " (CASE WHEN isnull(a.DOTTED_LINE_FLAG, 'N')='N'THEN 'Operational Reporting' ELSE 'Functional Reporting' END) DOTTED_LINE_FLAG, " +
                //                " '' AS COUNTRYNAME, " +
                //                " '' AS DeleteRecord, " +
                //                " substring(isnull(a.LEGALENTITY, '  '), 1, 2) COUNTRYCODE " +
                //                " from [LEVEL_INFO] a " +
                //                " JOIN TEXT_ORG d on a.orgunit = d.orgunit " +
                //                " JOIN PositionManagement e on a.POSITIONID = e.POSITIONID " +
                //                " where  a.START_DATE <= '" + keyDate + "' AND a.END_DATE >= '" + keyDate + "'" +
                //                " AND d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate + "'" +
                //              " UNION ALL " +
                //              "select POSITIONID, " +
                //                " '' NextPositionId," +
                //                " '' FIRSTNAME, " +
                //                " '' LASTNAME, " +
                //                " '' MIDDLENAME, " +
                //                " '' NICKNAME, " +
                //                " '' ORGUNITTEXT, " +
                //                " '' SITECODE, " +
                //                " '' DIVISION, " +
                //                " '' GDDBID, " +
                //                " '' PERSONID, " +
                //                " 'Operational Reporting' DOTTED_LINE_FLAG, " +
                //                " '' AS COUNTRYNAME, " +
                //                " '' AS DeleteRecord, " +
                //                " '' COUNTRYCODE " +
                //                " from PositionManagement where country=''";

                dt = csobj.SQLReturnDataTable(sSQL);

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new

                System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows =
                    new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;

                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName.Trim(), dr[col]);
                    }
                    rows.Add(row);
                }

                return serializer.Serialize(rows);
            }

            // Level Information
            public string SearchLevelInfo(string VN, string IV, string PI)
            {
                int StartPage = ((Convert.ToInt32(PI) - 1) * 15) + 1;
                int EndPage = Convert.ToInt32(PI) * 15;
                string FieldName = "", TableInfo = "", SQL = "";
                Dictionary<string, string> LevelInfo = new Dictionary<string, string>();

                Common csobj = new Common();

                // Gets the JSON data for Field information
                // Accumulates the Field information that exists
                TableInfo += "<div style='width:100%;overflow:scroll;'><table id='SearchLevelInfo' style='width: 2000px !Important;'>";
                TableInfo += "<thead><tr>";

                JsonFieldWidth = "[ ";
                DataTable dtTableFields = csobj.SQLReturnDataTable("SELECT * FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + VN + "'");
                foreach (DataRow drFI in dtTableFields.Rows)
                {
                    if (drFI["TABLE_IND"].ToString() == "Y")
                    {
                        TableInfo += "<th>" + drFI["FIELD_CAPTION"].ToString() + "</th>";
                        if (JsonFieldWidth != "[ ") JsonFieldWidth += ", ";
                        JsonFieldWidth += "{ \"sWidth\": \"" + drFI["FIELD_WIDTH"].ToString() + "\" }";
                    }
                    if (IV != "") FieldName += "OR " + drFI["FIELD_NAME"].ToString() + " LIKE '%" + IV + "%' ";
                }
                TableInfo += "</tr></thead>";
                JsonFieldWidth += " ]";

                SQL = "SELECT COUNT(*) AS MAX_ROW FROM LEVEL_INFO ";
                if (IV != "")
                {
                    SQL += " WHERE ";
                    SQL += FieldName.Substring(2);
                }
                DataTable dtRecordCount = csobj.SQLReturnDataTable(SQL);

                SQL = "SELECT * FROM ";
                SQL += "(SELECT ROW_NUMBER() OVER (ORDER BY " + OPR_LEVEL_ID + ") AS RANK_ID, * FROM LEVEL_INFO ";
                if (IV != "")
                {
                    SQL += " WHERE ";
                    SQL += FieldName.Substring(2);
                }
                SQL += ") a WHERE RANK_ID >=" + StartPage.ToString() + " AND RANK_ID <=" + EndPage.ToString();

                TableInfo += "<tbody>";
                int intIdx = 0;
                DataTable dtFieldInfo = csobj.SQLReturnDataTable(SQL);
                foreach (DataRow drFV in dtFieldInfo.Rows)
                {
                    TableInfo += "<tr>";
                    foreach (DataRow drFI in dtTableFields.Rows)
                    {
                        if (drFI["TABLE_IND"].ToString() == "Y")
                        {
                            if (drFI["FIELD_NAME"].ToString() == OPR_LEVEL_ID)
                                TableInfo += "<td><a class='OprOV' href='javascript:void(0)' onclick='OperationalViewInfo(\"" + drFV[drFI["FIELD_NAME"].ToString()].ToString() + "\")'>" + drFV[drFI["FIELD_NAME"].ToString()].ToString() + "</a></td>";
                            else
                                TableInfo += "<td>" + drFV[drFI["FIELD_NAME"].ToString()].ToString() + "</td>";
                        }
                    }
                    TableInfo += "</tr>";
                    intIdx++;
                }
                TableInfo += "</tbody></table></div>";

                LevelInfo.Add("LEVEL_INFO", TableInfo);
                LevelInfo.Add("FIELD_WIDTH", JsonFieldWidth);
                LevelInfo.Add("RECORD_COUNT", dtRecordCount.Rows[0]["MAX_ROW"].ToString());

                return JsonConvert.SerializeObject(LevelInfo, Formatting.Indented);
            }

            // Config Information
            public string ConfigInfo(string ViewName)
            {
                var theLEVEL_CONFIG_INFO = new List<LEVEL_CONFIG_INFO>();
                string FieldName = "", TemplateURL = "", LegalView = "", UserInfo = "", OrgInfo = "", ViewType = "N";
                int Index = 0;

                Common csobj = new Common();
                string sSQL = "SELECT TOP 1 a.*, b.LEVEL_ID, b.PARENT_ID, b.KEY_DATE, b.SOC_COUNT, b.NOR_COUNT, e.OrgUnitText,  c.PositionTitle, f.fullname " +
                              " FROM LEVEL_INFO a LEFT JOIN LEVEL_NOR_SOC b ON a." + OPR_LEVEL_ID + " = b.LEVEL_ID LEFT JOIN TEXT_POS c ON a." + OPR_LEVEL_ID + " =c.PositionID LEFT JOIN TEXT_ORG e ON a." + LGL_LEVEL_ID + " =e.OrgUnit  LEFT JOIN TEXT_EMP f ON a.PERSONID = f.personid " +
                              " WHERE a." + OPR_LEVEL_ID + " = '20002609' AND a.END_DATE >=getdate() AND a.START_DATE <= getdate()";
                DataTable dtLevelInfo = csobj.SQLReturnDataTable(sSQL);

                // Gets the JSON data for Field information
                // Accumulates the Field information that exists
                string[] CONFIG_INFO = SUPPRESS_FIELDS.Split(',');
                string SUP_FIELDS = "";
                for (int Idx = 0; Idx <= CONFIG_INFO.Length - 1; Idx++)
                    SUP_FIELDS += ",\'" + CONFIG_INFO[Idx] + "\'";

                DataTable dtTableFields = csobj.SQLReturnDataTable("SELECT * FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + ViewName + "'");
                foreach (DataRow drFI in dtTableFields.Rows)
                {
                    try
                    {
                        // the array contains the string and the pos variable
                        if (Array.IndexOf(CONFIG_INFO, drFI["FIELD_NAME"].ToString()) <= -1)
                        {
                            FieldName += ",'" + drFI["FIELD_NAME"].ToString() + "'";
                            theLEVEL_CONFIG_INFO.Add(new LEVEL_CONFIG_INFO(drFI["ID"].ToString(),
                                                                            drFI["FIELD_CAPTION"].ToString(),
                                                                            drFI["FIELD_NAME"].ToString(),
                                                                            drFI["FIELD_ROW"].ToString(),
                                                                            drFI["FIELD_ROW_TYPE"].ToString(),
                                                                            drFI["FIELD_COL"].ToString(),
                                                                            drFI["FIELD_COL_TYPE"].ToString(),
                                                                            drFI["WRAP"].ToString(),
                                                                            drFI["FONT_NAME"].ToString(),
                                                                            drFI["FONT_SIZE"].ToString(),
                                                                            drFI["FONT_COLOR"].ToString(),
                                                                            drFI["FONT_STYLE"].ToString(),
                                                                            drFI["FONT_FLOAT"].ToString(),
                                                                            drFI["ACTIVE_IND"].ToString(),
                                                                            drFI["TABLE_IND"].ToString(),
                                                                            drFI["FIELD_WIDTH"].ToString(),
                                                                            drFI["ADJUSTMENT"].ToString(),
                                                                            dtLevelInfo.Rows[0][drFI["FIELD_NAME"].ToString()].ToString()));
                            Index++;
                        }

                    }
                    catch (Exception ex)
                    {
                        string errMsg = ex.Message;
                        Console.WriteLine("  Message: {0}", errMsg);
                    }


                }

                int LvlIdentity = int.Parse(csobj.returnVal("SELECT IDENT_CURRENT ('LEVEL_CONFIG_INFO') AS Current_Identity")) + 1;

                //LEFT JOIN TEXT_POS c ON a." + OPR_LEVEL_ID + " =b.PositionID LEFT JOIN TEXT_ORG e ON a." + LGL_LEVEL_ID + " =e.OrgUnit
                //  OR t.name='TEXT_EMP' OR t.name='TEXT_ORG' OR t.name='TEXT_POS'
                // Accumulates the Field information that exists
                DataTable dtFieldInfo = csobj.SQLReturnDataTable("SELECT t.name as TableName, c.name as ColumnName FROM SYS.COLUMNS c " +
                                                                    "INNER JOIN SYS.TABLES t on c.object_id = t.object_id " +
                                                                    "WHERE (t.name='LEVEL_INFO' OR  t.name='LEVEL_NOR_SOC' OR t.name='TEXT_EMP' OR t.name='TEXT_ORG' OR t.name='TEXT_POS') AND c.name NOT IN (" + FieldName.Substring(1) + SUP_FIELDS + ")");
                foreach (DataRow drFI in dtFieldInfo.Rows)
                {
                    try
                    {
                        theLEVEL_CONFIG_INFO.Add(new LEVEL_CONFIG_INFO((LvlIdentity++).ToString(),
                                                                        drFI["ColumnName"].ToString(),
                                                                        drFI["ColumnName"].ToString(),
                                                                        "0",
                                                                        "px",
                                                                        "0",
                                                                        "px",
                                                                        "Y",
                                                                        "Arial",
                                                                        "8",
                                                                        "#000000",
                                                                        "normal",
                                                                        "left",
                                                                        "N",
                                                                        "N",
                                                                        "100",
                                                                        "N",
                                                                        dtLevelInfo.Rows[0][drFI["ColumnName"].ToString()].ToString()));
                    }
                    catch (Exception ex1)
                    {
                        string errMsg = ex1.ToString();
                        Console.WriteLine("  Message: {0}", errMsg);
                    }
                }

                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonField = javaScriptSerializer.Serialize(theLEVEL_CONFIG_INFO);

                // Gets the Template URL
                DataTable dtTemp = csobj.SQLReturnDataTable("SELECT FIELD_VALUE FROM ORG_CONFIG_INFO WHERE FIELD_NAME='TEMPLATE_URL'");
                foreach (DataRow item in dtTemp.Rows)
                {
                    TemplateURL += ",{\"TemplateURL\":\"" + item["FIELD_VALUE"].ToString() + "\"}";
                }

                // Gets the Legal View Country information
                LegalView = "";
                DataTable dtCountry = csobj.SQLReturnDataTable("SELECT * FROM LEGAL_COUNTRY_INFO");
                foreach (DataRow item in dtCountry.Rows)
                {
                    LegalView += ",{\"CountryView\":\"" + item["COUNTRY_ID"].ToString() + ":" + item["COUNTRY_NAME"].ToString() + "\"}";
                }

                UserInfo = "";
                DataTable dtUI = csobj.SQLReturnDataTable("SELECT * FROM VIEW_INFO WHERE UID='" + HttpContext.Current.Session["USERID"].ToString() + "'");
                foreach (DataRow item in dtUI.Rows)
                {
                    if (item["VIEW_ID"].ToString() == ViewName) ViewType = (item["VIEW_PUBLIC"].ToString() == "Y") ? "Y" : "N";
                    UserInfo += ",{\"UserView\":\"" + item["VIEW_ID"].ToString() + "\",\"ViewType\":\"" + item["VIEW_PUBLIC"].ToString() + "\"}";
                }

                // Org config info
                OrgInfo = "{";
                DataTable dtlevel = csobj.SQLReturnDataTable("SELECT * FROM ORG_CONFIG_INFO WHERE VIEW_ID='" + ViewName + "'");
                foreach (DataRow drlvl in dtlevel.Rows)
                {
                    if (drlvl["FIELD_NAME"].ToString() == "LEVEL") OrgInfo += "\"LEVEL_ID\":\"" + drlvl["FIELD_VALUE"].ToString() + "\",";
                    if (drlvl["FIELD_NAME"].ToString() == "HEIGHT") OrgInfo += "\"BOX_HEIGHT\":\"" + drlvl["FIELD_VALUE"].ToString() + "\",";
                    if (drlvl["FIELD_NAME"].ToString() == "TEMPLATE") OrgInfo += "\"TEMPLATE_URL\":\"" + drlvl["FIELD_VALUE"].ToString() + "\",";
                    if (drlvl["FIELD_NAME"].ToString() == "LINECOLOR") OrgInfo += "\"LINE_COLOR\":\"" + drlvl["FIELD_VALUE"].ToString() + "\",";
                    if (drlvl["FIELD_NAME"].ToString() == "LINEWIDTH") OrgInfo += "\"LINE_WIDTH\":\"" + drlvl["FIELD_VALUE"].ToString() + "\",";
                }
                OrgInfo += "\"BOX_FILE\":\"\",";
                OrgInfo += "\"VIEW_NAME\":\"" + ViewName + "\",";
                OrgInfo += "\"VIEW_TYPE\":\"" + ViewType + "\",";
                DataTable dtUser = csobj.SQLReturnDataTable("SELECT VIEW_DEFAULT FROM USER_INFO WHERE UID='" + HttpContext.Current.Session["USERID"].ToString() + "'");
                OrgInfo += "\"VIEW_DEFAULT\":\"" + dtUser.Rows[0]["VIEW_DEFAULT"].ToString() + ":" + ViewType + "\"";
                OrgInfo += "}";

                Dictionary<string, string> ConfigInf = new Dictionary<string, string>
            {
                { "TemplateURL","["+ TemplateURL.Substring(1) +"]" },
                { "LegalView", "["+ LegalView.Substring(1) +"]" },
                { "FieldInfo", jsonField },
                { "OrgInfo", "["+ OrgInfo +"]" },
                { "UserInfo", "["+ UserInfo.Substring(1) +"]"}
            };
                return JsonConvert.SerializeObject(ConfigInf, Formatting.Indented);
            }

            // Distinct Id
            public string IdInfo()
            {
                string ID = "";

                Common csobj = new Common();
                DataTable dtId = csobj.SQLReturnDataTable("SELECT DISTINCT " + OPR_LEVEL_ID + " FROM LEVEL_INFO");
                foreach (DataRow drId in dtId.Rows)
                {
                    ID += "<option>" + drId[OPR_LEVEL_ID].ToString() + "</option>";
                }

                return ID;
            }

            // Distinct User Information
            public string UserInfo()
            {
                string UID = "";

                // Gets the User View information
                Common csobj = new Common();
                DataTable dtUI = csobj.SQLReturnDataTable("SELECT * FROM USER_INFO");
                foreach (DataRow item in dtUI.Rows)
                {
                    UID += "<option>" + item["UID"].ToString() + "</option>"; ;
                }

                return UID;
            }

            // Draws the photo with respect to Position Id
            private string DrawPhoto(ObjectInf Obj, int col, int row)
            {
                string strElement = "";
                HttpContext.Current.Session["VIEW"] = "VIEW_DEFAULT";
                if (HttpContext.Current.Session["VIEW"].ToString() == "NBS Template with photo")
                {
                    string sPhotoDIR = HttpContext.Current.Server.MapPath("images/photos/") + Obj.GDDBID + ".jpg", sPhotoURL = "images/photos/" + Obj.GDDBID + ".jpg";
                    if (strOutput == "PDF")
                    {
                        if (File.Exists(sPhotoDIR))
                        {
                            if (Obj.Level == "0")
                            {
                                MyImage = new ceTe.DynamicPDF.PageElements.Image(sPhotoDIR, Obj.Col + col, Obj.Row + row + 17);
                                MyImage.Height = 70;
                                MyImage.Width = 70;

                                MyPage[CurPage].Elements.Add(MyImage);
                            }
                            else
                            {
                                MyImage = new ceTe.DynamicPDF.PageElements.Image(sPhotoDIR, Obj.Col, Obj.Row + row + 17);
                                MyImage.Height = 70;
                                MyImage.Width = 70;

                                MyPage[CurPage].Elements.Add(MyImage);
                            }
                        }
                    }
                    else if (strOutput == "PPT")
                    {
                        Bitmap BmpPhoto = null;
                        if (File.Exists(sPhotoDIR))
                        {
                            BmpPhoto = new Bitmap(sPhotoDIR);
                            GraphicImg[CurPage].DrawImage(BmpPhoto, Obj.Col + col - 32, Obj.Row + row + 17, 70, 70);
                        }
                    }
                    else if (strOutput == "HTML")
                    {
                        if (File.Exists(sPhotoDIR))
                            strElement = "<img src=\"" + sPhotoURL + "\" alt=\"Photo\" style=\"height:75px;position:absolute;left:" + (Obj.Col + col + 2).ToString() + "px;top:" + (Obj.Row + row + 2).ToString() + "px\"/>";
                    }
                }

                return strElement;
            }

            // Gets the Object Info from the table



            //***********CR:310012488, CD:, Modified By :NAGARVI5******************************
            private List<ObjectInf> GetLevelInfo()
            {
                var theObjectInf = new List<ObjectInf>();
                Common csobj = new Common();
                DataTable dtlevel = null;
                SqlCommand cmd = new SqlCommand();

                string InfoPos = "", SqlQry = "", ShowLevel = Level;
                // string[] SearchIDs = HttpContext.Current.Session["SearchPID"].ToString().Split(':');
                string[] SearchIDs = { "123 ", "123" };

                HttpContext.Current.Session["LANGUAGE"] = Language;
                LevelDate = DateTime.Now.ToString("dd-MM-yyyy");
                if (LevelDate != "")
                {
                    HttpContext.Current.Session["LEVEL_ID"] = ShowLevel;
                    HttpContext.Current.Session["LEVEL_DATE"] = LevelDate;
                }
                else
                {
                    HttpContext.Current.Session["LEVEL_ID"] = "";
                    HttpContext.Current.Session["LEVEL_DATE"] = "";
                }

                //DateTime fixtureDate = DateTime.Parse(LevelDate, new CultureInfo("en-GB"));
                //string keyDate = fixtureDate.ToString("MM/dd/yyyy");

                string sFM = "";
                if (FlagFM == "N") sFM = " AND DOTTED_LINE_FLAG='N'";
                DateTime fixtureDate = DateTime.Parse(LevelDate, new CultureInfo("en-GB"));
                fixtureDate = fixtureDate.AddDays(-1);
                string prevDate = fixtureDate.Year.ToString() + "-" + fixtureDate.Month.ToString("d2") + "-" + fixtureDate.Day.ToString("d2");
                string keyDate = LevelDate.Substring(6, 4) + "-" + LevelDate.Substring(3, 2) + "-" + LevelDate.Substring(0, 2);
                string SelLang = "";
                if (Language == "EN")
                    SelLang = "EN";
                else SelLang = "00";
                if (View == "OV")
                {
                    DataTable dtRE = csobj.SQLReturnDataTable("SELECT MAX(KEY_DATE) AS KEY_DATE FROM LEVEL_NOR_SOC");
                    if (dtRE != null)
                    {
                        if (dtRE.Rows.Count != 0)
                        {
                            keyDate = dtRE.Rows[0]["KEY_DATE"].ToString();
                        }
                    }


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "PROC_GET_POSITION_TREE_OPERATIONALCHART";

                    cmd.Parameters.Add("@STARTPOSITION", SqlDbType.VarChar, 15).Value = Level;
                    cmd.Parameters.Add("@DEPTH", SqlDbType.Int).Value = LevelFlag;
                    cmd.Parameters.Add("@VERSIONID", SqlDbType.Int).Value = FlagFM;
                    cmd.Parameters.Add("@USER", SqlDbType.VarChar, 15).Value = Common.GetUser();

                }
                else if (View == "LV")
                {

                    if (LevelFlag == "1")
                    {
                        SqlQry = "SELECT DISTINCT a.*, b.OLEVEL, c.*, ISNULL(d.orgunitText,'') OrgunitText, ISNULL(e.positiontitle,'') positiontitle, ISNULL(f.POSITIONID, '0') positionflag, g.FullName " +
                                " FROM REORG_LEVEL_INFO a JOIN LEGAL_NOR_SOC c ON  a." + LGL_LEVEL_ID + "=c.LEVEL_ID AND a." +
                                  LGL_PARENT_ID + "=c.PARENT_ID " +
                                " LEFT JOIN TEXT_ORG d on a.nextpositionid = d.orgunit AND D.LANGUAGE_SELECTED = '" + Language + "' " +
                                " LEFT JOIN TEXT_POS e on a.PositionID = e.PositionID  AND E.LANGUAGE_SELECTED = '" + Language + "' " +
                                " JOIN TEXT_EMP g on a.PERSONID = g.PERSONID  AND g.Language_selected = '" + SelLang + "'" +
                                " LEFT JOIN PositionManagement f on a.PositionID = f.PositionID " +
                                ",  (SELECT '" + ShowLevel +
                                "' OID, '-1' NextLevelOrg, '0' OLEVEL " +
                                " UNION ALL SELECT DISTINCT " + LGL_LEVEL_ID + " OID, li.NextLevelOrg, '1' OLEVEL FROM LEGAL_INFO LI WHERE " +
                                  LGL_PARENT_ID + "='" + ShowLevel + "'  AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "'" +
                                ") b " +
                                " WHERE a." + LGL_LEVEL_ID + "=b.OID   AND A.LANGUAGE_SELECTED = '" + "EN" + "' " +
                                " AND c.KEY_DATE='" + keyDate + "'" +
                                " AND A.END_DATE >= '" + keyDate + "'  AND A.START_DATE <= '" + keyDate + "' " +
                                " AND d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate + "' " +
                                " AND g.END_DATE >= '" + keyDate + "'  AND g.START_DATE <= '" + keyDate + "' " +
                            //" AND (( d.LANGUAGE_SELECTED = '" + Language + "'  AND D.ORGUNIT <> '" + ShowLevel + "') OR D.ORGUNIT = '" + ShowLevel + "') " +
                                " AND e.END_DATE >= '" + keyDate + "'  AND e.START_DATE <= '" + keyDate +
                                "' AND e.LANGUAGE_SELECTED = '" + Language + "' " +
                                "  AND (( b.NextLevelOrg = a.NextLevelOrg )  or (( b.NextLevelOrg = '-1' ) AND ( a.NextLevelOrg = '" + PreviousLevel + "' ))) ";
                    }
                    else if (LevelFlag == "2")
                    {
                        SqlQry = "SELECT DISTINCT a.*, b.OLEVEL,   c.* " +
                            " , ISNULL(d.orgunitText,'') OrgunitText, ISNULL(e.positiontitle,'') positiontitle, ISNULL(f.POSITIONID, '0') positionflag , g.FullName " +
                            " FROM REORG_LEGAL_INFO a JOIN LEGAL_NOR_SOC c ON  a." +
                            LGL_LEVEL_ID + "=c.LEVEL_ID AND a." + LGL_PARENT_ID + "=c.PARENT_ID " +
                            " JOIN TEXT_ORG d on a.nextpositionid = d.orgunit " +
                            " JOIN TEXT_POS e on a.PositionID = e.PositionID " +
                            " JOIN TEXT_EMP g on a.PERSONID = g.PERSONID  AND g.Language_selected = '" + SelLang + "'" +
                            " LEFT JOIN PositionManagement f on a.PositionID = f.PositionID " +
                            " , (SELECT '" +
                            ShowLevel + "' OID, '-1' NextLevelOrg, '0' OLEVEL " +
                            " UNION ALL SELECT DISTINCT " + LGL_LEVEL_ID + " OID, li.NextLevelOrg, '1' OLEVEL FROM REORG_LEGAL_INFO LI WHERE " +
                            LGL_PARENT_ID + "='" + ShowLevel + "'  AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "'" +
                            " UNION ALL SELECT DISTINCT " + LGL_LEVEL_ID + " OID, li.NextLevelOrg, '2' OLEVEL  FROM REORG_LEGAL_INFO LI WHERE " +
                            LGL_PARENT_ID + " IN (SELECT " + LGL_LEVEL_ID + " FROM REORG_LEGAL_INFO WHERE " + LGL_PARENT_ID + "='" + ShowLevel + "'  AND END_DATE >= '" +
                            keyDate + "'  AND START_DATE <= '" + keyDate + "')  AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "') b " +
                            " WHERE a." + LGL_LEVEL_ID + "=b.OID    AND A.LANGUAGE_SELECTED = '" + "EN" + "' " +
                            " AND c.KEY_DATE='" + keyDate + "'" +
                            " AND A.END_DATE >= '" + keyDate + "'  AND A.START_DATE <= '" + keyDate + "' " +
                            " AND d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate + "' " +
                            " AND G.END_DATE >= '" + keyDate + "'  AND G.START_DATE <= '" + keyDate + "' " +
                            //" AND (( d.LANGUAGE_SELECTED = '" + Language + "'  AND D.ORGUNIT <> '" + ShowLevel + "') OR D.ORGUNIT = '" + ShowLevel + "') " +
                            " AND e.END_DATE >= '" + keyDate + "'  AND e.START_DATE <= '" + keyDate +
                            "' AND e.LANGUAGE_SELECTED = '" + Language + "' " +
                            "  AND (( b.NextLevelOrg = a.NextLevelOrg )  or (( b.NextLevelOrg = '-1' ) AND ( a.NextLevelOrg = '" + PreviousLevel + "' ))) ";

                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "PROC_GET_POSITION_TREE_LEGALCHART";
                    cmd.Parameters.Add("@STARTPOSITION", SqlDbType.VarChar, 15).Value = Level;
                    cmd.Parameters.Add("@DEPTH", SqlDbType.Int).Value = LevelFlag;
                    cmd.Parameters.Add("@COUNTRYID", SqlDbType.Int).Value = Country;
                    cmd.Parameters.Add("@VERSIONID", SqlDbType.Int).Value = FlagFM;
                    cmd.Parameters.Add("@USER", SqlDbType.VarChar, 15).Value = Common.GetUser();

                }


                csobj = new Common();
                dtlevel = csobj.SQLReturnDataTable("SELECT * FROM REORG_ORG_CONFIG_INFO WHERE VIEW_ID='VIEW_DEFAULT'");
                foreach (DataRow drlvl in dtlevel.Rows)
                {
                    //if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = drlvl["FIELD_VALUE"].ToString();
                    if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = LevelFlag;
                    if (drlvl["FIELD_NAME"].ToString() == "HEIGHT") Height = drlvl["FIELD_VALUE"].ToString();
                    if (drlvl["FIELD_NAME"].ToString() == "LINECOLOR")
                    {
                        LineColor = drlvl["FIELD_VALUE"].ToString();
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "LINEWIDTH")
                    {
                        LineWidth = drlvl["FIELD_VALUE"].ToString();
                        blackPen = new Pen(GetDrawingLineColor(LineColor), Convert.ToInt16(LineWidth));
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "TEMPLATE")
                    {
                        TemplateURL = drlvl["FIELD_VALUE"].ToString();
                        string[] TempURL = TemplateURL.Split('~');
                        if (TempURL[0] == "IMG")
                        {
                            BmpURL = new Bitmap(System.Web.HttpContext.Current.Server.MapPath(TempURL[1]));
                            BmpURL.SetResolution(1200f, 1200f);
                        }
                        BmpUpArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/arrow-up.png"));
                        BmpDnArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/down_arrow_select.jpg"));
                    }
                }

                // Gets the Box Height
                Original_Height = Convert.ToInt32(Height);
                Original_Height_10 = Original_Height + 40;
                Adjustment_Height = ((((Original_Height - (Original_Height % 30)) / 30)) * LevelCount) - 1;
                HttpContext.Current.Session["BoxHeight"] = Original_Height;

                DataTable dtlbl = null;
                DataTable dtconf = csobj.SQLReturnDataTable("SELECT * FROM REORG_LEVEL_CONFIG_INFO WHERE VIEW_ID='VIEW_DEFAULT'");
                string OrderBy = "OLEVEL, ";

                if (LevelFlag == "1")
                {
                    OrderBy = "OLEVEL, SORTNO, ";
                }
                if (View == "OV")
                    OrderBy += OPR_PARENT_ID + ", " + OPR_LEVEL_ID;
                else if (View == "LV")
                    OrderBy += LGL_PARENT_ID + ", " + LGL_LEVEL_ID;


                //dtlbl = csobj.SQLReturnDataTable(SqlQry + " ORDER BY " + OrderBy);

                dtlbl = csobj.SPReturnDataTable(cmd);

                int runno = 1;
                foreach (DataRow dr1 in dtlbl.Rows)
                {
                    dr1["POSITIONID"] = dr1["KEY"].ToString();
                    dr1["NEXTPOSITIONID"] = dr1["parent"].ToString();


                    if (dr1["OLEVEL"].ToString() != "0")
                    {
                        dr1["SORTNO"] = runno.ToString();

                        runno++;
                    }
                }

                string[] CONFIG_INFO = SUPPRESS_FIELDS.Split(',');
                string SUP_FIELDS = "";
                for (int Idx = 0; Idx <= CONFIG_INFO.Length - 1; Idx++)
                    SUP_FIELDS += ",\'" + CONFIG_INFO[Idx] + "\'";

                int iTestHead = 0;
                foreach (DataRow dr in dtlbl.Rows)
                {
                    if (iTestHead == 0)
                    {
                        JsonFieldWidth = "[ ";
                        TableHTML = "<div style='cursor:pointer;position:absolute;top:5px;left:5px;z-index:1001;color:blue;text-decoration:underline;' id='divExportExcel' onclick='return ShowChartTable()'>Export to Excel</div><div style='width:100%;overflow:scroll;'>" +
                                    "<table id='tblLevelInfo' style='width:2000px !Important;'>" +
                                    "<thead><tr>";
                        foreach (DataRow drFI in dtFieldInformation.Rows)
                        {
                            if (!(CONFIG_INFO.Contains(drFI["FIELD_NAME"].ToString())))
                            {
                                if (drFI["TABLE_IND"].ToString() == "Y")
                                {
                                    try
                                    {
                                        string TestHead = dr[drFI["FIELD_NAME"].ToString()].ToString();
                                        TableHTML += "<th style=\"text-align:left;\">" + drFI["FIELD_CAPTION"].ToString() + "</th>";
                                        if (JsonFieldWidth != "[ ") JsonFieldWidth += ", ";
                                        JsonFieldWidth += "{ \"sWidth\": \"" + drFI["FIELD_WIDTH"].ToString() + "\" }";
                                    }
                                    catch (Exception ex)
                                    {
                                        string errMsg = ex.ToString();
                                        Console.WriteLine("  Message: {0}", errMsg);
                                    }
                                }
                            }
                        }
                        TableHTML += "</tr></thead><tbody>";
                        JsonFieldWidth += " ]";
                    }
                    iTestHead++;

                    InfoPos = "";
                    foreach (DataRow drconf in dtconf.Rows)
                    {
                        try
                        {
                            if (dr["POSITIONFLAG"].ToString() == dr["POSITIONID"].ToString())
                            {
                                if ((drconf["FIELD_NAME"].ToString() == "FIRSTNAME") || (drconf["FIELD_NAME"].ToString() == "PositionTitle"))
                                {
                                    InfoPos += ";    |" +
                                                     drconf["FIELD_ROW"].ToString() + "|" +
                                                     drconf["FIELD_COL"].ToString();
                                }
                                else
                                {
                                    InfoPos += ";" + dr[drconf["FIELD_NAME"].ToString()].ToString() + "|" +
                                                     drconf["FIELD_ROW"].ToString() + "|" +
                                                     drconf["FIELD_COL"].ToString();
                                }
                            }
                            else
                            {
                                InfoPos += ";" + dr[drconf["FIELD_NAME"].ToString()].ToString() + "|" +
                                                 drconf["FIELD_ROW"].ToString() + "|" +
                                                 drconf["FIELD_COL"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            string errMsg = ex.ToString();
                            Console.WriteLine("  Message: {0}", errMsg);
                        }
                    }

                    string sColor = "0", sBackColor = "0";
                    if (Convert.ToInt32(dr["OLEVEL"].ToString()) <= (Convert.ToInt32(LevelUpto)))
                    {
                        if (!((dr["POSITIONFLAG"].ToString() == dr["POSITIONID"].ToString()) && (dr["NOR_COUNT"].ToString() == "0") && (dr["SOC_COUNT"].ToString() == "0")))
                        {
                            if (View == "OV")
                            {
                                try
                                {
                                    if (SearchIDs.Length >= 2)
                                    {
                                        if (dr[OPR_LEVEL_ID].ToString() == SearchIDs[0] && dr[OPR_PARENT_ID].ToString() == SearchIDs[1]) sBackColor = "#FFFF00"; else sBackColor = "0";
                                    }
                                    else sBackColor = "0";
                                    string olevel = "";
                                    if (dr[OPR_PARENT_ID].ToString() == Level)
                                    {
                                        olevel = "1";

                                    }
                                    else
                                    {
                                        olevel = "2";
                                    }

                                    if (dr[OPR_LEVEL_ID].ToString() == Level)
                                    {
                                        olevel = "0";
                                        dr[OPR_PARENT_ID] = "-1";
                                    }


                                    theObjectInf.Add(new ObjectInf(dr[OPR_LEVEL_ID].ToString(),
                                                                     InfoPos.Substring(1),
                                                                     dr[OPR_PARENT_ID].ToString(),

                                                                    olevel,
                                                                     0, 0, 0, 0, 175, Original_Height,
                                                                     dr["NEXT_LEVEL_FLAG"].ToString(),
                                                                     dr["GRAY_COLORED_FLAG"].ToString(),
                                                                     dr["DOTTED_LINE_FLAG"].ToString(),
                                                                     dr["SHOW_FULL_BOX"].ToString(),
                                                                     dr["LANGUAGE_SELECTED"].ToString(),
                                                                     dr["SORTNO"].ToString(),
                                                                     dr["POSITIONFLAG"].ToString(),
                                                                     sColor,
                                                                     sBackColor,
                                                                     dr["FLAG"].ToString(),
                                                                     "0",
                                                                     dr["GDDBID"].ToString()
                                                                     ));

                                }
                                catch (Exception ex)
                                {
                                    string errMsg = ex.ToString();
                                    Console.WriteLine("  Message: {0}", errMsg);
                                }
                            }
                            else if (View == "LV")
                            {
                                if (SearchIDs.Length >= 2)
                                {
                                    if (dr[LGL_LEVEL_ID].ToString() == SearchIDs[0] && dr[LGL_PARENT_ID].ToString() == SearchIDs[1]) sBackColor = "#F5DEB3"; else sBackColor = "0";
                                }
                                else sBackColor = "0";
                                string Leglevel = "";
                                if (dr[LGL_PARENT_ID].ToString() == Level)
                                {
                                    Leglevel = "1";

                                }
                                else
                                {
                                    Leglevel = "2";
                                }

                                if (dr[LGL_LEVEL_ID].ToString() == Level)
                                {
                                    Leglevel = "0";
                                    dr[LGL_PARENT_ID] = "-1";
                                }

                                theObjectInf.Add(new ObjectInf(dr[LGL_LEVEL_ID].ToString(),
                                                                 InfoPos.Substring(1),
                                                                 dr[LGL_PARENT_ID].ToString(),

                                                                 Leglevel,
                                                                 0, 0, 0, 0, 175, Original_Height,
                                                                 dr["NEXT_LEVEL_FLAG"].ToString(),
                                                                 dr["GRAY_COLORED_FLAG"].ToString(),
                                                                 dr["DOTTED_LINE_FLAG"].ToString(),
                                                                 dr["SHOW_FULL_BOX"].ToString(),
                                                                 dr["LANGUAGE_SELECTED"].ToString(),
                                                                 dr["SORTNO"].ToString(),
                                                                 dr["POSITIONFLAG"].ToString(),
                                                                 sColor,
                                                                 sBackColor,
                                                                 dr["FLAG"].ToString(),
                                                                 "0",
                                                                 dr["GDDBID"].ToString()
                                                                 ));
                            }

                            TableHTML += "<tr>";
                            foreach (DataRow drFI in dtFieldInformation.Rows)
                            {
                                try
                                {
                                    if (!(CONFIG_INFO.Contains(drFI["FIELD_NAME"].ToString())))
                                    {
                                        if (drFI["TABLE_IND"].ToString() == "Y")
                                            TableHTML += "<td>" + dr[drFI["FIELD_NAME"].ToString()].ToString() + "</td>";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string errMsg = ex.ToString();
                                    Console.WriteLine("Message: {0}", errMsg);
                                }
                            }
                            TableHTML += "</tr>";
                        }
                    }
                }
                TableHTML += "</tbody></table></div>";

                // Json object to show level information
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JsonFieldInfo = javaScriptSerializer.Serialize(theObjectInf);

                return theObjectInf;
            }

            // Gets the Table Info from the DB
            public DataTable GetTableInfo(string View, string ShowLevel, string CN, string LevelDate, string LevelUpto, string Language)
            {

                DataTable workTable = new DataTable("ChartTable");

                string InfoPos = "", SqlQry = "";
                string sFM = "";
                if (FlagFM == "N") sFM = " AND DOTTED_LINE_FLAG='N'";
                DateTime fixtureDate = DateTime.Parse(LevelDate, new CultureInfo("en-GB"));
                fixtureDate = fixtureDate.AddDays(-1);
                string prevDate = fixtureDate.Year.ToString() + "-" + fixtureDate.Month.ToString("d2") + "-" + fixtureDate.Day.ToString("d2");
                string keyDate = LevelDate.Substring(6, 4) + "-" + LevelDate.Substring(3, 2) + "-" + LevelDate.Substring(0, 2);
                string SelLang = "";
                if (Language == "EN")
                    SelLang = "EN";
                else SelLang = "00";
                if (View == "OV")
                {
                    if (LevelUpto == "1")
                    {
                        SqlQry = "SELECT DISTINCT a.*, b.OLEVEL,c.*, ISNULL(d.orgunitText,'') OrgunitText, ISNULL(e.positiontitle,'') positiontitle, ISNULL(f.POSITIONID, '0') positionflag, g.FullName " +
                                        " FROM REORG_LEVEL_INFO a JOIN LEVEL_NOR_SOC c ON a." + OPR_LEVEL_ID + "=c.LEVEL_ID AND a." +
                                        OPR_PARENT_ID + "=c.PARENT_ID  " +
                                        " JOIN TEXT_ORG d on a.orgunit = d.orgunit " +
                                        " JOIN TEXT_POS e on a.PositionID = e.PositionID " +
                                        " JOIN TEXT_EMP g on a.PERSONID = g.PERSONID  AND g.Language_selected = '" + SelLang + "'" +
                                        " LEFT JOIN PositionManagement f on a.PositionID = f.PositionID " +
                                        " , (SELECT '" + ShowLevel + "' OID, '-1' NextPositionID,  '0' OLEVEL " +
                                        " UNION ALL SELECT DISTINCT " + OPR_LEVEL_ID + " OID, li.NextPositionID,  '1' OLEVEL FROM REORG_LEVEL_INFO LI WHERE " +
                                        OPR_PARENT_ID + "='" + ShowLevel + "' AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "'" +
                                        " ) b " +
                                        " WHERE a." + OPR_LEVEL_ID + "=b.OID AND a.LANGUAGE_SELECTED = '" + "EN" + "' AND c.KEY_DATE='" + keyDate + "'" +
                                        " AND A.END_DATE >= '" + keyDate + "'  AND A.START_DATE <= '" + keyDate + "' AND " +
                                        " d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate +
                                        "' AND d.LANGUAGE_SELECTED = '" + "EN" + "' " +
                                        " AND e.END_DATE >= '" + keyDate + "'  AND e.START_DATE <= '" + keyDate +
                                        "' AND g.END_DATE >= '" + keyDate + "'  AND g.START_DATE <= '" + keyDate +
                                        "' AND e.LANGUAGE_SELECTED = '" + "EN" + "' " +
                                        "  AND (( b.NextPositionID = a.NextPositionID )  or (( b.NextPositionID = '-1' ) AND (a.NextPositionID='" + PreviousLevel + "'))) " + sFM;
                    }
                    else if (LevelUpto == "2")
                    {
                        SqlQry = "SELECT DISTINCT a.*, b.OLEVEL, c.* , ISNULL(d.orgunitText,'') OrgunitText, ISNULL(e.positiontitle,'') positiontitle, ISNULL(f.POSITIONID, '0') positionflag, g.FullName " +
                                         " FROM REORG_LEVEL_INFO a JOIN LEVEL_NOR_SOC c ON a." +
                                        OPR_LEVEL_ID + "=c.LEVEL_ID AND a." + OPR_PARENT_ID +
                                        "=c.PARENT_ID " +
                                        " JOIN TEXT_ORG d on a.orgunit = d.orgunit " +
                                        " JOIN TEXT_POS e on a.PositionID = e.PositionID " +
                                        " JOIN TEXT_EMP g on a.PERSONID = g.PERSONID  AND g.Language_selected = '" + SelLang + "'" +
                                        " LEFT JOIN PositionManagement f on a.PositionID = f.PositionID " +
                                        ", (SELECT '" + ShowLevel + "' OID, '-1' NextPositionID,  '0' OLEVEL " +
                                        " UNION ALL SELECT DISTINCT " + OPR_LEVEL_ID + " OID, li.NextpositionID, '1' OLEVEL FROM REORG_LEVEL_INFO LI WHERE " +
                                        OPR_PARENT_ID + "='" + ShowLevel + "' AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "'" +
                                        " UNION ALL SELECT DISTINCT " + OPR_LEVEL_ID + " OID,  li.NextpositionID, '2' OLEVEL FROM REORG_LEVEL_INFO LI WHERE " +
                                        OPR_PARENT_ID + " IN (SELECT " + OPR_LEVEL_ID + " FROM REORG_LEVEL_INFO WHERE " + OPR_PARENT_ID + "='" + ShowLevel + "' AND END_DATE >= '" +
                                        keyDate + "'  AND START_DATE <= '" + keyDate + "')  AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "') b " +
                                        " WHERE a." + OPR_LEVEL_ID + "=b.OID AND a.LANGUAGE_SELECTED = '" + "EN" + "' AND c.KEY_DATE='" + keyDate + "' " +
                                        " AND d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate +
                                        "' AND d.LANGUAGE_SELECTED = '" + "EN" + "' " +
                                        " AND e.END_DATE >= '" + keyDate + "'  AND e.START_DATE <= '" + keyDate +
                                        "' AND e.LANGUAGE_SELECTED = '" + "EN" + "' " +
                                        " AND g.END_DATE >= '" + keyDate + "'  AND g.START_DATE <= '" + keyDate + "' " +
                                        " AND A.END_DATE >= '" + keyDate + "'  AND A.START_DATE <= '" + keyDate + "' " +
                                        "  AND (( b.NextPositionID = a.NextPositionID )  or (( b.NextPositionID = '-1' ) AND (a.NextPositionID='" + PreviousLevel + "'))) " + sFM;
                    }
                }
                else if (View == "LV")
                {
                    if (LevelUpto == "1")
                    {
                        SqlQry = "SELECT DISTINCT a.*, b.OLEVEL, c.*, ISNULL(d.orgunitText,'') OrgunitText, ISNULL(e.positiontitle,'') positiontitle, ISNULL(f.POSITIONID, '0') positionflag, g.FullName " +
                                " FROM REORG_LEGAL_INFO a JOIN LEGAL_NOR_SOC c ON  a." + LGL_LEVEL_ID + "=c.LEVEL_ID AND a." +
                                  LGL_PARENT_ID + "=c.PARENT_ID " +
                                " LEFT JOIN TEXT_ORG d on a.nextpositionid = d.orgunit AND D.LANGUAGE_SELECTED = '" + Language + "' " +
                                " LEFT JOIN TEXT_POS e on a.PositionID = e.PositionID  AND E.LANGUAGE_SELECTED = '" + Language + "' " +
                                " JOIN TEXT_EMP g on a.PERSONID = g.PERSONID  AND g.Language_selected = '" + SelLang + "'" +
                                " LEFT JOIN PositionManagement f on a.PositionID = f.PositionID " +
                                ",  (SELECT '" + ShowLevel +
                                "' OID, '-1' NextLevelOrg, '0' OLEVEL " +
                                " UNION ALL SELECT DISTINCT " + LGL_LEVEL_ID + " OID, li.NextLevelOrg, '1' OLEVEL FROM REORG_LEGAL_INFO LI WHERE " +
                                  LGL_PARENT_ID + "='" + ShowLevel + "'  AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "'" +
                                ") b " +
                                " WHERE a." + LGL_LEVEL_ID + "=b.OID   AND A.LANGUAGE_SELECTED = '" + "EN" + "' " +
                                " AND c.KEY_DATE='" + keyDate + "'" +
                                " AND A.END_DATE >= '" + keyDate + "'  AND A.START_DATE <= '" + keyDate + "' " +
                                " AND d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate + "' " +
                                " AND g.END_DATE >= '" + keyDate + "'  AND g.START_DATE <= '" + keyDate + "' " +
                            //" AND (( d.LANGUAGE_SELECTED = '" + Language + "'  AND D.ORGUNIT <> '" + ShowLevel + "') OR D.ORGUNIT = '" + ShowLevel + "') " +
                                " AND e.END_DATE >= '" + keyDate + "'  AND e.START_DATE <= '" + keyDate +
                                "' AND e.LANGUAGE_SELECTED = '" + Language + "' " +
                                "  AND (( b.NextLevelOrg = a.NextLevelOrg )  or (( b.NextLevelOrg = '-1' ) AND ( a.NextLevelOrg = '" + PreviousLevel + "' ))) ";
                    }
                    else if (LevelUpto == "2")
                    {
                        SqlQry = "SELECT DISTINCT a.*, b.OLEVEL,   c.* " +
                            " , ISNULL(d.orgunitText,'') OrgunitText, ISNULL(e.positiontitle,'') positiontitle, ISNULL(f.POSITIONID, '0') positionflag , g.FullName " +
                            " FROM REORG_LEGAL_INFO a JOIN LEGAL_NOR_SOC c ON  a." +
                            LGL_LEVEL_ID + "=c.LEVEL_ID AND a." + LGL_PARENT_ID + "=c.PARENT_ID " +
                            " JOIN TEXT_ORG d on a.nextpositionid = d.orgunit " +
                            " JOIN TEXT_POS e on a.PositionID = e.PositionID " +
                            " JOIN TEXT_EMP g on a.PERSONID = g.PERSONID  AND g.Language_selected = '" + SelLang + "'" +
                            " LEFT JOIN PositionManagement f on a.PositionID = f.PositionID " +
                            " , (SELECT '" +
                            ShowLevel + "' OID, '-1' NextLevelOrg, '0' OLEVEL " +
                            " UNION ALL SELECT DISTINCT " + LGL_LEVEL_ID + " OID, li.NextLevelOrg, '1' OLEVEL FROM REORG_LEGAL_INFO LI WHERE " +
                            LGL_PARENT_ID + "='" + ShowLevel + "'  AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "'" +
                            " UNION ALL SELECT DISTINCT " + LGL_LEVEL_ID + " OID, li.NextLevelOrg, '2' OLEVEL  FROM REORG_LEGAL_INFO LI WHERE " +
                            LGL_PARENT_ID + " IN (SELECT " + LGL_LEVEL_ID + " FROM REORG_LEGAL_INFO WHERE " + LGL_PARENT_ID + "='" + ShowLevel + "'  AND END_DATE >= '" +
                            keyDate + "'  AND START_DATE <= '" + keyDate + "')  AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "') b " +
                            " WHERE a." + LGL_LEVEL_ID + "=b.OID    AND A.LANGUAGE_SELECTED = '" + "EN" + "' " +
                            " AND c.KEY_DATE='" + keyDate + "'" +
                            " AND A.END_DATE >= '" + keyDate + "'  AND A.START_DATE <= '" + keyDate + "' " +
                            " AND d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate + "' " +
                            " AND G.END_DATE >= '" + keyDate + "'  AND G.START_DATE <= '" + keyDate + "' " +
                            //" AND (( d.LANGUAGE_SELECTED = '" + Language + "'  AND D.ORGUNIT <> '" + ShowLevel + "') OR D.ORGUNIT = '" + ShowLevel + "') " +
                            " AND e.END_DATE >= '" + keyDate + "'  AND e.START_DATE <= '" + keyDate +
                            "' AND e.LANGUAGE_SELECTED = '" + Language + "' " +
                            "  AND (( b.NextLevelOrg = a.NextLevelOrg )  or (( b.NextLevelOrg = '-1' ) AND ( a.NextLevelOrg = '" + PreviousLevel + "' ))) ";
                    }
                }

                Common csobj = new Common();
                DataTable dtlbl = null;
                DataTable dtconf = csobj.SQLReturnDataTable("SELECT * FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + HttpContext.Current.Session["VIEW"].ToString() + "'");
                string OrderBy = "OLEVEL, ";

                if (LevelFlag == "1")
                {
                    OrderBy = "OLEVEL, SORTNO, ";
                }
                if (View == "OV")
                    OrderBy += OPR_PARENT_ID + ", " + OPR_LEVEL_ID;
                else if (View == "LV")
                    OrderBy += LGL_PARENT_ID + ", " + LGL_LEVEL_ID;
                dtlbl = csobj.SQLReturnDataTable(SqlQry + " ORDER BY " + OrderBy);

                int runno = 1;
                foreach (DataRow dr1 in dtlbl.Rows)
                {
                    if (dr1["OLEVEL"].ToString() != "0")
                    {
                        dr1["SORTNO"] = runno.ToString();
                        runno++;
                    }
                }

                string[] CONFIG_INFO = SUPPRESS_FIELDS.Split(',');
                string SUP_FIELDS = "";
                for (int Idx = 0; Idx <= CONFIG_INFO.Length - 1; Idx++)
                    SUP_FIELDS += ",\'" + CONFIG_INFO[Idx] + "\'";

                int iTestHead = 0;
                foreach (DataRow dr in dtlbl.Rows)
                {
                    if (iTestHead == 0)
                    {
                        TableHTML = "<table id='tblLevelInfo' style='width:2000px !Important;'>" +
                                    "<thead><tr>";
                        foreach (DataRow drFI in dtFieldInformation.Rows)
                        {
                            if (!(CONFIG_INFO.Contains(drFI["FIELD_NAME"].ToString())))
                            {
                                if (drFI["TABLE_IND"].ToString() == "Y")
                                {
                                    try
                                    {
                                        TableHTML += "<th style=\"text-align:left;\">" + drFI["FIELD_CAPTION"].ToString() + "</th>";
                                        workTable.Columns.Add(drFI["FIELD_CAPTION"].ToString(), typeof(String));
                                    }
                                    catch (Exception ex)
                                    {
                                        string errMsg = ex.ToString();
                                        Console.WriteLine("  Message: {0}", errMsg);
                                    }
                                }
                            }
                        }
                        TableHTML += "</tr></thead><tbody>";
                    }
                    iTestHead++;

                    if (Convert.ToInt32(dr["OLEVEL"].ToString()) <= (Convert.ToInt32(LevelUpto)))
                    {
                        if (!((dr["POSITIONFLAG"].ToString() == dr["POSITIONID"].ToString()) && (dr["NOR_COUNT"].ToString() == "0") && (dr["SOC_COUNT"].ToString() == "0")))
                        {
                            TableHTML += "<tr>";
                            DataRow workRow = workTable.NewRow();
                            foreach (DataRow drFI in dtFieldInformation.Rows)
                            {
                                try
                                {
                                    if (!(CONFIG_INFO.Contains(drFI["FIELD_NAME"].ToString())))
                                    {
                                        if (drFI["TABLE_IND"].ToString() == "Y")
                                        {
                                            TableHTML += "<td>" + dr[drFI["FIELD_NAME"].ToString()].ToString() + "</td>";
                                            workRow[drFI["FIELD_CAPTION"].ToString()] = dr[drFI["FIELD_NAME"].ToString()].ToString();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string errMsg = ex.ToString();
                                    Console.WriteLine("  Message: {0}", errMsg);
                                }
                            }
                            workTable.Rows.Add(workRow);
                            TableHTML += "</tr>";
                        }
                    }
                }
                TableHTML += "</tbody></table>";

                return workTable;
            }

            // Check for level existence
            private string CheckLevelInfo(ObjectInf Obj)
            {
                if (Obj.NextLevelFlag == "Y") return Obj.Id;

                return "NO";
            }

            private int NoOfLevel(List<ObjectInf> theObjectInf, string LId)
            {

                int Idx = 0;
                foreach (ObjectInf Obj in theObjectInf)
                {
                    if (Obj.Level == LId) Idx++;
                }

                return Idx;
            }

            private string GetDistinctLevel(string ParentId, string Level)
            {
                string[] PId = ParentId.Split(',');

                if (PId.Length == 0) return Level;
                for (int Idx = 0; Idx <= PId.Length - 1; Idx++)
                {
                    if (PId[Idx] == Level) return "";
                }

                return Level;
            }

            private string RemoveCommasInLevel(string ParentId)
            {
                string[] PId = ParentId.Split(',');
                string sIds = "";

                if (PId.Length == 0) return "";
                for (int Idx = 0; Idx <= PId.Length - 1; Idx++)
                {
                    if (PId[Idx].Length >= 1) sIds += "," + PId[Idx];
                }

                return sIds.Substring(1);
            }

            private string IdInPId(List<ObjectInf> theObjectInf, string Id)
            {

                string ParentId = "";
                foreach (ObjectInf Obj in theObjectInf)
                {
                    if (Obj.PId == Id) ParentId += "," + Obj.Id;
                }
                if (ParentId == "") return "";

                return RemoveCommasInLevel(ParentId.Substring(1));
            }

            private string LevelInId(List<ObjectInf> theObjectInf, string LevelId, int Level)
            {
                string ParentId = "";
                foreach (ObjectInf Obj in theObjectInf)
                {
                    if (Obj.Level == LevelId)
                    {
                        ParentId += "," + Obj.Id;
                        switch (Level)
                        {
                            case 0:
                                lstObjLevel0.Add(Obj);
                                break;
                            case 1:
                                lstObjLevel1.Add(Obj);
                                break;
                            case 2:
                                lstObjLevel2.Add(Obj);
                                break;
                        }
                    }
                }
                if (ParentId == "") return "";

                return RemoveCommasInLevel(ParentId.Substring(1));
            }

            private ObjectInf GetIdInfo(List<ObjectInf> theObjectInf, string Id)
            {
                foreach (ObjectInf Obj in theObjectInf)
                {
                    if (Obj.Id == Id) return Obj;
                }
                return null;
            }

            private string GetAllLevels(List<ObjectInf> theObjectInf)
            {
                string ParentId = "";
                foreach (ObjectInf Obj in theObjectInf)
                {
                    ParentId += "," + GetDistinctLevel(ParentId, Obj.Level);
                }
                if (ParentId == "") return "";

                return RemoveCommasInLevel(ParentId.Substring(1));
            }

            // Gets the Child height information
            private int GetChildHeight(List<ObjectInf> theObjectInf, string Id)
            {
                int Height = Original_Height_10;
                foreach (ObjectInf Obj in theObjectInf)
                {
                    if (Obj.PId == Id) Height += Obj.Height;
                }

                return Height;
            }
            // Gets the page into which object is to be drawn
            private int GetPageRowStartPosition(int Row)
            {
                int StartPage = 0, EndPage = Output_Height - 2;

                for (int Idx = 0; Idx <= PageSizeNos.Count - 1; Idx++)
                {
                    if (PageSizeNos.Count >= 2)
                    {
                        StartPage = PageSizeNos[Idx];
                        if ((Idx + 1) != PageSizeNos.Count) EndPage = PageSizeNos[Idx + 1] - 1; else EndPage = PageSizeNos[Idx] + Output_Height - 2;
                    }
                    if ((Row >= StartPage) && (Row <= EndPage)) return PageSizeNos[Idx];
                }

                return PageSizeNos[0];
            }


            // Gets the page into which object is to be drawn
            private int GetPageRowPosition(int Row)
            {
                int StartPage = 0, EndPage = Output_Height - 2;

                CurPage = PDFPage;
                for (int Idx = 0; Idx <= PageSizeNos.Count - 1; Idx++)
                {
                    if (PageSizeNos.Count >= 2)
                    {
                        StartPage = PageSizeNos[Idx];
                        if ((Idx + 1) != PageSizeNos.Count) EndPage = PageSizeNos[Idx + 1] - 1; else EndPage = PageSizeNos[Idx] + Output_Height - 2;
                    }
                    if ((Row >= StartPage) && (Row <= EndPage)) CurPage = Idx;
                }

                return Row - PageSizeNos[CurPage];
            }

            // Gets row page
            private int GetRowPage(int Row)
            {
                int StartPage = 0, EndPage = Output_Height - 2, RowPage = 0;

                RowPage = 0;
                for (int Idx = 0; Idx <= PageSizeNos.Count - 1; Idx++)
                {
                    if (PageSizeNos.Count >= 2)
                    {
                        StartPage = PageSizeNos[Idx];
                        if ((Idx + 1) != PageSizeNos.Count) EndPage = PageSizeNos[Idx + 1] - 1; else EndPage = PageSizeNos[Idx] + Output_Height - 2;
                    }
                    if ((Row >= StartPage) && (Row <= EndPage)) RowPage = Idx;
                }

                return RowPage;
            }

            // Get the Page existence 
            private bool GetPageExistence(int Row)
            {
                int StartPage = 0, EndPage = Output_Height - 2;

                for (int Idx = 0; Idx <= PageSizeNos.Count - 1; Idx++)
                {
                    if (PageSizeNos.Count >= 2)
                    {
                        StartPage = PageSizeNos[Idx];
                        if ((Idx + 1) != PageSizeNos.Count) EndPage = PageSizeNos[Idx + 1] - 1; else EndPage = PageSizeNos[Idx] + Output_Height - 2;
                    }
                    if ((Row >= StartPage) && (Row <= EndPage)) return true;
                }

                return false;
            }

            // DrawLine X1, Y1, X2, Y2
            private void DrawLine(string Connector, int X1, int Y1, int X2, int Y2)
            {
                int StartPage = GetRowPage(Y1), EndPage = GetRowPage(Y2), LWidth = Convert.ToInt16(LineWidth);
                if (StartPage != EndPage)
                {
                    if (Connector.Substring(0, 6) != "Middle")
                    {
                        for (int Idx = StartPage; Idx <= EndPage; Idx++)
                        {
                            if (Idx == StartPage)
                            {
                                if (strOutput == "PDF")
                                {
                                    MyLine = new ceTe.DynamicPDF.PageElements.Line(X1, GetPageRowPosition(Y1) + 15, X2, Output_Height - 1, GetPDFLineColor(LineColor));
                                    MyLine.Width = LWidth;
                                    MyPage[Idx].Elements.Add(MyLine);
                                    MyRect = new ceTe.DynamicPDF.PageElements.Rectangle(X1 - 20, Output_Height - 1, 50, 12);
                                    MyPage[Idx].Elements.Add(MyRect);
                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label(Connector, X1 - 20, Output_Height + 1, 48, 8, ceTe.DynamicPDF.Font.Helvetica, 7, ceTe.DynamicPDF.TextAlign.Center);
                                    MyPage[Idx].Elements.Add(MyLabel);
                                }
                                else if (strOutput == "PPT")
                                {
                                    GraphicImg[Idx].DrawLine(blackPen, X1, GetPageRowPosition(Y1) + 15, X2, Output_Height - 1);
                                    GraphicImg[Idx].DrawRectangle(blackPen, X1 - 20, Output_Height - 1, 65, 17);
                                    GraphicImg[Idx].DrawString(Connector, drawFont, drawBrush, X1 - 20, Output_Height + 1);
                                }
                            }
                            else if (Idx == EndPage)
                            {
                                if (strOutput == "PDF")
                                {
                                    MyLine = new ceTe.DynamicPDF.PageElements.Line(X1, 12, X2, GetPageRowPosition(Y2) + 15, GetPDFLineColor(LineColor));
                                    MyLine.Width = LWidth;
                                    MyPage[Idx].Elements.Add(MyLine);
                                    MyRect = new ceTe.DynamicPDF.PageElements.Rectangle(X1 - 20, 0, 50, 12);
                                    MyPage[Idx].Elements.Add(MyRect);
                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label(Connector, X1 - 18, 2, 48, 8, ceTe.DynamicPDF.Font.Helvetica, 7, ceTe.DynamicPDF.TextAlign.Center);
                                    MyPage[Idx].Elements.Add(MyLabel);
                                }
                                else if (strOutput == "PPT")
                                {
                                    GraphicImg[Idx].DrawLine(blackPen, X1, 12, X2, GetPageRowPosition(Y2) + 15);
                                    GraphicImg[Idx].DrawRectangle(blackPen, X1 - 20, 0, 65, 17);
                                    GraphicImg[Idx].DrawString(Connector, drawFont, drawBrush, X1 - 18, 2);
                                }
                            }
                            else
                            {
                                if (strOutput == "PDF")
                                {
                                    MyLine = new ceTe.DynamicPDF.PageElements.Line(X1, 12, X2, Output_Height - 1, GetPDFLineColor(LineColor));
                                    MyLine.Width = LWidth;
                                    MyPage[Idx].Elements.Add(MyLine);

                                    MyRect = new ceTe.DynamicPDF.PageElements.Rectangle(X1 - 20, 0, 50, 12);
                                    MyPage[Idx].Elements.Add(MyRect);
                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label(Connector, X1 - 18, 2, 48, 8, ceTe.DynamicPDF.Font.Helvetica, 7, ceTe.DynamicPDF.TextAlign.Center);
                                    MyPage[Idx].Elements.Add(MyLabel);

                                    MyRect = new ceTe.DynamicPDF.PageElements.Rectangle(X1 - 20, Output_Height - 1, 50, 12);
                                    MyPage[Idx].Elements.Add(MyRect);
                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label(Connector, X1 - 20, Output_Height + 1, 48, 8, ceTe.DynamicPDF.Font.Helvetica, 7, ceTe.DynamicPDF.TextAlign.Center);
                                    MyPage[Idx].Elements.Add(MyLabel);
                                }
                                else if (strOutput == "PPT")
                                {
                                    GraphicImg[Idx].DrawLine(blackPen, X1, 12, X2, Output_Height - 1);

                                    GraphicImg[Idx].DrawRectangle(blackPen, X1 - 20, 0, 65, 17);
                                    GraphicImg[Idx].DrawString(Connector, drawFont, drawBrush, X1 - 18, 2);

                                    GraphicImg[Idx].DrawRectangle(blackPen, X1 - 20, Output_Height - 1, 65, 17);
                                    GraphicImg[Idx].DrawString(Connector, drawFont, drawBrush, X1 - 20, Output_Height + 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int Idx = StartPage; Idx <= EndPage; Idx++)
                        {
                            if (Idx == StartPage)
                            {
                                if (strOutput == "PDF")
                                {
                                    MyLine = new ceTe.DynamicPDF.PageElements.Line(X1, GetPageRowPosition(Y1), X2, Output_Height - 36, GetPDFLineColor(LineColor));
                                    MyLine.Width = LWidth;
                                    MyPage[Idx].Elements.Add(MyLine);
                                    MyCircle = new ceTe.DynamicPDF.PageElements.Circle(X1, Output_Height - 26, 8);
                                    MyPage[Idx].Elements.Add(MyCircle);
                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label("M", X1 - 25, Output_Height - 31, 48, 8, ceTe.DynamicPDF.Font.Helvetica, 7, ceTe.DynamicPDF.TextAlign.Center);
                                    MyPage[Idx].Elements.Add(MyLabel);
                                }
                                else if (strOutput == "PPT")
                                {
                                    GraphicImg[Idx].DrawLine(blackPen, X1, GetPageRowPosition(Y1), X2, Output_Height - 36);
                                    GraphicImg[Idx].DrawEllipse(blackPen, X1 - 8, Output_Height - 36, 16, 16);
                                    GraphicImg[Idx].DrawString("M", drawFont, drawBrush, X1 - 6, Output_Height - 33);
                                }
                            }
                            else if (Idx == EndPage)
                            {
                                if (Connector.Substring(7, 1) == "0")
                                {
                                    if (strOutput == "PDF")
                                    {
                                        MyCircle = new ceTe.DynamicPDF.PageElements.Circle(X1, 25, 8);
                                        MyPage[Idx].Elements.Add(MyCircle);
                                        MyLabel = new ceTe.DynamicPDF.PageElements.Label("M", X1 - 25, 20, 48, 8, ceTe.DynamicPDF.Font.Helvetica, 7, ceTe.DynamicPDF.TextAlign.Center);
                                        MyPage[Idx].Elements.Add(MyLabel);
                                        MyLine = new ceTe.DynamicPDF.PageElements.Line(X1, 35, X2, GetPageRowPosition(Y2), GetPDFLineColor(LineColor));
                                        MyLine.Width = LWidth;
                                        MyPage[Idx].Elements.Add(MyLine);
                                    }
                                    else if (strOutput == "PPT")
                                    {
                                        GraphicImg[Idx].DrawEllipse(blackPen, X1 - 8, 20, 16, 16);
                                        GraphicImg[Idx].DrawString("M", drawFont, drawBrush, X1 - 6, 22);
                                        GraphicImg[Idx].DrawLine(blackPen, X1, 35, X2, GetPageRowPosition(Y2));
                                    }
                                }
                                else
                                {
                                    if (strOutput == "PDF")
                                    {
                                        MyCircle = new ceTe.DynamicPDF.PageElements.Circle(X1, 5, 8);
                                        MyPage[Idx].Elements.Add(MyCircle);
                                        MyLabel = new ceTe.DynamicPDF.PageElements.Label("M", X1 - 25, 0, 48, 8, ceTe.DynamicPDF.Font.Helvetica, 7, ceTe.DynamicPDF.TextAlign.Center);
                                        MyPage[Idx].Elements.Add(MyLabel);
                                    }
                                    else if (strOutput == "PPT")
                                    {
                                        GraphicImg[Idx].DrawEllipse(blackPen, X1 - 8, 13, 16, 16);
                                        GraphicImg[Idx].DrawString("M", drawFont, drawBrush, X1 - 6, 0);
                                    }
                                }
                            }
                            else
                            {
                                if (strOutput == "PDF")
                                {
                                    MyCircle = new ceTe.DynamicPDF.PageElements.Circle(X1, 25, 8);
                                    MyPage[Idx].Elements.Add(MyCircle);

                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label("M", X1 - 25, 20, 48, 8, ceTe.DynamicPDF.Font.Helvetica, 7, ceTe.DynamicPDF.TextAlign.Center);
                                    MyPage[Idx].Elements.Add(MyLabel);
                                    MyLine = new ceTe.DynamicPDF.PageElements.Line(X1, 35, X2, Output_Height - 21, GetPDFLineColor(LineColor));
                                    MyLine.Width = LWidth;
                                    MyPage[Idx].Elements.Add(MyLine);

                                    MyCircle = new ceTe.DynamicPDF.PageElements.Circle(X1, Output_Height - 11, 8);
                                    MyPage[Idx].Elements.Add(MyCircle);
                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label("M", X1 - 25, Output_Height - 16, 48, 8, ceTe.DynamicPDF.Font.Helvetica, 7, ceTe.DynamicPDF.TextAlign.Center);
                                    MyPage[Idx].Elements.Add(MyLabel);
                                }
                                else if (strOutput == "PPT")
                                {
                                    GraphicImg[Idx].DrawEllipse(blackPen, X1 - 8, 20, 16, 16);

                                    GraphicImg[Idx].DrawString("M", drawFont, drawBrush, X1 - 6, 22);
                                    GraphicImg[Idx].DrawLine(blackPen, X1, 35, X2, Output_Height - 21);

                                    GraphicImg[Idx].DrawEllipse(blackPen, X1 - 8, Output_Height - 20, 16, 16);
                                    GraphicImg[Idx].DrawString("M", drawFont, drawBrush, X1 - 6, Output_Height - 18);
                                }
                            }
                        }
                    }

                    return;
                }
                else
                {
                    if (strOutput == "PDF")
                    {
                        MyLine = new ceTe.DynamicPDF.PageElements.Line(X1, GetPageRowPosition(Y1) + 15, X2, GetPageRowPosition(Y2) + 15, GetPDFLineColor(LineColor));
                        MyLine.Width = LWidth;
                        MyPage[CurPage].Elements.Add(MyLine);
                    }
                    else if (strOutput == "PPT")
                    {
                        int Y1Inf = GetPageRowPosition(Y1) + 15, Y2Inf = GetPageRowPosition(Y2) + 15;
                        GraphicImg[CurPage].DrawLine(blackPen, X1, Y1Inf, X2, Y2Inf);
                    }
                }
            }

            private ceTe.DynamicPDF.Color GetPDFLineColor(string LineColor)
            {
                if (LineColor == "#D3D3D3") return ceTe.DynamicPDF.RgbColor.LightGrey;
                else if (LineColor == "#DCDCDC") return ceTe.DynamicPDF.RgbColor.Gainsboro;
                else if (LineColor == "#C0C0C0") return ceTe.DynamicPDF.RgbColor.Silver;
                else if (LineColor == "#A9A9A9") return ceTe.DynamicPDF.RgbColor.DarkGray;
                else if (LineColor == "#808080") return ceTe.DynamicPDF.RgbColor.Gray;
                else if (LineColor == "#696969") return ceTe.DynamicPDF.RgbColor.DimGray;
                else if (LineColor == "#778899") return ceTe.DynamicPDF.RgbColor.LightSlateGray;
                else if (LineColor == "#708090") return ceTe.DynamicPDF.RgbColor.SlateGray;
                else if (LineColor == "#2F4F4F") return ceTe.DynamicPDF.RgbColor.DarkGray;
                else if (LineColor == "#000000") return ceTe.DynamicPDF.RgbColor.Black;

                return ceTe.DynamicPDF.RgbColor.LightGrey;
            }

            private System.Drawing.Color GetDrawingLineColor(string LineColor)
            {
                if (LineColor == "#D3D3D3") return System.Drawing.Color.LightGray;
                else if (LineColor == "#DCDCDC") return System.Drawing.Color.Gainsboro;
                else if (LineColor == "#C0C0C0") return System.Drawing.Color.Silver;
                else if (LineColor == "#A9A9A9") return System.Drawing.Color.DarkGray;
                else if (LineColor == "#808080") return System.Drawing.Color.Gray;
                else if (LineColor == "#696969") return System.Drawing.Color.DimGray;
                else if (LineColor == "#778899") return System.Drawing.Color.LightSlateGray;
                else if (LineColor == "#708090") return System.Drawing.Color.SlateGray;
                else if (LineColor == "#2F4F4F") return System.Drawing.Color.DarkGray;
                else if (LineColor == "#000000") return System.Drawing.Color.Black;

                return System.Drawing.Color.LightGray;
            }

            // DrawRectangle X1, Y1, W, H
            private void DrawRectangle(int X1, int Y1, int W, int H)
            {
                int Row = Y1 + H;
                if (Row >= Output_Height)
                {
                    if (!(GetPageExistence(Row)))
                    {
                        PageSizeNos.Add(Y1);
                        if (strOutput == "PDF")
                        {
                            //MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(PageSize.B4, PageOrientation.Landscape, 15.0F);
                            if (LevelCount == 4)
                                MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(1080F, 1050F, 15.3F);
                            else
                                MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(1600F, 1050F, 15.3F);

                            //MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(1050F, 768F, 15.3F);
                        }
                        else if (strOutput == "PPT")
                        {

                            if (LevelCount == 4)
                                ImageOut[PageSizeNos.Count - 1] = new Bitmap(1024, Output_Height + 30);
                            else
                                ImageOut[PageSizeNos.Count - 1] = new Bitmap(1600, Output_Height + 30);

                            // Set DPI of image (xDpi, yDpi)
                            ImageOut[PageSizeNos.Count - 1].SetResolution(256.0F, 256.0F);

                            GraphicImg[PageSizeNos.Count - 1] = Graphics.FromImage(ImageOut[PageSizeNos.Count - 1]);
                            GraphicImg[PageSizeNos.Count - 1].Clear(System.Drawing.Color.White);
                        }
                        MaxPage = PageSizeNos.Count;
                    }
                    if (strOutput == "PDF")
                    {
                        MyRect = new ceTe.DynamicPDF.PageElements.Rectangle(X1, GetPageRowPosition(Y1) + 15, W, H);
                        MyPage[CurPage].Elements.Add(MyRect);
                    }
                    else if (strOutput == "PPT")
                    {
                        int Y1Inf = GetPageRowPosition(Y1) + 15;
                        GraphicImg[CurPage].DrawRectangle(blackPen, X1, Y1Inf, W, H);
                    }

                    return;
                }
                CurPage = PDFPage;
                if (strOutput == "PDF")
                {
                    MyRect = new ceTe.DynamicPDF.PageElements.Rectangle(X1, Y1 + 15, W, H);
                    MyPage[CurPage].Elements.Add(MyRect);
                }
                else if (strOutput == "PPT")
                {
                    int Y1Inf = GetPageRowPosition(Y1) + 15;
                    GraphicImg[CurPage].DrawRectangle(blackPen, X1, Y1Inf, W, H);
                }
            }

            // Draws the Div PPT Image
            private void DrawDivPPTImage(string sFP, int iPage, int X, int Y, int W, int H, ObjectInf Obj)
            {
                Pen Pen = null;
                string[] sParam = sFP.Split('|');
                if (Obj.ShowFullBox == "N")
                {
                    if (Obj.Level == "1")
                    {
                        int H1 = H - Convert.ToInt16(sParam[2]);
                        H = Convert.ToInt16(sParam[2]);
                        if (LevelFlag == "2")
                            GraphicImg[CurPage].DrawLine(blackPen, X + 10, Y + H, X + 10, Y + H + H1 + 1);
                    }
                    else if ((Obj.Level == "2") || (Obj.Level == "0"))
                    {
                        Y = Y + (H - Convert.ToInt16(sParam[2]));
                        H = Convert.ToInt16(sParam[2]);
                    }
                }
                if (Obj.ColorFlag != "0") sParam[0] = Obj.ColorFlag;
                switch (sParam[0].ToLower())
                {
                    case "#663300":
                        Pen = new Pen(System.Drawing.Color.Brown, Convert.ToInt16(sParam[4]));
                        break;
                    case "#ffb266":
                        Pen = new Pen(System.Drawing.Color.Orange, Convert.ToInt16(sParam[4]));
                        break;
                    case "#ffffff":
                        Pen = new Pen(System.Drawing.Color.White, Convert.ToInt16(sParam[4]));
                        break;
                    case "#0000ff":
                        Pen = new Pen(System.Drawing.Color.Blue, Convert.ToInt16(sParam[4]));
                        break;
                    case "#008000":
                        Pen = new Pen(System.Drawing.Color.Green, Convert.ToInt16(sParam[4]));
                        break;
                    case "#911414":
                        Pen = new Pen(System.Drawing.Color.DarkRed, Convert.ToInt16(sParam[4]));
                        break;
                    case "#ff0000":
                        Pen = new Pen(System.Drawing.Color.Red, Convert.ToInt16(sParam[4]));
                        break;
                    case "#000000":
                        Pen = new Pen(System.Drawing.Color.Black, Convert.ToInt16(sParam[4]));
                        break;
                }
                if (Obj.BackColor.ToLower() == "#ffb266")
                {
                    Brush brush = new SolidBrush(System.Drawing.Color.Orange);
                    GraphicImg[iPage].FillRectangle(brush, X, Y, W, H);
                }
                else
                {
                    if (Obj.GrayColourFlag == "Y")
                    {
                        Brush brush = new SolidBrush(System.Drawing.Color.LightGray);
                        GraphicImg[iPage].FillRectangle(brush, X, Y, W, H);
                    }
                    if (Obj.DottedLineFlag == "Y")
                    {
                        Pen.DashStyle = DashStyle.Dash;
                        GraphicImg[iPage].DrawRectangle(Pen, X, Y, W, H);
                    }
                    else
                    {
                        Pen.DashStyle = DashStyle.Solid;
                        GraphicImg[iPage].DrawRectangle(Pen, X, Y, W, H);
                    }
                }


                // Rectangle information
                RectPPT.Col = X;
                RectPPT.Row = Y;
                RectPPT.Width = W;
                RectPPT.Height = H;

                if (Obj.ShowFullBox == "Y")
                {
                    int iCol = Convert.ToInt32(sParam[1]) + X;
                    int iRow = Convert.ToInt32(sParam[2]) + Y;
                    switch (sParam[3].ToLower())
                    {
                        case "#663300":
                            Pen = new Pen(System.Drawing.Color.Brown, Convert.ToInt16(sParam[5]));
                            break;
                        case "#ffb266":
                            Pen = new Pen(System.Drawing.Color.Orange, Convert.ToInt16(sParam[5]));
                            break;
                        case "#ffffff":
                            Pen = new Pen(System.Drawing.Color.White, Convert.ToInt16(sParam[5]));
                            break;
                        case "#0000ff":
                            Pen = new Pen(System.Drawing.Color.Blue, Convert.ToInt16(sParam[5]));
                            break;
                        case "#008000":
                            Pen = new Pen(System.Drawing.Color.Green, Convert.ToInt16(sParam[5]));
                            break;
                        case "#911414":
                            Pen = new Pen(System.Drawing.Color.DarkRed, Convert.ToInt16(sParam[5]));
                            break;
                        case "#ff0000":
                            Pen = new Pen(System.Drawing.Color.Red, Convert.ToInt16(sParam[5]));
                            break;
                        case "#000000":
                            Pen = new Pen(System.Drawing.Color.Black, Convert.ToInt16(sParam[5]));
                            break;
                    }
                    Pen.DashStyle = DashStyle.Solid;
                    if (sParam[6] == "Y") GraphicImg[iPage].DrawRectangle(Pen, iCol, iRow, W, 1);
                }
            }

            // Draws the Div PDF Image
            private void DrawDivPDFImage(string sFP, int iPage, int X, int Y, int W, int H, ObjectInf Obj)
            {
                ceTe.DynamicPDF.RgbColor Pen = null;
                string[] sParam = sFP.Split('|');
                if (Obj.ShowFullBox == "N")
                {
                    if (Obj.Level == "1")
                    {
                        int H1 = H - Convert.ToInt16(sParam[2]);
                        H = Convert.ToInt16(sParam[2]);
                        if (LevelFlag == "2")
                        {
                            MyLine = new ceTe.DynamicPDF.PageElements.Line(X + 10, Y + H, X + 10, Y + H + H1 + 1, GetPDFLineColor(LineColor));
                            MyLine.Width = Convert.ToInt16(LineWidth);
                            MyPage[iPage].Elements.Add(MyLine);
                        }
                    }
                    else if ((Obj.Level == "2") || (Obj.Level == "0"))
                    {
                        Y = Y + (H - Convert.ToInt16(sParam[2]));
                        H = Convert.ToInt16(sParam[2]);
                    }
                }
                if (Obj.ColorFlag != "0") sParam[0] = Obj.ColorFlag;
                switch (sParam[0].ToLower())
                {
                    case "#663300":
                        Pen = ceTe.DynamicPDF.WebColor.Brown;
                        break;
                    case "#ffb266":
                        Pen = ceTe.DynamicPDF.WebColor.Orange;
                        break;
                    case "#ffffff":
                        Pen = ceTe.DynamicPDF.WebColor.White;
                        break;
                    case "#0000ff":
                        Pen = ceTe.DynamicPDF.WebColor.Blue;
                        break;
                    case "#008000":
                        Pen = ceTe.DynamicPDF.WebColor.Green;
                        break;
                    case "#911414":
                        Pen = ceTe.DynamicPDF.WebColor.DarkRed;
                        break;
                    case "#ff0000":
                        Pen = ceTe.DynamicPDF.WebColor.Red;
                        break;
                    case "#000000":
                        Pen = ceTe.DynamicPDF.WebColor.Black;
                        break;
                }
                MyRect = new ceTe.DynamicPDF.PageElements.Rectangle(X, Y, W, H);
                MyRect.BorderColor = Pen;
                MyRect.BorderWidth = Convert.ToInt16(sParam[4]);
                if (Obj.GrayColourFlag == "Y") MyRect.FillColor = ceTe.DynamicPDF.WebColor.Gray;
                if (Obj.BackColor.ToLower() == "#ffb266") MyRect.FillColor = ceTe.DynamicPDF.WebColor.Orange;
                if (Obj.DottedLineFlag == "Y") MyRect.BorderStyle = LineStyle.Dash;
                MyPage[iPage].Elements.Add(MyRect);
                MyLabelRect = MyRect;
                if (Obj.ShowFullBox == "Y")
                {
                    int iCol = Convert.ToInt32(sParam[1]) + X;
                    int iRow = Convert.ToInt32(sParam[2]) + Y;
                    switch (sParam[3].ToLower())
                    {
                        case "#663300":
                            Pen = ceTe.DynamicPDF.WebColor.Brown;
                            break;
                        case "#ffb266":
                            Pen = ceTe.DynamicPDF.WebColor.Orange;
                            break;
                        case "#ffffff":
                            Pen = ceTe.DynamicPDF.WebColor.White;
                            break;
                        case "#0000ff":
                            Pen = ceTe.DynamicPDF.WebColor.Blue;
                            break;
                        case "#008000":
                            Pen = ceTe.DynamicPDF.WebColor.Green;
                            break;
                        case "#911414":
                            Pen = ceTe.DynamicPDF.WebColor.DarkRed;
                            break;
                        case "#ff0000":
                            Pen = ceTe.DynamicPDF.WebColor.Red;
                            break;
                        case "#000000":
                            Pen = ceTe.DynamicPDF.WebColor.Black;
                            break;
                    }
                    if (sParam[6] == "Y")
                    {
                        MyLine = new ceTe.DynamicPDF.PageElements.Line(iCol, iRow, iCol + W, iRow);
                        MyLine.Color = Pen;
                        MyLine.Width = Convert.ToInt16(sParam[5]);
                        MyPage[iPage].Elements.Add(MyLine);
                    }
                }
            }

            // DrawImage X1, Y1, W, H
            private void DrawImage(string sFilePath, int X1, int Y1, int W, int H, string Flag, ObjectInf Obj)
            {
                string[] sFP = sFilePath.Split('~');

                int Row = Y1 + H + 20;
                if (Row >= Output_Height)
                {
                    if (!(GetPageExistence(Row)))
                    {
                        if (PreviousObj != null)
                        {
                            if (PreviousObj.Level == "1")
                            {
                                if (!(GetPageExistence(PreviousObj.Row + PreviousObj.Oheight + 20)))
                                    PageSizeNos.Add(PreviousObj.Row - 60);
                                else
                                    PageSizeNos.Add(Y1 - 20);
                            }
                            else PageSizeNos.Add(Y1 - 20);
                        }
                        else PageSizeNos.Add(Y1 - 20);
                        if (strOutput == "PDF")
                        {
                            //MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(PageSize.B4, PageOrientation.Landscape, 15.0F);
                            if (LevelCount == 4)
                                MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(1080F, 1050F, 15.3F);
                            else
                                MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(1600F, 1050F, 15.3F);

                            //MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(1050F, 768F, 15.3F);
                            MaxPage = PageSizeNos.Count;
                        }
                        else if (strOutput == "PPT")
                        {
                            if (LevelCount == 4)
                                ImageOut[PageSizeNos.Count - 1] = new Bitmap(1024, Output_Height + 30);
                            else
                                ImageOut[PageSizeNos.Count - 1] = new Bitmap(1600, Output_Height + 30);

                            // Set DPI of image (xDpi, yDpi)
                            ImageOut[PageSizeNos.Count - 1].SetResolution(256.0F, 256.0F);

                            GraphicImg[PageSizeNos.Count - 1] = Graphics.FromImage(ImageOut[PageSizeNos.Count - 1]);
                            GraphicImg[PageSizeNos.Count - 1].Clear(System.Drawing.Color.White);
                            MaxPage = PageSizeNos.Count;
                        }
                    }
                    if (strOutput == "PDF")
                    {
                        if (sFP[0] == "BRD")
                        {
                            int Y1Inf = GetPageRowPosition(Y1) + 15;
                            DrawDivPDFImage(sFP[1], CurPage, X1, Y1Inf, W - 1, H - 1, Obj);

                            if (Flag == "Y")
                            {
                                thePageObjectInf.Add(new PageObjectInf(Obj.Id,
                                                                       Convert.ToInt32(MyLabelRect.X + (MyLabelRect.Width / 2) - 21),
                                                                       Convert.ToInt32(MyLabelRect.Y + 2 + MyLabelRect.Height),
                                                                       CurPage + iTotalPage,
                                                                       0));

                                MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath("images/downarrow.ico").ToString(),
                                                                                 MyLabelRect.X + (MyLabelRect.Width / 2) - 21,
                                                                                 MyLabelRect.Y + 2 + MyLabelRect.Height);
                                MyImage.Height = 14;
                                MyImage.Width = 42;
                                MyPage[CurPage].Elements.Add(MyImage);
                            }
                            else if (Flag == "U")
                            {
                                MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath("images/uparrow.png").ToString(),
                                                                                 MyLabelRect.X + (MyLabelRect.Width / 2) - 21,
                                                                                 MyLabelRect.Y - 19);
                                MyImage.Height = 14;
                                MyImage.Width = 42;
                                MyPage[CurPage].Elements.Add(MyImage);
                            }

                        }
                        else
                        {
                            MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath(sFP[1]).ToString(), X1, GetPageRowPosition(Y1) + 15);
                            MyImage.Height = H - 1;
                            MyImage.Width = W - 1;
                            MyPage[CurPage].Elements.Add(MyImage);

                            if (Flag == "Y")
                            {
                                int YInf = GetPageRowPosition(Y1) + 14 + H;
                                thePageObjectInf.Add(new PageObjectInf(Obj.Id,
                                                                       Convert.ToInt32(X1 + (W / 2) - 21),
                                                                       Convert.ToInt32(YInf),
                                                                       CurPage + iTotalPage,
                                                                       0));

                                MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath("images/downarrow.ico").ToString(), X1 + (W / 2) - 21, YInf);
                                MyImage.Height = 14;
                                MyImage.Width = 42;
                                MyPage[CurPage].Elements.Add(MyImage);
                            }
                            else if (Flag == "U")
                            {
                                MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath("images/uparrow.png").ToString(), X1 + (W / 2) - 21, Y1 - 3);
                                MyImage.Height = 14;
                                MyImage.Width = 42;
                                MyPage[CurPage].Elements.Add(MyImage);
                            }
                        }
                    }
                    else if (strOutput == "PPT")
                    {
                        int Y1Inf = GetPageRowPosition(Y1) + 15;
                        if (sFP[0] == "BRD")
                        {
                            DrawDivPPTImage(sFP[1], CurPage, X1, Y1Inf, W - 1, H - 1, Obj);

                            if (Flag == "Y")
                            {
                                thePageObjectInf.Add(new PageObjectInf(Obj.Id,
                                                                       Convert.ToInt32(RectPPT.Col + (RectPPT.Width / 2) - 21),
                                                                       Convert.ToInt32(RectPPT.Row + RectPPT.Height),
                                                                       CurPage + iTotalPage,
                                                                       0));

                                GraphicImg[CurPage].DrawImage(BmpDnArrow, RectPPT.Col + (RectPPT.Width / 2) - 21, RectPPT.Row + RectPPT.Height, 42, 14);
                            }
                            else if (Flag == "U")
                            {
                                GraphicImg[CurPage].DrawImage(BmpUpArrow, RectPPT.Col + (RectPPT.Width / 2) - 21, RectPPT.Row - 19, 42, 14);
                            }
                        }
                        else
                        {
                            GraphicImg[CurPage].DrawImage(BmpURL, X1, Y1Inf, W - 1, H - 1);
                            if (Flag == "Y")
                            {
                                thePageObjectInf.Add(new PageObjectInf(Obj.Id,
                                                                       Convert.ToInt32(X1 + (W / 2) - 21),
                                                                       Convert.ToInt32(Y1Inf + H),
                                                                       CurPage + iTotalPage,
                                                                       0));

                                GraphicImg[CurPage].DrawImage(BmpDnArrow, X1 + (W / 2) - 21, Y1Inf + H, 42, 14);
                            }
                            else if (Flag == "U")
                            {
                                GraphicImg[CurPage].DrawImage(BmpUpArrow, X1 + (W / 2) - 21, Y1Inf - 3, 42, 14);
                            }
                        }
                    }

                    return;
                }
                CurPage = PDFPage;
                if (strOutput == "PDF")
                {
                    if (sFP[0] == "BRD")
                    {
                        DrawDivPDFImage(sFP[1], CurPage, X1, Y1 + 15, W - 1, H - 1, Obj);
                        if (Flag == "Y")
                        {
                            thePageObjectInf.Add(new PageObjectInf(Obj.Id,
                                                                   Convert.ToInt32(MyLabelRect.X + (MyLabelRect.Width / 2) - 21),
                                                                   Convert.ToInt32(MyLabelRect.Y + 2 + MyLabelRect.Height),
                                                                   CurPage + iTotalPage,
                                                                   0));

                            MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath("images/downarrow.ico").ToString(),
                                                                             MyLabelRect.X + (MyLabelRect.Width / 2) - 21,
                                                                             MyLabelRect.Y + 2 + MyLabelRect.Height);
                            MyImage.Height = 14;
                            MyImage.Width = 42;
                            MyPage[CurPage].Elements.Add(MyImage);
                        }
                        else if (Flag == "U")
                        {
                            MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath("images/uparrow.png").ToString(),
                                                                             MyLabelRect.X + (MyLabelRect.Width / 2) - 21,
                                                                             MyLabelRect.Y - 19);
                            MyImage.Height = 14;
                            MyImage.Width = 42;
                            MyPage[CurPage].Elements.Add(MyImage);
                        }
                    }
                    else
                    {
                        MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath(sFP[1]).ToString(), X1, Y1 + 15);
                        MyImage.Height = H - 1;
                        MyImage.Width = W - 1;
                        MyPage[CurPage].Elements.Add(MyImage);

                        if (Flag == "Y")
                        {
                            MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath("images/downarrow.ico").ToString(), X1 + (W / 2) - 21, Y1 + 16 + H);
                            MyImage.Height = 14;
                            MyImage.Width = 42;
                            MyPage[CurPage].Elements.Add(MyImage);

                            thePageObjectInf.Add(new PageObjectInf(Obj.Id,
                                                                   Convert.ToInt32(X1 + (W / 2) - 21),
                                                                   Convert.ToInt32(Y1 + 16 + H),
                                                                   CurPage + iTotalPage,
                                                                   0));
                        }
                        else if (Flag == "U")
                        {
                            MyImage = new ceTe.DynamicPDF.PageElements.Image(System.Web.HttpContext.Current.Server.MapPath("images/uparrow.png").ToString(), X1 + (W / 2) - 21, Y1 - 3);
                            MyImage.Height = 14;
                            MyImage.Width = 42;
                            MyPage[CurPage].Elements.Add(MyImage);
                        }
                    }

                }
                else if (strOutput == "PPT")
                {
                    if (sFP[0] == "BRD")
                    {
                        DrawDivPPTImage(sFP[1], CurPage, X1, Y1 + 15, W - 1, H - 1, Obj);
                        if (Flag == "Y")
                        {
                            thePageObjectInf.Add(new PageObjectInf(Obj.Id,
                                                                   Convert.ToInt32(RectPPT.Col + (RectPPT.Width / 2) - 21),
                                                                   Convert.ToInt32(RectPPT.Row + RectPPT.Height + 2),
                                                                   CurPage + iTotalPage,
                                                                   0));
                            GraphicImg[CurPage].DrawImage(BmpDnArrow, RectPPT.Col + (RectPPT.Width / 2) - 21, RectPPT.Row + RectPPT.Height + 2, 42, 14);
                        }
                        else if (Flag == "U")
                            GraphicImg[CurPage].DrawImage(BmpUpArrow, RectPPT.Col + (RectPPT.Width / 2) - 21, RectPPT.Row - 19, 42, 14);
                    }
                    else
                    {
                        GraphicImg[CurPage].DrawImage(BmpURL, X1, Y1 + 15, W - 1, H - 1);
                        if (Flag == "Y")
                        {
                            thePageObjectInf.Add(new PageObjectInf(Obj.Id,
                                                                   Convert.ToInt32(X1 + (W / 2) - 21),
                                                                   Convert.ToInt32(Y1 + 15 + H),
                                                                   CurPage + iTotalPage,
                                                                   0));
                            GraphicImg[CurPage].DrawImage(BmpDnArrow, X1 + (W / 2) - 21, Y1 + 15 + H, 42, 14);
                        }
                        else if (Flag == "U")
                            GraphicImg[CurPage].DrawImage(BmpUpArrow, X1 + (W / 2) - 21, Y1 - 3, 42, 14);
                    }
                }
            }

            // Get Font Name for display
            private ceTe.DynamicPDF.Font GetPDFFontName(int Idx)
            {
                switch ((dtFieldInformation.Rows[Idx]["FONT_NAME"].ToString() + dtFieldInformation.Rows[Idx]["FONT_STYLE"].ToString()).ToUpper())
                {
                    case "ARIALNORMAL":
                        return ceTe.DynamicPDF.Font.Helvetica;
                    case "ARIALITALLIC":
                        return ceTe.DynamicPDF.Font.HelveticaBoldOblique;
                    case "ARIALBOLD":
                        return ceTe.DynamicPDF.Font.HelveticaBold;
                    case "TIMESNORMAL":
                        return ceTe.DynamicPDF.Font.TimesRoman;
                    case "TIMESITALLIC":
                        return ceTe.DynamicPDF.Font.TimesItalic;
                    case "TIMESBOLD":
                        return ceTe.DynamicPDF.Font.TimesBold;
                    case "COURIERNORMAL":
                        return ceTe.DynamicPDF.Font.Courier;
                    case "COURIERITALLIC":
                        return ceTe.DynamicPDF.Font.CourierOblique;
                    case "COURIERBOLD":
                        return ceTe.DynamicPDF.Font.CourierBold;
                }
                return ceTe.DynamicPDF.Font.Helvetica;
            }

            // Get Font Size for display
            private int GetPDFFontSize(int Idx)
            {
                return Convert.ToInt16(dtFieldInformation.Rows[Idx]["FONT_SIZE"]);
            }

            // Get Font Size for display
            private ceTe.DynamicPDF.TextAlign GetPDFFontFloat(string FontStyle)
            {
                if (FontStyle.ToUpper() == "RIGHT")
                    return ceTe.DynamicPDF.TextAlign.Right;
                else if (FontStyle.ToUpper() == "LEFT")
                    return ceTe.DynamicPDF.TextAlign.Left;
                else if (FontStyle.ToUpper() == "CENTER")
                    return ceTe.DynamicPDF.TextAlign.Center;

                return ceTe.DynamicPDF.TextAlign.Left;
            }

            // Get Font Size for display
            private ceTe.DynamicPDF.RgbColor GetPDFFontColor(int Idx)
            {
                switch (dtFieldInformation.Rows[Idx]["FONT_COLOR"].ToString().ToLower())
                {
                    case "#663300":
                        return ceTe.DynamicPDF.WebColor.Brown;
                        break;
                    case "#ffb266":
                        return ceTe.DynamicPDF.WebColor.Orange;
                        break;
                    case "#ffffff":
                        return ceTe.DynamicPDF.WebColor.White;
                        break;
                    case "#ff0000":
                        return ceTe.DynamicPDF.WebColor.Red;
                        break;
                    case "#911414":
                        return ceTe.DynamicPDF.WebColor.DarkRed;
                        break;
                    case "#000000":
                        return ceTe.DynamicPDF.WebColor.Black;
                        break;

                }
                return ceTe.DynamicPDF.WebColor.Black;
            }

            //Function to get Font style
            public System.Drawing.FontStyle GetFontStyle(string FontStyle)
            {
                switch (FontStyle)
                {
                    case "underline":
                        return System.Drawing.FontStyle.Underline;
                        break;
                    case "bold-ul":
                        return System.Drawing.FontStyle.Underline;
                        break;
                    case "bold":
                        return System.Drawing.FontStyle.Bold;
                        break;
                    case "itallic":
                        return System.Drawing.FontStyle.Italic;
                        break;
                    case "strikethru":
                        return System.Drawing.FontStyle.Strikeout;
                        break;
                }

                return System.Drawing.FontStyle.Regular;
            }

            // Place information in PDF
            private void PlaceInfoPDF(string Info, int Col, int Row, int Width, int Height, string Level, string ShowFullBox)
            {
                string[] LabelInfo = Info.Replace("&amp;", "&").Split(';');
                string FontName = "", FontSize = "", FontColor = "", FontStyle = "", FontFloat = "", FontWidth = "", Adjustment = "";
                string LGL_SHOW_FIELD = WebConfigurationManager.AppSettings["LGL_SHOW_FIELD"];
                DataTable dtFieldInf = null;
                if (sAllPDF == "N")
                    dtFieldInf = dtFieldInformation;
                else
                    dtFieldInf = dtFieldActive;
                if (LabelInfo.Length >= 1)
                {
                    int Idx = 0, LeftPos = 0;
                    FieldCount = dtFieldInf.Rows.Count;
                    for (int Idy = 0; Idy <= FieldCount - 1; Idy++)
                    {
                        if (dtFieldInf.Rows[Idx]["FIELD_NAME"].ToString().ToUpper() == "FULLNAME")
                        {
                            string sName = "";
                        }

                        try
                        {
                            int iWidth = 255;
                            //if (LevelCount == 6) iWidth = 168;


                            string[] LabelText = LabelInfo[Idx].Split('|');


                            if (dtFieldInf.Rows[Idx]["ACTIVE_IND"].ToString() == "Y")
                            {
                                FontName = dtFieldInf.Rows[Idx]["FONT_NAME"].ToString();
                                FontSize = dtFieldInf.Rows[Idx]["FONT_SIZE"].ToString();
                                FontColor = dtFieldInf.Rows[Idx]["FONT_COLOR"].ToString();
                                FontStyle = dtFieldInf.Rows[Idx]["FONT_STYLE"].ToString();
                                FontFloat = dtFieldInf.Rows[Idx]["FONT_FLOAT"].ToString();
                                FontWidth = dtFieldInf.Rows[Idx]["FIELD_WIDTH"].ToString();
                                Adjustment = dtFieldInf.Rows[Idx]["ADJUSTMENT"].ToString();

                                LeftPos = 0;
                                if ((Adjustment == "Y") && (FontFloat == "right"))
                                {
                                    LeftPos = iWidth - 255;
                                    if (Level == "1") LeftPos += -10;
                                    if (Level == "2") LeftPos += -20;
                                }
                                else if ((Adjustment == "Y") && (FontFloat == "center"))
                                {
                                    if (Convert.ToInt16(FontWidth) >= iWidth)
                                    {
                                        if (Level == "0") FontWidth = iWidth.ToString();
                                        if (Level == "1") FontWidth = (iWidth - 10).ToString();
                                        if (Level == "2") FontWidth = (iWidth - 20).ToString();
                                    }
                                }
                                if (strOutput == "PDF")
                                {
                                    if (ShowFullBox != "N")
                                    {
                                        if (dtFieldInf.Rows[Idx]["FIELD_NAME"].ToString() == "NOR_COUNT")
                                        {
                                            if (LabelText[0].ToString() != "0")
                                            {
                                                MyLabel = new ceTe.DynamicPDF.PageElements.Label("SoC",
                                                                                                 Col + Convert.ToInt32(LabelText[2]) + LeftPos - 50,
                                                                                                 Row + Convert.ToInt32(LabelText[1]),
                                                                                                 Convert.ToInt16(FontWidth),
                                                                                                 Height,
                                                                                                 GetPDFFontName(Idx),
                                                                                                 GetPDFFontSize(Idx),
                                                                                                 GetPDFFontFloat(FontFloat),
                                                                                                 GetPDFFontColor(Idx));
                                                if ((FontStyle == "bold-ul") || (FontStyle == "underline")) MyLabel.Underline = true;
                                                MyPage[CurPage].Elements.Add(MyLabel);
                                                MyLabel = new ceTe.DynamicPDF.PageElements.Label(Convert.ToInt32(LabelText[0]).ToString("#,##0"),
                                                                                             Col + Convert.ToInt32(LabelText[2]) + LeftPos,
                                                                                             Row + Convert.ToInt32(LabelText[1]),
                                                                                             Convert.ToInt16(FontWidth),
                                                                                             Height,
                                                                                             GetPDFFontName(Idx),
                                                                                             GetPDFFontSize(Idx),
                                                                                             GetPDFFontFloat(FontFloat),
                                                                                             GetPDFFontColor(Idx));
                                                if ((FontStyle == "bold-ul") || (FontStyle == "underline")) MyLabel.Underline = true;
                                                MyPage[CurPage].Elements.Add(MyLabel);

                                            }
                                        }
                                        else if (dtFieldInf.Rows[Idx]["FIELD_NAME"].ToString() == "SOC_COUNT")
                                        {
                                            if (LabelText[0].ToString() != "0")
                                            {
                                                MyLabel = new ceTe.DynamicPDF.PageElements.Label("NoR",
                                                                                                 Col + Convert.ToInt32(LabelText[2]) + LeftPos - 50,
                                                                                                 Row + Convert.ToInt32(LabelText[1]),
                                                                                                 Convert.ToInt16(FontWidth),
                                                                                                 Height,
                                                                                                 GetPDFFontName(Idx),
                                                                                                 GetPDFFontSize(Idx),
                                                                                                 GetPDFFontFloat(FontFloat),
                                                                                                 GetPDFFontColor(Idx));
                                                if ((FontStyle == "bold-ul") || (FontStyle == "underline")) MyLabel.Underline = true;
                                                MyPage[CurPage].Elements.Add(MyLabel);
                                                MyLabel = new ceTe.DynamicPDF.PageElements.Label(Convert.ToInt32(LabelText[0]).ToString("#,##0"),
                                                                                             Col + Convert.ToInt32(LabelText[2]) + LeftPos,
                                                                                             Row + Convert.ToInt32(LabelText[1]),
                                                                                             Convert.ToInt16(FontWidth),
                                                                                             Height,
                                                                                             GetPDFFontName(Idx),
                                                                                             GetPDFFontSize(Idx),
                                                                                             GetPDFFontFloat(FontFloat),
                                                                                             GetPDFFontColor(Idx));
                                                if ((FontStyle == "bold-ul") || (FontStyle == "underline")) MyLabel.Underline = true;
                                                MyPage[CurPage].Elements.Add(MyLabel);

                                            }
                                        }
                                        else
                                        {
                                            MyLabel = new ceTe.DynamicPDF.PageElements.Label(LabelText[0],
                                                                                             Col + Convert.ToInt32(LabelText[2]) + LeftPos,
                                                                                             Row + Convert.ToInt32(LabelText[1]),
                                                                                             Convert.ToInt16(FontWidth),
                                                                                             Height,
                                                                                             GetPDFFontName(Idx),
                                                                                             GetPDFFontSize(Idx),
                                                                                             GetPDFFontFloat(FontFloat),
                                                                                             GetPDFFontColor(Idx));
                                            if ((FontStyle == "bold-ul") || (FontStyle == "underline")) MyLabel.Underline = true;
                                            MyPage[CurPage].Elements.Add(MyLabel);
                                        }
                                    }
                                    else
                                    {
                                        if (dtFieldInf.Rows[Idx]["FIELD_NAME"].ToString().ToUpper() == LGL_SHOW_FIELD.ToUpper())
                                        {
                                            MyLabel = new ceTe.DynamicPDF.PageElements.Label(LabelText[0],
                                                                                             MyLabelRect.X,
                                                                                             MyLabelRect.Y,
                                                                                             MyLabelRect.Width,
                                                                                             MyLabelRect.Height,
                                                                                             GetPDFFontName(Idx),
                                                                                             14,
                                                                                             ceTe.DynamicPDF.TextAlign.Center,
                                                                                             GetPDFFontColor(Idx));
                                            if ((FontStyle == "bold-ul") || (FontStyle == "underline")) MyLabel.Underline = true;
                                            MyLabel.VAlign = ceTe.DynamicPDF.VAlign.Center;
                                            MyPage[CurPage].Elements.Add(MyLabel);
                                        }
                                    }

                                }
                                else if (strOutput == "PPT")
                                {
                                    if (ShowFullBox != "N")
                                    {
                                        System.Drawing.Font drawFontText = new System.Drawing.Font(dtFieldInf.Rows[Idx]["FONT_NAME"].ToString(),
                                                                                                   Convert.ToSingle(dtFieldInf.Rows[Idx]["FONT_SIZE"]),
                                                                                                   GetFontStyle(dtFieldInf.Rows[Idx]["FONT_STYLE"].ToString()),
                                                                                                   GraphicsUnit.Pixel);
                                        SolidBrush drawBrushText = new SolidBrush(System.Drawing.Color.Black);
                                        switch (FontColor.ToLower())
                                        {
                                            case "#663300":
                                                drawBrushText = new SolidBrush(System.Drawing.Color.Brown);
                                                break;
                                            case "#ffb266":
                                                drawBrushText = new SolidBrush(System.Drawing.Color.Orange);
                                                break;
                                            case "#ffffff":
                                                drawBrushText = new SolidBrush(System.Drawing.Color.White);
                                                break;
                                            case "#0000ff":
                                                drawBrushText = new SolidBrush(System.Drawing.Color.Blue);
                                                break;
                                            case "#008000":
                                                drawBrushText = new SolidBrush(System.Drawing.Color.Green);
                                                break;
                                            case "#911414":
                                                drawBrushText = new SolidBrush(System.Drawing.Color.DarkRed);
                                                break;
                                            case "#ff0000":
                                                drawBrushText = new SolidBrush(System.Drawing.Color.Red);
                                                break;
                                            case "#000000":
                                                drawBrushText = new SolidBrush(System.Drawing.Color.Black);
                                                break;
                                        }


                                        StringFormat stringFormat = new StringFormat();
                                        if (FontFloat.ToUpper() == "LEFT")
                                            stringFormat.Alignment = StringAlignment.Near;
                                        else if (FontFloat.ToUpper() == "RIGHT")
                                            stringFormat.Alignment = StringAlignment.Far;
                                        else if (FontFloat.ToUpper() == "CENTER")
                                            stringFormat.Alignment = StringAlignment.Center;
                                        if (dtFieldInf.Rows[Idx]["FIELD_NAME"].ToString() == "NOR_COUNT")
                                        {
                                            if (LabelText[0].ToString() != "0")
                                            {
                                                System.Drawing.Rectangle rectLabel = new System.Drawing.Rectangle(Col + Convert.ToInt32(LabelText[2]) + LeftPos - 50, Row + Convert.ToInt32(LabelText[1]), Convert.ToInt16(FontWidth), Height);
                                                GraphicImg[CurPage].DrawString("SoC", drawFontText, drawBrushText, rectLabel, stringFormat);
                                                rectLabel = new System.Drawing.Rectangle(Col + Convert.ToInt32(LabelText[2]) + LeftPos, Row + Convert.ToInt32(LabelText[1]), Convert.ToInt16(FontWidth), Height);
                                                GraphicImg[CurPage].DrawString(Convert.ToInt32(LabelText[0]).ToString("#,##0"), drawFontText, drawBrushText, rectLabel, stringFormat);
                                            }
                                        }
                                        else if (dtFieldInf.Rows[Idx]["FIELD_NAME"].ToString() == "SOC_COUNT")
                                        {
                                            if (LabelText[0].ToString() != "0")
                                            {
                                                System.Drawing.Rectangle rectLabel = new System.Drawing.Rectangle(Col + Convert.ToInt32(LabelText[2]) + LeftPos - 50, Row + Convert.ToInt32(LabelText[1]), Convert.ToInt16(FontWidth), Height);
                                                GraphicImg[CurPage].DrawString("NoR", drawFontText, drawBrushText, rectLabel, stringFormat);
                                                rectLabel = new System.Drawing.Rectangle(Col + Convert.ToInt32(LabelText[2]) + LeftPos, Row + Convert.ToInt32(LabelText[1]), Convert.ToInt16(FontWidth), Height);
                                                GraphicImg[CurPage].DrawString(Convert.ToInt32(LabelText[0]).ToString("#,##0"), drawFontText, drawBrushText, rectLabel, stringFormat);
                                            }
                                        }
                                        else
                                        {
                                            System.Drawing.Rectangle rectLabel = new System.Drawing.Rectangle(Col + Convert.ToInt32(LabelText[2]) + LeftPos, Row + Convert.ToInt32(LabelText[1]), Convert.ToInt16(FontWidth), Height);
                                            GraphicImg[CurPage].DrawString(LabelText[0], drawFontText, drawBrushText, rectLabel, stringFormat);
                                        }
                                    }
                                    else
                                    {
                                        if (dtFieldInf.Rows[Idx]["FIELD_NAME"].ToString().ToUpper() == LGL_SHOW_FIELD.ToUpper())
                                        {

                                            System.Drawing.Font drawFontText = new System.Drawing.Font(dtFieldInf.Rows[Idx]["FONT_NAME"].ToString(),
                                                                                                       14,
                                                                                                       GetFontStyle("bold"),
                                                                                                       GraphicsUnit.Pixel);
                                            SolidBrush drawBrushText = new SolidBrush(System.Drawing.Color.Black);
                                            switch (FontColor.ToLower())
                                            {
                                                case "#663300":
                                                    drawBrushText = new SolidBrush(System.Drawing.Color.Brown);
                                                    break;
                                                case "#ffb266":
                                                    drawBrushText = new SolidBrush(System.Drawing.Color.Orange);
                                                    break;
                                                case "#ffffff":
                                                    drawBrushText = new SolidBrush(System.Drawing.Color.White);
                                                    break;
                                                case "#0000ff":
                                                    drawBrushText = new SolidBrush(System.Drawing.Color.Blue);
                                                    break;
                                                case "#008000":
                                                    drawBrushText = new SolidBrush(System.Drawing.Color.Green);
                                                    break;
                                                case "#911414":
                                                    drawBrushText = new SolidBrush(System.Drawing.Color.DarkRed);
                                                    break;
                                                case "#ff0000":
                                                    drawBrushText = new SolidBrush(System.Drawing.Color.Red);
                                                    break;
                                                case "#000000":
                                                    drawBrushText = new SolidBrush(System.Drawing.Color.Black);
                                                    break;
                                            }


                                            StringFormat stringFormat = new StringFormat();
                                            stringFormat.Alignment = StringAlignment.Center;
                                            stringFormat.LineAlignment = StringAlignment.Center;
                                            System.Drawing.Rectangle rectLabel = new System.Drawing.Rectangle(RectPPT.Col, RectPPT.Row, RectPPT.Width, RectPPT.Height);
                                            GraphicImg[CurPage].DrawString(LabelText[0], drawFontText, drawBrushText, rectLabel, stringFormat);
                                        }
                                    }
                                }
                            }


                            Idx++;
                        }
                        catch (Exception ex)
                        {
                            string errMsg = ex.ToString();
                            Console.WriteLine("  Message: {0}", errMsg);
                        }
                    }
                }
            }

            // DrawLabel Obj, X1, Y1
            private void DrawLabel(ObjectInf Obj, int X1, int Y1)
            {
                if (Y1 + Obj.Oheight + 20 >= Output_Height)
                {
                    if (!(GetPageExistence(Y1 + Obj.Oheight)))
                    {
                        PageSizeNos.Add(Y1);
                        //MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(PageSize.B4, PageOrientation.Landscape, 15.0F);
                        if (LevelCount == 4)
                            MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(1080F, 1050F, 15.3F);
                        else
                            MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(1600F, 1050F, 15.3F);

                        // MyPage[PageSizeNos.Count - 1] = new ceTe.DynamicPDF.Page(1050F, 768F, 15.3F);
                        MaxPage = PageSizeNos.Count;
                    }
                    PlaceInfoPDF(Obj.Title, X1, GetPageRowPosition(Y1) + 15, Obj.Width - 30, Obj.Oheight - 2, Obj.Level, Obj.ShowFullBox);

                    return;
                }
                CurPage = PDFPage;
                PlaceInfoPDF(Obj.Title, X1, Y1 + 15, Obj.Width - 30, Obj.Oheight - 2, Obj.Level, Obj.ShowFullBox);
            }

            // Places the Level 0 Information
            private void Level_0_Information(List<ObjectInf> theObjectInf, string sArrow, string sTPage, string sCPage)
            {
                //lstID.Add(HttpContext.Current.Session["ID"].ToString());
                string sUpperArrow = "U";
                ObjectInf Obj = GetIdInfo(theObjectInf, LevelIds[0].ToString());

                HttpContext.Current.Session["ID"] = "20002609";

                if (Obj.Id != HttpContext.Current.Session["ID"].ToString())
                {
                    if (sArrow == "I")
                    {
                        if (strOutput == "PDF")
                        {
                            string AP = GetAncestorPageNo(Obj.Id);
                            XYDestination dest = new XYDestination(Convert.ToInt32(AP), 1, 1);
                            MyLink = new ceTe.DynamicPDF.PageElements.Link(Obj.Col + 288, 2, 255, 10, dest);
                            MyPage[CurPage].Elements.Add(MyLink);

                            MyLabel = new ceTe.DynamicPDF.PageElements.Label("( #" + sCPage + " / " + sTPage + ")", Obj.Col + 448, 12, 255, 10, ceTe.DynamicPDF.Font.HelveticaBold, 10, ceTe.DynamicPDF.TextAlign.Left);
                            MyPage[CurPage].Elements.Add(MyLabel);
                            MyLabel = new ceTe.DynamicPDF.PageElements.Label("( # " + AP + ")", Obj.Col + 288, 2, 255, 10, ceTe.DynamicPDF.Font.HelveticaBold, 10, ceTe.DynamicPDF.TextAlign.Center);
                            MyPage[CurPage].Elements.Add(MyLabel);

                            //MyLabel = new ceTe.DynamicPDF.PageElements.Label("( #"+sCPage+" / "+sTPage+")", Obj.Col + 448, 12, 255, 10, ceTe.DynamicPDF.Font.HelveticaBold, 10, ceTe.DynamicPDF.TextAlign.Left);
                            //MyPage[CurPage].Elements.Add(MyLabel);
                            //MyLabel = new ceTe.DynamicPDF.PageElements.Label("( # " + GetAncestorPageNo(Obj.Id) +")", Obj.Col + 288, 2, 255, 10, ceTe.DynamicPDF.Font.HelveticaBold, 10, ceTe.DynamicPDF.TextAlign.Center);
                            //MyPage[CurPage].Elements.Add(MyLabel);
                        }
                        else if (strOutput == "PPT")
                        {
                            System.Drawing.Font drawFontText = new System.Drawing.Font("Arail", 10, FontStyle.Bold, GraphicsUnit.Pixel);
                            SolidBrush drawBrushText = new SolidBrush(System.Drawing.Color.Black);
                            StringFormat stringFormat = new StringFormat();
                            stringFormat.Alignment = StringAlignment.Near;

                            System.Drawing.Rectangle rectLabel = new System.Drawing.Rectangle(Obj.Col + 448, 12, 255, 10);
                            GraphicImg[CurPage].DrawString("( #" + sCPage + " / " + sTPage + ")", drawFontText, drawBrushText, rectLabel, stringFormat);

                            stringFormat = new StringFormat();
                            stringFormat.Alignment = StringAlignment.Center;
                            rectLabel = new System.Drawing.Rectangle(Obj.Col + 288, 2, 255, 10);
                            GraphicImg[CurPage].DrawString("( # " + GetAncestorPageNo(Obj.Id) + " )", drawFontText, drawBrushText, rectLabel, stringFormat);
                        }
                    }
                }
                else sArrow = "C";

                // Add label to MyPage
                PreviousObj = null;
                if (LevelCount == 4)
                {
                    if (sArrow == "C") sUpperArrow = ((Obj.PId == "-1") || (Obj.PId == "10000000")) ? "N" : "U";
                    DrawImage(TemplateURL, Obj.Col + 33, Obj.Row, Obj.Width, Obj.Oheight, sUpperArrow, Obj);
                    if (IdInPId(theObjectInf, Obj.Id).Length != 0)
                        DrawLine(Obj.Id, Obj.Col + Convert.ToInt16(Obj.Width / 2) + 33, Obj.Row + Obj.Oheight, Obj.Col + Convert.ToInt16(Obj.Width / 2) + 33, Obj.Row + Obj.Oheight + 20);
                    DrawLabel(Obj, Obj.Col + 36, Obj.Row);
                    DrawPhoto(Obj, 33, 0);
                }
                else if (LevelCount == 6)
                {
                    if (sArrow == "C") sUpperArrow = ((Obj.PId == "-1") || (Obj.PId == "10000000")) ? "N" : "U";
                    DrawImage(TemplateURL, Obj.Col + 288, Obj.Row, Obj.Width, Obj.Oheight, sUpperArrow, Obj);
                    if (IdInPId(theObjectInf, Obj.Id).Length != 0)
                        DrawLine(Obj.Id, Obj.Col + Convert.ToInt16(Obj.Width / 2) + 288, Obj.Row + Obj.Oheight, Obj.Col + Convert.ToInt16(Obj.Width / 2) + 288, Obj.Row + Obj.Oheight + 20);
                    DrawLabel(Obj, Obj.Col + 291, Obj.Row);
                    DrawPhoto(Obj, 290, 0);
                }
            }

            // Places the Level 1 Information
            private void Level_1_Information(List<ObjectInf> theObjectInf)
            {
                var listObj = from p in theObjectInf
                              where p.Level == "1"
                              orderby p.Row, p.Col
                              select p;

                int icount = 1;

                PreviousObj = null;
                foreach (ObjectInf Obj in listObj)
                {
                    if (Obj.Level == "1")               // Gets the Start & End column, row information
                    {
                        // Add label to MyPage
                        string NextLevel = CheckLevelInfo(Obj);
                        DrawImage(TemplateURL, Obj.Col, Obj.Row, Obj.Width - 10, Obj.Oheight, NextLevel == "NO" ? "N" : "Y", Obj);
                        DrawLabel(Obj, Obj.Col + 2, Obj.Row);
                        DrawPhoto(Obj, 33, 0);
                    }

                    icount++;
                }
            }

            // Places the Level 1 Information
            private void Level_1_Line_Information(List<ObjectInf> theObjectInf)
            {
                int TotalCol = 0, TCol = 0, MaxHeight = 0, Idx = 0;
                ObjectInf ObjStart = null, ObjCur = null;

                var listObj = from p in theObjectInf
                              where p.Level == "1"
                              orderby p.Row, p.Col
                              select p;

                PreviousObj = null;
                foreach (ObjectInf Obj in listObj)
                {
                    if (Obj.Level == "1")               // Gets the Start & End column, row information
                    {
                        // Places the Line above Level 1
                        if (((Idx % LevelCount) == 0) && (Idx != 0))
                        {
                            // Places next level line
                            DrawLine(ObjCur.Id, ObjStart.Col + Convert.ToInt16(ObjStart.Width / 2), ObjStart.Row - 20, ObjCur.Col + Convert.ToInt16(ObjCur.Width / 2), ObjCur.Row - 20);

                            // Places center line
                            if ((listObj.Count() - (listObj.Count() % LevelCount)) == Idx)
                            {
                                if (listObj.Count() == Idx + 1)
                                {
                                    if ((GetPageRowStartPosition(Obj.Row) + 20) == Obj.Row)
                                        DrawLine("Middle:1", Convert.ToInt16(TotalCol / 2), ObjStart.Row - 20, Convert.ToInt16(TotalCol / 2), ObjCur.Row + MaxHeight);
                                    else
                                    {
                                        if ((ObjCur.Row + MaxHeight) <= 640)
                                            DrawLine("Middle:0", Convert.ToInt16(TotalCol / 2), ObjStart.Row - 20, Convert.ToInt16(TotalCol / 2), ObjCur.Row + MaxHeight - 20);
                                        else
                                        {
                                            if (LevelFlag == "1")
                                                DrawLine("Middle:1", Convert.ToInt16(TotalCol / 2), ObjStart.Row - 20, Convert.ToInt16(TotalCol / 2), ObjCur.Row + MaxHeight - 20);
                                            else
                                                DrawLine("Middle:0", Convert.ToInt16(TotalCol / 2), ObjStart.Row - 20, Convert.ToInt16(TotalCol / 2), ObjCur.Row + MaxHeight - 5);
                                        }
                                    }
                                }
                                else
                                {
                                    //MaxHeight = MaxHeight - 20;
                                    if (LevelFlag == "1")
                                        DrawLine("Middle:1", Convert.ToInt16(TotalCol / 2), ObjStart.Row - 20, Convert.ToInt16(TotalCol / 2), ObjCur.Row + MaxHeight - 20);
                                    else
                                        DrawLine("Middle:0", Convert.ToInt16(TotalCol / 2), ObjStart.Row - 20, Convert.ToInt16(TotalCol / 2), ObjCur.Row + MaxHeight - 5);
                                }
                            }
                            else
                            {
                                if (LevelFlag == "1")
                                    DrawLine("Middle:1", Convert.ToInt16(TotalCol / 2), ObjStart.Row - 20, Convert.ToInt16(TotalCol / 2), ObjCur.Row + MaxHeight - 20);
                                else
                                    DrawLine("Middle:0", Convert.ToInt16(TotalCol / 2), ObjStart.Row - 20, Convert.ToInt16(TotalCol / 2), ObjCur.Row + MaxHeight - 5);
                            }

                            // Initialise total column points
                            TCol = TotalCol;
                            TotalCol = ObjStart.Col;
                        }

                        ObjCur = Obj;
                        if ((Idx % LevelCount) == 0)
                        {
                            ObjStart = Obj;
                            MaxHeight = 0;
                        }
                        if (Idx == 0) TotalCol = ObjStart.Col;

                        TotalCol += Obj.Width;
                        if (MaxHeight <= Obj.Height) MaxHeight = Obj.Height;

                        // Draws the Lines for Level 1
                        if (IdInPId(theObjectInf, Obj.Id).Length != 0)
                        {
                            int Minus_Height = 0;
                            if (LevelCount == 4)
                            {
                                if (Original_Height == 80) Minus_Height = -7;
                                if (Original_Height == 90) Minus_Height = -6;
                                if (Original_Height == 100) Minus_Height = 0;
                                if (Original_Height == 110) Minus_Height = 4;
                                if (Original_Height == 120) Minus_Height = 5;
                            }
                            else if (LevelCount == 6)
                            {
                                if (Original_Height == 80) Minus_Height = -11;
                                if (Original_Height == 90) Minus_Height = -11;
                                if (Original_Height == 100) Minus_Height = -4;
                                if (Original_Height == 110) Minus_Height = -2;
                                if (Original_Height == 120) Minus_Height = -2;
                            }

                            DrawLine(Obj.Id, Obj.Col + 10, (Obj.Row + Obj.Oheight), Obj.Col + 10, (Obj.Row + ((Obj.Height + Adjustment_Height) - Obj.Oheight + Minus_Height)));
                        }
                        DrawLine(Obj.Id, Obj.Col + Convert.ToInt16(Obj.Width / 2), Obj.Row - 20, Obj.Col + Convert.ToInt16(Obj.Width / 2), Obj.Row);

                        Idx++;
                    }
                }

                // Places the last line 
                if (LevelCount == 4)
                {
                    if ((((Idx % LevelCount) <= 2) && ((Idx % LevelCount) != 0)))
                        DrawLine(ObjCur.Id, ObjStart.Col + Convert.ToInt16(ObjStart.Width / 2), ObjStart.Row - 20, Convert.ToInt16(785 / 2) + 120, ObjCur.Row - 20);
                    else
                        DrawLine(ObjCur.Id, ObjStart.Col + Convert.ToInt16(ObjStart.Width / 2), ObjStart.Row - 20, ObjCur.Col + Convert.ToInt16(ObjCur.Width / 2), ObjCur.Row - 20);
                }
                else if (LevelCount == 6)
                {
                    if ((((Idx % LevelCount) <= 3) && ((Idx % LevelCount) != 0)))
                        DrawLine(ObjCur.Id, ObjStart.Col + Convert.ToInt16(ObjStart.Width / 2), ObjStart.Row - 20, Convert.ToInt16(1300 / 2) + 117, ObjCur.Row - 20);
                    else
                        DrawLine(ObjCur.Id, ObjStart.Col + Convert.ToInt16(ObjStart.Width / 2), ObjStart.Row - 20, ObjCur.Col + Convert.ToInt16(ObjCur.Width / 2), ObjCur.Row - 20);
                }
            }

            // Places the Level without child Information
            private void Level_Information(List<ObjectInf> theObjectInf)
            {
                var listObj = from p in theObjectInf
                              orderby p.Row, p.Col
                              select p;

                PreviousObj = null;
                foreach (ObjectInf Obj in listObj)
                {
                    if ((Obj.Level != "0") && (Obj.Level != "1"))
                    {
                        string NextLevel = CheckLevelInfo(Obj);
                        DrawImage(TemplateURL, Obj.Col + 5, Obj.Row, Obj.Width - 30, Obj.Oheight, NextLevel == "NO" ? "N" : "Y", Obj);
                        DrawLabel(Obj, Obj.Col + 2, Obj.Row);
                        DrawLine(Obj.Id, Obj.Col - 5, Obj.Row + Convert.ToInt16(Obj.Oheight / 2), Obj.Col + 5, Obj.Row + Convert.ToInt16(Obj.Oheight / 2));
                    }
                    PreviousObj = Obj;
                }
            }

            private ImageCodecInfo GetEncoder(ImageFormat format)
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID == format.Guid)
                    {
                        return codec;
                    }
                }
                return null;
            }

            // Method to create PPT with HTML object
            public void ConvertToPPT()
            {
                int iWidth = 255;
                if (LevelCount == 6) iWidth = 255;

                // Collects the Hierarchy data to show 
                // http://www.dynamicpdf.com/RasterizerPDF-.NET.aspx
                strOutput = "PPT"; sAllPDF = "N";
                Output_Height = 1020;

                //var theObjectInf = GetLevelInfo();
                string jsonString = "";
                if (HttpContext.Current.Session["VIEW_TYPE"].ToString() == "OV")
                    jsonString = HttpContext.Current.Session["theOVObjectInf"].ToString();
                else
                    jsonString = HttpContext.Current.Session["theLVObjectInf"].ToString();
                var theObjectInf = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<ObjectInf>>(jsonString).OrderBy(o => Convert.ToInt32(o.SortNo)).ToList();

                Common csobj = new Common();
                DataTable dtlevel = csobj.SQLReturnDataTable("SELECT * FROM ORG_CONFIG_INFO WHERE VIEW_ID='" + HttpContext.Current.Session["VIEW"].ToString() + "'");
                foreach (DataRow drlvl in dtlevel.Rows)
                {
                    //if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = drlvl["FIELD_VALUE"].ToString();
                    if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = LevelFlag;
                    if (drlvl["FIELD_NAME"].ToString() == "HEIGHT") Height = drlvl["FIELD_VALUE"].ToString();
                    if (drlvl["FIELD_NAME"].ToString() == "LINECOLOR")
                    {
                        LineColor = drlvl["FIELD_VALUE"].ToString();
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "LINEWIDTH")
                    {
                        LineWidth = drlvl["FIELD_VALUE"].ToString();
                        blackPen = new Pen(GetDrawingLineColor(LineColor), Convert.ToInt16(LineWidth));
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "TEMPLATE")
                    {
                        TemplateURL = drlvl["FIELD_VALUE"].ToString();
                        string[] TempURL = TemplateURL.Split('~');
                        if (TempURL[0] == "IMG")
                        {
                            BmpURL = new Bitmap(System.Web.HttpContext.Current.Server.MapPath(TempURL[1]));
                            BmpURL.SetResolution(1200f, 1200f);
                        }
                        BmpUpArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/uparrow.png"));
                        BmpDnArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/downarrow.ico"));
                    }
                }

                // Gets the Box Height
                Original_Height = Convert.ToInt32(Height);
                Original_Height_10 = Original_Height + 40;
                Adjustment_Height = ((((Original_Height - (Original_Height % 30)) / 30)) * LevelCount) - 1;
                HttpContext.Current.Session["BoxHeight"] = Original_Height;



                //// Gets the available levels
                //LevelInf = GetAllLevels(theObjectInf).Split(',');
                //LevelIds = new string[LevelInf.Length];
                //ObjectInf Obj = null, CurObj = null, PrevObj = null;
                //if (LevelInf.Length >= 1)
                //{
                //    // Get Level based Ids
                //    for (int Idx = 0; Idx <= LevelInf.Length - 1; Idx++)
                //    {
                //        LevelIds[Convert.ToInt16(LevelInf[Idx])] = LevelInId(theObjectInf, LevelInf[Idx], Idx);
                //    }

                //    // Calculates the Height for each Ids
                //    Obj = GetIdInfo(theObjectInf, LevelIds[0].ToString());
                //    Obj.Height = Original_Height_10;
                //    Obj.Width = iWidth;
                //    string sSkip = "N";
                //    for (int Idx = LevelIds.Length - 1; Idx >= 1; Idx--)
                //    {
                //        string[] Ids = LevelIds[Idx].Split(',');
                //        for (int Idy = 0; Idy <= Ids.Length - 1; Idy++)
                //        {
                //            sSkip = "N";
                //            Obj = GetIdInfo(theObjectInf, Ids[Idy].ToString());
                //            if (Idx == 2) sSkip = Level2Check(LevelIds[1], Obj.PId);
                //            if (sSkip == "N")
                //            {
                //                if (Obj.Level == "2")                // Last Level
                //                {
                //                    Obj.Height = Original_Height_10 - 22;
                //                    Obj.Width = iWidth;
                //                }
                //                else
                //                {
                //                    Obj.Height = GetChildHeight(theObjectInf, Ids[Idy].ToString());
                //                    Obj.Width = iWidth;
                //                }
                //            }
                //        }
                //    }

                //    // Calculates the Row and Column info for each Ids
                //    int Row = 2, Col = 352, AddHeight = 0, MaxHeight = 0;
                //    Obj = GetIdInfo(theObjectInf, LevelIds[0].ToString());
                //    Obj.Row = Row;
                //    Obj.Col = Col;
                //    for (int Idx = 1; Idx <= LevelIds.Length - 1; Idx++)
                //    {
                //        string[] Ids = LevelIds[Idx].Split(',');
                //        for (int Idy = 0; Idy <= Ids.Length - 1; Idy++)
                //        {
                //            sSkip = "N";
                //            CurObj = GetIdInfo(theObjectInf, Ids[Idy].ToString());
                //            if (Idx == 2) sSkip = Level2Check(LevelIds[1], CurObj.PId);
                //            if (sSkip == "N")
                //            {
                //                if ((Idx == 1) && ((Idy % LevelCount) == 0) && (Idy != 0))
                //                {
                //                    AddHeight += MaxHeight;
                //                    MaxHeight = 0;
                //                }

                //                Obj = GetIdInfo(theObjectInf, CurObj.PId);             // To get the Row in which this object is displayed
                //                if (Idx == 1)
                //                {
                //                    Row = Obj.Row + Obj.Height + AddHeight;
                //                    Col = ((Idy % LevelCount) * iWidth) + 5;
                //                    if (MaxHeight <= CurObj.Height) MaxHeight = CurObj.Height;
                //                }
                //                else
                //                {
                //                    Row = Obj.Row;
                //                    for (int Idz = 0; Idz <= Ids.Length - 1; Idz++)
                //                    {
                //                        PrevObj = GetIdInfo(theObjectInf, Ids[Idz].ToString());
                //                        if (PrevObj.PId == Obj.Id)
                //                        {
                //                            Row += PrevObj.Height;
                //                            Col = Obj.Col + 15;
                //                            if (PrevObj.Id == Ids[Idy].ToString()) break;
                //                        }
                //                    }
                //                }
                //                CurObj.Row = Row;
                //                CurObj.Col = Col;
                //            }
                //        }
                //    }
                //}

                // Gets the available levels
                LevelInf = GetAllLevels(theObjectInf).Split(',');
                //if (LevelInf.Count() <= 1) return theObjectInf;
                LevelIds = new string[LevelInf.Length];
                ObjectInf Obj = null, CurObj = null, PrevObj = null;
                if (LevelInf.Length >= 1)
                {
                    // Get Level based Ids
                    for (int Idx = 0; Idx <= LevelInf.Length - 1; Idx++)
                    {
                        LevelIds[Convert.ToInt16(LevelInf[Idx])] = LevelInId(theObjectInf, LevelInf[Idx], Idx);
                    }

                    // Calculates the Height for each Ids
                    Obj = GetIdInfo(theObjectInf, LevelIds[0].ToString());
                    Obj.Height = Original_Height_10;
                    Obj.Width = iWidth;
                    string sSkip = "N";
                    int FLC = LevelIds[1].Split(',').Length;
                    for (int Idx = LevelIds.Length - 1; Idx >= 1; Idx--)
                    {
                        string[] Ids = LevelIds[Idx].Split(',');
                        if (Idx == 1) FLC = Ids.Length;
                        for (int Idy = 0; Idy <= Ids.Length - 1; Idy++)
                        {
                            sSkip = "N";
                            //Obj = GetIdInfo(theObjectInf, Ids[Idy].ToString());

                            if (Idx == 1) Obj = theObjectInf[Idy + 1]; else Obj = theObjectInf[Idy + FLC + 1];
                            if (Idx == 2) sSkip = Level2Check(LevelIds[1], Obj.PId);
                            if (sSkip == "N")
                            {
                                if (Obj.Level == "2")                // Last Level
                                {
                                    Obj.Height = Original_Height_10 - 22;
                                    Obj.Width = iWidth;
                                }
                                else
                                {
                                    Obj.Height = GetChildHeight(theObjectInf, Ids[Idy].ToString());
                                    Obj.Width = iWidth;
                                }
                            }
                        }
                    }

                    // Calculates the Row and Column info for each Ids
                    int Row = 2, Col = 352, AddHeight = 0, MaxHeight = 0;
                    FLC = 0;
                    Obj = GetIdInfo(theObjectInf, LevelIds[0].ToString());
                    Obj.Row = Row;
                    Obj.Col = Col;
                    for (int Idx = 1; Idx <= LevelIds.Length - 1; Idx++)
                    {
                        string[] Ids = LevelIds[Idx].Split(',');
                        if (Idx == 1) FLC = Ids.Length;
                        for (int Idy = 0; Idy <= Ids.Length - 1; Idy++)
                        {
                            sSkip = "N";
                            if (Idx == 1)
                            {
                                //CurObj = GetIdInfo(theObjectInf, Ids[Idy].ToString());
                                CurObj = theObjectInf[Idy + 1];
                            }
                            else
                                CurObj = theObjectInf[Idy + FLC + 1];

                            if (Idx == 2) sSkip = Level2Check(LevelIds[1], CurObj.PId);
                            if (sSkip == "N")
                            {
                                if ((Idx == 1) && ((Idy % LevelCount) == 0) && (Idy != 0))
                                {
                                    AddHeight += MaxHeight;
                                    MaxHeight = 0;
                                }

                                if (Idx == 1)
                                {
                                    CurObj = theObjectInf[Idy + 1];
                                    Obj = GetIdInfo(theObjectInf, CurObj.PId);             // To get the Row in which this object is displayed

                                    Row = Obj.Row + Obj.Height + AddHeight;
                                    Col = ((Idy % LevelCount) * iWidth) + 5;
                                    if (MaxHeight <= CurObj.Height) MaxHeight = CurObj.Height;
                                }
                                else
                                {
                                    CurObj = theObjectInf[Idy + FLC + 1];
                                    Obj = GetIdInfo(theObjectInf, CurObj.PId);             // To get the Row in which this object is displayed

                                    Row = Obj.Row;
                                    for (int Idz = 0; Idz <= Ids.Length - 1; Idz++)
                                    {
                                        //PrevObj = GetIdInfo(theObjectInf, Ids[Idz].ToString());
                                        PrevObj = theObjectInf[Idz + FLC + 1];
                                        if (PrevObj.PId == Obj.Id)
                                        {
                                            Row += PrevObj.Height;
                                            Col = Obj.Col + 15;
                                            if (PrevObj.Id == Ids[Idy].ToString()) break;
                                        }
                                    }
                                }
                                CurObj.Row = Row;
                                CurObj.Col = Col;
                            }
                        }
                    }
                }

                // Creates the Document
                PageSizeNos.Add(0);
                if (LevelCount == 6)
                    ImageOut[CurPage] = new Bitmap(1600, Output_Height + 30);
                else
                    ImageOut[CurPage] = new Bitmap(1024, Output_Height + 30);

                // Set DPI of image (xDpi, yDpi)
                ImageOut[CurPage].SetResolution(256.0F, 256.0F);

                GraphicImg[CurPage] = Graphics.FromImage(ImageOut[CurPage]);
                GraphicImg[CurPage].InterpolationMode = InterpolationMode.HighQualityBicubic;
                GraphicImg[CurPage].Clear(System.Drawing.Color.White);

                // Places the Level Information in the PDF
                MaxPage = 1;
                int LevelShowUpto = Convert.ToInt32(LevelUpto);
                if ((LevelIds.Length >= 3) && (LevelShowUpto >= 2)) Level_Information(theObjectInf);         // Level without child Information 
                if ((LevelIds.Length >= 2) && (LevelShowUpto >= 1)) Level_1_Information(theObjectInf);       // Level 1 Information 
                if ((LevelIds.Length >= 2) && (LevelShowUpto >= 1)) Level_1_Line_Information(theObjectInf);  // Level 1 Line Information 
                if ((LevelIds.Length >= 1) && (LevelShowUpto >= 0)) Level_0_Information(theObjectInf, "C", "", "");       // Level 0 Information

                string LastRefresh = "";
                csobj = new Common();
                DataTable dtLevel = csobj.SQLReturnDataTable("SELECT TOP 1 REFRESHDT FROM LEVEL_INFO ");
                if (dtLevel.Rows.Count >= 1)
                {
                    LastRefresh = Convert.ToDateTime(dtLevel.Rows[0]["REFRESHDT"]).AddDays(1).ToString("dd-MM-yyyy");

                }

                string objLevel = "-1";
                if (View == "OV")
                {
                    csobj = new Common();
                    dtLevel = csobj.SQLReturnDataTable("SELECT TOP 1 LEVELNO FROM DIRECT_REPORT WHERE PositionID= '" + Level + "'");
                    if (dtLevel.Rows.Count >= 1)
                    {
                        objLevel = dtLevel.Rows[0]["LEVELNO"].ToString();
                    }
                }

                //Add MyImages to MyDocument
                for (int Idx = 0; Idx <= MaxPage - 1; Idx++)
                {
                    System.Drawing.Font drawFontText = new System.Drawing.Font("Arail", 8, FontStyle.Bold, GraphicsUnit.Pixel);
                    SolidBrush drawBrushText = new SolidBrush(System.Drawing.Color.Black);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Near;

                    if (LevelCount == 6)
                    {
                        System.Drawing.Rectangle rectLabel;
                        if (objLevel != "-1")
                        {
                            rectLabel = new System.Drawing.Rectangle(5, 975, 200, 20);
                            GraphicImg[Idx].DrawString("Level No : N - " + objLevel, drawFontText, drawBrushText, rectLabel, stringFormat);
                        }
                        rectLabel = new System.Drawing.Rectangle(1350, 975, 200, 20);
                        GraphicImg[Idx].DrawString("Page No : " + (Idx + 1).ToString(), drawFontText, drawBrushText, rectLabel, stringFormat);
                        string sText = "This chart indicates operational and functional coordination relationships supporting the global business. The actual company relationships of the individuals on this chart are to executives within the companies that employ them. For business use only. (source: HR Core) Last updated on " + LastRefresh + " at 1am CET";
                        if (View == "OV")
                            sText = "This chart indicates operational and functional coordination relationships supporting the global business. The actual company relationships of the individuals on this chart are to executives within the companies that employ them. For business use only. (source: HR Core) Last updated on " + LastRefresh + " at 1am CET";
                        rectLabel = new System.Drawing.Rectangle(5, 995, 1550, 20);
                        GraphicImg[Idx].DrawString(sText, drawFontText, drawBrushText, rectLabel, stringFormat);
                    }
                    else
                    {
                        System.Drawing.Rectangle rectLabel;
                        if (objLevel != "-1")
                        {
                            rectLabel = new System.Drawing.Rectangle(5, 975, 200, 20);
                            GraphicImg[Idx].DrawString("Level No : N - " + objLevel, drawFontText, drawBrushText, rectLabel, stringFormat);
                        }
                        rectLabel = new System.Drawing.Rectangle(850, 975, 200, 20);
                        GraphicImg[Idx].DrawString("Page No : " + (Idx + 1).ToString(), drawFontText, drawBrushText, rectLabel, stringFormat);
                        string sText = "This chart indicates operational and functional coordination relationships supporting the global business. The actual company relationships of the individuals on this chart are to executives within the companies that employ them. For business use only. (source: HR Core) Last updated on " + LastRefresh + " at 1am CET";
                        if (View == "OV")
                            sText = "This chart indicates operational and functional coordination relationships supporting the global business. The actual company relationships of the individuals on this chart are to executives within the companies that employ them. For business use only. (source: HR Core) Last updated on " + LastRefresh + " at 1am CET";
                        rectLabel = new System.Drawing.Rectangle(5, 995, 1000, 20);
                        GraphicImg[Idx].DrawString(sText, drawFontText, drawBrushText, rectLabel, stringFormat);
                    }

                    string imgFileName100 = HttpContext.Current.Server.MapPath("PPTX/") + "OrgChart_100_" + Idx.ToString() + ".jpg";
                    ImageOut[Idx].SetResolution(256.0F, 256.0F);

                    // Create an Encoder object based on the GUID
                    // for the Quality parameter category.
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                    // Create an EncoderParameters object.
                    // An EncoderParameters object has an array of EncoderParameter
                    // objects. In this case, there is only one
                    // EncoderParameter object in the array.
                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    ImageOut[Idx].Save(imgFileName100, jgpEncoder, myEncoderParameters);
                }

                try
                {
                    //string pptLayoutPath = "";
                    //pptLayoutPath = HttpContext.Current.Server.MapPath("PPTX/") + "OrgChartInfo.pptx";
                    //string pptFilePath = HttpContext.Current.Server.MapPath("PPTX/") + HttpContext.Current.Session["ID"].ToString() + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_OrgChart.pptx";

                    //if (File.Exists(pptFilePath)) File.Delete(pptFilePath);
                    //File.Copy(pptLayoutPath, pptFilePath);
                    //using (PresentationDocument presentationDocument = PresentationDocument.Open(pptFilePath, true))
                    //{
                    //    string filename = "", imageExt = "image/jpeg";
                    //    PresentationPart presentationPart = presentationDocument.PresentationPart;
                    //    for (int Idx = 0; Idx <= MaxPage - 1; Idx++)
                    //    {
                    //        filename = HttpContext.Current.Server.MapPath("PPTX/") + "OrgChart_100_" + Idx.ToString() + ".jpg";
                    //        Slide slide = new InsertImage().InsertSlide(presentationPart, "Blank");
                    //        new InsertImage().InsertImageInLastSlide(slide, filename, imageExt);
                    //        slide.Save();
                    //        File.Delete(filename);
                    //    }
                    //    presentationDocument.PresentationPart.Presentation.Save();
                    //}

                    //// Deletes the first slide[Empty slide]
                    //InsertImage dslide = new InsertImage();
                    //dslide.DeleteSlide(pptFilePath, 0);

                    //HttpContext.Current.Response.Redirect("downloadFile.aspx?msg=" + pptFilePath, false);
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();

                }
                catch (Exception ex)
                {
                    string errMsg = ex.ToString();
                    Console.WriteLine("  Message: {0}", errMsg);
                }
            }

            private string Level2Check(string Level1Inf, string Id)
            {
                string[] Level1 = Level1Inf.Split(',');
                for (int Idx = 0; Idx <= Level1.Length - 1; Idx++)
                {
                    if (Level1[Idx] == Id) return "N";
                }

                return "Y";
            }

            // Gets the Object Info from the table
            private List<ObjectInf> GetPDFLevelInfo(string Level)
            {
                var theObjectInf = new List<ObjectInf>();
                Common csobj = new Common();
                DataTable dtlevel = null;

                string InfoPos = "", SqlQry = "", ShowLevel = Level;
                string[] SearchIDs = HttpContext.Current.Session["SearchPID"].ToString().Split(':');
                HttpContext.Current.Session["LANGUAGE"] = Language;

                LevelDate = DateTime.Now.ToString("dd-MM-yyyy");
                if (LevelDate != "")
                {
                    HttpContext.Current.Session["LEVEL_ID"] = ShowLevel;
                    HttpContext.Current.Session["LEVEL_DATE"] = LevelDate;
                }
                else
                {
                    HttpContext.Current.Session["LEVEL_ID"] = "";
                    HttpContext.Current.Session["LEVEL_DATE"] = "";
                }

                DateTime fixtureDate = DateTime.Parse(LevelDate, new CultureInfo("en-GB"));
                fixtureDate = fixtureDate.AddDays(-1);
                string prevDate = fixtureDate.Year.ToString() + "-" + fixtureDate.Month.ToString("d2") + "-" + fixtureDate.Day.ToString("d2");
                string keyDate = LevelDate.Substring(6, 4) + "-" + LevelDate.Substring(3, 2) + "-" + LevelDate.Substring(0, 2);
                string SelLang = "";
                if (Language == "EN")
                    SelLang = "EN";
                else
                    SelLang = "00";

                if (View == "OV")
                {
                    SqlQry = "SELECT DISTINCT a.*, b.OLEVEL,c.*, ISNULL(d.orgunitText,'') OrgunitText, ISNULL(e.positiontitle,'') positiontitle, ISNULL(f.POSITIONID, '0') positionflag, g.FullName " +
                                    " FROM REORG_LEVEL_INFO a JOIN LEVEL_NOR_SOC c ON a." + OPR_LEVEL_ID + "=c.LEVEL_ID AND a." +
                                    OPR_PARENT_ID + "=c.PARENT_ID  " +
                                    " JOIN TEXT_ORG d on a.orgunit = d.orgunit " +
                                    " JOIN TEXT_POS e on a.PositionID = e.PositionID " +
                                    " JOIN TEXT_EMP g on a.PERSONID = g.PERSONID AND LANGUAGE_SELECTED = '" + SelLang + "'" +
                                    " LEFT JOIN PositionManagement f on a.PositionID = f.PositionID " +
                                    " , (SELECT '" + ShowLevel + "' OID, '-1' NextPositionID,  '0' OLEVEL " +
                                    " UNION ALL SELECT DISTINCT " + OPR_LEVEL_ID + " OID, li.NextPositionID,  '1' OLEVEL FROM REORG_LEVEL_INFO LI WHERE " +
                                    OPR_PARENT_ID + "='" + ShowLevel + "' AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "'" +
                                    " ) b " +
                                    " WHERE a." + OPR_LEVEL_ID + "=b.OID AND a.LANGUAGE_SELECTED = '" + "EN" + "' AND c.KEY_DATE='" + keyDate + "'" +
                                    " AND A.END_DATE >= '" + keyDate + "'  AND A.START_DATE <= '" + keyDate + "' AND " +
                                    " d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate +
                                    "' AND d.LANGUAGE_SELECTED = '" + "EN" + "' " +
                                    " AND e.END_DATE >= '" + keyDate + "'  AND e.START_DATE <= '" + keyDate +
                                    " AND g.END_DATE >= '" + keyDate + "'  AND g.START_DATE <= '" + keyDate +
                                    "' AND e.LANGUAGE_SELECTED = '" + "EN" + "' " +
                                    "  AND (( b.NextPositionID = a.NextPositionID )  or ( b.NextPositionID = '-1' AND flag='OXM')) ";

                }
                else if (View == "LV")
                {
                    SqlQry = "SELECT DISTINCT a.*, b.OLEVEL, c.*, ISNULL(d.orgunitText,'') OrgunitText, ISNULL(e.positiontitle,'') positiontitle, ISNULL(f.POSITIONID, '0') positionflag, g.FullName " +
                                " FROM LEGAL_INFO a JOIN LEGAL_NOR_SOC c ON  a." + LGL_LEVEL_ID + "=c.LEVEL_ID AND a." +
                                  LGL_PARENT_ID + "=c.PARENT_ID " +
                                " LEFT JOIN TEXT_ORG d on a.nextpositionid = d.orgunit AND D.LANGUAGE_SELECTED = '" + Language + "' " +
                                " LEFT JOIN TEXT_POS e on a.PositionID = e.PositionID  AND E.LANGUAGE_SELECTED = '" + Language + "' " +
                                " JOIN TEXT_EMP g on a.PERSONID = g.PERSONID  AND g.Language_selected = '" + SelLang + "'" +
                                " LEFT JOIN PositionManagement f on a.PositionID = f.PositionID " +
                                ",  (SELECT '" + ShowLevel +
                                "' OID, '-1' NextLevelOrg, '0' OLEVEL " +
                                " UNION ALL SELECT DISTINCT " + LGL_LEVEL_ID + " OID, li.NextLevelOrg, '1' OLEVEL FROM LEGAL_INFO LI WHERE " +
                                  LGL_PARENT_ID + "='" + ShowLevel + "'  AND END_DATE >= '" + keyDate + "'  AND START_DATE <= '" + keyDate + "'" +
                                ") b " +
                                " WHERE a." + LGL_LEVEL_ID + "=b.OID   AND A.LANGUAGE_SELECTED = '" + "EN" + "' " +
                                " AND c.KEY_DATE='" + keyDate + "'" +
                                " AND A.END_DATE >= '" + keyDate + "'  AND A.START_DATE <= '" + keyDate + "' " +
                                " AND d.END_DATE >= '" + keyDate + "'  AND d.START_DATE <= '" + keyDate + "' " +
                                " AND g.END_DATE >= '" + keyDate + "'  AND g.START_DATE <= '" + keyDate + "' " +
                        //" AND (( d.LANGUAGE_SELECTED = '" + Language + "'  AND D.ORGUNIT <> '" + ShowLevel + "') OR D.ORGUNIT = '" + ShowLevel + "') " +
                                " AND e.END_DATE >= '" + keyDate + "'  AND e.START_DATE <= '" + keyDate +
                                "' AND e.LANGUAGE_SELECTED = '" + Language + "' " +
                                "  AND (( b.NextLevelOrg = a.NextLevelOrg )  or (( b.NextLevelOrg = '-1' ) AND ( a.NextLevelOrg = '" + PreviousLevel + "' ))) ";
                }

                csobj = new Common();
                dtlevel = csobj.SQLReturnDataTable("SELECT * FROM ORG_CONFIG_INFO WHERE VIEW_ID='" + HttpContext.Current.Session["VIEW"].ToString() + "'");
                foreach (DataRow drlvl in dtlevel.Rows)
                {
                    //if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = drlvl["FIELD_VALUE"].ToString();
                    if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = LevelFlag;
                    if (drlvl["FIELD_NAME"].ToString() == "HEIGHT") Height = drlvl["FIELD_VALUE"].ToString();
                    if (drlvl["FIELD_NAME"].ToString() == "LINECOLOR")
                    {
                        LineColor = drlvl["FIELD_VALUE"].ToString();
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "LINEWIDTH")
                    {
                        LineWidth = drlvl["FIELD_VALUE"].ToString();
                        blackPen = new Pen(GetDrawingLineColor(LineColor), Convert.ToInt16(LineWidth));
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "TEMPLATE")
                    {
                        TemplateURL = drlvl["FIELD_VALUE"].ToString();
                        string[] TempURL = TemplateURL.Split('~');
                        if (TempURL[0] == "IMG")
                        {
                            BmpURL = new Bitmap(System.Web.HttpContext.Current.Server.MapPath(TempURL[1]));
                            BmpURL.SetResolution(1200f, 1200f);
                        }
                        BmpUpArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/uparrow.png"));
                        BmpDnArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/downarrow.ico"));
                    }
                }

                // Gets the Box Height
                Original_Height = Convert.ToInt32(Height);
                Original_Height_10 = Original_Height + 40;
                Adjustment_Height = ((((Original_Height - (Original_Height % 30)) / 30)) * LevelCount) - 1;
                HttpContext.Current.Session["BoxHeight"] = Original_Height;

                DataTable dtlbl = null;
                DataTable dtconf = csobj.SQLReturnDataTable("SELECT * FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + HttpContext.Current.Session["VIEW"].ToString() + "'");
                string OrderBy = "OLEVEL, ";

                if (LevelFlag == "1")
                {
                    OrderBy = "OLEVEL, SORTNO, ";
                }
                if (View == "OV")
                    OrderBy += OPR_PARENT_ID + ", " + OPR_LEVEL_ID;
                else if (View == "LV")
                    OrderBy += LGL_PARENT_ID + ", " + LGL_LEVEL_ID;
                dtlbl = csobj.SQLReturnDataTable(SqlQry + " ORDER BY " + OrderBy);

                string[] CONFIG_INFO = SUPPRESS_FIELDS.Split(',');
                string SUP_FIELDS = "";
                for (int Idx = 0; Idx <= CONFIG_INFO.Length - 1; Idx++)
                    SUP_FIELDS += ",\'" + CONFIG_INFO[Idx] + "\'";

                foreach (DataRow dr in dtlbl.Rows)
                {
                    InfoPos = "";
                    foreach (DataRow drconf in dtconf.Rows)
                    {
                        try
                        {
                            if (dr["POSITIONFLAG"].ToString() == dr["POSITIONID"].ToString())
                            {
                                if ((drconf["FIELD_NAME"].ToString() == "FIRSTNAME") || (drconf["FIELD_NAME"].ToString() == "PositionTitle"))
                                {
                                    InfoPos += ";    |" +
                                                     drconf["FIELD_ROW"].ToString() + "|" +
                                                     drconf["FIELD_COL"].ToString();
                                }
                                else
                                {
                                    InfoPos += ";" + dr[drconf["FIELD_NAME"].ToString()].ToString() + "|" +
                                                     drconf["FIELD_ROW"].ToString() + "|" +
                                                     drconf["FIELD_COL"].ToString();
                                }
                            }
                            else
                            {
                                InfoPos += ";" + dr[drconf["FIELD_NAME"].ToString()].ToString() + "|" +
                                                 drconf["FIELD_ROW"].ToString() + "|" +
                                                 drconf["FIELD_COL"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            string errMsg = ex.ToString();
                            Console.WriteLine("  Message: {0}", errMsg);
                        }
                    }

                    string sColor = "0", sBackColor = "0";
                    if (Convert.ToInt32(dr["OLEVEL"].ToString()) <= (Convert.ToInt32(LevelUpto)))
                    {
                        if (!((dr["POSITIONFLAG"].ToString() == dr["POSITIONID"].ToString()) && (dr["NOR_COUNT"].ToString() == "0") && (dr["SOC_COUNT"].ToString() == "0")))
                        {
                            if (View == "OV")
                            {
                                if (dr[OPR_LEVEL_ID].ToString() != ShowLevel) sNextLevelNo = sNextLevelNo + "," + dr[OPR_LEVEL_ID].ToString();
                                if (SearchIDs.Length >= 2)
                                {
                                    if (dr[OPR_LEVEL_ID].ToString() == SearchIDs[0] && dr[OPR_PARENT_ID].ToString() == SearchIDs[1]) sBackColor = "#F5DEB3"; else sBackColor = "0";
                                }
                                else sBackColor = "0";
                                if ((dr["EMPGROUP"].ToString() == "9") || (dr["EMPGROUP"].ToString() == "7")) sColor = "#ffb266"; else sColor = "0";
                                if (dr["EMPSUBGROUP"].ToString().ToUpper() == "FJ")                      // CR No               : 1100055528
                                {
                                    sBackColor = "#ffb266";
                                    sColor = "0";
                                }

                                if ((dr["NOR_COUNT"].ToString() != "0") && dr["OLEVEL"].ToString() != "0") AddPositionID(dr[OPR_LEVEL_ID].ToString());
                                theObjectInf.Add(new ObjectInf(dr[OPR_LEVEL_ID].ToString(),
                                                                 InfoPos.Substring(1),
                                                                 dr[OPR_PARENT_ID].ToString(),
                                                                 dr["OLEVEL"].ToString(),
                                                                 0, 0, 0, 0, 175, Original_Height,
                                                                 dr["NEXT_LEVEL_FLAG"].ToString(),
                                                                 dr["GRAY_COLORED_FLAG"].ToString(),
                                                                 dr["DOTTED_LINE_FLAG"].ToString(),
                                                                 dr["SHOW_FULL_BOX"].ToString(),
                                                                 dr["LANGUAGE_SELECTED"].ToString(),
                                                                 dr["SORTNO"].ToString(),
                                                                 dr["POSITIONFLAG"].ToString(),
                                                                 sColor,
                                                                 sBackColor,
                                                                 dr["FLAG"].ToString(),
                                                                 "0",
                                                                 dr["GDDBID"].ToString()));
                            }
                            else if (View == "LV")
                            {
                                if (SearchIDs.Length >= 2)
                                {
                                    if (dr[OPR_LEVEL_ID].ToString() == SearchIDs[0] && dr[OPR_PARENT_ID].ToString() == SearchIDs[1]) sBackColor = "#F5DEB3"; else sBackColor = "0";
                                }
                                else sBackColor = "0";
                                if ((dr["EMPGROUP"].ToString() == "9") || (dr["EMPGROUP"].ToString() == "7")) sColor = "#ffb266"; else sColor = "0";
                                if (dr["EMPSUBGROUP"].ToString().ToUpper() == "FJ")                      // CR No               : 1100055528
                                {
                                    sBackColor = "#ffb266";
                                    sColor = "0";
                                }

                                if (dr["NOR_COUNT"].ToString() != "0") AddPositionID(dr[LGL_LEVEL_ID].ToString());
                                theObjectInf.Add(new ObjectInf(dr[LGL_LEVEL_ID].ToString(),
                                                                 InfoPos.Substring(1),
                                                                 dr[LGL_PARENT_ID].ToString(),
                                                                 dr["OLEVEL"].ToString(),
                                                                 0, 0, 0, 0, 175, Original_Height,
                                                                 dr["NEXT_LEVEL_FLAG"].ToString(),
                                                                 dr["GRAY_COLORED_FLAG"].ToString(),
                                                                 dr["DOTTED_LINE_FLAG"].ToString(),
                                                                 dr["SHOW_FULL_BOX"].ToString(),
                                                                 dr["LANGUAGE_SELECTED"].ToString(),
                                                                 dr["SORTNO"].ToString(),
                                                                 dr["POSITIONFLAG"].ToString(),
                                                                 sColor,
                                                                 sBackColor,
                                                                 dr["FLAG"].ToString(),
                                                                 "0",
                                                                 dr["GDDBID"].ToString()));
                            }
                        }
                    }
                }

                // Json object to show level information
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JsonFieldInfo = javaScriptSerializer.Serialize(theObjectInf);

                return theObjectInf;
            }
            //***********CR:310012488, CD: 1400032929, Modified By :NAGARVI5******************************
            public void AddPositionID(string PostionID)
            {
                sParentLevelNo = sParentLevelNo + "," + PostionID;
                for (int Idx = 0; Idx <= lstID.Count - 1; Idx++)
                {
                    if (lstID[Idx] == PostionID) return;
                }
                lstID.Add(PostionID);
            }

            //***********CR:310012488, CD: 1400032929, Modified By :NAGARVI5******************************
            public List<ObjectInf> ConvertToPDF(string AllPDF, string ShowID)
            {
                string LastRefresh = "";
                Common csobj = new Common();
                DataTable dtLevel = csobj.SQLReturnDataTable("SELECT TOP 1 REFRESHDT FROM LEVEL_INFO ");
                //if (dtLevel.Rows.Count >= 1)
                //{
                LastRefresh = Convert.ToDateTime("2017-08-10 00:00:00.000").AddDays(1).ToString("dd-MM-yyyy");

                //}

                int iWidth = 255, FLC = 0; sAllPDF = "N";
                if (LevelCount == 6) iWidth = 255;

                // Collects the Hierarchy data to show 
                strOutput = "PDF";
                PDFPage = CurPage;
                Output_Height = 1020;
                List<ObjectInf> theObjectInf = null;
                if (AllPDF == "Y")
                    theObjectInf = GetPDFLevelInfo(ShowID);
                else
                {

                    theObjectInf = GetLevelInfo();
                    string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(theObjectInf);
                    theObjectInf = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<ObjectInf>>(json).OrderBy(o => Convert.ToInt32(o.Level)).ToList();
                    csobj = new Common();
                    DataTable dtlevel = csobj.SQLReturnDataTable("SELECT * FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='VIEW_DEFAULT'");
                    foreach (DataRow drlvl in dtlevel.Rows)
                    {
                        //if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = drlvl["FIELD_VALUE"].ToString();
                        if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = LevelFlag;
                        if (drlvl["FIELD_NAME"].ToString() == "HEIGHT") Height = drlvl["FIELD_VALUE"].ToString();
                        if (drlvl["FIELD_NAME"].ToString() == "LINECOLOR")
                        {
                            LineColor = drlvl["FIELD_VALUE"].ToString();
                        }
                        if (drlvl["FIELD_NAME"].ToString() == "LINEWIDTH")
                        {
                            LineWidth = drlvl["FIELD_VALUE"].ToString();
                            blackPen = new Pen(GetDrawingLineColor(LineColor), Convert.ToInt16(LineWidth));
                        }
                        if (drlvl["FIELD_NAME"].ToString() == "TEMPLATE")
                        {
                            TemplateURL = drlvl["FIELD_VALUE"].ToString();
                            string[] TempURL = TemplateURL.Split('~');
                            if (TempURL[0] == "IMG")
                            {
                                BmpURL = new Bitmap(System.Web.HttpContext.Current.Server.MapPath(TempURL[1]));
                                BmpURL.SetResolution(1200f, 1200f);
                            }
                            BmpUpArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/uparrow.png"));
                            BmpDnArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/downarrow.ico"));
                        }
                    }

                    // Gets the Box Height
                    Original_Height = Convert.ToInt32(Height);
                    Original_Height_10 = Original_Height + 40;
                    Adjustment_Height = ((((Original_Height - (Original_Height % 30)) / 30)) * LevelCount) - 1;
                    HttpContext.Current.Session["BoxHeight"] = Original_Height;
                    //theObjectInf = GetLevelInfo();
                }

                // Gets the available levels
                LevelInf = GetAllLevels(theObjectInf).Split(',');
                if (LevelInf.Count() <= 1) return theObjectInf;
                LevelIds = new string[LevelInf.Length];
                ObjectInf Obj = null, CurObj = null, PrevObj = null;
                if (LevelInf.Length >= 1)
                {
                    // Get Level based Ids
                    for (int Idx = 0; Idx <= LevelInf.Length - 1; Idx++)
                    {
                        LevelIds[Convert.ToInt16(LevelInf[Idx])] = LevelInId(theObjectInf, LevelInf[Idx], Idx);
                    }

                    // Calculates the Height for each Ids
                    Obj = GetIdInfo(theObjectInf, LevelIds[0].ToString());
                    Obj.Height = Original_Height_10;
                    Obj.Width = iWidth;
                    string sSkip = "N";
                    FLC = LevelIds[1].Split(',').Length;
                    for (int Idx = LevelIds.Length - 1; Idx >= 1; Idx--)
                    {
                        string[] Ids = LevelIds[Idx].Split(',');
                        if (Idx == 1) FLC = Ids.Length;
                        for (int Idy = 0; Idy <= Ids.Length - 1; Idy++)
                        {
                            sSkip = "N";
                            //Obj = GetIdInfo(theObjectInf, Ids[Idy].ToString());

                            if (Idx == 1) Obj = theObjectInf[Idy + 1]; else Obj = theObjectInf[Idy + FLC + 1];
                            if (Idx == 2) sSkip = Level2Check(LevelIds[1], Obj.PId);
                            if (sSkip == "N")
                            {
                                if (Obj.Level == "2")                // Last Level
                                {
                                    Obj.Height = Original_Height_10 - 22;
                                    Obj.Width = iWidth;
                                }
                                else
                                {
                                    Obj.Height = GetChildHeight(theObjectInf, Ids[Idy].ToString());
                                    Obj.Width = iWidth;
                                }
                            }
                        }
                    }

                    // Calculates the Row and Column info for each Ids
                    int Row = 2, Col = 352, AddHeight = 0, MaxHeight = 0;
                    FLC = 0;
                    Obj = GetIdInfo(theObjectInf, LevelIds[0].ToString());
                    Obj.Row = Row;
                    Obj.Col = Col;
                    //  try{
                    for (int Idx = 1; Idx <= LevelIds.Length - 1; Idx++)
                    {
                        string[] Ids = LevelIds[Idx].Split(',');
                        if (Idx == 1) FLC = Ids.Length;
                        for (int Idy = 0; Idy <= Ids.Length - 1; Idy++)
                        {
                            sSkip = "N";
                            if (Idx == 1)
                            {
                                //CurObj = GetIdInfo(theObjectInf, Ids[Idy].ToString());
                                CurObj = theObjectInf[Idy + 1];
                            }
                            else
                                CurObj = theObjectInf[Idy + FLC + 1];

                            if (Idx == 2) sSkip = Level2Check(LevelIds[1], CurObj.PId);
                            if (sSkip == "N")
                            {
                                if ((Idx == 1) && ((Idy % LevelCount) == 0) && (Idy != 0))
                                {
                                    AddHeight += MaxHeight;
                                    MaxHeight = 0;
                                }
                                //changed by vignesh
                                if (Idx == 1)
                                {
                                    try
                                    {
                                        CurObj = theObjectInf[Idy + 1];
                                        Obj = GetIdInfo(theObjectInf, CurObj.PId);             // To get the Row in which this object is displayed

                                        Row = Obj.Row + Obj.Height + AddHeight;
                                        Col = ((Idy % LevelCount) * iWidth) + 5;
                                        if (MaxHeight <= CurObj.Height) MaxHeight = CurObj.Height;
                                    }
                                    catch (Exception Ex)
                                    {

                                        string errMsg = Ex.ToString();
                                        Console.WriteLine("  Message: {0}", errMsg);

                                    }

                                }
                                else
                                {
                                    CurObj = theObjectInf[Idy + FLC + 1];
                                    Obj = GetIdInfo(theObjectInf, CurObj.PId);             // To get the Row in which this object is displayed

                                    Row = Obj.Row;
                                    for (int Idz = 0; Idz <= Ids.Length - 1; Idz++)
                                    {
                                        //PrevObj = GetIdInfo(theObjectInf, Ids[Idz].ToString());
                                        PrevObj = theObjectInf[Idz + FLC + 1];
                                        if (PrevObj.PId == Obj.Id)
                                        {
                                            Row += PrevObj.Height;
                                            Col = Obj.Col + 15;
                                            if (PrevObj.Id == Ids[Idy].ToString()) break;
                                        }
                                    }
                                }
                                CurObj.Row = Row;
                                CurObj.Col = Col;
                            }
                        }
                    }
                    // }
                    // catch(Exception EX)
                    // {}


                }

                // Creates the Document
                if (AllPDF == "N")
                {
                    MyDocument = new ceTe.DynamicPDF.Document();
                    MyDocument.Creator = "DynamicChart.aspx";
                    MyDocument.Author = "Subramanian";
                    MyDocument.Title = "Organization Chart";
                    PageSizeNos.Add(0);
                    //(PageSize.B4, PageOrientation.Landscape, 0.3F);
                    if (LevelCount == 4)
                        MyPage[CurPage] = new ceTe.DynamicPDF.Page(1080F, 1050F, 15.3F);
                    else
                        MyPage[CurPage] = new ceTe.DynamicPDF.Page(1600F, 1050F, 15.3F);
                    MaxPage = 1;


                    // Places the Level Information in the PDF
                    int LevelShowUpto = Convert.ToInt32(LevelUpto);
                    if ((LevelIds.Length >= 3) && (LevelShowUpto >= 2)) Level_Information(theObjectInf);         // Level without child Information 
                    if ((LevelIds.Length >= 2) && (LevelShowUpto >= 1)) Level_1_Information(theObjectInf);       // Level 1 Information 
                    if ((LevelIds.Length >= 2) && (LevelShowUpto >= 1)) Level_1_Line_Information(theObjectInf);  // Level 1 Line Information 
                    if ((LevelIds.Length >= 1) && (LevelShowUpto >= 0)) Level_0_Information(theObjectInf, "C", "", "");       // Level 0 Information


                    string objLevel = "-1";
                    if (View == "OV")
                    {
                        csobj = new Common();
                        //JAI
                        dtLevel = csobj.SQLReturnDataTable("SELECT TOP 1 LEVELNO FROM [DIRECT_REPORT] WHERE PositionID= '" + Level + "'");
                        //dtLevel = csobj.SQLReturnDataTable("SELECT TOP 1 LEVELNO FROM REORG_DIRECT_REPORT WHERE PositionID= '" + Level + "'");
                        if (dtLevel.Rows.Count >= 1)
                        {
                            objLevel = dtLevel.Rows[0]["LEVELNO"].ToString();
                        }
                    }

                    for (int Idx = 0; Idx <= MaxPage - 1; Idx++)
                    {
                        if (LevelCount == 6)
                        {
                            if (objLevel != "-1")
                            {
                                MyLabel = new ceTe.DynamicPDF.PageElements.Label("Level No : N - " + objLevel, 5, 975, 200, 20, ceTe.DynamicPDF.Font.HelveticaBold, 15, ceTe.DynamicPDF.TextAlign.Left);
                                MyPage[Idx].Elements.Add(MyLabel);
                            }

                            MyLabel = new ceTe.DynamicPDF.PageElements.Label("Page No : " + (Idx + 1).ToString(), 1350, 975, 200, 20, ceTe.DynamicPDF.Font.HelveticaBold, 15, ceTe.DynamicPDF.TextAlign.Right);
                            MyPage[Idx].Elements.Add(MyLabel);

                            string LevelDate = HttpContext.Current.Session["LEVEL_DATE"].ToString();
                            string keyDate = LevelDate.Substring(6, 4) + "-" + LevelDate.Substring(3, 2) + "-" + LevelDate.Substring(0, 2);
                            string sText = "This org. chart shows the legal entity situation of Novartis. The primary structure is the country-based followed by the legal entities. The legal chart shows only the legal relationship between employee and manager, hence there are no dotted lines shown in this chart. Source HR Core and last refresh on current date(" + LastRefresh + ") at 1am CET.";
                            if (View == "OV")
                                sText = "This chart indicates operational and functional coordination relationships supporting the global business. The actual company relationships of the individuals on this chart are to executives within the companies that employ them. For business use only. (source: HR Core) Last updated on " + LastRefresh + " at 1am CET";
                            MyLabel = new ceTe.DynamicPDF.PageElements.Label(sText, 5, 995, 1550, 20, ceTe.DynamicPDF.Font.HelveticaBold, 10, ceTe.DynamicPDF.TextAlign.Left);
                            MyPage[Idx].Elements.Add(MyLabel);

                        }
                        else
                        {
                            if (objLevel != "-1")
                            {
                                MyLabel = new ceTe.DynamicPDF.PageElements.Label("Level No : N - " + objLevel, 5, 975, 200, 20, ceTe.DynamicPDF.Font.HelveticaBold, 8, ceTe.DynamicPDF.TextAlign.Left);
                                MyPage[Idx].Elements.Add(MyLabel);
                            }

                            MyLabel = new ceTe.DynamicPDF.PageElements.Label("Page No : " + (Idx + 1).ToString(), 850, 975, 200, 20, ceTe.DynamicPDF.Font.HelveticaBold, 8, ceTe.DynamicPDF.TextAlign.Right);
                            MyPage[Idx].Elements.Add(MyLabel);

                            string LevelDate = HttpContext.Current.Session["LEVEL_DATE"].ToString();
                            string keyDate = LevelDate.Substring(6, 4) + "-" + LevelDate.Substring(3, 2) + "-" + LevelDate.Substring(0, 2);
                            string sText = "This org. chart shows the legal entity situation of Novartis. The primary structure is the country-based followed by the legal entities. The legal chart shows only the legal relationship between employee and manager, hence there are no dotted lines shown in this chart. Source HR Core and last refresh on current date(" + LastRefresh + ") at 1am CET.";
                            if (View == "OV")
                                sText = "This chart indicates operational and functional coordination relationships supporting the global business. The actual company relationships of the individuals on this chart are to executives within the companies that employ them. For business use only. (source: HR Core) Last updated on " + LastRefresh + " at 1am CET";
                            MyLabel = new ceTe.DynamicPDF.PageElements.Label(sText, 5, 995, 1040, 20, ceTe.DynamicPDF.Font.HelveticaBold, 8, ceTe.DynamicPDF.TextAlign.Left);
                            MyPage[Idx].Elements.Add(MyLabel);
                        }
                        //MyLabel = new ceTe.DynamicPDF.PageElements.Label("Page No.: " + (Idx + 1).ToString(), 5, 985, 200, 8, ceTe.DynamicPDF.Font.Helvetica, 8, ceTe.DynamicPDF.TextAlign.Left);
                        //MyPage[Idx].Elements.Add(MyLabel);

                        MyDocument.Pages.Add(MyPage[Idx]);
                    }

                    //Outputs the MyDocument to the current web MyPage
                    MyDocument.DrawToWeb("OrgChart.pdf");
                }

                return theObjectInf;
            }

            // Sets the sort information
            public string SetSort(string ID)
            {
                string jsonString = "";
                if (HttpContext.Current.Session["VIEW_TYPE"].ToString() == "OV")
                    jsonString = HttpContext.Current.Session["theOVObjectInf"].ToString();
                else
                    jsonString = HttpContext.Current.Session["theLVObjectInf"].ToString();
                List<ObjectInf> theObjectInf = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<ObjectInf>>(jsonString);

                string[] stR = ID.Split(',');
                for (int idx = 0; idx <= stR.Length - 1; idx++)
                {
                    string[] aValue = stR[idx].Split(':');
                    if (aValue[0] != "undefined")
                    {
                        if (theObjectInf.Count >= 1)
                        {
                            foreach (ObjectInf obj in theObjectInf)
                            {
                                if (obj.Id == aValue[0].Substring(0, 8)) obj.SortNo = aValue[1];
                            }
                        }
                    }
                }

                jsonString = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(theObjectInf);
                if (HttpContext.Current.Session["VIEW_TYPE"].ToString() == "OV")
                    HttpContext.Current.Session["theOVObjectInf"] = jsonString;
                else
                    HttpContext.Current.Session["theLVObjectInf"] = jsonString;


                return "success";
            }

            // Get the Parent ID
            private DataRow GetParentID(DataTable dtLevel, string PID)
            {
                DataRow dtRow = null;
                foreach (DataRow dtLevelRow in dtLevel.Select("LEVEL_FLAG='Y'"))
                {
                    if (dtLevelRow[OPR_LEVEL_ID].ToString() == PID) return dtLevelRow;
                }

                return dtRow;
            }

            // Gets the ancestor Page No. 
            private string GetAncestorPageNo(string sID)
            {
                for (int Idx = 0; Idx <= lstID.Count - 1; Idx++)
                {
                    if (lstID[Idx] == sID) return lstPageNo[Idx].ToString();
                }

                return "-1";
            }

            //Set the Page number
            private void SetPositionPageNo(string sID, int PageNo)
            {
                if (thePageObjectInf.Count >= 1)
                {
                    foreach (PageObjectInf PObj in thePageObjectInf)
                    {
                        if (PObj.Id == sID) PObj.PageNo = PageNo;
                    }
                }
            }

            private void PlacePageNumberInPPTSlide(int PageIdx)
            {
                if (thePageObjectInf.Count >= 1)
                {
                    var listObj = from p in thePageObjectInf
                                  where p.CurPageNo == PageIdx
                                  orderby p.Row, p.Col
                                  select p;

                    if ((listObj != null) && (listObj.Count() >= 1))
                    {
                        System.Drawing.Font drawFontText = new System.Drawing.Font("Arail", 12, FontStyle.Bold, GraphicsUnit.Pixel);
                        SolidBrush drawBrushText = new SolidBrush(System.Drawing.Color.Black);
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Near;

                        foreach (PageObjectInf PObj in listObj)
                        {
                            if (PObj != null)
                            {
                                System.Drawing.Rectangle rectLabel = new System.Drawing.Rectangle(PObj.Col + 40, PObj.Row, 200, 20);
                                GraphicPIC[PageIdx].DrawString("( # " + PObj.PageNo.ToString() + " )", drawFontText, drawBrushText, rectLabel, stringFormat);
                            }
                        }
                    }
                }
            }

            // Create the all level PDF
            public string CreateAllLevelPDF(string ShowType)
            {
                DateTime fixtureDate = DateTime.Now;
                string KeyDate = fixtureDate.Year.ToString() + fixtureDate.Month.ToString("d2") + fixtureDate.Day.ToString("d2");
                string curFile = "";
                if (ShowType == "PDF")
                {
                    curFile = HttpContext.Current.Server.MapPath("PDF/" + HttpContext.Current.Session["ID"].ToString() + "_" + KeyDate + ".pdf");
                    if (File.Exists(curFile))
                    {
                        FileInfo myDoc = new FileInfo(curFile);

                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=" + myDoc.Name);
                        HttpContext.Current.Response.AddHeader("Content-Length", myDoc.Length.ToString());
                        HttpContext.Current.Response.ContentType = "application/octet-stream";
                        HttpContext.Current.Response.WriteFile(myDoc.FullName);
                        HttpContext.Current.Response.End();

                        return "";
                    }
                }
                string sID = "", sTPage = "", sCPage = "", strCVIEW = "";

                string pptLayoutPath = HttpContext.Current.Server.MapPath("PPTX/") + "OrgChartInfo.pptx";
                string pptFilePath = HttpContext.Current.Server.MapPath("PPTX/") + HttpContext.Current.Session["ID"].ToString() + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_OrgChart.pptx";

                // Gets the Height for the image
                Common csobj = new Common();
                if (View == "OV") sAllPDF = "Y"; else sAllPDF = "N";
                strCVIEW = HttpContext.Current.Session["VIEW"].ToString();
                if (strCVIEW != "VIEW_DEFAULT" && strCVIEW != "NBS Template with photo") strCVIEW = "VIEW_DEFAULT";
                DataTable dtlevel = csobj.SQLReturnDataTable("SELECT * FROM ORG_CONFIG_INFO WHERE VIEW_ID='" + strCVIEW + "'");

                foreach (DataRow drlvl in dtlevel.Rows)
                {
                    if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = LevelFlag;
                    if (drlvl["FIELD_NAME"].ToString() == "HEIGHT") Height = drlvl["FIELD_VALUE"].ToString();
                    if (drlvl["FIELD_NAME"].ToString() == "LINECOLOR")
                    {
                        LineColor = drlvl["FIELD_VALUE"].ToString();
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "LINEWIDTH")
                    {
                        LineWidth = drlvl["FIELD_VALUE"].ToString();
                        blackPen = new Pen(GetDrawingLineColor(LineColor), Convert.ToInt16(LineWidth));
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "TEMPLATE")
                    {
                        TemplateURL = drlvl["FIELD_VALUE"].ToString();
                        string[] TempURL = TemplateURL.Split('~');
                        if (TempURL[0] == "IMG")
                        {
                            BmpURL = new Bitmap(System.Web.HttpContext.Current.Server.MapPath(TempURL[1]));
                            BmpURL.SetResolution(1200f, 1200f);
                        }
                        BmpUpArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/uparrow.png"));
                        BmpDnArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/downarrow.ico"));
                    }
                }

                // Gets the Box Height
                Output_Height = 1020;
                Original_Height = Convert.ToInt32(Height);
                Original_Height_10 = Original_Height + 40;
                Adjustment_Height = ((((Original_Height - (Original_Height % 30)) / 30)) * LevelCount) - 1;
                HttpContext.Current.Session["BoxHeight"] = Original_Height;

                // Gets all information
                csobj = new Common();
                string LastRefresh = "";
                DataTable dtLevel = csobj.SQLReturnDataTable("SELECT TOP 1 REFRESHDT FROM LEVEL_INFO");
                if (dtLevel.Rows.Count >= 1)
                {
                    LastRefresh = Convert.ToDateTime(dtLevel.Rows[0]["REFRESHDT"]).AddDays(1).ToString("dd-MM-yyyy");

                }

                csobj = new Common();
                string sSQL = "";
                DataTable dtObj = null;
                if (View == "OV")
                {
                    sSQL = "SELECT DISTINCT a.*, b.NOR_COUNT, b.SOC_COUNT " +
                                " FROM DIRECT_REPORT a LEFT JOIN LEVEL_NOR_SOC b ON a.POSITIONID=b.LEVEL_ID AND b.KEY_DATE='" + DateTime.Now.ToString("MM-dd-yyyy") + "'" +
                                " WHERE a.DrType='OV'";
                    dtObj = csobj.SQLReturnDataTable("SELECT * FROM DIRECT_REPORT_DATA WHERE DrType='" + View + "'");
                }
                else if (View == "LV")
                {
                    //sSQL = "SELECT DISTINCT a.*, b.NOR_COUNT, b.SOC_COUNT " +
                    //            " FROM DIRECT_REPORT_LV a LEFT JOIN LEGAL_NOR_SOC b ON a.POSITIONID=b.LEVEL_ID AND b.KEY_DATE='" + DateTime.Now.ToString("MM-dd-yyyy") + "'" +
                    //            " WHERE a.DrType='LV'";
                    //dtObj = csobj.SQLReturnDataTable("SELECT * FROM DIRECT_REPORT_DATA_LV WHERE DrType='" + View + "'");
                    if (Language == "FR")
                    {
                        sSQL = "SELECT DISTINCT a.*, b.NOR_COUNT, b.SOC_COUNT " +
                                    " FROM DIRECT_REPORT_FR_LV a LEFT JOIN LEGAL_NOR_SOC b ON a.POSITIONID=b.LEVEL_ID AND b.KEY_DATE='" + DateTime.Now.ToString("MM-dd-yyyy") + "'" +
                                    " WHERE a.DrType='LV'";
                        dtObj = csobj.SQLReturnDataTable("SELECT * FROM DIRECT_REPORT_DATA_FR_LV WHERE DrType='" + View + "'");
                    }
                    else if (Language == "EN")
                    {
                        sSQL = "SELECT DISTINCT a.*, b.NOR_COUNT, b.SOC_COUNT " +
                                    " FROM DIRECT_REPORT_LV a LEFT JOIN LEGAL_NOR_SOC b ON a.POSITIONID=b.LEVEL_ID AND b.KEY_DATE='" + DateTime.Now.ToString("MM-dd-yyyy") + "'" +
                                    " WHERE a.DrType='LV'";
                        dtObj = csobj.SQLReturnDataTable("SELECT * FROM DIRECT_REPORT_DATA_LV WHERE DrType='" + View + "'");
                    }

                }
                DataTable dtLevelInfo = csobj.SQLReturnDataTable(sSQL);

                lstID.Clear();
                lstLevel.Clear();
                lstID.Add(HttpContext.Current.Session["ID"].ToString());
                lstPageNo.Add("1");

                DataRow[] drRow = dtLevelInfo.Select("POSITIONID='" + lstID[0].ToString() + "'");
                DataRow[] drNOR = dtLevelInfo.Select("POSITIONID='" + lstID[0].ToString() + "'");
                if (drRow.Length >= 1)
                {
                    if (Convert.ToInt32(drRow[0]["SOC_COUNT"]) <= 60000)
                    {
                        MyDocument = new ceTe.DynamicPDF.Document();
                        MyDocument.Creator = "DynamicChart.aspx";
                        MyDocument.Author = "Raj";
                        MyDocument.Title = "All Organization Chart";

                        int iLevel = 2;
                        iTotalPage = 0;
                        lstLevel.Add(iLevel.ToString());
                        for (int Idc = 0; Idc <= lstID.Count - 1; Idc++)
                        {
                            try
                            {
                                drRow = dtLevelInfo.Select("POSITIONID='" + lstID[Idc].ToString() + "'");
                                if (drRow.Length >= 1)
                                {

                                    SetPositionPageNo(lstID[Idc].ToString(), iTotalPage + 1);
                                    if (drRow[0]["NextLevel"].ToString() != "")
                                    {
                                        //iLevel = Convert.ToInt16(lstLevel[Idy]);
                                        string[] LevelInf = drRow[0]["NextLevel"].ToString().Split(',');
                                        //if (LevelInf.Length >= 1) iLevel++;
                                        for (int Ida = 0; Ida <= LevelInf.Length - 1; Ida++)
                                        {
                                            if (dtLevelInfo.Select("POSITIONID='" + LevelInf[Ida].ToString() + "'").Count() >= 1)
                                            {
                                                lstID.Add(LevelInf[Ida].ToString());
                                                lstPageNo.Add((iTotalPage + 1).ToString());
                                            }
                                            iLevel = Convert.ToInt32(drRow[0]["LevelNo"].ToString());
                                        }

                                        DataRow[] drObj = dtObj.Select("POSITIONID='" + lstID[Idc].ToString() + "'");
                                        foreach (DataRow drInf in drObj)
                                        {
                                            sTPage = (Convert.ToInt32(drRow[0]["MaxLevel"].ToString()) + 1).ToString();
                                            sCPage = (Convert.ToInt32(drInf["DRIndex"].ToString()) + 1).ToString();
                                            string jsonString = drInf["Data1"].ToString();
                                            if (drInf["Data2"] != null) jsonString += drInf["Data2"].ToString();
                                            if (drInf["Data3"] != null) jsonString += drInf["Data3"].ToString();
                                            if (drInf["Data4"] != null) jsonString += drInf["Data4"].ToString();
                                            jsonString = jsonString.Substring(0, jsonString.IndexOf("}]") + 2);
                                            List<ObjectInf> theObjectInf = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<ObjectInf>>(jsonString);

                                            foreach (ObjectInf Obj in theObjectInf)
                                            {
                                                Obj.Oheight = 120;
                                            }

                                            LevelInf = GetAllLevels(theObjectInf).Split(',');
                                            LevelIds = new string[LevelInf.Length];
                                            if (LevelInf.Length >= 1)
                                            {
                                                // Get Level based Ids
                                                for (int Idx = 0; Idx <= LevelInf.Length - 1; Idx++)
                                                {
                                                    LevelIds[Convert.ToInt16(LevelInf[Idx])] = LevelInId(theObjectInf, LevelInf[Idx], Idx);
                                                }

                                                // Calculates the Height for each Ids
                                                int iWidth = 255, FLC = 0;
                                                if (LevelCount == 6) iWidth = 255;

                                                ObjectInf Obj = null, CurObj = null, PrevObj = null;
                                                Obj = GetIdInfo(theObjectInf, LevelIds[0].ToString());
                                                Obj.Height = Original_Height_10;
                                                Obj.Width = iWidth;
                                                string sSkip = "N";
                                                FLC = LevelIds[1].Split(',').Length;
                                                for (int Idx = LevelIds.Length - 1; Idx >= 1; Idx--)
                                                {
                                                    string[] Ids = LevelIds[Idx].Split(',');
                                                    if (Idx == 1) FLC = Ids.Length;
                                                    for (int Idy = 0; Idy <= Ids.Length - 1; Idy++)
                                                    {
                                                        sSkip = "N";
                                                        //Obj = GetIdInfo(theObjectInf, Ids[Idy].ToString());

                                                        if (Idx == 1) Obj = theObjectInf[Idy + 1]; else Obj = theObjectInf[Idy + FLC + 1];
                                                        if (Idx == 2) sSkip = Level2Check(LevelIds[1], Obj.PId);
                                                        if (sSkip == "N")
                                                        {
                                                            if (Obj.Level == "2")                // Last Level
                                                            {
                                                                Obj.Height = Original_Height_10 - 22;
                                                                Obj.Width = iWidth;
                                                            }
                                                            else
                                                            {
                                                                Obj.Height = GetChildHeight(theObjectInf, Ids[Idy].ToString());
                                                                Obj.Width = iWidth;
                                                            }
                                                        }
                                                    }
                                                }

                                                // Calculates the Row and Column info for each Ids
                                                int Row = 20, Col = 352, AddHeight = 0, MaxHeight = 0;
                                                FLC = 0;
                                                Obj = GetIdInfo(theObjectInf, LevelIds[0].ToString());
                                                Obj.Row = Row;
                                                Obj.Col = Col;
                                                for (int Idx = 1; Idx <= LevelIds.Length - 1; Idx++)
                                                {
                                                    string[] Ids = LevelIds[Idx].Split(',');
                                                    if (Idx == 1) FLC = Ids.Length;
                                                    for (int Idy = 0; Idy <= Ids.Length - 1; Idy++)
                                                    {
                                                        sSkip = "N";
                                                        if (Idx == 1)
                                                        {
                                                            //CurObj = GetIdInfo(theObjectInf, Ids[Idy].ToString());
                                                            CurObj = theObjectInf[Idy + 1];
                                                        }
                                                        else
                                                            CurObj = theObjectInf[Idy + FLC + 1];

                                                        if (Idx == 2) sSkip = Level2Check(LevelIds[1], CurObj.PId);
                                                        if (sSkip == "N")
                                                        {
                                                            if ((Idx == 1) && ((Idy % LevelCount) == 0) && (Idy != 0))
                                                            {
                                                                AddHeight += MaxHeight;
                                                                MaxHeight = 0;
                                                            }

                                                            if (Idx == 1)
                                                            {
                                                                CurObj = theObjectInf[Idy + 1];
                                                                Obj = GetIdInfo(theObjectInf, CurObj.PId);             // To get the Row in which this object is displayed

                                                                Row = Obj.Row + Obj.Height + AddHeight;
                                                                Col = ((Idy % LevelCount) * iWidth) + 5;
                                                                if (MaxHeight <= CurObj.Height) MaxHeight = CurObj.Height;
                                                            }
                                                            else
                                                            {
                                                                CurObj = theObjectInf[Idy + FLC + 1];
                                                                Obj = GetIdInfo(theObjectInf, CurObj.PId);             // To get the Row in which this object is displayed

                                                                Row = Obj.Row;
                                                                for (int Idz = 0; Idz <= Ids.Length - 1; Idz++)
                                                                {
                                                                    PrevObj = theObjectInf[Idz + FLC + 1];
                                                                    if (PrevObj.PId == Obj.Id)
                                                                    {
                                                                        Row += PrevObj.Height;
                                                                        Col = Obj.Col + 15;
                                                                        if (PrevObj.Id == Ids[Idy].ToString()) break;
                                                                    }
                                                                }
                                                            }
                                                            CurObj.Row = Row;
                                                            CurObj.Col = Col;
                                                        }
                                                    }
                                                }
                                            }

                                            if (ShowType == "PDF")
                                            {
                                                PageSizeNos.Clear();

                                                CurPage = 0; MaxPage = 1; strOutput = "PDF";
                                                PDFPage = CurPage; sID = "";
                                                PageSizeNos.Add(0);
                                                MyPage[CurPage] = new ceTe.DynamicPDF.Page(1600F, 1050F, 15.3F);

                                                // Places the Level Information in the PDF
                                                LevelUpto = "1";

                                                int LevelShowUpto = Convert.ToInt32(LevelUpto);
                                                if ((LevelIds.Length >= 3) && (LevelShowUpto >= 2)) Level_Information(theObjectInf);         // Level without child Information 
                                                if ((LevelIds.Length >= 2) && (LevelShowUpto >= 1)) Level_1_Information(theObjectInf);       // Level 1 Information 
                                                if ((LevelIds.Length >= 2) && (LevelShowUpto >= 1)) Level_1_Line_Information(theObjectInf);  // Level 1 Line Information 
                                                if ((LevelIds.Length >= 1) && (LevelShowUpto >= 0)) Level_0_Information(theObjectInf, "I", sTPage, sCPage);  // Level 0 Information

                                                for (int Idx = 0; Idx <= MaxPage - 1; Idx++)
                                                {
                                                    MyAllPage[iTotalPage] = MyPage[Idx];
                                                    pageLevel.Add((iLevel).ToString());
                                                    iTotalPage++;
                                                }
                                            }
                                            else if (ShowType == "PPT")
                                            {
                                                // Creates the Document
                                                CurPage = 0; MaxPage = 1; LevelUpto = "1"; strOutput = "PPT";
                                                PageSizeNos.Clear();
                                                PageSizeNos.Add(0);
                                                ImageOut[CurPage] = new Bitmap(1600, Output_Height + 30);

                                                // Set DPI of image (xDpi, yDpi)
                                                ImageOut[CurPage].SetResolution(256.0F, 256.0F);

                                                GraphicImg[CurPage] = Graphics.FromImage(ImageOut[CurPage]);
                                                GraphicImg[CurPage].InterpolationMode = InterpolationMode.HighQualityBicubic;
                                                GraphicImg[CurPage].Clear(System.Drawing.Color.White);

                                                // Places the Level Information in the PDF
                                                int LevelShowUpto = Convert.ToInt32(LevelUpto);
                                                if ((LevelIds.Length >= 3) && (LevelShowUpto >= 2)) Level_Information(theObjectInf);         // Level without child Information 
                                                if ((LevelIds.Length >= 2) && (LevelShowUpto >= 1)) Level_1_Information(theObjectInf);       // Level 1 Information 
                                                if ((LevelIds.Length >= 2) && (LevelShowUpto >= 1)) Level_1_Line_Information(theObjectInf);  // Level 1 Line Information 
                                                if ((LevelIds.Length >= 1) && (LevelShowUpto >= 0)) Level_0_Information(theObjectInf, "I", sTPage, sCPage);       // Level 0 Information


                                                //Add MyImages to MyDocument
                                                for (int Idx = 0; Idx <= MaxPage - 1; Idx++)
                                                {
                                                    System.Drawing.Font drawFontText = new System.Drawing.Font("Arail", 10, FontStyle.Bold, GraphicsUnit.Pixel);
                                                    SolidBrush drawBrushText = new SolidBrush(System.Drawing.Color.Black);
                                                    StringFormat stringFormat = new StringFormat();
                                                    stringFormat.Alignment = StringAlignment.Near;

                                                    System.Drawing.Rectangle rectLabel = new System.Drawing.Rectangle(5, 975, 200, 20);
                                                    GraphicImg[Idx].DrawString("Level No : N - " + iLevel.ToString(), drawFontText, drawBrushText, rectLabel, stringFormat);
                                                    rectLabel = new System.Drawing.Rectangle(1400, 975, 200, 20);
                                                    GraphicImg[Idx].DrawString("Page No : " + (iTotalPage + 1).ToString(), drawFontText, drawBrushText, rectLabel, stringFormat);
                                                    string sText = "This chart indicates operational and functional coordination relationships supporting the global business. The actual company relationships of the individuals on this chart are to executives within the companies that employ them. For business use only. (source: HR Core) Last updated on " + LastRefresh + " at 1am CET";
                                                    rectLabel = new System.Drawing.Rectangle(5, 995, 1550, 20);
                                                    GraphicImg[Idx].DrawString(sText, drawFontText, drawBrushText, rectLabel, stringFormat);

                                                    //ImagePIC[iTotalPage] = ImageOut[Idx];
                                                    //GraphicPIC[iTotalPage] = GraphicImg[Idx];

                                                    string imgFileName100 = HttpContext.Current.Server.MapPath("PPTX/") + "OrgChart_100_" + iTotalPage.ToString() + ".jpg";
                                                    ImageOut[Idx].SetResolution(256.0F, 256.0F);

                                                    // Create an Encoder object based on the GUID
                                                    // for the Quality parameter category.
                                                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                                                    // Create an EncoderParameters object.
                                                    // An EncoderParameters object has an array of EncoderParameter
                                                    // objects. In this case, there is only one
                                                    // EncoderParameter object in the array.
                                                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                                                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                                                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                                                    myEncoderParameters.Param[0] = myEncoderParameter;
                                                    ImageOut[Idx].Save(imgFileName100, jgpEncoder, myEncoderParameters);
                                                    ImageOut[Idx].Dispose();

                                                    iTotalPage++;
                                                    pageLevel.Add((iLevel).ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                sID += "," + lstID[Idc].ToString();
                                string errMsg = ex.Message;
                            }
                        }

                        try
                        {
                            if (ShowType == "PDF")
                            {
                                for (int Idx = 0; Idx <= iTotalPage - 1; Idx++)
                                {
                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label("Level No : N - " + pageLevel[Idx].ToString(), 5, 975, 200, 20, ceTe.DynamicPDF.Font.HelveticaBold, 15, ceTe.DynamicPDF.TextAlign.Left);
                                    MyAllPage[Idx].Elements.Add(MyLabel);

                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label("Page No : " + (Idx + 1).ToString(), 1400, 975, 200, 20, ceTe.DynamicPDF.Font.HelveticaBold, 15, ceTe.DynamicPDF.TextAlign.Left);
                                    MyAllPage[Idx].Elements.Add(MyLabel);

                                    string LevelDate = HttpContext.Current.Session["LEVEL_DATE"].ToString();
                                    string keyDate = LevelDate.Substring(6, 4) + "-" + LevelDate.Substring(3, 2) + "-" + LevelDate.Substring(0, 2);
                                    string sText = "This chart indicates operational and functional coordination relationships supporting the global business. The actual company relationships of the individuals on this chart are to executives within the companies that employ them. For business use only. (source: HR Core) Last updated on " + LastRefresh + " at 1am CET";
                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label(sText, 5, 995, 1550, 20, ceTe.DynamicPDF.Font.HelveticaBold, 10, ceTe.DynamicPDF.TextAlign.Left);
                                    MyAllPage[Idx].Elements.Add(MyLabel);

                                    MyDocument.Pages.Add(MyAllPage[Idx]);
                                }

                                if (thePageObjectInf.Count >= 1)
                                {
                                    foreach (PageObjectInf PObj in thePageObjectInf)
                                    {
                                        try
                                        {
                                            string baseUrl = HttpContext.Current.Request.Url.Scheme + "://" +
                                                                HttpContext.Current.Request.Url.Authority +
                                                                HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
                                            baseUrl = "http://phchbs-s5487.eu.novartis.net:8011";
                                            fixtureDate = DateTime.Now;
                                            KeyDate = fixtureDate.Year.ToString() + fixtureDate.Month.ToString("d2") + fixtureDate.Day.ToString("d2");
                                            if (PObj.PageNo != 0)
                                            {
                                                XYDestination dest = new XYDestination(PObj.PageNo, 1, 1);
                                                MyLink = new ceTe.DynamicPDF.PageElements.Link(PObj.Col + 40, PObj.Row, 200, 20, dest);
                                                if (PObj.PageNo <= iTotalPage) MyAllPage[PObj.CurPageNo].Elements.Add(MyLink);

                                                MyLink = new ceTe.DynamicPDF.PageElements.Link(PObj.Col - 90, PObj.Row - 60, 200, 20, dest);
                                                if (PObj.PageNo <= iTotalPage) MyAllPage[PObj.CurPageNo].Elements.Add(MyLink);
                                            }
                                            MyLabel = new ceTe.DynamicPDF.PageElements.Label("( # " + PObj.PageNo.ToString() + " )", PObj.Col + 40, PObj.Row, 200, 20, ceTe.DynamicPDF.Font.HelveticaBold, 12, ceTe.DynamicPDF.TextAlign.Left);
                                            MyAllPage[PObj.CurPageNo].Elements.Add(MyLabel);
                                        }
                                        catch (Exception ex)
                                        {
                                            string errMsg = ex.ToString();
                                            Console.WriteLine("  Message: {0}", errMsg);
                                        }
                                    }
                                }

                                ////Outputs the MyDocument to the current web MyPage
                                //MyDocument.DrawToWeb("OrgChart.pdf");

                                //Outputs the MyDocument to the current web MyPage
                                string sFP = HttpContext.Current.Server.MapPath("PDF/" + lstID[0].ToString() + "_" + KeyDate + ".pdf");
                                MyDocument.Draw(sFP);
                                if (File.Exists(curFile))
                                {
                                    FileInfo myDoc = new FileInfo(curFile);

                                    HttpContext.Current.Response.Clear();
                                    HttpContext.Current.Response.ContentType = "application/pdf";
                                    HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=" + myDoc.Name);
                                    HttpContext.Current.Response.AddHeader("Content-Length", myDoc.Length.ToString());
                                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                                    HttpContext.Current.Response.WriteFile(myDoc.FullName);
                                    HttpContext.Current.Response.End();

                                    return "";
                                }


                            }
                            else if (ShowType == "PPT")
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            string errMsg = ex.ToString();
                            Console.WriteLine("  Message: {0}", errMsg);
                        }


                        return pptFilePath;
                    }
                    else return "Ignore";
                }
                else return "Nodata";
            }

            // Create the all level PDF
            public string TempCreateAllLevelPDF(string ShowType)
            {
                string sID = "";

                // Gets all information
                Common csobj = new Common();
                dtFieldInformation = csobj.SQLReturnDataTable("SELECT * FROM LEVEL_CONFIG_INFO WHERE VIEW_ID='" + HttpContext.Current.Session["VIEW"].ToString() + "'");

                // Gets the Height for the image
                csobj = new Common();
                DataTable dtlevel = csobj.SQLReturnDataTable("SELECT * FROM ORG_CONFIG_INFO WHERE VIEW_ID='" + HttpContext.Current.Session["VIEW"].ToString() + "'");
                foreach (DataRow drlvl in dtlevel.Rows)
                {
                    //if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = drlvl["FIELD_VALUE"].ToString();
                    if (drlvl["FIELD_NAME"].ToString() == "LEVEL") LevelUpto = LevelFlag;
                    if (drlvl["FIELD_NAME"].ToString() == "HEIGHT") Height = drlvl["FIELD_VALUE"].ToString();
                    if (drlvl["FIELD_NAME"].ToString() == "LINECOLOR")
                    {
                        LineColor = drlvl["FIELD_VALUE"].ToString();
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "LINEWIDTH")
                    {
                        LineWidth = drlvl["FIELD_VALUE"].ToString();
                        blackPen = new Pen(GetDrawingLineColor(LineColor), Convert.ToInt16(LineWidth));
                    }
                    if (drlvl["FIELD_NAME"].ToString() == "TEMPLATE")
                    {
                        TemplateURL = drlvl["FIELD_VALUE"].ToString();
                        string[] TempURL = TemplateURL.Split('~');
                        if (TempURL[0] == "IMG")
                        {
                            BmpURL = new Bitmap(System.Web.HttpContext.Current.Server.MapPath(TempURL[1]));
                            BmpURL.SetResolution(1200f, 1200f);
                        }
                        BmpUpArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/uparrow.png"));
                        BmpDnArrow = new Bitmap(System.Web.HttpContext.Current.Server.MapPath("images/downarrow.ico"));
                    }
                }


                if (ShowType == "PDF")
                {
                    if (WebConfigurationManager.AppSettings["AllPDF"].ToString().ToUpper() == "CREATE")
                    {
                        sID = "";
                        lstID.Add(HttpContext.Current.Session["ID"].ToString());
                        for (int Idy = 0; Idy <= lstID.Count - 1; Idy++)
                        {
                            CurPage = 0; MaxPage = 1;
                            PDFPage = CurPage;

                            MyDocument = new ceTe.DynamicPDF.Document();
                            MyDocument.Creator = "DynamicChart.aspx";
                            MyDocument.Author = "Subramanian";
                            MyDocument.Title = "All Organization Chart";
                            PageSizeNos.Add(0);

                            if (LevelCount == 4)
                                MyPage[CurPage] = new ceTe.DynamicPDF.Page(1080F, 1050F, 15.3F);
                            else
                                MyPage[CurPage] = new ceTe.DynamicPDF.Page(1600F, 1050F, 15.3F);
                            MaxPage = 1;


                            try
                            {
                                ConvertToPDF("Y", lstID[Idy].ToString());

                                for (int Idx = 0; Idx <= MaxPage - 1; Idx++)
                                {
                                    MyLabel = new ceTe.DynamicPDF.PageElements.Label("Page No.: " + (Idx + 1).ToString(), 5, 885, 200, 8, ceTe.DynamicPDF.Font.Helvetica, 8, ceTe.DynamicPDF.TextAlign.Left);
                                    MyPage[Idx].Elements.Add(MyLabel);

                                    MyDocument.Pages.Add(MyPage[Idx]);
                                }

                                //Outputs the MyDocument to the current web MyPage
                                string sFP = HttpContext.Current.Server.MapPath("PDF/" + lstID[Idy].ToString() + ".pdf");
                                MyDocument.Draw(sFP);

                            }
                            catch (Exception ex)
                            {
                                sID += "," + lstID[Idy].ToString();
                            }

                            for (int Idz = PageSizeNos.Count - 1; Idz >= 0; Idz--)
                                PageSizeNos.Remove(Idz);

                        }
                    }

                    for (int Idz = lstID.Count - 1; Idz >= 0; Idz--)
                        lstID.RemoveAt(Idz);

                    // Gets all information
                    csobj = new Common();
                    DataTable dtLevelInfo = csobj.SQLReturnDataTable("SELECT * FROM DIRECT_REPORT");

                    MyDocument = new ceTe.DynamicPDF.Document();
                    MyDocument.Creator = "DynamicChart.aspx";
                    MyDocument.Author = "Subramanian";
                    MyDocument.Title = "All Organization Chart";

                    int iLevel = 0;
                    CurPage = 0; sID = "";
                    lstID.Add(HttpContext.Current.Session["ID"].ToString());
                    lstLevel.Add(iLevel.ToString());
                    for (int Idy = 0; Idy <= lstID.Count - 1; Idy++)
                    {
                        DataRow[] Row = dtLevelInfo.Select("POSITIONID='" + lstID[Idy].ToString() + "'");
                        if (Row.Length >= 1)
                        {
                            if (Row[0]["NextLevel"].ToString() != "")
                            {
                                string[] LevelInf = Row[0]["NextLevel"].ToString().Substring(1).Split(',');
                                if (LevelInf.Length >= 1) iLevel++;
                                for (int Ida = 0; Ida <= LevelInf.Length - 1; Ida++)
                                {
                                    lstID.Add(LevelInf[Ida].ToString());
                                    lstLevel.Add(iLevel.ToString());
                                }

                                try
                                {
                                    ceTe.DynamicPDF.Merger.PdfDocument pdfA = new ceTe.DynamicPDF.Merger.PdfDocument(HttpContext.Current.Server.MapPath("PDF/" + lstID[Idy].ToString() + ".pdf"));
                                    for (int Idx = 0; Idx <= pdfA.Pages.Count - 1; Idx++)
                                    {
                                        MyPage[CurPage] = new ceTe.DynamicPDF.Merger.ImportedPage(pdfA.Pages[Idx]);

                                        MyLabel = new ceTe.DynamicPDF.PageElements.Label("Level No.: " + lstLevel[Idy].ToString(), 5, 885, 200, 10, ceTe.DynamicPDF.Font.Helvetica, 8, ceTe.DynamicPDF.TextAlign.Left);
                                        MyPage[CurPage].Elements.Add(MyLabel);

                                        MyLabel = new ceTe.DynamicPDF.PageElements.Label("Page No.: " + (CurPage + 1).ToString(), 1400, 885, 200, 10, ceTe.DynamicPDF.Font.Helvetica, 8, ceTe.DynamicPDF.TextAlign.Left);
                                        MyPage[CurPage].Elements.Add(MyLabel);

                                        MyDocument.Pages.Add(MyPage[CurPage]);
                                        CurPage++;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    sID += "," + lstID[Idy].ToString();
                                }
                            }
                        }

                    }

                    //Outputs the MyDocument to the current web MyPage
                    MyDocument.DrawToWeb("OrgChartAllLevels.pdf");
                }
                else if (ShowType == "PPT")
                {
                }

                return "";
            }

            // Place the Level information
            private string GetLevelInformation(string ID)
            {
                string[] LevelInfo = HttpContext.Current.Session["LL"].ToString().Split(';');
                string ArrangeLI = "";

                for (int Idx = 0; Idx <= LevelInfo.Length - 1; Idx++)
                {
                    string[] LI = LevelInfo[Idx].Split(':');
                    ArrangeLI = ";" + LevelInfo[Idx];
                    if (LI[0] == ID)
                    {
                        HttpContext.Current.Session["LL"] = ArrangeLI.Substring(1);
                        return LevelInfo[Idx];
                    }
                }

                return "";
            }

            // Gets the Web font style to show the fonts
            private string GetWebFontStyle(string WebFontStyle)
            {
                if (WebFontStyle == "normal")
                    return "font-style:normal;";
                else if (WebFontStyle == "bold")
                    return "font-weight:bold;";
                else if (WebFontStyle == "itallic")
                    return "font-style:italic;";
                else if (WebFontStyle == "underline")
                    return "text-decoration:underline;";
                else if (WebFontStyle == "bold-ul")
                    return "text-decoration:underline;font-weight:bold;";

                return "";
            }

            // Place information in HTML
            private string PlaceInfoHTML(int Level, int Index, string Flag)
            {
                string HTML = "", FontName = "", FontSize = "", FontColor = "", FontStyle = "", FontFloat = "", FontWidth = "", Adjustment = "";
                int Idx = 0, ShowHeight = 0, MarginHeight = 0;
                string LGL_SHOW_FIELD = WebConfigurationManager.AppSettings["LGL_SHOW_FIELD"];
                try
                {
                    string[] TempURL = TemplateURL.Split('~'), BorderInfo;

                    ObjectInf Obj = null;
                    try
                    {
                        switch (Level)
                        {
                            case 0:
                                Obj = lstObjLevel0[Index];
                                break;
                            case 1:
                                Obj = lstObjLevel1[Index];
                                break;
                            case 2:
                                Obj = lstObjLevel[Index];
                                break;
                        }
                    }
                    catch (Exception ex1)
                    {
                        string errMsg = ex1.ToString();
                        Console.WriteLine("  Message: {0}", errMsg);
                    }
                    if (Obj != null)
                    {
                        string[] LabelInfo = Obj.Title.Split(';');
                        string TextAlign = "text-align:center;vertical-align:middle;display:table;";
                        if (LabelInfo.Length >= 1)
                        {
                            if ((Flag == "U") && ((Obj.PId != "-1") && (Obj.PId != "10000000")))
                            {
                                HTML += "<div style='width:99.5%;height:16px;text-align:center;'><img src='images/uparrow.png' style='width:42px;height:16px;'/></div>";
                            }

                            string ShowFullBox = Obj.ShowFullBox, sDotted = "";
                            BorderInfo = TempURL[1].Split('|');
                            ShowHeight = Original_Height;
                            if (TempURL[0] == "BRD")
                            {
                                if (ShowFullBox == "N")
                                {
                                    MarginHeight = 0;
                                    ShowHeight = Convert.ToInt32(BorderInfo[2]);
                                    if (Obj.Level == "2")
                                        MarginHeight = Original_Height - ShowHeight;
                                }

                                string BackColor = "white";
                                if (Obj.BackColor != "0") BackColor = "yellow";
                                if (Obj.ColorFlag != "0")
                                {
                                    HTML += "<div class='divclass'  data-sortno='" + Obj.SortNo + "'  data-owidth='' id='" + Obj.Id + Obj.PId + LevelDate.Replace("-", "") + "' data-gddbid='" + Obj.GDDBID + "' data-id='" + Obj.Id + "' style='" + TextAlign + "height:" + ShowHeight.ToString() + "px;border:" + BorderInfo[4] + "px solid " + Obj.ColorFlag + ";background-color:" + BackColor + ";margin-top:" + MarginHeight + "px'>";
                                    HTML += DrawPhoto(Obj, 0, 0);
                                }
                                else if (Obj.GrayColourFlag == "Y")
                                {
                                    if (Obj.DottedLineFlag == "Y") sDotted = "dotted"; else sDotted = "solid";
                                    if (Obj.BackColor == "0") BackColor = "lightgray";
                                    HTML += "<div class='divclass' data-sortno='" + Obj.SortNo + "'  data-owidth='' id='" + Obj.Id + Obj.PId + LevelDate.Replace("-", "") + "' data-gddbid='" + Obj.GDDBID + "' data-id='" + Obj.Id + "' style='" + TextAlign + "height:" + ShowHeight.ToString() + "px;border:" + BorderInfo[4] + "px " + sDotted + " " + BorderInfo[0] + ";background-color:" + BackColor + ";margin-top:" + MarginHeight + "px'>";
                                    HTML += DrawPhoto(Obj, 0, 0);
                                }
                                else if (Obj.DottedLineFlag == "Y")
                                {
                                    HTML += "<div class='divclass' data-sortno='" + Obj.SortNo + "'  data-owidth='' id='" + Obj.Id + Obj.PId + LevelDate.Replace("-", "") + "' data-gddbid='" + Obj.GDDBID + "' data-id='" + Obj.Id + "' style='" + TextAlign + "height:" + ShowHeight.ToString() + "px;border:" + BorderInfo[4] + "px dotted " + BorderInfo[0] + ";background-color:" + BackColor + ";margin-top:" + MarginHeight + "px'>";
                                    HTML += DrawPhoto(Obj, 0, 0);
                                }
                                else
                                {
                                    HTML += "<div class='divclass'  data-sortno='" + Obj.SortNo + "'  data-owidth='' id='" + Obj.Id + Obj.PId + LevelDate.Replace("-", "") + "' data-gddbid='" + Obj.GDDBID + "' data-id='" + Obj.Id + "' style='" + TextAlign + "height:" + ShowHeight.ToString() + "px;border:" + BorderInfo[4] + "px solid " + BorderInfo[0] + ";background-color:" + BackColor + ";margin-top:" + MarginHeight + "px'>";
                                    HTML += DrawPhoto(Obj, 0, 0);
                                }
                                if (ShowFullBox != "N")
                                {
                                    if (BorderInfo[6] == "Y")
                                        HTML += "<div style='height:1px;border-top:" + BorderInfo[5] + "px solid " + BorderInfo[3] + ";top:" + BorderInfo[2] + "px;left:" + BorderInfo[1] + "px;width:100%;background-color:white;'></div>";
                                }

                            }
                            else HTML += "<div class='divclass' data-sortno='" + Obj.SortNo + "'  data-owidth=''  id='" + Obj.Id + Obj.PId + LevelDate.Replace("-", "") + "' data-gddbid='" + Obj.GDDBID + "' data-id='" + Obj.Id + "' style='" + TextAlign + "height:" + Original_Height.ToString() + "px;background:url(" + TempURL[1] + ") no-repeat!important;background-size: 100% 100%!important;;margin-top:" + MarginHeight + "px'>";

                            Idx = 0;
                            FieldCount = dtFieldInformation.Rows.Count;
                            for (int Idy = 0; Idy <= FieldCount - 1; Idy++)
                            {
                                try
                                {
                                    FontName = dtFieldInformation.Rows[Idx]["FONT_NAME"].ToString();
                                    FontSize = dtFieldInformation.Rows[Idx]["FONT_SIZE"].ToString();
                                    FontColor = dtFieldInformation.Rows[Idx]["FONT_COLOR"].ToString();
                                    FontStyle = dtFieldInformation.Rows[Idx]["FONT_STYLE"].ToString();
                                    FontFloat = dtFieldInformation.Rows[Idx]["FONT_FLOAT"].ToString();
                                    FontWidth = dtFieldInformation.Rows[Idx]["FIELD_WIDTH"].ToString();
                                    Adjustment = dtFieldInformation.Rows[Idx]["ADJUSTMENT"].ToString();

                                    string[] LabelText = LabelInfo[Idx].Split('|');
                                    if (dtFieldInformation.Rows[Idx]["ACTIVE_IND"].ToString() == "Y")
                                    {
                                        if (ShowFullBox != "N")
                                        {

                                            if (dtFieldInformation.Rows[Idx]["FIELD_NAME"].ToString() == "NOR_COUNT")
                                            {
                                                if (LabelText[0].ToString() != "0")
                                                {
                                                    HTML += "<label data-title='" + LabelText[0] + "' data-width='" + FontWidth + "' style='width:" + FontWidth + "px;font-family:" + FontName + ";font-size:" + FontSize + "px;text-align:" + FontFloat + ";" + GetWebFontStyle(FontStyle) + "color:" + FontColor + ";line-height:14px;cursor:pointer;position:absolute;top:" + LabelText[1] + "px;left:" + (Convert.ToInt32(LabelText[2]) - 50).ToString() + "px;display:block;'>" + "SoC" + "</label>";
                                                    HTML += "<label data-title='" + LabelText[0] + "' data-width='" + FontWidth + "' style='width:" + FontWidth + "px;font-family:" + FontName + ";font-size:" + FontSize + "px;text-align:" + FontFloat + ";" + GetWebFontStyle(FontStyle) + "color:" + FontColor + ";line-height:14px;cursor:pointer;position:absolute;top:" + LabelText[1] + "px;left:" + LabelText[2] + "px;display:block;'>" + Convert.ToInt32(LabelText[0]).ToString("#,##0") + "</label>";
                                                }
                                            }
                                            else if (dtFieldInformation.Rows[Idx]["FIELD_NAME"].ToString() == "SOC_COUNT")
                                            {
                                                if (LabelText[0].ToString() != "0")
                                                {
                                                    HTML += "<label data-title='" + LabelText[0] + "' data-width='" + FontWidth + "' style='width:" + FontWidth + "px;font-family:" + FontName + ";font-size:" + FontSize + "px;text-align:" + FontFloat + ";" + GetWebFontStyle(FontStyle) + "color:" + FontColor + ";line-height:14px;cursor:pointer;position:absolute;top:" + LabelText[1] + "px;left:" + (Convert.ToInt32(LabelText[2]) - 50).ToString() + "px;display:block;'>" + "NoR" + "</label>";
                                                    HTML += "<label data-title='" + LabelText[0] + "' data-width='" + FontWidth + "' style='width:" + FontWidth + "px;font-family:" + FontName + ";font-size:" + FontSize + "px;text-align:" + FontFloat + ";" + GetWebFontStyle(FontStyle) + "color:" + FontColor + ";line-height:14px;cursor:pointer;position:absolute;top:" + LabelText[1] + "px;left:" + LabelText[2] + "px;display:block;'>" + Convert.ToInt32(LabelText[0]).ToString("#,##0") + "</label>";
                                                }
                                            }
                                            else
                                            {
                                                HTML += "<label data-title='" + LabelText[0] + "' data-width='" + FontWidth + "' style='width:" + FontWidth + "px;font-family:" + FontName + ";font-size:" + FontSize + "px;text-align:" + FontFloat + ";" + GetWebFontStyle(FontStyle) + "color:" + FontColor + ";line-height:14px;cursor:pointer;position:absolute;top:" + LabelText[1] + "px;left:" + LabelText[2] + "px;display:block;'>" + LabelText[0] + "</label>";
                                            }
                                        }
                                        else
                                        {
                                            if (dtFieldInformation.Rows[Idx]["FIELD_NAME"].ToString().ToUpper() == LGL_SHOW_FIELD.ToUpper())
                                                HTML += "<div data-title='" + LabelText[0] + "' style='font-weight:bold;font-family:" + FontName + ";font-size:14px;" + GetWebFontStyle(FontStyle) + "color:" + FontColor + ";line-height:14px;display:table-cell;vertical-align:middle;'>" + LabelText[0] + "</div>";
                                        }
                                    }
                                }
                                catch (Exception ex3)
                                {
                                    string errMsg = ex3.ToString();
                                    Console.WriteLine("  Message: {0}", errMsg);
                                }
                                Idx++;
                            }
                            HTML += "</div>";
                            if (Flag == "Y") HTML += "<img src='images/downarrow.ico' style='width:42px;height:16px;border:0px;'/>";
                        }
                    }
                }
                catch (Exception ex2)
                {
                    string errMsg = ex2.ToString();
                    Console.WriteLine("  Message: {0}", errMsg);

                }

                return HTML;
            }

            // Level 2 information for Level 1 using ID
            private string Level2HTML(List<ObjectInf> theObjectInf, string ID)
            {
                string retLevel2 = "";
                lstObjLevel = (from obj in lstObjLevel2
                               where obj.PId == ID
                               select obj).ToList();
                try
                {
                    string[] Level2 = IdInPId(theObjectInf, ID).Split(',');
                    if (Level2.Length >= 1)
                    {
                        for (int Idx = 0; Idx <= Level2.Length - 1; Idx++)
                        {
                            string NextLevel = "NO", PrevLevel = "";

                            try
                            {
                                NextLevel = CheckLevelInfo(lstObjLevel[Idx]);
                                if (NextLevel != "NO") PrevLevel = lstObjLevel[Idx].Flag; else PrevLevel = "";
                            }
                            catch (Exception ex3)
                            {
                                string errMsg = ex3.ToString();
                                Console.WriteLine("  Message: {0}", errMsg);
                            }
                            string sHeight = Original_Height.ToString();
                            if (NextLevel != "NO") sHeight = (Original_Height + 20).ToString();

                            string HTML = PlaceInfoHTML(2, Idx, NextLevel == "NO" ? "N" : "Y");
                            if (HTML.Length >= 1)
                            {
                                retLevel2 += "<li class='section' style='margin-top:-20px'>" +
                                                "<a href='javascript:void(0);' onclick='ShowLevel(&#39;" + NextLevel + "&#39;,&#39;" + PrevLevel + "&#39;)' style='text-align:center;'>" + HTML + "</a>" +
                                             "</li>";
                            }
                            else
                            {
                                retLevel2 += "<li class='section' style='visibility:hidden;margin-top:-20px'>" +
                                                "<a href='javascript:void(0);' onclick='ShowLevel(&#39;" + NextLevel + "&#39;,&#39;" + PrevLevel + "&#39;)' style='text-align:center;'>" + HTML + "</a>" +
                                             "</li>";
                            }
                        }
                    }
                }
                catch (Exception ex1)
                {
                    retLevel2 = "";
                    string errMsg = ex1.Message;

                    Console.WriteLine("  Message: {0}", errMsg);
                }

                return retLevel2;
            }

            // Convert to HTML
            public string ConvertToHTML()
            {
                string retHTML = "", retLevel1 = "", retLevel2 = "";

                // Check for level, which is clicked
                string LLI = GetLevelInformation(Level);
                if (LLI.Length >= 1)
                {
                    string[] LL = LLI.Split(':');
                    Level = LL[0];
                    PreviousLevel = LL[1];
                }
                else HttpContext.Current.Session["LL"] += ";" + Level + ":" + PreviousLevel;

                // Create the list of objects
                var theObjectInf = GetLevelInfo();
                string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(theObjectInf);
                if (HttpContext.Current.Session["VIEW_TYPE"].ToString() == "OV")
                    HttpContext.Current.Session["theOVObjectInf"] = json;
                else
                    HttpContext.Current.Session["theLVObjectInf"] = json;

                // Gets all Level information
                LevelInf = GetAllLevels(theObjectInf).Split(',');

                LevelIds = new string[LevelInf.Length];
                if (LevelInf.Length >= 1)
                {
                    // Get Level based Ids
                    for (int Idx = 0; Idx <= LevelInf.Length - 1; Idx++)
                    {
                        LevelIds[Convert.ToInt16(LevelInf[Idx])] = LevelInId(theObjectInf, LevelInf[Idx], Idx);
                    }

                    // Places the first level information
                    if (LevelIds.Length >= 2)
                    {
                        string[] Level1 = LevelIds[1].Split(',');
                        int MaxLevel = 0; retLevel1 = "";
                        if (Level1.Length >= 1)
                        {
                            retLevel1 += "<ul class='departments'>";
                            for (int Idx = 0; Idx <= Level1.Length - 1; Idx++)
                            {
                                retLevel2 = "";
                                if ((Idx % LevelCount) == 0)
                                {
                                    MaxLevel = 0;
                                    for (int Idy = Idx; Idy <= Level1.Length - 1; Idy++)
                                    {
                                        if (Idy <= (Idx + (LevelCount - 1)))
                                        {
                                            string[] LvlInf = IdInPId(theObjectInf, Level1[Idy]).Split(',');
                                            if ((LvlInf.Length >= MaxLevel) && (LvlInf[0].Length != 0)) MaxLevel = LvlInf.Length;
                                        }
                                    }
                                    if (MaxLevel >= 1)
                                    {
                                        MaxLevel = (Original_Height + 22) * MaxLevel;
                                        MaxLevel += Original_Height_10;
                                    }
                                    else MaxLevel = Original_Height_10;
                                }

                                if (IdInPId(theObjectInf, Level1[Idx]).Length != 0) retLevel2 = Level2HTML(theObjectInf, Level1[Idx]);
                                if (retLevel2.Length >= 1)
                                {
                                    string NextLevel = CheckLevelInfo(lstObjLevel1[Idx]);
                                    string PrevLevel = "";
                                    if (NextLevel != "NO") PrevLevel = lstObjLevel1[Idx].Flag;
                                    string sHeight = Original_Height.ToString();
                                    if (NextLevel != "NO") sHeight = (Original_Height + 20).ToString();

                                    retLevel1 += "<li class='department' style='height:" + (MaxLevel + 20).ToString() + "px' custheight='" + (MaxLevel + 20).ToString() + "px' custnl='Y'>" +
                                                    "<a class='lelveL1' href='javascript:void(0);' onclick='ShowLevel(&#39;" + NextLevel + "&#39;,&#39;" + PrevLevel + "&#39;)' style='text-align:center;'>" + PlaceInfoHTML(1, Idx, "Y") + "</a>" +
                                                    "<ul class='sections'>" + retLevel2 + "</ul>" +
                                                    "</li>";
                                }
                                else
                                {
                                    string NextLevel = CheckLevelInfo(lstObjLevel1[Idx]);
                                    string PrevLevel = "";
                                    if (NextLevel != "NO") PrevLevel = lstObjLevel1[Idx].Flag;
                                    string sHeight = Original_Height.ToString();
                                    if (NextLevel != "NO") sHeight = (Original_Height + 20).ToString();

                                    retLevel1 += "<li class='department' style='height:" + (MaxLevel + 20).ToString() + "px' custheight='" + (MaxLevel + 20).ToString() + "px' custnl='N'>" +
                                                    "<a class='lelveL1' href='javascript:void(0);' onclick='ShowLevel(&#39;" + NextLevel + "&#39;,&#39;" + PrevLevel + "&#39;)' style='text-align:center;'>" + PlaceInfoHTML(1, Idx, NextLevel == "NO" ? "N" : "Y") + "</a>" +
                                                    "</li>";
                                }
                            }
                            retLevel1 += "</ul>";
                        }
                    }
                }
                if ((PreviousLevel == "10000000") || (Level == "20002609")) PreviousLevel = "NOLEVEL";

                string ContryName = "", sPrevLevel = "";
                if (HttpContext.Current.Session["VIEW_TYPE"].ToString() == "LV")
                    ContryName += "<div id='divCountryName' style='color:#993300; text-align:center; " +
                                  " width:100%;padding:10px 0 10px 0;height:60px;'" +
                                  " onclick='ShowMenu(12);' " +
                                  ">" + Country + "</div>";

                retHTML += "<div style = \"width:100%;clear:both;position:absolute;text-align:left;left:20px;top:0px;\"><div style=\"margin-top:5px;margin-left:0px;margin-right:4%;\"><a target=\"_blank\" style=\"margin-top:-50px;\" href=\"https://pesap53.eu.novartis.net:51601/irj/portal\"><img id=\"imgHR\" src=\"images/HRCore_Logo.png\" /></a></div></div>";
                retHTML += "<li class='header'>" + ContryName +
                        "<a href='javascript:void(0);' onclick='ShowLevel(&#39;" + PreviousLevel + "&#39;,&#39;" + sPrevLevel + "&#39;)'>" + PlaceInfoHTML(0, 0, "U") + "</a>" +
                        "</li>" + retLevel1;

                return retHTML;
            }
        }


    }
}