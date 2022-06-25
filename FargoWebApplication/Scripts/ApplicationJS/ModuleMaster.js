var GridModulelMaster = null;
$(function () {
    LoadModuleMasterGrid();
    RegBtnSubmitEvt();
    RegBtnUpdateEvt();
    RegbtnResetEvt();
});
function GetModuleModel() {
    var model = {
        "MM_ID": 0,
        "TM_ID": $("#ddlTechnologyName").val(),
        "MODULE_NAME": $.trim($("#txtModuleName").val()),
        "MODULE_DESCRIPTION": $.trim($("#txtModuleDescription").val()),
        "IS_ACTIVE": true,
    }
    return model;
}
function LoadModuleMasterGrid() {
    var spinner = $('#loader');
    spinner.show();
    $.ajax({
        type: "GET",
        url: "/ModuleMaster/GetPackageList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            BindModuleDataTable(response);
            spinner.hide();
        },
        error: function (response) {
            spinner.hide();
        }
    });
}
function BindModuleDataTable(data) {
    debugger

    if (GridModulelMaster != null) {
        GridModulelMaster.destroy();
    }
    GridModulelMaster = $('#ModuleMasterGrid').DataTable({
        data: data,
        scrollX: true,
        columns: [
            { title: "Sr.No.", width: 20, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; } },
            { title: "Technology Name", data: "TECHNOLOGYNAME" },
            { title: "Module Name", data: "MODULE_NAME" },
            { title: "Module Desciption", data: "MODULE_DESCRIPTION" },
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
    $('#ModuleMasterGrid tbody').off('click');
    $('#ModuleMasterGrid tbody').on('click', '#delete', function () {
        _RowSelectedForDelete = $(this).parents('tr');
        var data = GridModulelMaster.row(_RowSelectedForDelete).data();
        ConfirmMessage("Are you sure want to delete module name", "DeleteModuleMaster(" + data.MM_ID + ")");
    });
    $('#ModuleMasterGrid tbody').on('click', '#edit', function () {
        _RowSelectedForEdit = $(this).parents('tr');
        var data = GridModulelMaster.row(_RowSelectedForEdit).data();
        EditModuleMaster(data);
    });

}
function RegBtnSubmitEvt() {
    $("#btnSubmit").click(function () {
        var spinner = $('#loader');
        spinner.show();
        var model = GetModuleModel();
        if (model.TM_ID == "" || model.TM_ID == null) {
            PopupMessage("Warning", "Please Select Technology Name");
            spinner.hide();
            return false;
        }
        else if (model.MODULE_NAME == "" || model.MODULE_NAME == null) {
            PopupMessage("Warning", "Please Enter Module Name");
            spinner.hide();
            return false;
        }
        var IsExists = false;
        var _gridData = GridModulelMaster.rows().data();
        $(_gridData).each(function () {
            if (this.MODULE_NAME.toUpperCase() == model.MODULE_NAME.toUpperCase() && this.TM_ID == model.TM_ID)
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/ModuleMaster/SaveModule",
                data: JSON.stringify(model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Save Successfully");
                        LoadModuleMasterGrid();
                        spinner.hide();
                        Reset();
                    }
                    else {
                        PopupMessage("Error", "Data Not Saved");
                        spinner.hide();
                        return false;
                    }
                },
                error: function (response) {
                    PopupMessage("Error", "Data Not Saved");
                    spinner.hide();
                }
            });
        }
        else {
            PopupMessage("Warning", "Same Module Name Already Exists!");
            spinner.hide();
            return false;
        }

    })
}
function RegBtnUpdateEvt() {
    $("#btnUpdate").click(function () {
        var spinner = $('#loader');
        spinner.show();
        var model = GetModuleModel();
        var MM_ID = ($(this).attr("data-id"));
        model.MM_ID = MM_ID;
        if (model.TM_ID == "" || model.TM_ID == null) {
            PopupMessage("Warning", "Please Select Technology Name");
            spinner.hide();
            return false;
        }
        else if (model.MODULE_NAME == "" || model.MODULE_NAME == null) {
            PopupMessage("Warning", "Please Enter Module Name");
            spinner.hide();
            return false;
        }
        var IsExists = false;
        var _gridData = GridModulelMaster.rows().data();
        $(_gridData).each(function () {
            if (this.MODULE_NAME.toUpperCase() == model.MODULE_NAME.toUpperCase() && this.TM_ID == model.TM_ID && this.MODULE_DESCRIPTION==model.MODULE_DESCRIPTION)
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/ModuleMaster/UpdateModule",
                data: JSON.stringify(model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Update Successfully");
                        Reset();
                        LoadModuleMasterGrid();
                        spinner.hide();
                    }
                    else {
                        PopupMessage("Error", "Data Not Updated");
                        return false;
                        spinner.hide();
                    }
                },
                error: function (response) {
                    PopupMessage("Error", "Data Not Updated");
                    spinner.hide();
                }
            });
        } else {
            PopupMessage("Warning", "Same Module Name Already Exists!");
            spinner.hide();
            return false;
        }

    })
}
function EditModuleMaster(data) {
    $("#btnUpdate").attr("data-id", data.MM_ID);
    $("#ddlTechnologyName").val(data.TM_ID);
    $("#txtModuleName").val(data.MODULE_NAME);
    $("#txtModuleDescription").val(data.MODULE_DESCRIPTION);
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    $("#btnUpdate").show();
    $("#btnSubmit").hide();
}
function RegbtnResetEvt() {
    $("#btnReset").click(function () {
        Reset();
    })
};
function Reset() {
    $("#ddlTechnologyName").val(""); $("#txtModuleName").val(""); $("#txtModuleDescription").val("");
    $("#btnSubmit").show();
    $("#btnUpdate").hide();
}
function DeleteModuleMaster(MM_ID) {
    var spinner = $('#loader');
    spinner.show();
    var model = GetModuleModel();
    model.MM_ID = MM_ID;
    $.ajax({
        type: "POST",
        url: "/ModuleMaster/Delete",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response == "SUCCESS") {
                PopupMessage("Success", "Data Delete Successfully");
                LoadModuleMasterGrid();
                Reset();
                spinner.show();
            }
            else {
                PopupMessage("Error", "Data Not Delete");
                spinner.show();
                return false;
            }
        },
        error: function (response) {
            spinner.show();
        }
    });
}
