var GridUserMaster = null;

$(function () {
    debugger
    $(document).ready(function () {
        LoadUserMasterGrid();
        RegBtnSubmitEvt();
        RegBtnUpdateEvt();
        RegbtnResetEvt();
    });
});
function GetClientModel() {
    var model = {
        "UM_ID": 0,
        "RM_ID": $("#ddlRoleName").val(),
        "FIRST_NAME": $.trim($("#txtFirstName").val()),
        "LAST_NAME": $.trim($("#txtLastName").val()),
        "EMAIL_ID": $.trim($("#txtEmailID").val()),
        "PASSWORD": $.trim($("#txtpassword").val()),
        "MOBILE_NO": $.trim($("#txtMobileNo").val()),
        "Profile_User_Img": $.trim($("#txtfileLogo").val()),
        "IS_ACTIVE": true,
    }
    return model;
}
function LoadUserMasterGrid() {
    $.ajax({
        type: "GET",
        url: "/UserMaster/GetUser_MasterList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            BindUserMasterDataTable(response);
        },
        error: function (response) {
        }
    });
}
function BindUserMasterDataTable(data) {
    if (GridUserMaster != null) {
        GridUserMaster.destroy();
    }
    GridUserMaster = $('#UserMasterGrid').DataTable({
        data: data,
        scrollX: true,
        columns: [
            { title: "Sr.No.", width: 20, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; } },
            { title: "Role Name", data: "ROLE_NAME" },
            { title: "First Name", data: "FIRST_NAME" },
            { title: "Last Name", data: "LAST_NAME" },
            { title: "Mobile No", data: "MOBILE_NO" },
            { title: "Email Id", data: "EMAIL_ID" },
            {
                title: "User Profile", data: "PROFILE_PHOTO", render: function (data, type, row, meta) {
                    return '<img src="/UploadImg/' + row.PROFILE_PHOTO + '" width="35px" height="35px" />';
                }
            },
            { title: "Is Active", data: "IS_ACTIVE" },
            { title: "Created By", data: "CREATED_BY" },
            { title: "Created On", data: "CREATED_ON" },
            {
                title: "Action", render: function (data, type, row, meta) {
                    return "<span id='edit'><span title='Edit' class='glyphicon glyphicon-edit' aria-hidden='true' style='cursor:pointer'></span></span> &nbsp;<span id='delete'><span title='Remove' class='glyphicon glyphicon-trash' aria-hidden='true' style='cursor:pointer'></span></span>";
                }
            },
        ],
    });
    $('#UserMasterGrid tbody').off('click');
    $('#UserMasterGrid tbody').on('click', '#delete', function () {
        _RowSelectedForDelete = $(this).parents('tr');
        var data = GridUserMaster.row(_RowSelectedForDelete).data();
        ConfirmMessage("Are you sure want to delete package", "DeleteUserMaster(" + data.UM_ID + ")");
    });
    $('#UserMasterGrid tbody').on('click', '#edit', function () {
        _RowSelectedForEdit = $(this).parents('tr');
        var data = GridUserMaster.row(_RowSelectedForEdit).data();
        EditUserMaster(data);
    });
}
function RegBtnSubmitEvt() {
    $("#btnSubmit").click(function () {
        debugger
        var model = GetClientModel()
        var formdata = new FormData();
        var Save_Profile = {};
        var File_Profile = $("#fileLogo").get(0);
        Save_Profile.files = File_Profile.files;
        Save_Profile.filePath = File_Profile.value;
        Save_Profile.allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
        for (var i = 0; i < Save_Profile.files.length; i++) {
            formdata.append(Save_Profile.files[i].name, Save_Profile.files[i]);
        }
        if (model.FIRST_NAME == "" || model.FIRST_NAME == null) {
            PopupMessage("Warning", "Please Enter First Name");
            return false;
        }
        else if (model.LAST_NAME == "" || model.LAST_NAME == null) {
            PopupMessage("Warning", "Please Enter Last Name");
            return false;
        }
        else if (model.RM_ID == "" || model.RM_ID == null) {
            PopupMessage("Warning", "Please Select Role Name");
            return false;
        }
        else if (model.EMAIL_ID == "" || model.EMAIL_ID == null) {
            PopupMessage("Warning", "Please Enter Email Id");
            return false;
        }

        else if (model.PASSWORD == "" || model.PASSWORD == null) {
            PopupMessage("Warning", "Please Enter Password");
            return false;
        }

        else if (model.MOBILE_NO == "" || model.MOBILE_NO == null) {
            PopupMessage("Warning", "Please Enter Mobile No");
            return false;
        }
        formdata.append('UM_ID', model.UM_ID); formdata.append('RM_ID', model.RM_ID); formdata.append('FIRST_NAME', model.FIRST_NAME);
        formdata.append('LAST_NAME', model.LAST_NAME); formdata.append('MOBILE_NO', model.MOBILE_NO);
        formdata.append('EMAIL_ID', model.EMAIL_ID); formdata.append('PASSWORD', model.PASSWORD); formdata.append('IS_ACTIVE', model.IS_ACTIVE);
        var IsExists = false;
        var _gridData = GridUserMaster.rows().data();
        $(_gridData).each(function () {
            if (this.EMAIL_ID.toUpperCase() == model.EMAIL_ID.toUpperCase())
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/UserMaster/SaveUser",
                contentType: false,
                processData: false,
                data: formdata,
                success: function (response) {
                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Save Successfully");
                        LoadUserMasterGrid();
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
        }
        else {
            PopupMessage("Warning", "Same Email Id And Mobile No Already Exists!");
            return false;
        }
    });
}
function RegBtnUpdateEvt() {
    $("#btnUpdate").click(function () {
        debugger
        var UM_ID = ($(this).attr("data-id"));
        var model = GetClientModel();
        model.UM_ID = UM_ID;
        var formdata = new FormData();
        var Save_Profile = {};
        var File_Profile = $("#fileLogo").get(0);
        Save_Profile.files = File_Profile.files;
        Save_Profile.filePath = File_Profile.value;
        Save_Profile.allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;

        for (var i = 0; i < Save_Profile.files.length; i++) {
            formdata.append(Save_Profile.files[i].name, Save_Profile.files[i]);
        }

        if (model.FIRST_NAME == "" || model.FIRST_NAME == null) {
            PopupMessage("Warning", "Please Enter First Name");
            return false;
        }
        else if (model.LAST_NAME == "" || model.LAST_NAME == null) {
            PopupMessage("Warning", "Please Enter Last Name");
            return false;
        }
        else if (model.RM_ID == "" || model.RM_ID == null) {
            PopupMessage("Warning", "Please Select Role Name");
            return false;
        }
        else if (model.EMAIL_ID == "" || model.EMAIL_ID == null) {
            PopupMessage("Warning", "Please Enter Email Id");
            return false;
        }
        else if (model.MOBILE_NO == "" || model.MOBILE_NO == null) {
            PopupMessage("Warning", "Please Enter Mobile No");
            return false;
        }
        formdata.append('UM_ID', model.UM_ID); formdata.append('RM_ID', model.RM_ID); formdata.append('FIRST_NAME', model.FIRST_NAME);
        formdata.append('LAST_NAME', model.LAST_NAME); formdata.append('MOBILE_NO', model.MOBILE_NO);
        formdata.append('EMAIL_ID', model.EMAIL_ID); formdata.append('PASSWORD', model.PASSWORD); formdata.append('IS_ACTIVE', model.IS_ACTIVE);
        formdata.append('Profile_User_Img', model.Profile_User_Img);   
        var IsExists = false;
        var _gridData = GridUserMaster.rows().data();
        var _gridData = GridUserMaster.rows().data();
        $(_gridData).each(function () {
            if (this.EMAIL_ID.toUpperCase() == model.EMAIL_ID.toUpperCase() && this.FIRST_NAME.toUpperCase() == model.FIRST_NAME.toUpperCase() && this.LAST_NAME.toUpperCase() == model.LAST_NAME.toUpperCase() && this.RM_ID == model.RM_ID && this.MOBILE_NO == model.MOBILE_NO)
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/UserMaster/UpdateUser",
                contentType: false,
                processData: false,
                data: formdata,
                success: function (response) {
                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Update Successfully");
                        LoadUserMasterGrid();
                        Reset();
                    }
                    else {
                        PopupMessage("Error", "Data Not Updated");
                        return false;
                    }
                },
                error: function (response) {
                    PopupMessage("Error", "Data Not Updated");
                }
            });
        }
        else {
            PopupMessage("Warning", "Same Email Id And Mobile No Already Exists!");
            return false;
        }
    });
}
function EditUserMaster(data) {
    debugger
    $("#btnUpdate").attr("data-id", data.UM_ID);
    $("#ddlRoleName").val(data.RM_ID);
    $("#txtFirstName").val(data.FIRST_NAME);
    $("#txtLastName").val(data.LAST_NAME);
    $("#txtMobileNo").val(data.MOBILE_NO);
    $("#txtEmailID").val(data.EMAIL_ID);
    $("#txtpassword").val(data.PASSWORD);
    $("#txtfileLogo").val(data.PROFILE_PHOTO),
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    $("#hdnpassword").hide();
    $("#btnUpdate").show();
    $("#btnSubmit").hide();
}
function Reset() {
    $("#ddlRoleName").val("");
    $("#txtFirstName").val("");
    $("#txtLastName").val("");
    $("#txtMobileNo").val("");
    $("#txtEmailID").val("");
    $("#txtpassword").val("");
    $("#txtfileLogo").val("");
    $("#fileLogo").val("");
    $("#btnSubmit").show();
    $("#btnUpdate").hide();
    $("#hdnpassword").show();
}
function RegbtnResetEvt() {
    $("#btnReset").click(function () {
        Reset();
    })
};
function DeleteUserMaster(UM_ID) {
    var model = GetClientModel();
    model.UM_ID = UM_ID;
    $.ajax({
        type: "POST",
        url: "/UserMaster/Delete",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response == "SUCCESS") {
                PopupMessage("Success", "Data Delete Successfully");
                LoadUserMasterGrid();
            }
            else {
                PopupMessage("Error", "Data Not Delete");
                return false;
            }
        },
        error: function (response) {
        }
    });
}
function validateEmail(emailField) {
    debugger
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (reg.test(emailField.value) == false) {
        PopupMessage("Warning", "Invalid Email Address");
        $("#txtEmailID").val("");
        return false;
    }
    return true;
}