var GridClientRegistration = null;
$(function () {
    $(document).ready(function () {
        LoadClientRegisterGrid();
        RegBtnSubmitEvt();
        RegbtnResetEvt();
        RegBtnUpdateEvt();
        RegtxtGSTNOEvt();
        RegtxtPAN_NOEvt();
        RegChkSameAddressEvt();
    });
});

function GetClientModel() {
    var model = {
        "CR_ID": 0,
        "ORGNIZATIO_NAME": $.trim($("#txtOrgnization_Name").val()),
        "CONTACT_PERSON_NAME": $.trim($("#txtContactPersonName").val()),
        "PRIMARY_CONTACT_NO": $.trim($("#txtPrimaryContactNo").val()),
        "SECONDARY_CONTACT_NO": $.trim($("#txtSecondaryContactNo").val()) == "" ? 0 : $.trim($("#txtSecondaryContactNo").val()),
        "PRIMARY_EMAIL": $.trim($("#txtPrimaryEmailID").val()),
        "SECONDARY_EMAIL": $.trim($("#txtSecondaryEmail").val()),
        "GST_NO": $.trim($("#txtGSTNO").val()),
        "PAN_NO": $.trim($("#txtPanNo").val()),
        "STATE_ID": $("#ddlStateName").val(),
        "PRIMARY_ADDRESS": $.trim($("#txtPrimaryAddress").val()),
        "TEMPORARY": $.trim($("#txtTemporaryAddress").val()),
        "CORPORATE_ID": $.trim($("#txtCorporatedId").val()),
        "CLIENT_APPLICATION_URL": $.trim($("#txtCorporatedURL").val()),
        "IS_ACTIVE": true,
        "ClientLogo": $.trim($("#txtClientLogo").val()),
    }
    return model;
}

function LoadClientRegisterGrid() {
    $.ajax({
        type: "GET",
        url: "/ClientRegistration/GetClientRegisterList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            BindClientDataTable(response);
        },
        error: function (response) {
        }
    });
}

function BindClientDataTable(data) {
    if (GridClientRegistration != null) {
        GridClientRegistration.destroy();
    }
    GridClientRegistration = $('#ClientRegistrtionGrid').DataTable({
        data: data,
        scrollX: true,
        columns: [
            { title: "Sr.No.", width: 20, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; } },
            { title: "Orgnization Name", data: "ORGNIZATIO_NAME" },
            { title: "Proprietor Name", data: "CONTACT_PERSON_NAME" },
            { title: "Primary Contact No", data: "PRIMARY_CONTACT_NO" },
            { title: "Primary Email Id", data: "PRIMARY_EMAIL" },
            { title: "GST No", data: "GST_NO" },
            { title: "State Name", data: "STATE_NAME" },
            {
                title: "Logo", data: "ORGNIZATION_LOGO", render: function (data, type, row, meta) {
                    return '<img src="/UploadImg/' + row.ORGNIZATION_LOGO + '" width="30px" height="30px" />';
                }
            },
            { title: "CorporatedId", data: "STATE_NAME" },
            { title: "State Name", data: "STATE_NAME" },
            { title: "Is Active", data: "IS_ACTIVE" },
            { title: "Created By", data: "CREATED_BY" },
            { title: "CorporatedId", data: "CORPORATE_ID" },
            { title: "Orgnization App URL", data: "CLIENT_APPLICATION_URL" },
            {
                title: "Action", render: function (data, type, row, meta) {
                    return "<span id='edit'><span title='Edit' class='glyphicon glyphicon-edit' aria-hidden='true' style='cursor:pointer'></span></span>&nbsp;<span id='delete'><span title='Remove' class='glyphicon glyphicon-trash' aria-hidden='true' style='cursor:pointer'></span></span>";
                }
            },
        ],
    });
    $('#ClientRegistrtionGrid tbody').off('click');
    $('#ClientRegistrtionGrid tbody').on('click', '#delete', function () {
        _RowSelectedForDelete = $(this).parents('tr');
        var data = GridClientRegistration.row(_RowSelectedForDelete).data();
        ConfirmMessage("Are you sure want to delete client", "DeleteClient(" + data.CR_ID + ")");
    });
    $('#ClientRegistrtionGrid tbody').on('click', '#edit', function () {
        _RowSelectedForEdit = $(this).parents('tr');
        var data = GridClientRegistration.row(_RowSelectedForEdit).data();
        EditClient(data);
    });
}

function RegBtnSubmitEvt() {
    $("#btnSubmit").click(function () {
        debugger
        var formdata = new FormData();
        var Save_Logo = {};
        var File_Logo = $("#fileLogo").get(0);
        Save_Logo.files = File_Logo.files;
        Save_Logo.filePath = File_Logo.value;
        Save_Logo.allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
        for (var i = 0; i < Save_Logo.files.length; i++) {
            formdata.append(Save_Logo.files[i].name, Save_Logo.files[i]);
        }
        var model = GetClientModel();

        if (model.ORGNIZATIO_NAME == "" || model.ORGNIZATIO_NAME == null) {
            PopupMessage("Warning", "Please Enter Orgnization Name");
            return false;
        }
        else if (model.CONTACT_PERSON_NAME == "" || model.CONTACT_PERSON_NAME == null) {
            PopupMessage("Warning", "Please Enter Contact Person Name");
            return false;
        }
        else if (model.PRIMARY_CONTACT_NO == "" || model.PRIMARY_CONTACT_NO == null) {
            PopupMessage("Warning", "Please Enter Primary Contact No");
            return false;
        }
        else if (model.STATE_ID == "" || model.STATE_ID == null) {
            PopupMessage("Warning", "Please Select State Name");
            return false;
        }
        else if (model.CORPORATE_ID == "" || model.CORPORATE_ID == null) {
            PopupMessage("Warning", "Please Enter Corporated Id");
            return false;
        }
        else if (model.CLIENT_APPLICATION_URL == "" || model.CLIENT_APPLICATION_URL == null) {
            PopupMessage("Warning", "Please Orgnization URL");
            return false;
        }
        formdata.append('CR_ID', model.CR_ID); formdata.append('ORGNIZATIO_NAME', model.ORGNIZATIO_NAME); formdata.append('CONTACT_PERSON_NAME', model.CONTACT_PERSON_NAME);
        formdata.append('PRIMARY_CONTACT_NO', model.PRIMARY_CONTACT_NO); formdata.append('SECONDARY_CONTACT_NO', model.SECONDARY_CONTACT_NO);
        formdata.append('PRIMARY_EMAIL', model.PRIMARY_EMAIL); formdata.append('SECONDARY_EMAIL', model.SECONDARY_EMAIL);
        formdata.append('GST_NO', model.GST_NO); formdata.append('PAN_NO', model.PAN_NO); formdata.append('STATE_ID', model.STATE_ID);
        formdata.append('PRIMARY_ADDRESS', model.PRIMARY_ADDRESS); formdata.append('TEMPORARY', model.TEMPORARY);
        formdata.append('CORPORATE_ID', model.CORPORATE_ID); formdata.append('CLIENT_APPLICATION_URL', model.CLIENT_APPLICATION_URL); formdata.append('IS_ACTIVE', model.IS_ACTIVE);
      
        var IsExists = false;
        var _gridData = GridClientRegistration.rows().data();
        $(_gridData).each(function () {
            if (this.CORPORATE_ID.toUpperCase() == model.CORPORATE_ID.toUpperCase() && this.CLIENT_APPLICATION_URL.toUpperCase() == model.CLIENT_APPLICATION_URL.toUpperCase())
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/ClientRegistration/Create",
                contentType: false,
                processData: false,
                data: formdata,
                success: function (response) {
                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Save Successfully");
                        LoadClientRegisterGrid();
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
    });
}

function RegBtnUpdateEvt() {
    $("#btnUpdate").click(function () {
        debugger
        var CR_ID = ($(this).attr("data-id"));
        var model = GetClientModel();
        model.CR_ID = CR_ID;

        var formdata = new FormData();
        var Save_Logo = {};
        var File_Logo = $("#fileLogo").get(0);
        Save_Logo.files = File_Logo.files;
        Save_Logo.filePath = File_Logo.value;
        Save_Logo.allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
        for (var i = 0; i < Save_Logo.files.length; i++) {
            formdata.append(Save_Logo.files[i].name, Save_Logo.files[i]);
        }
        if (model.ORGNIZATIO_NAME == "" || model.ORGNIZATIO_NAME == null) {
            PopupMessage("Warning", "Please Enter Orgnization Name");
            return false;
        }
        else if (model.CONTACT_PERSON_NAME == "" || model.CONTACT_PERSON_NAME == null) {
            PopupMessage("Warning", "Please Enter Contact Person Name");
            return false;
        }
        else if (model.PRIMARY_CONTACT_NO == "" || model.PRIMARY_CONTACT_NO == null) {
            PopupMessage("Warning", "Please Enter Primary Contact No");
            return false;
        }
        else if (model.STATE_ID == "" || model.STATE_ID == null) {
            PopupMessage("Warning", "Please Select State Name");
            return false;
        }
        else if (model.CORPORATE_ID == "" || model.CORPORATE_ID == null) {
            PopupMessage("Warning", "Please Enter Corporated Id");
            return false;
        }
        else if (model.CLIENT_APPLICATION_URL == "" || model.CLIENT_APPLICATION_URL == null) {
            PopupMessage("Warning", "Please Orgnization URL");
            return false;
        }
        formdata.append('CR_ID', model.CR_ID); formdata.append('ORGNIZATIO_NAME', model.ORGNIZATIO_NAME); formdata.append('CONTACT_PERSON_NAME', model.CONTACT_PERSON_NAME);
        formdata.append('PRIMARY_CONTACT_NO', model.PRIMARY_CONTACT_NO); formdata.append('SECONDARY_CONTACT_NO', model.SECONDARY_CONTACT_NO);
        formdata.append('PRIMARY_EMAIL', model.PRIMARY_EMAIL); formdata.append('SECONDARY_EMAIL', model.SECONDARY_EMAIL);
        formdata.append('GST_NO', model.GST_NO); formdata.append('PAN_NO', model.PAN_NO); formdata.append('STATE_ID', model.STATE_ID);
        formdata.append('PRIMARY_ADDRESS', model.PRIMARY_ADDRESS); formdata.append('TEMPORARY', model.TEMPORARY);
        formdata.append('CORPORATE_ID', model.CORPORATE_ID); formdata.append('CLIENT_APPLICATION_URL', model.CLIENT_APPLICATION_URL); formdata.append('IS_ACTIVE', model.IS_ACTIVE);
        formdata.append('ClientLogo', model.ClientLogo);
        var IsExists = false;
        var _gridData = GridClientRegistration.rows().data();
        $(_gridData).each(function () {
            if (this.CR_ID == model.CR_ID && this.CORPORATE_ID.toUpperCase() == model.CORPORATE_ID.toUpperCase() && this.CLIENT_APPLICATION_URL.toUpperCase() == model.CLIENT_APPLICATION_URL.toUpperCase())
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/ClientRegistration/Edit",
                contentType: false,
                processData: false,
                data: formdata,
                success: function (response) {
                    debugger
                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Update Successfully");
                        LoadClientRegisterGrid();
                        Reset();
                    }
                    else {
                        PopupMessage("Error", "Data Not Updated");
                    }
                },
                error: function (response) {
                    PopupMessage("Error", "Data Not Updated");
                }
            });
        }
        else {
            PopupMessage("Warning", "Same Corporated Id And Orgnization URL Already Exists!");
            return false;
        }
    });
}

function EditClient(data) {
    debugger
    $("#btnUpdate").attr("data-id", data.CR_ID);
    $("#txtRoleName").val(data.ROLE_NAME);
    $("#txtOrgnization_Name").val(data.ORGNIZATIO_NAME);
    $("#txtContactPersonName").val(data.CONTACT_PERSON_NAME);
    $("#txtPrimaryContactNo").val(data.PRIMARY_CONTACT_NO);
    $("#txtSecondaryContactNo").val(data.SECONDARY_CONTACT_NO);
    $("#txtPrimaryEmailID").val(data.PRIMARY_EMAIL);
    $("#txtSecondaryEmail").val(data.SECONDARY_EMAIL);
    $("#txtGSTNO").val(data.GST_NO);
    $("#txtPanNo").val(data.PAN_NO);
    $("#ddlStateName").val(data.STATE_ID);
    $("#txtPrimaryAddress").val(data.PRIMARY_ADDRESS);
    $("#txtTemporaryAddress").val(data.TEMPORARY);
    $("#txtCorporatedId").val(data.CORPORATE_ID);
    $("#txtCorporatedURL").val(data.CLIENT_APPLICATION_URL);
    $("#txtClientLogo").val(data.ORGNIZATION_LOGO);
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    $("#btnUpdate").show();
    $("#btnSubmit").hide();
}

function Reset() {
    $("#txtOrgnization_Name").val("");
    $("#txtContactPersonName").val("");
    $("#txtPrimaryContactNo").val("");
    $("#txtSecondaryContactNo").val("");
    $("#txtPrimaryEmailID").val("");
    $("#txtSecondaryEmail").val("");
    $("#txtGSTNO").val("");
    $("#txtPanNo").val("");
    $("#txtPrimaryAddress").val("");
    $("#txtTemporaryAddress").val("");
    $("#txtCorporatedId").val("");
    $("#txtCorporatedURL").val("");
    $("#ddlStateName").val("");
    $("#fileLogo").val("");
    $("#btnSubmit").show();
    $("#btnUpdate").hide();
}

function RegbtnResetEvt() {
    $("#btnReset").click(function () {
        Reset();
    })
};

function DeleteClient(CR_ID) {
    debugger
    var model = GetClientModel();
    model.CR_ID = CR_ID;
    $.ajax({
        type: "POST",
        url: "/ClientRegistration/Delete",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response == "SUCCESS") {
                PopupMessage("Success", "Data Delete Successfully");
                LoadClientRegisterGrid();
            }
            else {
                PopupMessage("Error", "Data Not Deleted");
                return false;
            }
        },
        error: function (response) {
            PopupMessage("Error", "Data Not Deleted");
        }
    });
}

function validatePrimaryEmail(emailField) {
    debugger
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (reg.test(emailField.value) == false) {
        PopupMessage("Warning", "Invalid Email Address");
        $("#txtPrimaryEmailID").val("");
        return false;
    }
    return true;
}

function validateSecondaryEmail(emailField) {
    debugger
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    if (reg.test(emailField.value) == false) {
        PopupMessage("Warning", "Invalid Email Address");
        $("#txtSecondaryEmail").val("");
        return false;
    }
    return true;
}

function RegtxtGSTNOEvt() {
    $("#txtGSTNO").change(function () {
        debugger
        var inputvalues = $(this).val();
        var gstinformat = new RegExp('^([0][1-9]|[1-2][0-9]|[3][0-7])([a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}[1-9a-zA-Z]{1}[zZ]{1}[0-9a-zA-Z]{1})+$');
        if (gstinformat.test(inputvalues)) {
            return true;
        } else {
            PopupMessage("Warning", "Please Enter Valid GST Number");
            $("#txtGSTNO").val('');
        }
    });
}

function RegtxtPAN_NOEvt() {
    $("#txtPanNo").change(function () {
        debugger
        var inputvalues = $(this).val();
        var panformat = new RegExp('^[A-Za-z]{5}[0-9]{4}[A-Za-z]{1}');
        if (panformat.test(inputvalues)) {
            return true;
        } else {
            PopupMessage("Warning", "Please Enter Valid Pan Number");
            $("#txtPanNo").val('');
        }
    });
}

function RegChkSameAddressEvt() {
    $("#chkFillTempAddress").on('change', function () {
        debugger
        var PrimaryAddress = $('#txtPrimaryAddress').val();
        if ($(this).is(':checked')) {
            if (PrimaryAddress == undefined || PrimaryAddress == "") {
                PopupMessage("Warning", "Primary Address Is Empty!!! Please Enter Primary Address");
                $('input:checkbox:checked').prop('checked', false);
                return false;
            }
            else {
                $("#txtTemporaryAddress").val(PrimaryAddress);
            }
        }
        else {
            $("#txtTemporaryAddress").val("");
        }
    });
}


