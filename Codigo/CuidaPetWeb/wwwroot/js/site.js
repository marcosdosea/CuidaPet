// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const btnMenu = document.getElementById("btnMenu");
const closeMenu = document.getElementById("closeMenu");

btnMenu.addEventListener("click", function () {
    $("#sidebar").addClass("active");
});

closeMenu.addEventListener("click", function () {
    $("#sidebar").removeClass("active");
});
