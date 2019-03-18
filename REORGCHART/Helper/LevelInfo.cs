using System;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

using REORGCHART.Data;
using REORGCHART.Models;

namespace REORGCHART.Helper
{
    // Org chart creation 
    public class LevelInfo
    {
        Models.DBContext db = new Models.DBContext();
        ApplicationUser UserData = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        public DataTable GetLevelInfo(string UserId, string CompanyName, string UserType,
                                      string ShowLevel, string Levels, string Version, string OperType)
        {
            DataTable retDT = null;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.CommandText = "PROC_GET_POSITION_TREE_OPERATIONALCHART";

                cmd.Parameters.Add("@STARTPOSITION", SqlDbType.VarChar, 15).Value = ShowLevel;
                cmd.Parameters.Add("@DEPTH", SqlDbType.VarChar, 15).Value = Levels;
                cmd.Parameters.Add("@VERSION", SqlDbType.VarChar, 150).Value = Version;
                cmd.Parameters.Add("@USERTYPE", SqlDbType.VarChar, 50).Value = UserType;
                cmd.Parameters.Add("@COMPANYNAME", SqlDbType.VarChar, 150).Value = CompanyName;
                cmd.Parameters.Add("@USERID", SqlDbType.VarChar, 150).Value = UserId;
                cmd.Parameters.Add("@OPER", SqlDbType.VarChar, 150).Value = OperType;

                Common csobj = new Common();
                retDT = csobj.SPReturnDataTable(cmd);
            }

            return retDT;
        }

        public DataTable GetVersion(string OperType, string CompanyName, string Version, string ShowLevel)
        {
            DataTable retDT = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.CommandText = "PROC_GET_VERSION_TREE_OPERATIONALCHART";

            cmd.Parameters.Add("@VERSION", SqlDbType.VarChar, 150).Value = Version;
            cmd.Parameters.Add("@COMPANYNAME", SqlDbType.VarChar, 150).Value = CompanyName;
            cmd.Parameters.Add("@OPER", SqlDbType.VarChar, 150).Value = OperType;
            cmd.Parameters.Add("@SHOWLEVEL", SqlDbType.VarChar, 150).Value = ShowLevel;

            Common csobj = new Common();
            retDT = csobj.SPReturnDataTable(cmd);

            // Remove predefined column
            if (retDT.Columns.Count >= 1)
            {
                try
                {
                    retDT.Columns.Remove("USER_ID");
                    retDT.Columns.Remove("LEVEL_ID");
                    retDT.Columns.Remove("PARENT_LEVEL_ID");
                    retDT.Columns.Remove("COUNTRY");
                    retDT.Columns.Remove("VERSION");
                    retDT.Columns.Remove("DATE_UPDATED");
                    retDT.Columns.Remove("FULL_NAME");
                    retDT.Columns.Remove("VERIFY_FLAG");
                    retDT.Columns.Remove("LEVEL_NO");
                    retDT.Columns.Remove("BREAD_GRAM");
                    retDT.Columns.Remove("BREAD_GRAM_NAME");
                    retDT.Columns.Remove("NOR_COUNT");
                    retDT.Columns.Remove("SOC_COUNT");
                    retDT.Columns.Remove("POSITION_CALCULATED_COST");
                    retDT.Columns.Remove("NEXT_LEVEL_FLAG");
                    retDT.Columns.Remove("GRAY_COLORED_FLAG");
                    retDT.Columns.Remove("DOTTED_LINE_FLAG");
                    retDT.Columns.Remove("SHOW_FULL_BOX");
                    retDT.Columns.Remove("LANGUAGE_SELECTED");
                    retDT.Columns.Remove("SORTNO");
                    retDT.Columns.Remove("POSITIONFLAG");
                    retDT.Columns.Remove("FLAG");
                }
                catch (Exception ex)
                {
                }
            }

            return retDT;
        }

        public MyLastAction GetUserCurrentAction(string RoleMethod)
        {
            var UCA = (from uca in db.UserLastActions
                       where uca.UserId == UserData.UserName
                       select uca).FirstOrDefault();

            MyLastAction viewModel = new MyLastAction();
            if (UCA == null)
            {
                UserLastActions uca = new UserLastActions();
                uca.Company = UserData.CompanyName;
                uca.KeyDate = "2018/07/11";
                uca.UserId = UserData.UserName;
                uca.Version = "1";
                uca.UsedView = "Normal";
                uca.Oper = "OV";
                uca.Levels = "One";
                uca.ShowLevel = "101212";
                uca.PartitionShowLevel = "101212";
                uca.Country = "CH";
                uca.Role = RoleMethod;
                uca.SelectedInitiative = "SelectInitiative";
                uca.SelectedPopulation = "SelectPopulation";
                uca.SelectedUser = UserData.UserName;
                uca.SelectedVersion = "SelectVersion";
                uca.SelectedShape = "RoundedRectangle";
                uca.SelectedSkin = "white";
                uca.SelectedShowPicture = "Yes";
                uca.SelectedSplitScreen = "Yes";
                uca.SelectedSplitScreenDirection = "Vertical";
                uca.SelectedTextColor = "black";
                uca.SelectedBorderColor = "cyan";
                uca.SelectedBorderWidth = "3";
                uca.SelectedLineColor = "#634329";
                uca.OrgChartType = "OD";
                uca.CopyPaste = "";
                db.UserLastActions.Add(uca);

                db.SaveChanges();

                viewModel.ShowLevel = "101212";
                viewModel.PartitionShowLevel = "101212";
                viewModel.KeyDate = "2018/07/11";
                viewModel.Levels = "One";
                viewModel.Version = "1";
                viewModel.Oper = "OV";
                viewModel.View = "Normal";
                viewModel.Country = "CH";
                viewModel.SelectedInitiative = "SelectInitiative";
                viewModel.SelectedPopulation = "SelectPopulation";
                viewModel.SelectedUser = UserData.UserName;
                viewModel.SelectedVersion = "SelectVersion";
                viewModel.SelectedShape = "RoundedRectangle";
                viewModel.SelectedSkin = "white";
                viewModel.SelectedShowPicture = "Yes";
                viewModel.SelectedSplitScreen = "Yes";
                viewModel.SelectedSplitScreenDirection = "Vertical";
                viewModel.SelectedTextColor = "black";
                viewModel.SelectedBorderColor = "cyan";
                viewModel.SelectedBorderWidth = "3";
                viewModel.SelectedLineColor = "#634329";
                viewModel.OrgChartType = "OD";
                viewModel.CopyPaste = "";
                viewModel.Role = RoleMethod;
            }
            else
            {
                viewModel.KeyDate = UCA.KeyDate;
                viewModel.SelectedInitiative = UCA.SelectedInitiative;
                viewModel.SelectedPopulation = UCA.SelectedPopulation;
                viewModel.SelectedUser = UCA.SelectedUser;
                viewModel.SelectedVersion = UCA.SelectedVersion;
                viewModel.SelectedShape = UCA.SelectedShape;
                viewModel.SelectedSkin = UCA.SelectedSkin;
                viewModel.SelectedShowPicture = UCA.SelectedShowPicture;
                viewModel.SelectedSplitScreen = UCA.SelectedSplitScreen;
                viewModel.SelectedSplitScreenDirection = UCA.SelectedSplitScreenDirection;
                viewModel.SelectedTextColor = UCA.SelectedTextColor;
                viewModel.SelectedBorderColor = UCA.SelectedBorderColor;
                viewModel.SelectedBorderWidth = UCA.SelectedBorderWidth;
                viewModel.SelectedLineColor = UCA.SelectedLineColor;
                viewModel.OrgChartType = UCA.OrgChartType;
                viewModel.ShowLevel = UCA.ShowLevel;
                viewModel.PartitionShowLevel = UCA.PartitionShowLevel;
                viewModel.Levels = UCA.Levels;
                viewModel.Version = UCA.Version;
                viewModel.Oper = UCA.Oper;
                viewModel.View = UCA.UsedView;
                viewModel.Country = UCA.Country;
                viewModel.Role = UCA.Role;
                if (RoleMethod != "")
                {
                    UCA.Role = RoleMethod;
                    viewModel.Role = RoleMethod;
                    db.SaveChanges();
                }
            }

            return viewModel;
        }

        public string GetHRCoreVersion(string Country, string Oper, int VersionNo)
        {
            VersionDetails VD = (from vd in db.VersionDetails
                                 where vd.CompanyName == UserData.CompanyName &&
                                     vd.OperType == Oper &&
                                     vd.UserRole == "Finalyzer" &&
                                     vd.ActiveVersion == "Y"
                                 select vd).OrderByDescending(x => x.VersionNo).FirstOrDefault();

            if (VD != null) return VD.Version;

            return "NOT AN HR CORE VERSION";
        }

        public string GetUserRoles()
        {
            UserRoles UR = (from ur in db.UserRoles where ur.UserId == UserData.UserName select ur).FirstOrDefault();
            if (UR != null) return UR.Role;
            return "User";
        }

        public DataTable GetOrgChartDataTable(string UserType, string Country, string ShowLevel, string Levels, string Oper, string Version)
        {
            try
            {
                DataTable orgChartData = null;

                if (ShowLevel == null) ShowLevel = "";
                if (Levels == null || Levels == "" || Levels == " ") Levels = "1";

                // Operation & Legal chart details              
                orgChartData = GetLevelInfo(UserData.UserName, UserData.CompanyName, UserType, ShowLevel, Levels, Version, Oper);

                return orgChartData;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> Info = new Dictionary<string, string>();
                Info.Add("WebHTML", "Error");
                Info.Add("Message", ex.Message);
                Info.Add("StackTrace", ex.StackTrace);

                return null;
            }
        }

        public string GetOrgChartData(string UserType, string Country, string ShowLevel, string Levels, string Oper, string Version)
        {
            try
            {
                DataTable orgChartData = null;

                if (ShowLevel == null) ShowLevel = "";
                if (Levels == null || Levels == "" || Levels == " ") Levels = "1";

                // Operation & Legal chart details              
                orgChartData = GetLevelInfo(UserData.UserName, UserData.CompanyName, UserType, ShowLevel, Levels, Version, Oper);

                return JsonConvert.SerializeObject(orgChartData);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> Info = new Dictionary<string, string>();
                Info.Add("WebHTML", "Error");
                Info.Add("Message", ex.Message);
                Info.Add("StackTrace", ex.StackTrace);

                return JsonConvert.SerializeObject(Info);
            }
        }

        public string GetOrgChartHrCoreData(string UserType, string Country, string ShowLevel, string Levels, string Oper, string Version)
        {
            try
            {
                DataTable orgChartData = null;

                if (ShowLevel == null) ShowLevel = "";
                if (Levels == null || Levels == "" || Levels == " ") Levels = "1";

                VersionDetails VD = (from vd in db.VersionDetails
                                     where vd.CompanyName == UserData.CompanyName &&
                                         vd.Country == Country &&
                                         vd.OperType == Oper &&
                                         vd.UserRole == "Finalyzer" &&
                                         vd.ActiveVersion == "Y"
                                     select vd).FirstOrDefault();
                if (VD != null)
                {
                    // Operation chart details              
                    orgChartData = GetLevelInfo(UserData.UserName, UserData.CompanyName, "Finalyzer", VD.ShowLevel, "All", VD.VersionNo.ToString(), Oper);
                }

                return JsonConvert.SerializeObject(orgChartData);
            }
            catch (Exception ex)
            {
                Dictionary<string, string> Info = new Dictionary<string, string>();
                Info.Add("WebHTML", "Error");
                Info.Add("Message", ex.Message);
                Info.Add("StackTrace", ex.StackTrace);

                return JsonConvert.SerializeObject(Info);
            }
        }

        public string GetMenuItems(string UserRole)
        {
            DataSet retDS = null;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.CommandText = "PROC_GET_USER_MENU_LIST";

                cmd.Parameters.Add("@USER_ID", SqlDbType.VarChar, 500).Value = UserData.UserName;
                cmd.Parameters.Add("@COMPANY_NAME", SqlDbType.VarChar, 500).Value = UserData.CompanyName;

                Common csobj = new Common();
                retDS = csobj.SPReturnDataSet(cmd);
                retDS.Tables[0].TableName = "MT";
                retDS.Tables[1].TableName = "VT";
            }

            return JsonConvert.SerializeObject(retDS);
        }

        public string GetFinalyzerVerion(string Oper)
        {
            VersionDetails myIV = (from iv in db.VersionDetails
                                   where iv.CompanyName == UserData.CompanyName &&
                                         iv.UserRole == "Finalyzer" &&
                                         iv.ActiveVersion == "Y" &&
                                         iv.OperType == Oper
                                   select iv).FirstOrDefault();
            if (myIV != null)
            {
                if (myIV.Version!=null) return "Yes";
            }

            return "No";
        }
    }
}


