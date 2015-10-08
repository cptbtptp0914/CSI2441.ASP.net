$(function() {
    $("#Dob").datetimepicker({
        defaultDate: "@Model.Dob",
        format: "L",
        showClose: true,
        showClear: true,
        toolbarPlacement: "top"
    });
});