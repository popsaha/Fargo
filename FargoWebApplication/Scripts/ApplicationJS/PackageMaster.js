var GridPackage = null;
$(function () {
    $(document).ready(function () {
        LoadPackageGrid();
        RegBtnSubmitEvt();
        RegbtnResetEvt();
        RegBtnUpdateEvt();
        RegDecimalConversionFn();
    });
});
function RegDecimalConversionFn() {
    $(".decimalFormat").change(function () {
        $(this).val(parseFloat($(this).val()).toFixed(2));
    });
}

function GetPackageModel() {
    var model = {
        "PMID": 0,
        "PACKAGE_NAME": $.trim($("#txtPackageName").val()),
        "PRICE_WO_GST": $.trim($("#txtPriceWOGST").val()),
        "GST_PERCENTAGE": $("#ddlGSTPercent").val(),
        "GST_AMOUNT": $.trim($("#txtGSTAmount").val()),
        "TOTAL_AMOUNT": $.trim($("#txtTotalAmount").val()),
        "IS_ACTIVE": true,
    }
    return model;
}

function RegBtnSubmitEvt() {
    $("#btnSubmit").click(function () {
        var model = GetPackageModel();

        if (model.PACKAGE_NAME == "" || model.PACKAGE_NAME == null) {
            PopupMessage("Warning", "Please Enter Package Name");
            return false;
        }
        else if (model.PRICE_WO_GST == "" || model.PRICE_WO_GST == null) {
            PopupMessage("Warning", "Please Enter Price With Out GST");
            return false;
        }
        else if (model.GST_PERCENTAGE == "" || model.GST_PERCENTAGE == null) {
            PopupMessage("Warning", "Please Select GST Percentage");
            return false;
        }
        var IsExists = false;
        var _gridData = GridPackage.rows().data();
        $(_gridData).each(function () {
            if (this.PACKAGE_NAME.toUpperCase() == model.PACKAGE_NAME.toUpperCase())
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/PackageMaster/Create",
                data: JSON.stringify(model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    debugger
                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Save Successfully");
                        LoadPackageGrid();
                        Reset();
                    }
                    else {
                        PopupMessage("Error", "Data Not Save");
                        return false;
                    }
                },
                error: function (response) {
                }
            });
        } else {
            PopupMessage("Warning", "Same Package Name Already Exists!");
            return false;
        }
    });
}

function CalculateGSTAndTotalAmt() {
    debugger
    var SS = $("#ddlGSTPercent option:selected").text();
    var GSTPercentage = parseFloat($("#ddlGSTPercent option:selected").text());
    var PriceWOGST = parseFloat($("#txtPriceWOGST").val());
    if ($("#ddlGSTPercent option:selected").text() == "Select GST Percentage") {
        GSTPercentage = 0;
    }
    if ($("#txtPriceWOGST").val() == "") {
        PriceWOGST = 0;
    }
    var TotalGST = parseFloat((PriceWOGST * GSTPercentage) / 100);
    var TotalAmount = parseFloat((PriceWOGST + TotalGST));
    $("#txtGSTAmount").val(parseFloat(TotalGST).toFixed(2));
    $("#txtTotalAmount").val(parseFloat(TotalAmount).toFixed(2));
};

function LoadPackageGrid() {
    $.ajax({
        type: "GET",
        url: "/PackageMaster/GetPackageList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            BindPackageDataTable(response);
        },
        error: function (response) {
        }
    });
}

function BindPackageDataTable(data) {
    if (GridPackage != null) {
        GridPackage.destroy();
    }
    GridPackage = $('#PackageGrid').DataTable({
        data: data,
        scrollX: true,
        columns: [
            { title: "Sr.No.", width: 20, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; } },
            { title: "Package Name", data: "PACKAGE_NAME" },
            { title: "Price W/O GST", data: "PRICE_WO_GST", render: function (data, type, row, meta) { return "₹ " + parseFloat(row.PRICE_WO_GST).toFixed(2); } },
            { title: "GST Percentage", data: "GST_PERCENT", render: function (data, type, row, meta) { return parseFloat(row.GST_PERCENT).toFixed(2) + " %"; } },
            { title: "GST Amount", data: "GST_AMOUNT", render: function (data, type, row, meta) { return "₹ " + parseFloat(row.GST_AMOUNT).toFixed(2); } },
            { title: "Total Amount", data: "TOTAL_AMOUNT", render: function (data, type, row, meta) { return "₹ " + parseFloat(row.TOTAL_AMOUNT).toFixed(2); } },
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
    $('#PackageGrid tbody').off('click');
    $('#PackageGrid tbody').on('click', '#delete', function () {
        _RowSelectedForDelete = $(this).parents('tr');
        var data = GridPackage.row(_RowSelectedForDelete).data();
        ConfirmMessage("Are you sure want to delete package", "DeletePackage(" + data.PMID + ")");
    });
    $('#PackageGrid tbody').on('click', '#edit', function () {
        _RowSelectedForEdit = $(this).parents('tr');
        var data = GridPackage.row(_RowSelectedForEdit).data();
        EditPackage(data);
    });
}

function EditPackage(data) {
    debugger
    $("#btnUpdate").attr("data-id", data.PMID);
    $("#txtPackageName").val(data.PACKAGE_NAME);
    $("#txtPriceWOGST").val(data.PRICE_WO_GST);
    $("#ddlGSTPercent").val(data.GST_ID);
    $("#txtGSTAmount").val(data.GST_AMOUNT);
    $("#txtTotalAmount").val(data.TOTAL_AMOUNT);
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    $("#btnUpdate").show();
    $("#btnSubmit").hide();
}

function Reset() {
    $("#ddlGSTPercent").val("");
    $("#txtPackageName").val("");
    $("#txtPriceWOGST").val("");
    $("#txtGSTAmount").val("");
    $("#txtTotalAmount").val("");
    $("#btnUpdate").hide();
    $("#btnSubmit").show();
}

function RegbtnResetEvt() {
    $("#btnReset").click(function () {
        Reset();
    })
};

function RegBtnUpdateEvt() {
    $("#btnUpdate").click(function () {
        debugger
        var model = GetPackageModel();
        var PMID = ($(this).attr("data-id"));
        model.PMID = PMID;
        if (model.PACKAGE_NAME == "" || model.PACKAGE_NAME == null) {
            PopupMessage("Warning", "Please Enter Package Name");
            return false;
        }
        else if (model.PRICE_WO_GST == "" || model.PRICE_WO_GST == null) {
            PopupMessage("Warning", "Please Enter Price With Out GST");
            return false;
        }
        else if (model.GST_PERCENTAGE == "" || model.GST_PERCENTAGE == null) {
            PopupMessage("Warning", "Please select GST Percentage");
            return false;
        }
        var IsExists = false;
        var _gridData = GridPackage.rows().data();
        $(_gridData).each(function () {
            if (this.PACKAGE_NAME.toUpperCase() == model.PACKAGE_NAME.toUpperCase() && this.PRICE_WO_GST == model.PRICE_WO_GST && this.GST_ID == model.GST_PERCENTAGE)
                IsExists = true;
        });
        if (!IsExists) {
            $.ajax({
                type: "POST",
                url: "/PackageMaster/Edit",
                data: JSON.stringify(model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response == "SUCCESS") {
                        PopupMessage("Success", "Data Update Successfully");
                        LoadPackageGrid();
                        Reset();
                    }
                    else {
                        PopupMessage("Error", "Data Not Update");
                        return false;
                    }
                },
                error: function (response) {
                }
            });
        }
        else {
            PopupMessage("Warning", "Same Package Name Already Exists!");
            return false;
        }
    });
}

function DeletePackage(PMID) {
    var model = GetPackageModel();
    model.PMID = PMID;
    $.ajax({
        type: "POST",
        url: "/PackageMaster/Delete",
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response == "SUCCESS") {
                PopupMessage("Success", "Data Delete Successfully");
                LoadPackageGrid();
                Reset();
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