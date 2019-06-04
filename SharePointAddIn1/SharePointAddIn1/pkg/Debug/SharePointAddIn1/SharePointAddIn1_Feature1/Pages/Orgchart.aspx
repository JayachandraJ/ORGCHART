<%@ Page language="C#" MasterPageFile="../_catalogs/masterpage/MyMasterPage.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.6.1/css/all.css" integrity="sha384-gfdkjb5BdAXd+lj+gudLWI+BXq4IuLW5IT+brZEZsLFm++aCMlF1V92rMkPaX4PP" crossorigin="anonymous" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../assets/js/vendor/go.js"></script>

    <script type="text/javascript" src="../Scripts/OrgchartScript/Orgchart.js?cd=820"></script>
    <script type="text/javascript" src="../Scripts/OrgchartScript/OrgchartGOJS.js?cd=820"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            // Org chart in Canvas
            h = parseInt($("#divRightOrgChart").css("height").substr(0, $("#divRightOrgChart").css("height").indexOf("px")));
            w = parseInt($("#divRightOrgChart").css("width").substr(0, $("#divRightOrgChart").css("width").indexOf("px")));

            var key = "StartNode";
            var SN= decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
            init(w, h, SN);
        });

    </script>
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" />

    <div id="divRightOrgChart">
      <div id="myDiagramDiv" style="height: 500px"></div>
      <div>
        <div id="myInspector">
        </div>
      </div>
      <div style="display:none;">
        <div>
          <button id="SaveButton" onclick="save()">Save</button>
          <button onclick="load()">Load</button>
          Diagram Model saved in JSON format:
        </div>
        <textarea id="mySavedModel" style="width:100%;height:250px">{ "class": "go.TreeModel",
          "nodeDataArray": [
                {"key":1, "name":"Stella Payne Diaz", "title":"CEO"},
                {"key":2, "name":"Luke Warm", "title":"VP Marketing/Sales", "parent":1},
                {"key":3, "name":"Meg Meehan Hoffa", "title":"Sales", "parent":2},
                {"key":4, "name":"Peggy Flaming", "title":"VP Engineering", "parent":1},
                {"key":5, "name":"Saul Wellingood", "title":"Manufacturing", "parent":4},
                {"key":6, "name":"Al Ligori", "title":"Marketing", "parent":2},
                {"key":7, "name":"Dot Stubadd", "title":"Sales Rep", "parent":3},
                {"key":8, "name":"Les Ismore", "title":"Project Mgr", "parent":5},
                {"key":9, "name":"April Lynn Parris", "title":"Events Mgr", "parent":6},
                {"key":10, "name":"Xavier Breath", "title":"Engineering", "parent":4},
                {"key":11, "name":"Anita Hammer", "title":"Process", "parent":5},
                {"key":12, "name":"Billy Aiken", "title":"Software", "parent":10},
                {"key":13, "name":"Stan Wellback", "title":"Testing", "parent":10},
                {"key":14, "name":"Marge Innovera", "title":"Hardware", "parent":10},
                {"key":15, "name":"Evan Elpus", "title":"Quality", "parent":5},
                {"key":16, "name":"Lotta B. Essen", "title":"Sales Rep", "parent":3}
         ]
        }
        </textarea>
      </div>
    </div>
</asp:Content>
