// Up Coming Events information
var slideUpcomingEventIndex = 1;
ShowUpcomingEventSlides(slideUpcomingEventIndex);

function PlusUpcomingEventSlides(n) {
    ShowUpcomingEventSlides(slideUpcomingEventIndex += n);
}

function CurrentUpcomingEventSlide(n) {
    ShowUpcomingEventSlides(slideUpcomingEventIndex = n);
}

function ShowUpcomingEventSlides(n) {
    var idx;
    var slides = document.getElementsByClassName("upcomingeventslides");
    if (slides.length >= 1) {
        if (n > slides.length) { slideUpcomingEventIndex = 1 }
        if (n < 1) { slideUpcomingEventIndex = slides.length }
        for (idx = 0; idx < slides.length; idx++) {
            slides[idx].style.display = "none";
        }
        slides[slideUpcomingEventIndex - 1].style.display = "block";
    }
    else {
        $('#divUpComingEvent').html("No Upcoming Events available");
    }
}

// Department List information
var slideDepartmentListIndex = 1;
ShowDepartmentListSlides(slideDepartmentListIndex);

function PlusDepartmentListSlides(n) {
    ShowDepartmentListSlides(slideDepartmentListIndex += n);
}

function CurrentDepartmentListSlide(n) {
    ShowDepartmentListSlides(slideDepartmentListIndex = n);
}

function ShowDepartmentListSlides(n) {
    var idx;
    var slides = document.getElementsByClassName("departmentlistslides");
    if (slides.length >= 1) {
        if (n > slides.length) { slideDepartmentListIndex = 1 }
        if (n < 1) { slideDepartmentListIndex = slides.length }
        for (idx = 0; idx < slides.length; idx++) {
            slides[idx].style.display = "none";
        }
        slides[slideDepartmentListIndex - 1].style.display = "block";
    }
    else {
        $('#divDepartmentList').html("No Department List available");
    }
}

// Sirius News information
var slideSiriusNewsIndex = 1;
ShowSiriusNewsSlides(slideSiriusNewsIndex);

function PlusSiriusNewsSlides(n) {
    ShowSiriusNewsSlides(slideSiriusNewsIndex += n);
}

function CurrentSiriusNewsSlide(n) {
    ShowSiriusNewsSlides(slideSiriusNewsIndex = n);
}

function ShowSiriusNewsSlides(n) {
    var idx;
    var slides = document.getElementsByClassName("siriusnewsslides");
    if (slides.length >= 1) {
        if (n > slides.length) { slideSiriusNewsIndex = 1 }
        if (n < 1) { slideSiriusNewsIndex = slides.length }
        for (idx = 0; idx < slides.length; idx++) {
            slides[idx].style.display = "none";
        }
        slides[slideSiriusNewsIndex - 1].style.display = "block";
    }
    else {
        $('#divSiriusNews').html("No Sirius News available");
    }
}

// Announcement information
var slideAnnouncementIndex = 1;
ShowAnnouncementSlides(slideAnnouncementIndex);

function PlusAnnouncementSlides(n) {
    ShowAnnouncementSlides(slideAnnouncementIndex += n);
}

function CurrentAnnouncementSlide(n) {
    ShowAnnouncementSlides(slideAnnouncementIndex = n);
}

function ShowAnnouncementSlides(n) {
    var idx;
    var slides = document.getElementsByClassName("announcementslides");
    if (slides.length >= 1) {
        if (n > slides.length) { slideAnnouncementIndex = 1 }
        if (n < 1) { slideAnnouncementIndex = slides.length }
        for (idx = 0; idx < slides.length; idx++) {
            slides[idx].style.display = "none";
        }
        slides[slideAnnouncementIndex - 1].style.display = "block";
    }
    else {
        $('#divAnnouncement').html("No Announcement available");
    }
}

// Help Content information
var slideHelpContentIndex = 1;
ShowHelpContentSlides(slideHelpContentIndex);

function PlusHelpContentSlides(n) {
    ShowHelpContentSlides(slideHelpContentIndex += n);
}

function CurrentHelpContentSlide(n) {
    ShowHelpContentSlides(slideHelpContentIndex = n);
}

function ShowHelpContentSlides(n) {
    var idx;
    var slides = document.getElementsByClassName("helpcontentslides");
    if (slides.length >= 1) {
        if (n > slides.length) { slideHelpContentIndex = 1 }
        if (n < 1) { slideHelpContentIndex = slides.length }
        for (idx = 0; idx < slides.length; idx++) {
            slides[idx].style.display = "none";
        }
        slides[slideHelpContentIndex - 1].style.display = "block";
    }
    else {
        $('#divHelpContent').html("No Help Content available");
    }
}
