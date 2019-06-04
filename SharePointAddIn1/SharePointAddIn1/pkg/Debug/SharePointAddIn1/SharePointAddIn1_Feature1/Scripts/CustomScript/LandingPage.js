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

function loadData() {
    loadListData("PromotedURLlist", PromotedURLSuccess, globalError);
    loadListData("UpcomingEvent", UpcomingEventSuccess, globalError);
    loadListData("DepartmentList", DepartmentListSuccess, globalError);
    loadListData("SiriusNews", SiriusNewsSuccess, globalError);
    loadListData("Announcement", AnnouncementSuccess, globalError);
    loadListData("HelpContent", HelpContentSuccess, globalError);
    loadListData("SurveyList", SurveyPostSuccess, globalError);
    loadListData("SurveyUserAsnwer", SurveyUserAnswerSuccess, globalError);
}

function loadListData(listName, onSuccess, onFail) {
    context = SP.ClientContext.get_current();
    var list = context.get_web().get_lists().getByTitle(listName);
    var camlQuery = SP.CamlQuery.createAllItemsQuery();
    var listItems = list.getItems(camlQuery);

    context.load(listItems);

    context.executeQueryAsync(
        function (sender, args) {
            // listItem is defined on same closure, you do not need to declare globally
            onSuccess(listItems);
        },
        onFail
    );
}

var PromotedURL = [];
function PromotedURLSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        var listItemInfo = '<div class="slideshow-container-upcomingevent" style=\"min-height:100px;\">';
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            PromotedURL.push({
                Title: oListItem.get_item('Title'),
                Name: oListItem.get_item('PromotedName'),
                Description: oListItem.get_item('PromotedDescription'),
                URL: oListItem.get_item('PromotedURL')
            });
        }
        ShowSlide("myCarousel", "Courasol", "olCarouselIndicators", "divCarouselShow");
    }
    catch (ex) {
        alert(ex);
    }
}

function ShowSurvey(SurveyName) {
    window.location.href = "https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/pages/Survey.aspx?Survey=" + SurveyName
}

function GetQueryStringValue(key) {
    return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}

function ShowSurveyQA() {
    try {
        var SurveyName = GetQueryStringValue("Survey");
        var SelectedType = "", ShowQuestions = "";

        $.each(SurveyQuestion, function (keyQues, itemQues) {
            if (itemQues.Title == SurveyName) {
                ShowQuestions += "<div class=\"col-md-12\" style=\"margin: 0px 0px;padding:10px 0px 5px 0px;border-bottom:1px solid #8B0000\">";
                ShowQuestions += "   <div style=\"width:100%;text-align:left;\">" + itemQues.QuestionName + "</div>";
                $.each(SurveyAnswer, function (keyAns, itemAns) {
                    if (itemAns.Title == SurveyName && itemAns.QuestionNo == itemQues.QuestionNo) {
                        SelectedType = itemQues.QuestionType.toLowerCase();
                        ShowQuestions += "<div class=\"col-md-3\" style=\"margin: 0px 0px;padding-bottom:5px;\">";
                        ShowQuestions += "   <div style=\"width:100%;padding:2px;\">";
                        ShowQuestions += "       <input type=\"" + SelectedType + "\" data-ans=\"" + itemAns.AnswerNo + "\" data-ques=\"" + itemAns.QuestionNo + "\" id=\"div" + itemAns.AnswerNo + "\" name=\"Ques" + itemQues.QuestionNo + "\" value=\"\"  class=\"form-control searchtext\" style=\"width:30px!important;float:left;\"/>";
                        ShowQuestions += "       <label for=\"div" + itemAns.AnswerNo + "\" style=\"float:left;padding-top:10px;\">" + itemAns.AnswerName + "</label>";
                        ShowQuestions += "   </div>";
                        ShowQuestions += "</div>";
                    }
                });
                ShowQuestions += "</div>";
            }
        });
        $("#divSurveyInf").html(ShowQuestions);
    }
    catch (ex) {
        alert(ex);
    }

}

function ShowSurveyPost() {
    var ShowSurveyPostInf = "";

    for (var Idx = 0; Idx < SurveyPost.length; Idx++) {
        if (SurveyPost[Idx].SurveyActive == "Y") {
            ShowSurveyPostInf += "<div style=\"width:100%;cursor:pointer;\" onclick=\"ShowSurvey('" + SurveyPost[Idx].SurveyName + "')\">" + SurveyPost[Idx].SurveyName+"</div>";
        }
    }
    $("#divSurvey").html(ShowSurveyPostInf);
}

var SurveyPost = [], SurveyQuestion=[], SurveyAnswer=[], SurveyUserAnswer=[];
function SurveyPostSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            SurveyPost.push({
                Title: oListItem.get_item('Title'),
                SurveyName: oListItem.get_item('SurveyName'),
                SurveyCreatedBy: oListItem.get_item('SurveyCreatedBy'),
                SurveyCreatedDate: oListItem.get_item('SurveyCreatedDate'),
                SurveyActive: oListItem.get_item('SurveyActive')
            });
        }

        try {
            // Show the Active Survey
            ShowSurveyPost();
            loadListData("Question", SurveyQuestionSuccess, globalError);
        }
        catch (ex) {
            error = ex;
        }
    }
    catch (ex) {
        alert(ex);
    }
}

function SurveyQuestionSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            SurveyQuestion.push({
                Title: oListItem.get_item('Title'),
                QuestionNo: oListItem.get_item('QuestionNo'),
                QuestionName: oListItem.get_item('QuestionName'),
                QuestionType: oListItem.get_item('QuestionType')
            });
        }
        loadListData("Answer", SurveyAnswerSuccess, globalError);
    }
    catch (ex) {
        alert(ex);
    }
}

function SurveyAnswerSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            SurveyAnswer.push({
                Title: oListItem.get_item('Title'),
                AnswerName: oListItem.get_item('AnswerName'),
                AnswerNo: oListItem.get_item('AnswerNo'),
                QuestionNo: oListItem.get_item('QuestionNo')
            });
        }

        try {
            ShowSurveyQA();
        }
        catch (ex) {
            error=ex;
        }
    }
    catch (ex) {
        alert(ex);
    }
}

function SurveyUserAnswerSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            SurveyUserAnswer.push({
                Title: oListItem.get_item('Title'),
                QuestionNo: oListItem.get_item('QuestionNo'),
                AnswerNo: oListItem.get_item('AnswerNo'),
                AnsweredBy: oListItem.get_item('AnsweredBy'),
                AnsweredDate: oListItem.get_item('AnsweredDate')
            });
        }
    }
    catch (ex) {
        alert(ex);
    }
}

function ShowSlide(Id, Name, IndicatorContainer, ImageContainer) {
    var Index = 0;
    var Indicator = "", Container = "";
    for (var Idx = 0; Idx < PromotedURL.length; Idx++) {
        if (PromotedURL[Idx].Title == "Slider" && PromotedURL[Idx].Name == Name) {
            if (Index == 0)
                Indicator += "<li data-target=\"#" + Id + "\" data-slide-to=\"" + Index.toString() + "\" class=\"active\"></li>";
            else
                Indicator += "<li data-target=\"#" + Id + "\" data-slide-to=\"" + Index.toString() + "\"></li>";

            var Anchor = PromotedURL[Idx].Description.split(";");
            Container +=
                "<div class=\"item " + (Index == 0 ? "active" : "") + "\">" +
                "   <img src=\"" + PromotedURL[Idx].URL + "\" alt=\"Image" + Index.toString() + "\" style=\"width:100%;\" />" +
                "   <div class=\"carousel-caption\">";
            for (var Idy = 0; Idy <= Anchor.length - 1; Idy++) {
                var AnchorTag = Anchor[Idy].split("|");
                Container += "<a target=\"_blank\" href=\"" + AnchorTag[1] + "\"><p style=\"color:black;font-weight:bold;\">" + AnchorTag[0] + "</p></a>";
            }
            Container += "</div></div>";

            Index++;
        }
    }

    $("#" + IndicatorContainer).html(Indicator);
    $("#" + ImageContainer).html(Container);
}

function UpcomingEventSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        var listItemInfo = '<div class="slideshow-container-upcomingevent" style=\"min-height:100px;\">';
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            listItemInfo +=
                '   <div class=\"upcomingeventslides\">' +
                '       <div>' + oListItem.get_item('Title') + '</div>' +
                '       <div>' + oListItem.get_item('Location') + '</div>' +
                '       <div>' + oListItem.get_item('EventType') + '</div>' +
                '       <div>' + oListItem.get_item('EventDate') + '</div>' +
                '   </div>';
        }
        listItemInfo += '   <a class="prev-upcomingevent" onclick="PlusUpcomingEventSlides(-1)">&#10094;</a>' +
            '   <a class="next-upcomingevent" onclick="PlusUpcomingEventSlides(1)">&#10095;</a>' +
        '</div> ';
        $('#divUpComingEvent').html(listItemInfo);
        ShowUpcomingEventSlides(1);
    }
    catch (ex) {
        $('#divUpComingEvent').html("Up coming Event: " + ex);
    }
}

function DepartmentListSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        var listItemInfo = '<div class="slideshow-container-departmentlist" style=\"min-height:100px;\">';
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            listItemInfo +=
                '   <div class=\"departmentlistslides\">' +
                '       <div style=\"cursor:pointer;\" onclick=\"window.location.href=\'https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/pages/Orgchart.aspx?StartNode=' + GetOrgTitleKey(oListItem.get_item('Title')) + '\'\">' + oListItem.get_item('Title') + '</div>' +
                '       <div>' + oListItem.get_item('DepartmentHead') + '</div>' +
                '   </div>';
        }
        listItemInfo += '   <a class="prev-departmentlist" onclick="PlusDepartmentListSlides(-1)">&#10094;</a>' +
            '   <a class="next-departmentlist" onclick="PlusDepartmentListSlides(1)">&#10095;</a>' +
        '</div> ';
        $('#divDepartmentList').html(listItemInfo);
        ShowDepartmentListSlides(1);
    }
    catch (ex) {
        $('#divDepartmentList').html("Department List: " + ex);
    }
}

function SiriusNewsSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        var listItemInfo = '<div class="slideshow-container-siriusnews" style=\"min-height:100px;\">';
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            listItemInfo +=
                '   <div class=\"siriusnewsslides\">' +
                '       <div>' + oListItem.get_item('Title') + '</div>' +
                '       <div>' + oListItem.get_item('News') + '</div>' +
                '       <div>' + oListItem.get_item('NewsDate') + '</div>' +
                '   </div>';

        }
        listItemInfo += '   <a class="prev-siriusnews" onclick="PlusSiriusNewsSlides(-1)">&#10094;</a>' +
            '   <a class="next-siriusnews" onclick="PlusSiriusNewsSlides(1)">&#10095;</a>' +
        '</div> ';
        $('#divSiriusNews').html(listItemInfo);
        ShowSiriusNewsSlides(1);
    }
    catch (ex) {
        $('#divSiriusNews').html("Sirius New: " + ex);
    }
}

function AnnouncementSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        var listItemInfo = '<div class="slideshow-container-announcement" style=\"min-height:100px;\">';
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            listItemInfo +=
                '   <div class=\"announcementslides\">' +
                '       <div>' + oListItem.get_item('Title') + '</div>' +
                '       <div>' + oListItem.get_item('AnnouncementDesc') + '</div>' +
                '       <div>' + oListItem.get_item('AnnouncementDate') + '</div>' +
                '   </div>';
        }
        listItemInfo += '   <a class="prev-announcement" onclick="PlusAnnouncementSlides(-1)">&#10094;</a>' +
            '   <a class="next-announcement" onclick="PlusAnnouncementSlides(1)">&#10095;</a>' +
        '</div> ';
        $('#divAnnouncement').html(listItemInfo);
        ShowAnnouncementSlides(1);
    }
    catch (ex) {
        $('#divAnnouncement').html("Announcement: " + ex);
    }
}

function HelpContentSuccess(data) {
    try {
        var listItemEnumerator = data.getEnumerator();
        var listItemInfo = '<div class="slideshow-container-helpcontent" style=\"min-height:100px;\">';
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            listItemInfo +=
                '   <div class=\"helpcontentslides\">' +
                '       <a target=\"_blank\" style=\"cursor:pointer;\" href=\"' + oListItem.get_item('HelpURL') + '\">' + oListItem.get_item('Title') + '</a>' +
                '   </div>';
        }
        listItemInfo += '   <a class="prev-helpcontent" onclick="PlusHelpContentSlides(-1)">&#10094;</a>' +
            '   <a class="next-helpcontent" onclick="PlusHelpContentSlides(1)">&#10095;</a>' +
        '</div> ';
        $('#divHelpContent').html(listItemInfo);
        ShowHelpContentSlides(1);

        //App responsive on load 
        reSize.AppPart.init();

        setTimeout(function () {
            reSize.AppPart.autoSize();
        }, 500);
    }
    catch (ex) {
        $('#divHelpContent').html("Help Content: " + ex);
    }
}

function globalError(sender, args) {
    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}

function ShowDetailedWeatherReport() {
    window.location.href = "https://joetellez.sharepoint.com/sites/iterondev/SharePointAddIn1/Pages/ShowDetailedWeatherReport.aspx";
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

window.fbAsyncInit = function () {
    FB.init({
        appId: '2263756927217584',
        cookie: true,
        xfbml: true,
        version: 'v3.2'
    });

    FB.AppEvents.logPageView();
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

// This is called with the results from from FB.getLoginStatus().
function statusChangeCallback(response) {
    console.log('statusChangeCallback');
    console.log(response);
    // The response object is returned with a status field that lets the
    // app know the current login status of the person.
    // Full docs on the response object can be found in the documentation
    // for FB.getLoginStatus().
    if (response.status === 'connected') {
        // Logged into your app and Facebook.
        testAPI();
    } else {
        // The person is not logged into your app or we are unable to tell.
        document.getElementById('status').innerHTML = 'Please log ' +
            'into this app.';
    }
}

// This function is called when someone finishes with the Login
// Button.  See the onlogin handler attached to it in the sample
// code below.
function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}

// Here we run a very simple test of the Graph API after login is
// successful.  See statusChangeCallback() for when this call is made.
function testAPI() {
    console.log('Welcome!  Fetching your information.... ');
    FB.api('/me', function (response) {
        console.log('Successful login for: ' + response.name);
        document.getElementById('status').innerHTML =
            'Thanks for logging in, ' + response.name + '!';
    });
}

// Testing function Start
function getUserData() {
    var employeeList = clientContext.get_site().get_rootWeb().get_lists().getByTitle('Служители');
    var userTitle = user.get_title();
    var collListItem;
    var oListItem;

    var camlQuery = new SP.CamlQuery();
    camlQuery.set_viewXml(
        "<View><Query>" +
        "<Where>" +
        "<Eq><FieldRef Name=\"Title\"/><Value Type=\"Text\">" + userTitle + "</Value></Eq>" +
        "</Where>" +
        "</Query></View>");
    collListItem = employeeList.getItems(camlQuery);

    clientContext.load(collListItem, 'Include(Title, empNickName, empPosition, empDepartment)');
    clientContext.executeQueryAsync(
        Function.createDelegate(this, successHandler),
        Function.createDelegate(this, errorHandler)
    );

    function successHandler() {
        var listItemEnumerator = collListItem.getEnumerator();
        while (listItemEnumerator.moveNext()) {
            var oListItem = listItemEnumerator.get_current();
            alert(oListItem.get_item('empNickName'));
        }
        //userDepartment.innerHTML = oListItem.get_item('empNickName');
    }

    function errorHandler() {
        userDepartment.innerHTML = "Request failed: " + arguments[1].get_message();
    }
}

function retrieveAllListsAllFields() {

    var clientContext = new SP.ClientContext.get_current();
    var oWebsite = clientContext.get_web();
    var rootWebsite = clientContext.get_site().get_rootWeb();
    var collList = oWebsite.get_lists();

    this.listInfoArray = clientContext.loadQuery(collList,
        'Include(Title,Fields.Include(Title,InternalName))');

    clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded),
        Function.createDelegate(this, this._onQueryFailed));
}

function onQuerySucceeded() {

    var listInfo = '';

    for (var idx = 0; idx < this.listInfoArray.length; idx++) {

        var oList = this.listInfoArray[idx];
        var collField = oList.get_fields();

        var fieldEnumerator = collField.getEnumerator();

        while (fieldEnumerator.moveNext()) {
            var oField = fieldEnumerator.get_current();
            var regEx = new RegExp('name', 'ig');

            if (regEx.test(oField.get_internalName())) {
                listInfo += '\nList: ' + oList.get_title() +
                    '\n\tField Title: ' + oField.get_title() +
                    '\n\tField Name: ' + oField.get_internalName();
            }
        }

        alert(listInfo);
    }
}

function onQueryFailed(sender, args) {
    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}
// Testing function End
