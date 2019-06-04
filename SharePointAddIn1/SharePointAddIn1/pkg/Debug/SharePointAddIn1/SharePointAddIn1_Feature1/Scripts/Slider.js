<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>

<script type="text/javascript">
    var NewSlider=$(".newslider").length.toString();
    var HTML="<div id=\"myCarousel"+NewSlider+"\" class=\"carousel slide newslider\" data-ride=\"carousel\" style=\"overflow:hidden;\">"+
             "   <!-- Indicators --> "+
             "   <ol class=\"carousel-indicators\" id=\"olCarouselIndicators"+NewSlider+"\"></ol>" +
             "   <!-- Wrapper for slides --> " +
             "   <div class=\"carousel-inner\" id=\"divCarouselShow"+NewSlider+"\"></div>"+
             "   <!-- Left and right controls -->" +
             "   <a class=\"left carousel-control\" href=\"#myCarousel" + NewSlider +"\" data-slide=\"prev\">"+
             "      <span class=\"glyphicon glyphicon-chevron-left\"></span>" +
             "      <span class=\"sr-only\">Previous</span>" +
             "   </a>" +
             "   <a class=\"right carousel-control\" href=\"#myCarousel" + NewSlider +"\" data-slide=\"next\">" +
             "      <span class=\"glyphicon glyphicon-chevron-right\"></span>" +
             "      <span class=\"sr-only\">Next</span>" +
             "   </a>" +
             "</div>";

    document.write(HTML);
    ExecuteOrDelayUntilScriptLoaded(loadData, "sp.js");
        
    function loadData() {
        loadListData("PromotedLinks", PromotedURLSuccess, globalError);
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

    var PromotedURL = [];
    function PromotedURLSuccess(data) {
        try {
            var listItemEnumerator = data.getEnumerator();
            var listItemInfo = '<div class="slideshow-container-upcomingevent" style=\"min-height:100px;\">';
            while (listItemEnumerator.moveNext()) {
                var oListItem = listItemEnumerator.get_current();
                PromotedURL.push({
                    Title: (!oListItem.get_item('Title')) ? "" : oListItem.get_item('Title'),
                    Description: (!oListItem.get_item('Description')) ? "" : oListItem.get_item('Description'),
                    URL: (!oListItem.get_item('BackgroundImageLocation').get_url()) ? "" : oListItem.get_item('BackgroundImageLocation').get_url(),
                    LinkURL: (!oListItem.get_item('LinkLocation').get_url()) ? "" : oListItem.get_item('LinkLocation').get_url(),
                    LinkDesc: (!oListItem.get_item('LinkLocation').get_description()) ? "" : oListItem.get_item('LinkLocation').get_description()
                });
            }
            ShowSlide("myCarousel" + NewSlider, "Courasol", "olCarouselIndicators" + NewSlider, "divCarouselShow" + NewSlider);
        }
        catch (ex) {
            alert(ex);
        }
    }
    
    function globalError(sender, args) {
        alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
    }

    function LinkURL(LinkURL) {
        window.location.href = LinkURL;
    }
        
    function ShowSlide(Id, Name, IndicatorContainer, ImageContainer) {
        var Index = 0;
        var Indicator = "", Container = "", LinkURL = "", ImageStyle = "";

        if (PromotedURL.length) {

            // Get the First image size and it set the carousal size.
            var newImage = new Image(),
                realWidth = 0,
                realHeight = 0;
            if (newImage.addEventListener) {
                newImage.addEventListener('load', function () {
                    realWidth = newImage.width;
                    realHeight = newImage.height;

                    $("#myCarousel" + NewSlider).css("width", realWidth + "px");
                    $("#myCarousel" + NewSlider).css("height", realHeight + "px");

                });
            } else {
                // it's IE!
                newImage.attachEvent('onload', function () {
                    realWidth = newImage.width;
                    realHeight = newImage.height;

                    $("#myCarousel" + NewSlider).css("width", realWidth + "px");
                    $("#myCarousel" + NewSlider).css("height", realHeight + "px");
                });
            }
            newImage.src = PromotedURL[0].URL;

            for (var Idx = 0; Idx < PromotedURL.length; Idx++) {
                if (Idx == 0)
                    Indicator += "<li data-target=\"#" + Id + "\" data-slide-to=\"" + Index.toString() + "\" class=\"active\"></li>";
                else
                    Indicator += "<li data-target=\"#" + Id + "\" data-slide-to=\"" + Index.toString() + "\"></li>";

                if (PromotedURL[Idx].LinkURL.length >= 1) {
                    LinkURL = " onclick=\"LinkURL('" + PromotedURL[Idx].LinkURL + "')\"";
                }
                else LinkURL = "";

                Container +=
                    "<div class=\"item " + (Index == 0 ? "active" : "") + "\" style=\"cursor:pointer;min-width:100px;min-height:100px;\"" + LinkURL + ">" +
                    "   <img src=\"" + PromotedURL[Idx].URL + "\" alt=\"Image" + Index.toString() + "\"/>" +
                    "   <div class=\"carousel-caption\">";
                if (PromotedURL[Idx].Description.length >= 1) {
                    Container += "<p style=\"color:black;font-weight:bold;\">" + PromotedURL[Idx].Description + "</p>";
                }
                Container += "</div></div>";

                Index++;
            }

            $("#" + IndicatorContainer).html(Indicator);
            $("#" + ImageContainer).html(Container);
        }
        else {
            $("#" + IndicatorContainer).empty();
            $("#" + ImageContainer).empty();
        }
    }

    
</script>