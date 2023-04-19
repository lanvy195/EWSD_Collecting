// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Functions of Button to Open & Close the Sidebar
let btn = document.querySelector('#btn');
let sidebar = document.querySelector('.sidebar');
let srcBtn = document.querySelector('bx-search-alt');

btn.onclick = function () {
    sidebar.classList.toggle('active');
}

srcBtn.onclick = function () {
    sidebar.classList.toggle('active');
}

// Get all checkboxes and "Delete all" button
//var checkboxes = document.querySelectorAll('input[type="checkbox"]');
//var deleteAllButton = document.getElementById("delete-all");

// Add change event to each checkbox
//checkboxes.forEach(function (checkbox) {
    //checkbox.addEventListener("change", function () {
        // Check if any checkbox is checked
        //var checked = false;
        //checkboxes.forEach(function (cb) {
            //if (cb.checked) {
                //checked = true;
            //}
        //});
        // If any checkbox is selected, show "delete all" button. Otherwise, hide the "delete all" button
        //if (checked) {
            //deleteAllButton.style.display = "block";
        //} else {
            //deleteAllButton.style.display = "none";
        //}
    //});
//});


//function setContainerHeight() {
//    var footer = document.getElementByTagName("footer");
//   var footerHeight = footer.offsetHeight;
//    var windowHeight = window.innerHeight;
//    var container = document.getElementByClassName("container");
//    var containerHeight = windowHeight - footerHeight;
//    container.style.minHeight = containerHeight + "px";
//}

//window.onload = setContainerHeight;
//window.onresize = setContainerHeight;