<%@ Page language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<WebPartPages:AllowFraming ID="AllowFraming" runat="server" />

<html>
<head>
    <title></title>

    <style>
        .col-md-3 {
	       padding:0px;
	    }
        .col-md-9 {
	       padding:0px;
	    }	
        .col-md-6 {
	       padding:0px;
	    }	
	    .ms-core-overlay {
	       background-color:#ffff;
	    }
	    .showbox {
	       float:left;
	       color:black;
	       width:98%;
	       border-radius: 5px;
           border: 2px solid #73AD21;
	       background: #ffffff;
           background: rgba(43, 61, 81, 0.78);
           padding: 20px;	
           margin:5px;	   
	       text-align:center;	   
	    }
	    .text:hover {
		    text-decoration:solid;
	    }
    </style>

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

        var context = SP.ClientContext.get_current();
        var collListItem = '', QV='', ListTableHead = "";
        var ListFieldInfo = [], ListUsedFields = [], ListTableData = [], ListTableColumn = [];

        $(document).ready(function () {
            QV = GetQueryStringList("Entity");
            RetriveFieldsFromAList(QV);
        });

        function GetQueryStringList(key) {  
            return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));  
        }

        function GetViewFieldDisplayName(FieldName) {
            if (ListFieldInfo.length >= 1) {
                for (var Idx = 0; Idx <= ListFieldInfo.length - 1; Idx++) {
                    if (ListFieldInfo[Idx].FieldName == FieldName) return ListFieldInfo[Idx].DisplayName;
                }
            }

            return "No Field";
        }

        function RetriveFieldsFromAList(ListName) {

            // Open current SPWeb: _spPageContextInfo.webServerRelativeUrl
            // ctx = new SP.ClientContext(_spPageContextInfo.webServerRelativeUrl);
            ctx = context;

            // Get Documents library
            var list = ctx.get_web().get_lists().getByTitle(ListName);

            //get_fields() returns SP.FieldCollection object --- contains all SP.Field object properties > https://msdn.microsoft.com/en-us/library/office/jj246815.aspx
            var fieldCollection = list.get_fields();
            ctx.load(fieldCollection,'Include(InternalName,StaticName,Title)');

            var view = list.get_views().getByTitle('All Items');
            //get_viewFields() returns SP.ViewFieldCollection object --- only field names (Internal Names), but not a SP.Field object > https://msdn.microsoft.com/en-us/library/office/jj244841.aspx
            var viewFieldCollection = view.get_viewFields();
            ctx.load(viewFieldCollection);

            ctx.executeQueryAsync(function () {

		        var cont=0;
		        var fields = 'SP.FieldCollection from list.get_fields()'
		        fields    += 'Internal Name - Static Name - Title \n';
		        fields    += '--------------------------- \n';
		        var listEnumerator = fieldCollection.getEnumerator();
		        while (listEnumerator.moveNext()) {
                    fields += listEnumerator.get_current().get_internalName() + ' - ' + listEnumerator.get_current().get_staticName() + ' - ' + listEnumerator.get_current().get_title() + ";\n ";  //
                    ListFieldInfo.push({
                        FieldName: listEnumerator.get_current().get_internalName(),
                        StaticName: listEnumerator.get_current().get_staticName(),
                        DisplayName: listEnumerator.get_current().get_title()
                    });

			        cont++;
		        }
		        console.log(fields + '-------------------------- \n Number of Fields: ' + cont);
		
                var cont = 0;
		        var viewfields = '\nSP.ViewFieldCollection from view.get_viewFields() \n';
		        viewfields    += 'Internal Name \n';
                viewfields += '--------------------------- \n';

                var HTML = "<div class=\"row\">";
                ListTableHead = "<thead><tr style=\"background-color:lightgray;\">";
                var listEnumerator = viewFieldCollection.getEnumerator();
		        while (listEnumerator.moveNext()) {
			        viewfields += listEnumerator.get_current() + "\n"; 
                    cont++;

                    HTML += "<div class=\"col-md-3\">" +
                            "   <div style=\"width:100%\">" + listEnumerator.get_current() + "</div>" +
                            "   <div style=\"width:100%\"><input class=\"form-control\" type=\"text\" id=\"txt" + listEnumerator.get_current() + "\" /></div>" +
                            "</div>";

                    var DisplayName = GetViewFieldDisplayName(listEnumerator.get_current());
                    ListUsedFields.push({
                        FieldName: listEnumerator.get_current(),
                        DisplayName: DisplayName
                    });
                    
                    ListTableHead += "<th style=\"text-align:left;border-top:1px solid gray;border-right:1px solid gray\">"+DisplayName+"</th>";
                    ListTableColumn.push({
                                            "data": listEnumerator.get_current(), "render": function (data, type, full, meta) {
                                                return data;
                                            }
                                        });


			         ////If we need more data about view fields we can call again to server using
			         //var field = fieldCollection.getByInternalNameOrTitle(listEnumerator.get_current());
			         //ctx.load(field);
			         //ctx.executeQueryAsync(.....)
			         ////Or, If we would like to save a server query, we could iterate again 
			         ////the fieldCollection.getEnumerator(); checking the Internal Name of the field
                }
                HTML += "<div class=\"col-md-3\" style=\"padding-top:20px;\">" +
                        "     <button id=\"AddFieldToListButton\" type=\"button\" class=\"btn btn-info\">Add Item</button>" +
                        "     <button id=\"btnContextInfo\" type=\"button\" class=\"btn btn-info\" onclick=\"getAllLists();\">Info</button>" +
                    "</div></div>";
                alert(HTML);
                $("#divGetInputElement").html(HTML);
                console.log(viewfields + '-------------------------- \n Number of Fields: ' + cont);

                LoadLists(ListName);

                $("#AddFieldToListButton").click(function () {
                    var oList = context.get_web().get_lists().getByTitle(ListName);
                    var itemCreateInfo = new SP.ListItemCreationInformation();
                    var oListItem = oList.addItem(itemCreateInfo);

                    if (ListUsedFields.length >= 1) {
                        for (var Idx = 0; Idx <= ListUsedFields.length - 1; Idx++) {
                            oListItem.set_item(ListUsedFields[Idx].FieldName, $("#txt"+ListUsedFields[Idx].FieldName).val());
                        }
                    }
                    oListItem.update();
                    context.load(oListItem);
                    context.executeQueryAsync(ListAddingSuccess, ListAddingFail);
                });
	        }, 
	        function(sender, args){
		        console.log('Request collListItem failed. ' + args.get_message() + '\n' + args.get_stackTrace());
	        });
        }

        function ListAddingSuccess() {
           LoadLists(QV);
        }

        function ListAddingFail(sender, args) {
           alert("Failed adding note" + args.get_message());
        }

        function LoadLists(CURDlist) {
           var oList = context.get_web().get_lists().getByTitle(CURDlist);
           var camlQuery = new SP.CamlQuery();
           camlQuery.set_viewXml('<View><RowLimit>100</RowLimit></View>');
           collListItem = oList.getItems(camlQuery);
           context.load(collListItem);
           context.executeQueryAsync(OnListLoadSucceeded, OnListLoadFailed);
        }

        function OnListLoadSucceeded() {

            var listItemInfo = '';
            var listItemEnumerator = collListItem.getEnumerator();
            while (listItemEnumerator.moveNext()) {
                var oListItem = listItemEnumerator.get_current();

                if (ListUsedFields.length >= 1) {
                    listItemInfo = '';
                    for (var Idx = 0; Idx <= ListUsedFields.length - 1; Idx++) {
                        listItemInfo += ",\"" + ListUsedFields[Idx].FieldName + "\":\"" + oListItem.get_item(ListUsedFields[Idx].FieldName) + "\"";
                    }
                    alert(listItemInfo.substring(1));
                    ListTableData.push(JSON.parse("{ "+listItemInfo.substring(1)+" }"));
                }
            }

            ShowDataTable(ListTableData, ListTableColumn, ListTableHead);
        }

        function OnListLoadFailed(sender, args) {
          alert("Failed loading notes" + args.get_message());
        }

        function ShowEmptyTable() {
            return "<b style='color:red'>There is no data</b>";
        }

        function ShowDataTable(ObjList, ObjColumn, TableHead) {
            $(".overlay").show();

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

            $(".overlay").hide();
        }

        function getAllLists() {
            window.open("https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/Lists/UpcomingEvent/AllItems.aspx");
        }
 
    </script>
</head>
<body>
    <div class="container">
        <div id="divGetInputElement"></div>
        <br />
        <div class="row" style="margin-bottom:10px;">
            <div class="col-md-12">
                <table id="tblShowTableList"></table>
                <div id="divShowTableList"></div>
            </div>
        </div>
    </div>
</body>
</html>
