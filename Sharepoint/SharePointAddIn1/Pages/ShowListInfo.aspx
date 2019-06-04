<%@ Page language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<WebPartPages:AllowFraming ID="AllowFraming" runat="server" />

<html>
<head>
    <title></title>

    <%--<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">--%>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.6.1/css/all.css" integrity="sha384-gfdkjb5BdAXd+lj+gudLWI+BXq4IuLW5IT+brZEZsLFm++aCMlF1V92rMkPaX4PP" crossorigin="anonymous">

    <script type="text/javascript" src="../Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/init.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>
    
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    
    <%--    
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/dt/dt-1.10.18/r-2.2.2/datatables.min.css"/>
    <script type="text/javascript" src="https://cdn.datatables.net/v/dt/dt-1.10.18/r-2.2.2/datatables.min.js"></script>
    --%>

    
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/responsive/2.2.3/css/responsive.dataTables.min.css" />
    <script src="https://code.jquery.com/jquery-3.3.1.js" type="text/javascript"></script>
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="https://cdn.datatables.net/responsive/2.2.3/js/dataTables.responsive.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        // Set the style of the client web part page to be consistent with the host web.
        (function () {
            'use strict';

            var hostUrl = '';
            var link = document.createElement('link');
            link.setAttribute('rel', 'stylesheet');
            if (document.URL.indexOf('?') != -1) {
                var params = document.URL.split('?')[1].split('&');
                for (var i = 0; i < params.length; i++) {
                    var p = decodeURIComponent(params[i]);
                    if (/^SPHostUrl=/i.test(p)) {
                        hostUrl = p.split('=')[1];
                        link.setAttribute('href', hostUrl + '/_layouts/15/defaultcss.ashx');
                        break;
                    }
                }
            }
            if (hostUrl == '') {
                link.setAttribute('href', '/_layouts/15/1033/styles/themable/corev15.css');
            }
            document.head.appendChild(link);
        })();

        $(document).ready(function () {
            window.reSize = window.reSize || {};

            //App responsive width and height
            reSize.AppPart = {
                senderId: '',      // the App Part provides a Sender Id in the URL parameters,
                // every time the App Part is loaded, a new Id is generated.
                // The Sender Id identifies the rendered App Part.
                previousHeight: 0, // the height
                minHeight: 0,      // the minimal allowed height
                firstResize: true, // On the first call of the resize the App Part might be
                // already too small for the content, so force to resize.

                init: function () {
                    // parse the URL parameters and get the Sender Id
                    var params = document.URL.split("?")[1].split("&");
                    for (var i = 0; i < params.length; i = i + 1) {
                        var param = params[i].split("=");
                        if (param[0].toLowerCase() == "senderid")
                            this.senderId = decodeURIComponent(param[1]);
                    }

                    // find the height of the app part, uses it as the minimal allowed height
                    this.previousHeight = this.minHeight = $('body').height();

                    // display the Sender Id
                    $('#senderId').text(this.senderId);

                    // make an initial resize (good if the content is already bigger than the
                    // App Part)
                    this.autoSize();
                },

                autoSize: function () {
                    // Post the request to resize the App Part, but just if has to make a resize
                    var step = 30, // the recommended increment step is of 30px. Source:// http://msdn.microsoft.com/en-us/library/jj220046.aspx
                    height = $('body').height() + 7,  // the App Part height // (now it's 7px more than the body)
                    newHeight,                        // the new App Part height
                    contentHeight = $('.container').height(), //Specify your name of parent div
                    resizeMessage = '<message senderId={Sender_ID}>resize({Width}, {Height})</message>';

                    // if the content height is smaller than the App Part's height,
                    // shrink the app part, but just until the minimal allowed height
                    if (contentHeight < height - step && contentHeight >= this.minHeight) {
                        height = contentHeight;
                    }

                    // if the content is bigger or smaller then the App Part
                    // (or is the first resize)
                    //alert(this.previousHeight.toString() + ":" + height.toString() + ":" +this.firstResize.toString());
                    if (this.previousHeight !== height || this.firstResize === true) {
                        // perform the resizing
                        newHeight = contentHeight;

                        // set the parameters
                        resizeMessage = resizeMessage.replace("{Sender_ID}", this.senderId);
                        resizeMessage = resizeMessage.replace("{Height}", newHeight);
                        resizeMessage = resizeMessage.replace("{Width}", "100%");
                        // we are not changing the width here, but we could

                        // post the message
                        window.parent.postMessage(resizeMessage, "*");

                        // memorize the height
                        this.previousHeight = newHeight;

                        // further resizes are not the first ones
                        this.firstResize = false;
                    }
                }
            };

            ExecuteOrDelayUntilScriptLoaded(loadData, "sp.js");
        });

        function GetQueryStringValue(key) {  
            return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));  
        }

        function ShowListInformation(Entity) {
            var QV = GetQueryStringValue("Entity");
            if (QV == Entity) {
                switch (QV) {
                    case "UpcomingEvent":
                        ShowDataTable(UpcomingEvent, UpcomingEventColumn, UpcomingEventHead, UpcomingEventLI);
                        break;
                    case "DepartmentList":
                        ShowDataTable(DepartmentList, DepartmentListColumn, DepartmentListHead, DepartmentListLI);
                        break;
                    case "SiriusNews":
                        ShowDataTable(SiriusNews, SiriusNewsColumn, SiriusNewsHead, SiriusNewsLI);
                        break;
                    case "Announcement":
                        ShowDataTable(Announcement, AnnouncementColumn, AnnouncementHead, AnnouncementLI);
                        break;
                }
            }
        }

        function ShowEmptyTable() {
            return "<b style='color:red'>There is no data</b>";
        }

        function ShowDataTable(ObjList, ObjColumn, TableHead, DataInfo) {
            $(".overlay").show();

            if (!($(".navbar-toggle").is(":visible"))) {
                $("#tblShowTableList").show();
                $("#divShowTableList").hide();

                $("#tblShowTableList").empty();

                if ($("#tblShowTableList").hasClass("dataTable") == true) {
                    var oTable = $('#tblShowTableList').dataTable();
                    oTable.fnDestroy();
                }

                $("#tblShowTableList").html(TableHead);
                $('#tblShowTableList').dataTable({
                    "responsive": true,
                    "sDom": 'T<"clear">lfrtip',
                    "aaData": ObjList,
                    "language": {
                        "emptyTable": ShowEmptyTable("1")
                    },
                    "aoColumnDefs": [{ "sWidth": "27%", "aTargets": [0] }],
                    "columns": ObjColumn
                });
            }
            else {
                $("#tblShowTableList").hide();
                $("#divShowTableList").show();
                $("#divShowTableList").html(DataInfo);
            }
            $(".overlay").hide();
        }

        function loadData() {
            loadListData("UpcomingEvent", UpcomingEventSuccess, globalError);
            loadListData("DepartmentList", DepartmentListSuccess, globalError);
            loadListData("SiriusNews", SiriusNewsSuccess, globalError);
            loadListData("Announcement", AnnouncementSuccess, globalError);
        }

        function loadListData(listName, onSuccess, onFail) {
          context = SP.ClientContext.get_current();
          var list = context.get_web().get_lists().getByTitle(listName);
          var camlQuery = SP.CamlQuery.createAllItemsQuery();
          var listItems = list.getItems(camlQuery);

          context.load(listItems);

          context.executeQueryAsync(
                function(sender, args) { 
                    // listItem is defined on same closure, you do not need to declare globally
                    onSuccess(listItems); 
                }, 
                onFail
          );
        }

        var UpcomingEvent = [], UpcomingEventColumn = [];
        var UpcomingEventHead = "", UpcomingEventLI = "";
        function UpcomingEventSuccess(data) {
            try {
                var listItemEnumerator = data.getEnumerator();
                UpcomingEventHead = ""; UpcomingEventLI = "";
                UpcomingEvent = []; UpcomingEventColumn = [];

                while (listItemEnumerator.moveNext()) {
                    var oListItem = listItemEnumerator.get_current();
                    UpcomingEventLI = UpcomingEventLI + '<div><div>' + oListItem.get_item('Title') + '</div><div>' + oListItem.get_item('Location') + '</div><div>' + oListItem.get_item('EventType') + '</div><div>' + oListItem.get_item('EventDate') + '</div></div>';
                    UpcomingEvent.push({
                        "Title": oListItem.get_item('Title'),
                        "Location": oListItem.get_item('Location'),
                        "EventType": oListItem.get_item('EventType'),
                        "EventDate": oListItem.get_item('EventDate')
                    });

                    if (UpcomingEventHead == "") {
                        UpcomingEventHead = "<thead><tr style=\"background-color:lightgray;\">" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Title</th>" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Location</th>" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Event Type</th>" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Event Date</th>" +
                            "  </tr></thead>";

                        UpcomingEventColumn.push(
                                            {
                                                "data": "Title", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            },
                                            {
                                                "data": "Location", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            },
                                            {
                                                "data": "EventType", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            },
                                            {
                                                "data": "EventDate", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            });
                    }
                }

                ShowListInformation("UpcomingEvent");
            }
            catch (ex) {
                $('#divShowTableList').html("Up coming Event: " + ex);
            }
        }

        var DepartmentList = [], DepartmentColumn = [];
        var DepartmentListHead = "", DepartmentListLI = "";
        function DepartmentListSuccess(data) {
            try {
                var listItemEnumerator = data.getEnumerator();
                DepartmentListHead = ""; DepartmentListLI = "";
                DepartmentList = []; DepartmentColumn = [];

                while (listItemEnumerator.moveNext()) {
                    var oListItem = listItemEnumerator.get_current();
                    DepartmentListLI = DepartmentListLI + '<div><div>' + oListItem.get_item('Title') + '</div><div>' + oListItem.get_item('DepartmentHead') + '</div></>';
                    DepartmentList.push({
                        "Title": oListItem.get_item('Title'),
                        "DepartmentHead": oListItem.get_item('DepartmentHead')
                    });

                    if (DepartmentListHead == "") {
                        DepartmentListHead = "<thead><tr style=\"background-color:lightgray;\">" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Title</th>" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Department Head</th>" +
                            "  </tr></thead>";

                        DepartmentListColumn.push(
                                                {
                                                    "data": "Title", "render": function (data, type, full, meta) {
                                                        return data;
                                                    }
                                                },
                                                {
                                                    "data": "DepartmentHead", "render": function (data, type, full, meta) {
                                                        return data;
                                                    }
                                                });
                    }
                }
                ShowListInformation("DepartmentList");
            }
            catch (ex) {
                $('#divShowTableList').html("Department List: " + ex);
            }
        }

        var SiriusNews = [], SiriusNewsColumn = [];
        var SiriusNewsHead = "", SiriusNewsLI="";
        function SiriusNewsSuccess(data) {
            try {
                var listItemEnumerator = data.getEnumerator();
                SiriusNewsHead = ""; SiriusNewsLI = "";
                SiriusNews = []; SiriusNewsColumn = [];

                while (listItemEnumerator.moveNext()) {
                    var oListItem = listItemEnumerator.get_current();
                    SiriusNewsLI = SiriusNewsLI + '<div><div>' + oListItem.get_item('Title') + '</div><div>' + oListItem.get_item('News') + '</div><div>' + oListItem.get_item('NewsDate') + '</div></div>';
                    SiriusNews.push({
                        "Title": oListItem.get_item('Title'),
                        "News": oListItem.get_item('News'),
                        "NewsDate": oListItem.get_item('NewsDate')
                    });
                    if (SiriusNewsHead == "") {
                        SiriusNewsHead = "<thead><tr style=\"background-color:lightgray;\">" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Title</th>" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">News</th>" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">News Date</th>" +
                            "  </tr></thead>";

                        SiriusNewsColumn.push(
                                            {
                                                "data": "Title", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            },
                                            {
                                                "data": "News", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            },
                                            {
                                                "data": "NewsDate", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            });

                    }
                }
                ShowListInformation("SiriusNews");
            }
            catch (ex) {
                $('#divShowTableList').html("Sirius New: " + ex);
            }
        }

        var Announcement = [], AnnouncementColumn = [];
        var AnnouncementHead = "", AnnouncementLI="";
        function AnnouncementSuccess(data) {
            try {
                var listItemEnumerator = data.getEnumerator();
                AnnouncementHead = ""; AnnouncementLI = "";
                Announcement = []; AnnouncementColumn = [];

                while (listItemEnumerator.moveNext()) {
                    var oListItem = listItemEnumerator.get_current();
                    AnnouncementLI = AnnouncementLI + '<div><div>' + oListItem.get_item('Title') + '</div><div>' + oListItem.get_item('Announcement') + '</div><div>' + oListItem.get_item('AnnouncementDate') + '</div></div>';
                    Announcement.push({
                        "Title": oListItem.get_item('Title'),
                        "News": oListItem.get_item('Announcement'),
                        "NewsDate": oListItem.get_item('AnnouncementDate')
                    });
                    if (AnnouncementHead == "") {
                        AnnouncementHead = "<thead><tr style=\"background-color:lightgray;\">" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Title</th>" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Announcement</th>" +
                            "     <th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">Announcement Date</th>" +
                            "  </tr></thead>";

                        AnnouncementColumn.push(
                                            {
                                                "data": "Title", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            },
                                            {
                                                "data": "Announcement", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            },
                                            {
                                                "data": "AnnouncementDate", "render": function (data, type, full, meta) {
                                                    return data;
                                                }
                                            });

                    }
                }
                ShowListInformation("Announcement");
                
                //App responsive on load 
                reSize.AppPart.init();

                setTimeout(function () {
                   reSize.AppPart.autoSize();
                }, 500);
            }
            catch (ex) {
                $('#divShowTableList').html("Announcement: " + ex);
            }
        }

        function globalError(sender, args) {
          alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
        }

    </script>
</head>
<body>
    <div class="container">
        <div class="row" style="margin-bottom:10px;">
            <div class="col-md-12">
                <table id="tblShowTableList"></table>
                <div id="divShowTableList"></div>
            </div>
        </div>
    </div>
</body>
</html>
