var GridUserMaster = null;
$(function () {
    $(document).ready(function () {
       

        $("#txtDateOfBirth").datepicker();
        LoadUserMasterGrid();
        validateNumbers();
        addUsers();
    })

});

function validateNumbers() {
    $('#txtPinCode, #txtContactNo, #txtAlternateContactNo').keypress(function (e) {
        var charCode = (e.which) ? e.which : event.keyCode
        if (String.fromCharCode(charCode).match(/[^0-9]/g))
            return false;
    });
}

function ValidateUserInputs()
{
    if ($("#txtFirstName").val() == "") {
        $("#divFirstName").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divFirstName").hide().val(''));
    }

    if ($("#txtLastName").val() == "") {
        $("#divLastName").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divLastName").hide().val(''));
    }

    if ($("#txtEmailId").val() == "") {
        $("#divEmailId").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divEmailId").hide().val(''));
    }

    if ($("#ddlGender").val() == "0") {
        $("#divGender").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divGender").hide().val(''));
    }

    if ($("#txtContactNo").val() == "") {
        $("#divContactNo").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divContactNo").hide().val(''));
    }

    if ($("#ddlCountry").val() < 1) {
        $("#divCountry").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divCountry").hide().val(''));
    }

    if ($("#ddlState").val() < 1) {
        $("#divState").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divState").hide().val(''));
    }

    if ($("#txtDateOfBirth").val() < 1) {
        $("#divDateOfBirth").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divDateOfBirth").hide().val(''));
    }

    if ($("#ddlUserType").val() < 1) {
        $("#divUserType").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divUserType").hide().val(''));
    }


    if ($("#txtUsername").val() =="") {
        $("#divUsername").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divUsername").hide().val(''));
    }

    if ($("#txtPassword").val() == "") {
        $("#divPassword").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divPassword").hide().val(''));
    }

    if ($("#txtConfirmPassword").val() == "") {
        $("#divConfirmPassword").show().val('This field is required.');
        return false;
    }
    else {
        ($("#divConfirmPassword").hide().val(''));
    }

}

function USER_MASTER_MODEL() {
    var model =
      {
          "USER_ID": 0,
          "USER_CODE": "",
          "FIRST_NAME": $("#txtFirstName").val(),
          "LAST_NAME": $("#txtLastName").val(),
          "EMAIL_ID": $("#txtEmailId").val(),
          "GENDER": $("#ddlGender").val(),
          "STREET": $("#txtStreet").val(),
          "LANDMARK": $("#txtLandmark").val(),
          "CITY": $("#txtCity").val(),
          "DISTRICT": $("#txtDistrict").val(),
          "PINCODE": $("#txtPinCode").val(),
          "STATE_ID": $("#ddlState").val(),
          "COUNTRY_ID": $("#ddlCountry").val(),
          "DATE_OF_BIRTH": $("#txtDateOfBirth").val(),
          "CONTACT_NO": $("#txtContactNo").val(),
          "ALTERNATE_CONTACT_NO": $("#txtAlternateContactNo").val(),
          "WEBSITE": $("#txtWebsite").val(),
          "USERTYPE_ID": $("#ddlUserType").val(),
          "USERNAME": $("#txtUsername").val(),
          "PASSWORD": $("#txtPassword").val()
      }
    return model;
}

function addUsers() {
    $('#btnSubmit').click(function () {
        var spinner = $('#loader');
        spinner.show();

        var formData = new FormData();
        var Save_Logo = {};
        var File_Logo = $("#fileUserImage").get(0);
        Save_Logo.files = File_Logo.files;
        Save_Logo.filePath = File_Logo.value;
        Save_Logo.allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
        for (var i = 0; i < Save_Logo.files.length; i++) {
            formData.append(Save_Logo.files[i].name, Save_Logo.files[i]);
        }
        var _USER_MASTER = USER_MASTER_MODEL();

        formData.append('USER_ID', _USER_MASTER.CR_ID);
        formData.append('USER_CODE', _USER_MASTER.USER_CODE);
        formData.append('FIRST_NAME', _USER_MASTER.FIRST_NAME);
        formData.append('LAST_NAME', _USER_MASTER.LAST_NAME);
        formData.append('EMAIL_ID', _USER_MASTER.EMAIL_ID);
        formData.append('GENDER', _USER_MASTER.GENDER);
        formData.append('STREET', _USER_MASTER.STREET);
        formData.append('LANDMARK', _USER_MASTER.LANDMARK);
        formData.append('CITY', _USER_MASTER.CITY);
        formData.append('DISTRICT', _USER_MASTER.DISTRICT);
        formData.append('PINCODE', _USER_MASTER.PINCODE);
        formData.append('STATE_ID', _USER_MASTER.STATE_ID);
        formData.append('COUNTRY_ID', _USER_MASTER.COUNTRY_ID);
        formData.append('DATE_OF_BIRTH', _USER_MASTER.DATE_OF_BIRTH);
        formData.append('CONTACT_NO', _USER_MASTER.CONTACT_NO);
        formData.append('ALTERNATE_CONTACT_NO', _USER_MASTER.ALTERNATE_CONTACT_NO);
        formData.append('WEBSITE', _USER_MASTER.WEBSITE);
        formData.append('USERTYPE_ID', _USER_MASTER.USERTYPE_ID);
        formData.append('USERNAME', _USER_MASTER.USERNAME);
        formData.append('PASSWORD', _USER_MASTER.PASSWORD);

        $.ajax({
            type: "POST",
            url: "/User/Create",
            contentType: false,
            processData: false,
            data: formData,
            success: function (response) {
                if (response == "SUCCESS") {
                    PopupMessage("Success", "User successfully added.");
                    LoadUserMasterGrid();
                    spinner.hide();
                    Reset();
                }
                else {
                    PopupMessage("Error", "User not added.");
                    spinner.hide();
                    return false;
                }
            },
            error: function (response) {
                PopupMessage("Error", "User not added.");
                spinner.hide();
            }
        });
    });
}


function LoadUserMasterGrid() {
    var spinner = $('#loader');
    spinner.show();
    $.ajax({
        type: "GET",
        url: "/User/LstOfUsers",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            BindUserMasterDataTable(response);
            spinner.hide();
        },
        error: function (response) {
            spinner.hide();
        }
    });
}



function BindUserMasterDataTable(data) {
    $.noConflict();
    if (GridUserMaster != null) {
        GridUserMaster.destroy();
    }
    GridUserMaster = $('#tblUserRegistration').DataTable({
        data: data,
        scrollX: true,
        columns: [
            { title: "SL NO.", width: 20, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; } },
            { title: "FIRST_NAME", data: "FIRST_NAME" },
            { title: "LAST_NAME", data: "LAST_NAME" },
            { title: "EMAIL_ID", data: "EMAIL_ID" },
            { title: "CONTACT_NO", data: "CONTACT_NO" },
            { title: "IMEI_NUMBER", data: "IMEI_NUMBER" },
            { title: "CITY", data: "CITY" },
            { title: "CREATED_BY", data: "CREATED_BY" },
            { title: "CREATED_ON", data: "CREATED_ON" },
            {
                title: "Action", render: function (data, type, row, meta) {
                    return "<span id='edit'><span title='Edit' class='glyphicon glyphicon-edit' aria-hidden='true' style='cursor:pointer'></span></span>&nbsp;<span id='delete'><span title='Remove' class='glyphicon glyphicon-trash' aria-hidden='true' style='cursor:pointer'></span></span>";
                }
            },
        ],
    });

    $('#tblUserRegistration tbody').off('click');
    $('#tblUserRegistration tbody').on('click', '#delete', function () {
       
        _RowSelectedForDelete = $(this).parents('tr');
        var data = GridUserMaster.row(_RowSelectedForDelete).data();
        ConfirmMessage("Are you sure want to delete ?", "DeleteUserMasterFormData(" + data.USER_ID + ")");
    });
    $('#tblUserRegistration tbody').on('click', '#edit', function () {
        _RowSelectedForEdit = $(this).parents('tr');
        var data = GridUserMaster.row(_RowSelectedForEdit).data();
        EditUserMasterFormData(data);
    });
}

function EditUserMasterFormData(data) {
    var USER_ID = data.USER_ID;
    var spinner = $('#loader');
    spinner.show();
    $.ajax({
        type: "GET",
        url: "/User/GetUserData?USER_ID=" + USER_ID,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            BindUserMasterDataOnEdit(response);
            spinner.hide();
        },
        error: function (response) {
            spinner.hide();
        }
    });


}

function BindUserMasterDataOnEdit(data) {
    $("#hdnUserId").val(data.USER_ID);
    $("#txtFirstName").val(data[0].FIRST_NAME);
    $("#txtLastName").val(data[0].LAST_NAME);
    $("#txtEmailId").val(data[0].EMAIL_ID);
    $("#ddlGender").val(data[0].GENDER);
    $("#txtStreet").val(data[0].STREET);
    $("#txtLandmark").val(data[0].LANDMARK);
    $("#txtCity").val(data[0].CITY);
    $("#txtDistrict").val(data[0].DISTRICT);
    $("#txtPinCode").val(data[0].PINCODE);
    $("#ddlCountry").val(data[0].COUNTRY_ID);
    $("#ddlState").val(data[0].STATE_ID);
    $("#txtDateOfBirth").val(data[0].DATE_OF_BIRTH);
    $("#txtContactNo").val(data[0].CONTACT_NO);
    $("#txtAlternateContactNo").val(data[0].ALTERNATE_CONTACT_NO);
    $("#txtWebsite").val(data[0].WEBSITE);
    // $("#fileUserImage").val(data.FIRST_NAME);
    $("#ddlUserType").val(data[0].USERTYPE_ID);
    $("#txtUsername").val(data[0].USERNAME);
    // $("#txtPassword").val(data.PASSWORD);
    // $("#txtConfirmPassword").val(data.PASSWORD);

    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    $("#btnUpdate").show();
    $("#btnSubmit").hide(); $("#ddlSubModuleName").val(data.SMM_ID);
}



function DeleteUserMasterFormData(USER_ID) {
    var USER_MASTER = USER_MASTER_MODEL();
    USER_MASTER.USER_ID = USER_ID;
    var spinner = $('#loader');
    spinner.show();
    $.ajax({
        type: "POST",
        url: "/User/Delete",
        data: JSON.stringify(USER_MASTER),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response == "SUCCESS") {
                PopupMessage("Success", "Data deleted successfully.");
                LoadUserMasterGrid();
                spinner.hide();
            }
            else {
                PopupMessage("Error", "Data not deleted.");
                spinner.hide();
                return false;
            }
        },
        error: function (response) {
            PopupMessage("Error", "Data not deleted.");
            spinner.hide();
        }
    });
}
