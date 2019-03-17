$(function () {

    $('#invite').click(function () {
        inHTML = "";
        $("#Users option:selected").each(function () {
            inHTML += '<option value="' + $(this).val() + '">' + $(this).text() + '</option>';
        });
        $("#Users option:selected").remove();
        $("#InvitedUsers").append(inHTML);
    });

    $("form").submit(function (e) {
        $("#InvitedUsers option").attr("selected", "selected");
    });

    $("#save").click(function (e) {
        $("#InvitedUsers option").attr("selected", "selected");
        $("#Users option").attr("selected", "selected");
    });

    $('#datetimepicker1').datetimepicker({
        stepping: 30,
        enabledHours: [8, 9, 10, 11, 12, 13, 14, 15, 16, 17],
        daysOfWeekDisabled: [0, 6],
        minDate: moment(),
        useCurrent: false,
    });

    $('#datetimepicker2').datetimepicker({
        stepping: 30,
        enabledHours: [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18],
        daysOfWeekDisabled: [0, 6],
        minDate: moment(),
        useCurrent: false,
    });

    $("#datetimepicker1").on("dp.change", function (e) {
        $('#datetimepicker2').data("DateTimePicker").minDate(e.date);
       
    });
    $("#datetimepicker2").on("dp.change", function (e) {
        $('#datetimepicker1').data("DateTimePicker").maxDate(e.date);
    });
    
    $('#remove').click(function () {
        inHTML = "";
        $("#InvitedUsers option:selected").each(function () {
            inHTML += '<option value="' + $(this).val() + '">' + $(this).text() + '</option>';
        });
        $("#Users").append(inHTML);
        $("#InvitedUsers option:selected").remove();
    });
});