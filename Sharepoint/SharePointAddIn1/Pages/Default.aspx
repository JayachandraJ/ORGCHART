<%-- The following 4 lines are ASP.NET directives needed when using SharePoint components --%>

<%@ Page Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" MasterPageFile="../_catalogs/masterpage/MyMasterPage.master" Language="C#" %>

<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%--To show the List items --%>
<%--https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/Lists/DepartmentList/AllItems.aspx--%>

<%-- The markup and script in the following Content element will be placed in the <head> of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

    <style type="text/css">
        .webparttitle {
            width:100%;
            height:35px;
            background-color:antiquewhite;
        }

        i.fa {
            display: inline-block;
            border-radius: 60px;
            box-shadow: 0px 0px 2px #888;
            padding: 0.5em 0.6em;
        }

        .overlaycontainer {
          position: relative;
        }

        .overlayimage {
          opacity: 1;
          display: block;
          width: 100%;
          height: auto;
          transition: .5s ease;
          backface-visibility: hidden;
        }
        .overlaymiddle {

          transition: .5s ease;
          opacity: 0;
          position: absolute;
          top: 50%;
          left: 50%;
          transform: translate(-50%, -50%);
          -ms-transform: translate(-50%, -50%);
          text-align: center;
        }

        .overlaycontainer:hover .overlayimage {
          opacity: 0.3;
        }

        .overlaycontainer:hover .overlaymiddle {
          opacity: 1;
        }

        .overlaytext {
          background-color: #4CAF50;
          color: white;
          font-size: 16px;
          padding: 16px 32px;
        }

    </style>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.6.1/css/all.css" integrity="sha384-gfdkjb5BdAXd+lj+gudLWI+BXq4IuLW5IT+brZEZsLFm++aCMlF1V92rMkPaX4PP" crossorigin="anonymous" />
    <link rel="stylesheet" href="../Content/CustomScript/Custom.css" />

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <script type="text/javascript" src="../Scripts/CustomScript/Custom.js?cd=820"></script>
    <script type="text/javascript" src="../Scripts/OrgchartScript/Orgchart.js?cd=820"></script>
    <script type="text/javascript" src="../Scripts/CustomScript/LandingPage.js"></script>

    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/init.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>

    <meta name="WebPartPageExpansion" content="full" />

    <!-- Add your CSS styles to the following file -->
    <link rel="Stylesheet" type="text/css" href="../Content/App.css" />

    <!-- Add your JavaScript to the following file -->
    <script type="text/javascript" src="../Scripts/App.js"></script>

</asp:Content>

<%-- The markup and script in the following Content element will be placed in the <body> of the page --%>
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">

<div class="container">
        <div class="row" style="margin-bottom:10px;">
            <div class="col-md-4">
                 <div id="divUpComingEventTitle" class="webparttitle">
                    <div style="float:left;padding:5px;font-weight:bold;">Up Coming Events</div>
                    <div style="float:right;padding:5px;width:90px;">
                        <a title="List" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/pages/ShowListInfo.aspx?Entity=UpcomingEvent" style="cursor:pointer;padding-left:5px;"><i class="fa fa-list-alt"></i></a>
                        <a title="Edit" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/Lists/UpcomingEvent/AllItems.aspx" style="cursor:pointer;padding-left:5px;"><i class="fa fa-edit"></i></a>
                    </div>
                 </div>
                 <div id="divUpComingEvent"></div>
            </div>
            <div class="col-md-4">
                <div id="divDepartmentListTitle" class="webparttitle">
                    <div style="float:left;padding:5px;font-weight:bold;">Department List</div>
                    <div style="float:right;padding:5px;width:90px;">
                        <a title="List" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/pages/ShowListInfo.aspx?Entity=DepartmentList" style="cursor:pointer;padding-left:5px;"><i class="fa fa-list-alt"></i></a>
                        <a title="Edit" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/Lists/DepartmentList/AllItems.aspx" style="cursor:pointer;padding-left:5px;"><i class="fa fa-edit"></i></a>
                    </div>
                </div>
                <div id="divDepartmentList"></div>
            </div>
            <div class="col-md-4">
                <div id="divSiriusNewsTitle" class="webparttitle">
                    <div style="float:left;padding:5px;font-weight:bold;">Sirius New</div>
                    <div style="float:right;padding:5px;width:90px;">
                        <a title="List" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/pages/ShowListInfo.aspx?Entity=SiriusNews" style="cursor:pointer;padding-left:5px;"><i class="fa fa-list-alt"></i></a>
                        <a title="Edit" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/Lists/SiriusNews/AllItems.aspx" style="cursor:pointer;padding-left:5px;"><i class="fa fa-edit"></i></a>
                    </div>
                </div>
                <div id="divSiriusNews"></div>
            </div>
        </div>
        <div class="row" style="margin-bottom:10px;">
            <div class="col-md-8">
                <div id="myCarousel" class="carousel slide" data-ride="carousel">
                    <!-- Indicators -->
                    <ol class="carousel-indicators"  id="olCarouselIndicators">
                    </ol>

                    <!-- Wrapper for slides -->
                    <div class="carousel-inner" id="divCarouselShow">
                    </div>

                    <!-- Left and right controls -->
                    <a class="left carousel-control" href="#myCarousel" data-slide="prev">
                        <span class="glyphicon glyphicon-chevron-left"></span>
                        <span class="sr-only">Previous</span>
                    </a>
                    <a class="right carousel-control" href="#myCarousel" data-slide="next">
                        <span class="glyphicon glyphicon-chevron-right"></span>
                        <span class="sr-only">Next</span>
                    </a>
                </div>
            </div>
            <div class="col-md-4 overlaycontainer">
                <div class="overlayimage">
                    <div id="divWeatherNewsTitle" class="webparttitle">
                        <div style="float:left;padding:5px;font-weight:bold;">Weather New</div>
                        <div style="float:right;padding:5px;width:90px;">
                        </div>
                    </div>
                    <div id="divWeatherNews">
                        <center>
				            <div id="divImage" style="width:100%;height:100px;text-align:center;">
					            <img id="imgWeatherIcon" src="" alt="" style="width:100px;height:100px;" />
				            </div>
				            <div id="divTemperature" style="width:100%;height:20px;text-align:center;"></div>
                            <div id="divTemperatureLookAndFeel" style="width:100%;height:60px;text-align:center;">&nbsp;</div>
				            <div id="divDescription" style="width:100%;height:30px;text-align:center;"></div>
			            </center>
                    </div>
                    <div class="overlaymiddle">
                        <div class="overlaytext" onclick="ShowDetailedWeatherReport();">Detailed Report</div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" style="margin-bottom:10px;">
            <div class="col-md-4">
                <div id="divAnnoucementTitle" class="webparttitle">
                    <div style="float:left;padding:5px;font-weight:bold;">Announcement</div>
                    <div style="float:right;padding:5px;width:90px;">
                        <a title="List" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/pages/ShowListInfo.aspx?Entity=Announcement" style="cursor:pointer;padding-left:5px;"><i class="fa fa-list-alt"></i></a>
                        <a title="Edit" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/Lists/Announcement/AllItems.aspx" style="cursor:pointer;padding-left:5px;"><i class="fa fa-edit"></i></a>
                    </div>
                </div>
                <div id="divAnnouncement"></div>
            </div>
            <div class="col-md-4">
                <div id="divSurveyTitle" class="webparttitle">
                    <div style="float:left;padding:5px;font-weight:bold;">Survey</div>
                    <div style="float:right;padding:5px;width:90px;">
                        <a title="List" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/pages/ShowListInfo.aspx?Entity=Survey" style="cursor:pointer;padding-left:5px;"><i class="fa fa-list-alt"></i></a>
                        <a title="Edit" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/Lists/Survey/AllItems.aspx" style="cursor:pointer;padding-left:5px;"><i class="fa fa-edit"></i></a>
                    </div>
                </div>
                <div id="divSurvey"></div>
            </div>
            <div class="col-md-4">
                <div id="divHelpTitle" class="webparttitle">
                    <div style="float:left;padding:5px;font-weight:bold;">Help</div>
                    <div style="float:right;padding:5px;width:50px;">
                        <a title="Edit" target="_blank" href="https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/Lists/HelpContent/AllItems.aspx" style="cursor:pointer;padding-left:5px;"><i class="fa fa-edit"></i></a>
                    </div>
                </div>
                <div id="divHelpContent"></div>
            </div>
        </div>
        <div class="row" style="margin-bottom:10px;">
            <div class="col-md-4">
                <div class="fb-login-button"
                     data-max-rows="1"
                     data-size="medium"
                     data-button-type="continue_with"
                     data-width="100%"
                     data-scope="public_profile, email">
                </div>

                <div id="status"></div>
            </div>
        </div>
    </div>

    <div>
        <p id="message">
            <!-- The following content will be replaced with the user name when you run the app - see App.js -->
            initializing...
        </p>
    </div>

</asp:Content>

