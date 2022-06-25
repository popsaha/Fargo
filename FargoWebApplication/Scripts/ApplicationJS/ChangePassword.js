$(function () {
    RegBtnChangePasswordEvt();

});

function ChangePasswordModel() {
    var model = {
        "UserId":      $.trim($("#txtUserId").val()),
        "OldPassword": $.trim($("#txtOldPassword").val()),
        "NewPassword": $.trim($("#txtNewPassword").val()),
        "ConformNewPassword": $.trim($("#txtConformNewPassword").val()),
    }
    return model;
}
function NewPasswordValidate() {
    debugger
    var password = $("#txtNewPassword").val();
    var confirmPassword = $("#txtConformNewPassword").val();
    if (confirmPassword != "") {
        if (password != confirmPassword) {
            PopupMessage("Warning", "Passwords Do Not Match.");
            $("#txtNewPassword").val("");
            $("#txtConformNewPassword").val("");
            return false;
        }
    }
};
function ConformNewPasswordValidate() {
    var password = $("#txtNewPassword").val();
    var confirmPassword = $("#txtConformNewPassword").val();
    if (password != confirmPassword) {
        PopupMessage("Warning", "Passwords Do Not Match.");
        $("#txtConformNewPassword").val("");
        return false;
    }
};
function RegBtnChangePasswordEvt() {
    $("#btnChangePassword").click(function () {    
        debugger
        var ID = $(this).prop('disabled', true);
        model = ChangePasswordModel();
        if (model.UserId == "" || model.UserId == undefined) {
            PopupMessage("Warning", "Please Enter user ID");
            return false;
        }
        else if (model.OldPassword == "" || model.OldPassword == null) {
            PopupMessage("Warning", "Please Enter Old Password");
            return false;
        }
        else if (model.NewPassword == "" || model.NewPassword == null) {
            PopupMessage("Warning", "Please Enter New Password");
            return false;
        }
        else if (model.ConformNewPassword == "" || model.ConformNewPassword == null) {
            PopupMessage("Warning", "Please Enter Confirm New Password");
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/ChangePassword/ChangeNewPassword",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response == "SUCCESS") {
                    PopupMessage("Success", "Change Password Successfully!!!");
                    $("#btnChangePassword").prop('disabled', false);
                    Reset();
                }
                if (response == "NOTMATCH") {
                    PopupMessage("Warning", "Not Match Old Password!!!");
                    $("#btnChangePassword").prop('disabled', false);
                    return false;
                }
            },
            error: function (response) {
                PopupMessage("Error", "Some Error Occured!!!");
                $("#btnChangePassword").prop('disabled', false);
            }
        });
    });
}

function Reset() {
    $("#txtUserId").val();
    $("#txtOldPassword").val();
    $("#txtNewPassword").val();
    $("#txtConformNewPassword").val();
}