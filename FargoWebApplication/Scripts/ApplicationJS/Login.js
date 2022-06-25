$(function () {
   BtnLoginEvt();
})

function GetLoginModel() {
    var model = {
        "EMAIL_ID": $.trim($("#txtEmailId").val()),
        "PASSWORD": $.trim($("#txtPassword").val()),
    }
    return model;
}

function BtnLoginEvt() {
    $("#btnLogin").click(function () {
        var model = GetLoginModel();
        if (model.EMAIL_ID == "" || model.EMAIL_ID == null) {
            alert("Please Enter Email Id");
            return false;
        }
        else if (model.PASSWORD == "" || model.PASSWORD == null) {
            alert("Please Enter Password");
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/Login/LoginUser",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response == "SUCCESS") {
                    PopupMessage("Success", "Data Save Successfully");
                    LoadRoleGrid();
                    Reset();
                }
                else {
                    PopupMessage("Error", "Data Not Saved");
                    return false;
                }
            },
            error: function (response) {
                PopupMessage("Error", "Data Not Saved");
            }
        });
    });
}