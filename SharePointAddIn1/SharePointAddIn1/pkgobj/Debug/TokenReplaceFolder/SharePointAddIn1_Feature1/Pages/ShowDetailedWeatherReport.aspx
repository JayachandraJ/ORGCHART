<%@ Page language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.6.1/css/all.css" integrity="sha384-gfdkjb5BdAXd+lj+gudLWI+BXq4IuLW5IT+brZEZsLFm++aCMlF1V92rMkPaX4PP" crossorigin="anonymous" />
    <link rel="stylesheet" href="../Content/CustomScript/Custom.css" />

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../Scripts/CustomScript/Custom.js"></script>
    <script type="text/javascript" src="../Scripts/CustomScript/LandingPage.js"></script>

    <script type="text/javascript" src="/_layouts/15/MicrosoftAjax.js"></script>
    <script type="text/javascript" src="/_layouts/15/init.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.runtime.js"></script>
    <script type="text/javascript" src="/_layouts/15/sp.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            // Weather forecast routine
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var lat = position.coords.latitude;
                    var lon = position.coords.longitude;
                    var WeatherUrl = 'https://api.openweathermap.org/data/2.5/weather?appid=ac9d9fc53d7474c41df352931241a97e&lat=' + lat + '&lon=' + lon;

                    var ajax = new XMLHttpRequest();
                    var json;
                    ajax.open("GET", WeatherUrl, true);
                    ajax.send();
                    ajax.onreadystatechange = function () {
                        if (ajax.readyState == 4 && ajax.status == 200) {
                            json = JSON.parse(ajax.responseText);
                            if (json != undefined) {
                                ShowWeatherInfo(json);
                            } else {
                                description = "Oops, I couldn't find the weather in " + lat + ":" + lon;
                                document.getElementById("divDescription").innerHTML = description;
                            }
                        }
                    };
                });
            }
        });

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
                description = "Looks like rain.";
            } else if (weather == "thunderstorm") {
                icon = "thunder.svg";
                description = "Yikes, looks like a storm's brewing!";
            } else if (weather == "snow") {
                icon = "snow.svg";
                description = "Wrap up, it's going to snow!";
            } else if (weather == "mist") {
                icon = "mist.svg";
                description = "Looks a little misty today.";
            } else {
                icon = "default.svg";
                description = "Normal Weather";
            }

            document.getElementById("imgWeatherIcon").src = "../Images/SiteImages/Forecast/" + icon;
            document.getElementById("divDescription").innerHTML = description;
            /*kelvin, for celsius: (temperature - 273.15) + " °C"*/
            document.getElementById("divTemperature").innerHTML = (parseFloat(temperature) - 273.15).toString() + " C";
        }
    </script>
</asp:Content>

<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
    <WebPartPages:WebPartZone runat="server" FrameType="TitleBarOnly" ID="full" Title="loc:full" />

    <div class="container">
        <div class="row" style="margin-bottom:10px;">
            <div class="col-md-12">
                <div id="divWeatherNews">
                    <center>
				        <div id="divImage" style="width:100%;height:100px;text-align:center;">
					        <img id="imgWeatherIcon" src="" alt="" style="width:100px;height:100px;" />
				        </div>
				        <div id="divTemperature" style="width:100%;height:30px;text-align:center;"></div>
                        <div id="divTemperatureLookAndFeel" style="width:100%;text-align:center;">
                            <div class="row" style="margin-bottom:10px;">
                                <div class="col-md-12">
                                    <p style="font-weight:bold;">Place</p>
                                    <div id="divPlace">Nanganallur</div>
                                </div>
                            </div>
                            <div class="row" style="margin-bottom:10px;">
                                <div class="col-md-6">
                                    <p style="font-weight:bold;">Sun Raise</p>
                                    <div id="divSunRise">5:49</div>
                                </div>
                                <div class="col-md-6">
                                    <p style="font-weight:bold;">Sun Set</p>
                                    <div id="divSunSet">18.35</div>
                                </div>
                            </div>
                            <div class="row" style="margin-bottom:10px;">
                                <div class="col-md-6">
                                    <p style="font-weight:bold;">Humidity</p>
                                    <div id="divHumidity">67%</div>
                                </div>
                                <div class="col-md-6">
                                    <p style="font-weight:bold;">Wind</p>
                                    <div id="divWind">5.87</div>
                                </div>
                            </div>
                            <div class="row" style="margin-bottom:10px;">
                                <div class="col-md-12">
                                    <p style="font-weight:bold;">Pressure</p>
                                    <div id="divPressure">1006 hPa</div>
                                </div>
                            </div>
                        </div>
				        <div id="divDescription" style="width:100%;height:30px;text-align:center;"></div>
			        </center>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
