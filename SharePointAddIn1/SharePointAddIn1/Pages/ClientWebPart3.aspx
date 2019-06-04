<%@ Page language="C#" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<WebPartPages:AllowFraming ID="AllowFraming" runat="server" />

<html>
<head>
    <title></title>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link href="../Content/CustomScript/custom.css" rel="stylesheet" />

    <script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style>
        /* Style buttons */
        .btn1 {
          background-color: DodgerBlue; /* Blue background */
          border: none; /* Remove borders */
          color: white; /* White text */
          padding: 12px 16px; /* Some padding */
          font-size: 16px; /* Set a font size */
          cursor: pointer; /* Mouse pointer on hover */
        }

        /* Darker background on mouse-over */
        .btn1:hover {
          background-color: RoyalBlue;
        }
    </style>


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
    </script>
</head>
<body>
    JOE Testing(Buttons and Images)

    <div style="width:100%;height:200px;display:inline-block;margin-bottom:50px;">
        <img src="../Images/SiteImages/img_snow_wide.jpg" class="img-responsive" style="width:100%;height:200px;" alt="Cinque Terre">
    </div>

    <div style="width:100%;height:60px;display:inline-block;margin-bottom:50px;">
        <button type="button" class="btn">Basic</button>
        <button type="button" class="btn btn-default">Default</button>
        <button type="button" class="btn btn-primary">Primary</button>
        <button type="button" class="btn btn-success">Success</button>
        <button type="button" class="btn btn-info">Info</button>
        <button type="button" class="btn btn-warning">Warning</button>
        <button type="button" class="btn btn-danger">Danger</button>
        <button type="button" class="btn btn-link">Link</button>    
    </div>

    <div style="width:100%;height:200px;display:inline-block;margin-bottom:50px;">
        <!-- Add font awesome icons to buttons  -->
        <p>Icon buttons:</p>
        <button class="btn1"><i class="fa fa-home"></i></button>
        <button class="btn1"><i class="fa fa-bars"></i></button>
        <button class="btn1"><i class="fa fa-trash"></i></button>
        <button class="btn1"><i class="fa fa-close"></i></button>
        <button class="btn1"><i class="fa fa-folder"></i></button>

        <p>Icon buttons with text:</p>
        <button class="btn1"><i class="fa fa-home"></i> Home</button>
        <button class="btn1"><i class="fa fa-bars"></i> Menu</button>
        <button class="btn1"><i class="fa fa-trash"></i> Trash</button>
        <button class="btn1"><i class="fa fa-close"></i> Close</button>
        <button class="btn1"><i class="fa fa-folder"></i> Folder</button>    
    </div>
</body>
</html>
