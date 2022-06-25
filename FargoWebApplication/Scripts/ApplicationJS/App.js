$(function () {

});

function PopupMessage(type, message) {
    $("#divModalType").removeClass();
    if (type.toUpperCase() == "SUCCESS")
        $("#divModalType").addClass("modal modal-success");
    else if (type.toUpperCase() == "WARNING")
        $("#divModalType").addClass("modal modal-warning");
    else if (type.toUpperCase() == "ERROR")
        $("#divModalType").addClass("modal modal-danger");
    else
        $("#divModalType").addClass("modal");

    $("#AlertHeading").text(type);
    $("#AlertMsg").text(message);
    $("#divModalType").modal("show");
}

function ConfirmMessage(message,callbackfunc) {
    $("#ConfirmMsg").text(message);
    $("#btnConfirmSubmit").attr("onclick", callbackfunc);
    $("#divConfirmModalType").modal("show");
}