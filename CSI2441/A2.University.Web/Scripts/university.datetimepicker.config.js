$(function () {

    // 100 years old
    var maxAge = moment().subtract("years", 100);

    $("#Dob").datetimepicker({
        format: "DD/MM/YYYY",
        locale: "en-au",
        showClose: true,
        showClear: true,
        toolbarPlacement: "top",
        viewMode: "years",
        minDate: maxAge
    });
});