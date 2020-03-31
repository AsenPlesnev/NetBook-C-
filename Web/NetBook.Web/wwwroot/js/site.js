// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('#welcomeIndex').fadeIn(900);
});

$(document).ready(function () {
    $('#welcome').fadeIn(900);
});

var cc = $(window).width();
$(document).ready(function () {
    $('.button-collapse').sideNav({
            menuWidth: 250,
            edge: 'left',
            closeOnClick: !(cc > 992),
            draggable: true // Choose whether you can drag to open on touch screens
        }
    );
    $('.dropdown-button').dropdown({
            inDuration: 300,
            outDuration: 225,
            constrainWidth: true, // Does not change width of dropdown to that of the activator
            belowOrigin: true, // Displays dropdown below the button
        }
    );
    $('.collapsible').collapsible();
    $('.modal').modal();
    $('select').material_select();
});

function openNav() {
    document.getElementById("nav-mobile").style.width = "250px";
    document.getElementById("main").style.paddingLeft = "250px";
    document.getElementById("footer").style.paddingLeft = "250px";
}

function closeNav() {
    document.getElementById("nav-mobile").style.width = "0";
    document.getElementById("main").style.paddingLeft = "0";
    document.getElementById("footer").style.paddingLeft = "0";
    document.getElementById("show-nav").style.display = "inline-block";
}

function deleteItem(form) {
    $(form).parents('li').remove();
}

$('.datepicker').pickadate({
    selectMonths: true, // Creates a dropdown to control month
    selectYears: 100, // Creates a dropdown of 100 years to control year,
    today: '',
    clear: 'Изчисти',
    close: 'Избери',
    format: 'yyyy-mm-dd',
    closeOnSelect: true // Close upon selecting a date,
});
