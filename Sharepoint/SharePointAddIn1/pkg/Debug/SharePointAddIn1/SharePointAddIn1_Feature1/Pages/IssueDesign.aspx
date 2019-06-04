<%@ Page language="C#" MasterPageFile="../_catalogs/masterpage/MyMasterPage.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../Content/CustomScript/custom.css" rel="stylesheet" />

    <script type="text/javascript" src="../Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/init.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>
    
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>


    <script type="text/javascript">
        // Set the style of the client web part page to be consistent with the host web.
        (function () {
            'use strict';
            ExecuteOrDelayUntilScriptLoaded(retrieveAllListsAllFields, "sp.js");
        })();

        function retrieveAllListsAllFields()  
        {  
            var siteUrl="https://joetellez.sharepoint.com/sites/iterondev/";  
            var clientContext = new SP.ClientContext(siteUrl);  
            var oWebsite = clientContext.get_web();  
            var collList = oWebsite.get_lists();  
  
            this.listInfoArray = clientContext.loadQuery(collList,   
            'Include(Title,Fields.Include(Title,InternalName))');  
  
            clientContext.executeQueryAsync(  
                Function.createDelegate(this, this.onQueryListsAllFieldsSucceeded),   
                Function.createDelegate(this, this._onQueryListsAllFieldsFailed)  
            );  
        }  
  
        function onQueryListsAllFieldsSucceeded()   
        {  
            var listInfo = '';  
            for (var i = 0; i < this.listInfoArray.length; i++)   
            {  
                var oList = this.listInfoArray[i];  
                var collField = oList.get_fields();  
                var fieldEnumerator = collField.getEnumerator();  
  
                while (fieldEnumerator.moveNext())   
                {  
                    var oField = fieldEnumerator.get_current();  
                    var regEx = new RegExp('name', 'ig');  
  
                    if (regEx.test(oField.get_internalName()))   
                    {  
                        listInfo += '\nList: ' + oList.get_title() +   
                        '\n\tField Title: ' + oField.get_title() +   
                        '\n\tField Name: ' + oField.get_internalName();  
                    }  
                }  
            }  
            alert(listInfo);  
        }  
  
        function onQueryListsAllFieldsFailed(sender, args)   
        {  
            alert('Request failed. ' + args.get_message() +   
            '\n' + args.get_stackTrace());  
        }  

        var web;
        var collList;
        function getHostSiteName() {

            var siteUrl="https://joetellez.sharepoint.com/sites/iterondev/"; 
            var hostUrl = decodeURIComponent(getURLParameters(siteUrl)); 
            var currentcontext = new SP.ClientContext.get_current(); 
            var hostcontext = new SP.AppContextSite(currentcontext, hostUrl); 
            web = hostcontext.get_web(); 
            collList = web.get_lists(); 
            alert(colList);
            currentcontext.load(collList);
 
            currentcontext.executeQueryAsync(onSuccess, onFailure);
        }
 
        function onSuccess() {
            var listInfo = '';
            var listEnumerator = collList.getEnumerator();
 
            while (listEnumerator.moveNext()) {
                 var oList = listEnumerator.get_current();
                listInfo += 'Title: ' + oList.get_title() + '<br />';
            }
 
            $("#divIssueInf").html(listInfo);
        }
         
        function onFailure(sender, args) {
            alert('Failed to get web title. Error:' + args.get_message());
        }
 
        function getURLParameters(param) {
            var params = document.URL.split('?')[1].split('&');
            var strParams = '';
 
            for (var i = 0; i < params.length; i = i + 1) {
                var singleParam = params[i].split('=');
                if (singleParam[0] == param) {
                    return singleParam[1];
                }
            }
        }


    </script>


</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" />
    <div id="divIssueInf"></div>
    <div class="row">
        <div class="col-md-3" style="float:right;margin-top:20px;">
            <button type="button" class="btn btn-primary">Save</button>
        </div>
    </div>
</asp:Content>
