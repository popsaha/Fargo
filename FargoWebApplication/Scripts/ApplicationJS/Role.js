var GridRole = null; var tableRow = null;
$(function () {
  
    $(document).ready(function () {
        const dataTable = new simpleDatatables.DataTable("#tblRole");
     //   LoadRoleGrid();
        RegBtnSubmitEvt();
        RegBtnUpdateEvt();
        RegBtnResetEvt();
    });
});


function GetRoleModel() {
    var model = {
        "ROLE_ID": 0,
        "ROLE_NAME": $.trim($("#txtRoleName").val()),
        "DESCRIPTION": $.trim($("#txtDescription").val()),
        "IS_ACTIVE": true,
    }
    return model;
}

function LoadRoleGrid() {
    $.ajax({
        type: "GET",
        url: "/Role/LST_ROLES",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            const dataTable_ = new simpleDatatables.DataTable("#tblRole").clear();
            $.each(response, function (index, value) {
                tableRow += "<tr>" +
                            "<td>" + (index+1) + "</td>" +
                            "<td>" + response[index].ROLE_NAME + "</td>" +
                            "<td>" + response[index].DESCRIPTION + "</td>" +
                            "<td>" + response[index].CREATED_BY + "</td>" +
                            "<td>" + response[index].CREATED_ON + "</td>" +
                            "<td>Edit || Delete</td>"+
                            "</tr>";
            });
            $("#tblRole tbody").append(tableRow);
        },
        error: function (response) {
        }
    });
}

function BindRoleDataTable(data) {
  var jQ=  $.noConflict();
    if (GridRole != null) {
        GridRole.destroy();
    }
    GridRole = jQ('#tblRole').DataTable({
        data: data,
        scrollX: true,
        columns: [
            { title: "SL NO.", width: 20, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; } },
            { title: "ROLE NAME", data: "ROLE_NAME" },
            { title: "DESCRIPTION", data: "DESCRIPTION" },
            { title: "CREATED_BY", data: "CREATED_BY" },
            { title: "CREATED_ON", data: "CREATED_ON" },
            {
                title: "Action", render: function (data, type, row, meta) {
                    return "<span id='edit'><span title='Edit' class='fa fa-pencil' aria-hidden='true' style='cursor:pointer'></span></span>&nbsp;&nbsp;&nbsp;<span id='delete'><span title='Delete' class='fa fa-trash' aria-hidden='true' style='cursor:pointer'></span></span>";
                }
            },
        ],
    });


    $('#tblRole tbody').off('click');
    $('#tblRole tbody').on('click', '#delete', function () {
        _RowSelectedForDelete = $(this).parents('tr');
        var data = GridRole.row(_RowSelectedForDelete).data();
        ConfirmMessage("Are you sure want to delete role", "DeleteRole(" + data.ROLE_ID + ")");
    });
    $('#tblRole tbody').on('click', '#edit', function () {
        _RowSelectedForEdit = $(this).parents('tr');
        var data = GridRole.row(_RowSelectedForEdit).data();
        EditRole(data);
    });
}

function RegBtnSubmitEvt() {
    $("#btnSubmit").click(function () {
        var model = GetRoleModel();
        if (model.ROLE_NAME == "" || model.ROLE_NAME == null) {
            alert("Please enter role name.");
            return false;
        }
        if (model.DESCRIPTION == "" || model.DESCRIPTION == null) {
            alert("Please describe about role.");
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/Role/Create",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response == "SUCCESS") {
                    alert("Data Saved Successfully");
                    LoadRoleGrid();
                    Reset();
                }
                else {
                    alert("Data Saved Successfully");
                    return false;
                }
            },
            error: function (response) {
                alert("Data Saved Successfully");
            }
        });
    });
}

function EditRole(data) {
    $("#btnUpdate").attr("data-id", data.ROLE_ID);
    $("#txtRoleName").val(data.ROLE_NAME);
    $("#txtDescription").val(data.DESCRIPTION);
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    $("#btnUpdate").show();
    $("#btnSubmit").hide();
}

function RegBtnUpdateEvt() {
    $("#btnUpdate").click(function () {
        var ROLE_ID = ($(this).attr("data-id"));
        var model = {
            "ROLE_ID": ROLE_ID,
            "ROLE_NAME": $("#txtRoleName").val(),
            "DESCRIPTION": $("#txtDescription").val(),
        }
        if (model.ROLE_NAME == "" || model.ROLE_NAME == null) {
            alert("Please enter role name.");
            return false;
        }
        if (model.DESCRIPTION == "" || model.DESCRIPTION == null) {
            alert("Please describe about role.");
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/Role/Edit",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response == "SUCCESS") {
                    alert("Data Updated Successfully");
                    LoadRoleGrid();
                    Reset();
                }
                else {
                    alert("Data not updated.");
                    return false;
                }
            },
            error: function (response) {
                alert("Data not updated.");
            }
        });
    });
}

function Reset() {
    $("#txtRoleName").val("");
    $("#txtDescription").val("");
    $("#btnSubmit").show();
    $("#btnUpdate").hide();
}

function DeleteRole(ROLE_ID) {
    var model = GetRoleModel();
    model.ROLE_ID = ROLE_ID;
    $.ajax({
        type: "POST",
        url: "/Role/Delete",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response == "SUCCESS") {
                alert("Data Deleted Successfully");
                LoadRoleGrid();
            }
            else {
                alert("Data Not Deleted");
                return false;
            }
        },
        error: function (response) {
            alert("Data Not Deleted");
        }
    });
}

function RegBtnResetEvt() {
    $("#btnReset").click(function () {
        Reset();
    });
}

