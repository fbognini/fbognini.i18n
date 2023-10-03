// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $.extend(true, $.fn.dataTable.defaults, {
        language: {
            url: "/_content/fbognini.i18n.Dashboard/lib/Datatables/it-IT.json"
        },
    });
});