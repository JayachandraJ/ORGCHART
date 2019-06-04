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
	
	    .boxborder {
	       /*border-top:1px solid #ced3da;
	       border-left:1px solid #ced3da;
	       border-right:1px solid #ded3d3;
	       border-bottom:1px solid #ded3d3; */
        }
	    .text:hover {
		    text-decoration:solid;
	    }
    </style>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../Content/CustomScript/custom.css" rel="stylesheet" />

    <script type="text/javascript" src="../Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

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

        var slideIndex = 0;
        $(document).ready(function () {
            GetWeatherReport();
	        ShowSlides();
        });

        function GetWeatherReport() {

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var lat = position.coords.latitude;
                    var lon = position.coords.longitude;
                    var WeatherUrl = 'https://api.openweathermap.org/data/2.5/weather?appid=ac9d9fc53d7474c41df352931241a97e&lat=' + lat + '&lon=' + lon;

                    $.ajax({
                        url: WeatherUrl,
                        type: "GET",
                        headers: { "accept": "application/json; odata=verbose" },
                        success: function (results) {
                            if (!results) { // should handle null and empty strings
                                try {
                                }
                                catch (e) { // catch any JavaScript runtime exception (error)
                                    console.log(e); // print the error to the console 
                                    // (hit F12 in most browsers to see 
                                    // the console BEFORE you refresh 
                                    // the page to run your code)
                                }
                            }
                            else {
                                ShowWeatherInfo(results);
                            }
                        },
                        error: function (error) {
                            // this 'error' variable can be named
                            // anything you'd like and is a string 
                            // description of the AJAX error.  
                            // This description comes from $.ajax - 
                            // which is part of jQuery (a JS library).
                            // This "error" is not a native JS 
                            // exception; therefore, you wouldn't 
                            // use a TRY-CATCH.  Also, since it's 
                            // only a string, if you want to show it 
                            // as an error in the console, you should 
                            // use `console.err`, not `console.log`.
                            console.log("Error in getting List: (0)", error);
                        }
                    });
                });
            }
        }

        function ShowWeatherInfo(weatherInfo) {
	        var weather = weatherInfo.weather[0].main.toLowerCase(),
		        temperature = weatherInfo.main.temp;

	        if (weather == "clear sky" || weather == "clear") {
		        icon = "clear.svg";
		        description = "Yay, sunshine!";
	        } else if (weather == "few clouds") {
		        icon = "few-clouds.svg";
		        description = "It's a little cloudy.";
	        } else if (weather == "scattered clouds" || weather == "broken clouds" || weather == "clouds") {
		        icon = "clouds.svg";
		        description = "Looks like scattered clouds today.";
	        } else if (weather == "rain" || weather == "light rain" || weather == "shower rain") {
		        icon = "rain.svg";
		        description = "Looks like rain."
	        } else if (weather == "thunderstorm") {
		        icon = "thunder.svg";
		        description = "Yikes, looks like a storm's brewing!"
	        } else if (weather == "snow") {
		        icon = "snow.svg";
		        description = "Wrap up, it's going to snow!"
	        } else if (weather == "mist") {
		        icon = "mist.svg";
		        description = "Looks a little misty today.";
	        } else {
		        icon = "default.svg";
		        description = "Oops, I couldn't find the weather in " + location;
	        }

	        document.getElementById("imgWeatherIcon").src = "images/" + icon;
	        document.getElementById("divDescription").innerHTML = description;
	        /*kelvin, for celsius: (temperature - 273.15) + " °C"*/
	        document.getElementById("divTemperature").innerHTML = (parseFloat(temperature)-273.15).toString() + " C"; 
        }


        function ShowSlides() {
          var idx;
          var slides = document.getElementsByClassName("mySlides");
          var dots = document.getElementsByClassName("dot");
          for (idx = 0; idx < slides.length; idx++) {
            slides[idx].style.display = "none";  
          }
          slideIndex++;
          if (slideIndex > slides.length) {slideIndex = 1}    
          for (idx = 0; idx < dots.length; idx++) {
            dots[idx].className = dots[idx].className.replace(" active", "");
          }
          slides[slideIndex-1].style.display = "block";  
          dots[slideIndex-1].className += " active";
          setTimeout(ShowSlides, 2000); // Change image every 2 seconds
        }
    </script>
</head>
<body>
    <div class="row">
	    <div class="col-md-3 boxborder">
	        <div class="showbox" style="min-height:100px">
		         1.Sirius Logo
                <img src="../Images/SiteImages/img_snow_wide.jpg" class="img-responsive" alt="Cinque Terre">
		    </div>
	    </div>
	    <div class="col-md-9 boxborder">
	        <div class="showbox" style="min-height:100px;width:99%;">
		         2.Sirius Connect
		    </div>
	    </div>	
	    <div class="col-md-3 boxborder" style="min-height:300px;">
		     <div class="showbox" style="width:98%;height:145px;">3.Sirius in News</div>
		     <div class="showbox" style="width:98%;height:145px;">
		        <center>
				    <div id="divImage" style="width:100%;height:30px;text-align:center;">
					    <img id="imgWeatherIcon" src="" />
				    </div>
				    <div id="divTemperature" style="width:100%;height:60px;text-align:center;"></div>
				    <div id="divDescription" style="width:100%;height:30px;text-align:center;"></div>
			    </center>
		     </div>
	    </div>
	    <div class="col-md-6 boxborder" style="min-height:300px;">
	        <div class="showbox" style="width:99%;height:300px;border:0px;">
			    <div class="slideshow-container showbox">
				    <div class="mySlides fade">
				      <div class="numbertext">1 / 3</div>
				      <a href="#" style="margin-bottom:10px;">
					      <img src="../Images/SiteImages/img_mountains_wide.jpg" style="width:100%">
					      <div class="text" style="margin-bottom:10px;text-decoration:solid;">Caption Text</div>
				      </a>
				    </div>

				    <div class="mySlides fade">
				      <div class="numbertext">2 / 3</div>
				      <a href="#" style="margin-bottom:10px;">
					      <img src="../Images/SiteImages/img_nature_wide.jpg" style="width:100%">
					      <div class="text" style="margin-bottom:10px;text-decoration:solid;">Caption Two</div>
				      </a>
				    </div>

				    <div class="mySlides fade">
				      <div class="numbertext">3 / 3</div>
				      <a href="#" style="margin-bottom:10px;">
					      <img src="../Images/SiteImages/img_snow_wide.jpg" style="width:100%">
					      <div class="text" style="margin-bottom:10px;text-decoration:solid;">Caption Three</div>
				      </a>
				    </div>
			    </div>
			    <br>
			    <div style="text-align:center">
			      <span class="dot"></span> 
			      <span class="dot"></span> 
			      <span class="dot"></span> 
			    </div>
            </div>		
	    </div>
	    <div class="col-md-3 boxborder">
	        <div class="showbox" style="max-height:300px;max-width:300px;"> 
		         6.Department Listing
                <img src="../Images/SiteImages/img_snow_wide.jpg" alt="Snow">
                <button class="btn">Button</button>
		    </div>
	    </div>
	    <div class="col-md-3 boxborder">
	        <div class="showbox" style="min-height:100px;"> 
		         7.Upcoming Events
		    </div>
	    </div>
	    <div class="col-md-3 boxborder">
	        <div class="showbox" style="min-height:100px;">  
                 8.Announcements		
	        </div>
	    </div>
	    <div class="col-md-3 boxborder">
	        <div class="showbox" style="min-height:100px;"> 
		         9.Survey Section
		    </div>
	    </div>
	    <div class="col-md-3 boxborder">
	        <div class="showbox" style="min-height:100px;"> 
	            10.Helpful Link OKTA
		    </div>
	    </div>
	    <div class="col-md-12 boxborder showbox" style="min-height:100px;width:100%">
            11.Footer notifications
	    </div>
    </div>
</body>
</html>
