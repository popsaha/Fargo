var GridSubModule = null;
$(function () {
    $(document).ready(function () {
        LoadSubModuleGrid();
        RegBtnSubmitEvt();
        RegBtnUpdateEvt();
        RegBtnResetEvt();
    })
});

function GetSubModuleMaster() {
    var model = {
        "SMM_ID": 0,
        "MM_ID": $("#ddlModuleName").val(),
        "SUB_MODULE_NAME": $.trim($("#txtSubModuleName").val()),
        "IS_ACTIVE": true,
    }
    return model;
}
function RegBtnSubmitEvt() {
    $("#btnSubmit").click(function () {
        var spinner = $('#loader');
        spinner.show();
        var model = GetSubModuleMaster();
        if (model.MM_ID == "" || model.MM_ID == null) {
            PopupMessage("Warning", "Please Select Module Name");
            spinner.hide();
            return false;
        }
        else if (model.SUB_MODULE_NAME == "" || model.SUB_MODULE_NAME == undefined) {
            PopupMessage("Warning", "Please Enter Sub Module Name");
            spinner.hide();
            return false;
        }
        var IsExists = false;
        var _gridData = GridSubModule.rows().data();
        $(_gridData).each(function () {
            if (this.SUB_MODULE_NAME.toUpperCase() == model.SUB_MODULE_NAME.toUpperCase() && this.MM_ID == model.MM_ID)
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/SubModuleMaster/SBMTSubModule",
                data: JSON.stringify(model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Save Successfully");
                        LoadSubModuleGrid();
                        Reset();
                        spinner.hide();
                    }
                    else {
                        PopupMessage("Error", "Data Not Saved");
                        spinner.hide();
                        return false;
                    }
                },
                error: function (error) {
                    PopupMessage("Error", "Data Not Saved");
                    spinner.hide();
                }
            })
        }
        else {
            PopupMessage("Warning", "Same Sub Module Name Already Exists!");
            spinner.hide();
            return false;
        }
       
    })
}

function LoadSubModuleGrid() {
    debugger
    var spinner = $('#loader');
    spinner.show();
    $.ajax({
        type: "GET",
        url: "/SubModuleMaster/GetSubModuleList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            BindSubModuleDataTable(response);
            spinner.hide();
        },
        error: function (response) {
            spinner.hide();
        }
    });
}

function BindSubModuleDataTable(data) {
    if (GridSubModule != null) {
        GridSubModule.destroy();
    }
    GridSubModule = $('#SubModuleMasterGrid').DataTable({
        data: data,
        scrollX: true,
        columns: [
            { title: "Sr.No.", width: 20, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; } },
            { title: "Module Name", data: "MODULE_NAME" },
            { title: "Sub Module Name", data: "SUB_MODULE_NAME" },
            { title: "Is Active", data: "IS_ACTIVE" },
            { title: "Created By", data: "CREATED_BY" },
            { title: "Created On", data: "CREATED_ON" },
            {
                title: "Action", render: function (data, type, row, meta) {
                    return "<span id='edit'><span title='Edit' class='glyphicon glyphicon-edit' aria-hidden='true' style='cursor:pointer'></span></span>&nbsp;<span id='delete'><span title='Remove' class='glyphicon glyphicon-trash' aria-hidden='true' style='cursor:pointer'></span></span>";
                }
            },
        ],
    });
    $('#SubModuleMasterGrid tbody').off('click');
    $('#SubModuleMasterGrid tbody').on('click', '#delete', function () {
        _RowSelectedForDelete = $(this).parents('tr');
        var data = GridSubModule.row(_RowSelectedForDelete).data();
        ConfirmMessage("Are you sure want to delete role", "DeleteSubModuleById(" + data.SMM_ID + ")");
    });
    $('#SubModuleMasterGrid tbody').on('click', '#edit', function () {
        _RowSelectedForEdit = $(this).parents('tr');
        var data = GridSubModule.row(_RowSelectedForEdit).data();
        EditSubModule(data);
    });
}

function EditSubModule(data) {
    $("#btnUpdate").attr("data-id", data.SMM_ID);
    $("#ddlModuleName").val(data.MM_ID);
    $("#txtSubModuleName").val(data.SUB_MODULE_NAME);
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    $("#btnUpdate").show();
    $("#btnSubmit").hide();
}

function Reset() {
    $("#ddlModuleName").val("");
    $("#txtSubModuleName").val("");
    $("#btnSubmit").show();
    $("#btnUpdate").hide();
}

function RegBtnUpdateEvt() {
    $("#btnUpdate").click(function () {
        debugger
        var spinner = $('#loader');
        spinner.show();
        var model = GetSubModuleMaster();
        model.SMM_ID = ($(this).attr("data-id"));

        if (model.MM_ID == "" || model.MM_ID == null) {
            PopupMessage("Warning", "Please Select Module Name");
            spinner.hide();
            return false;
        }
        else if (model.SUB_MODULE_NAME == "" || model.SUB_MODULE_NAME == undefined) {
            PopupMessage("Warning", "Please Enter Sub Module Name");
            spinner.hide();
            return false;
        }
        var IsExists = false;
        var _gridData = GridSubModule.rows().data();
        $(_gridData).each(function () {
            if (this.SUB_MODULE_NAME.toUpperCase() == model.SUB_MODULE_NAME.toUpperCase() && this.MM_ID == model.MM_ID)
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/SubModuleMaster/UPDATESubModule",
                data: JSON.stringify(model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Update Successfully");
                        LoadSubModuleGrid();
                        Reset();
                        spinner.hide();
                    }
                    else {
                        PopupMessage("Error", "Data Not Updated");
                        spinner.hide();
                        return false;
                    }
                },
                error: function (response) {
                    PopupMessage("Error", "Data Not Updated");
                    spinner.hide();
                }
            });
        }
        else {
            PopupMessage("Warning", "Same Sub Module Name Already Exists!");
            spinner.hide();
            return false;
        }
    });
}

function DeleteSubModuleById(SMM_ID) {
    var spinner = $('#loader');
    spinner.show();
    var model = GetSubModuleMaster();
    model.SMM_ID = SMM_ID;
    $.ajax({
        type: "POST",
        url: "/SubModuleMaster/Delete",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response == "SUCCESS") {
                PopupMessage("Success", "Data Delete Successfully");
                LoadSubModuleGrid();
                spinner.hide();
            }
            else {
                PopupMessage("Error", "Data Not Deleted");
                spinner.hide();
                return false;
            }
        },
        error: function (response) {
            PopupMessage("Error", "Data Not Deleted");
            spinner.hide();
        }
    });
}

function RegBtnResetEvt() {
    $("#btnReset").click(function () {
        Reset();
    });
}