var ClientMappingGrid = null;
$(function () {
    $(document).ready(function () {
        RegBtnSearchEvt();
    });
});
function GetClientPackageModuleMappingModel_() {
    var model = {
        "CLIENT_ID": $("#ddlClientName").val() == "" || $("#ddlClientName").val() == undefined ? null : $("#ddlClientName").val(),
        "FROMDATE": $("#txtFromDate").val() == "" || $("#txtFromDate").val() == undefined ? null : moment($.trim($("#txtFromDate").val()), "DD/MM/YYYY").format("MM-DD-YYYY"),
        "TODATE": $("#txtToDate").val() == "" || $("#txtToDate").val() == undefined ? null : moment($.trim($("#txtToDate").val()), "DD/MM/YYYY").format("MM-DD-YYYY"),
    }
    return model;
}
function RegBtnSearchEvt() {
    $("#btnSearch").click(function () {
        debugger
        var model = GetClientPackageModuleMappingModel_();
        if (model.CLIENT_ID == null && model.FROMDATE == null && model.TODATE == null) {
            PopupMessage("Warning", "Please Select Client Name OR (From Date And To Date) ");
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/ClientPackageMappingReport/SearchClientMapping",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.length > 0) {
                    ClientPackageMappingGrid(response);
                }
                else {
                    PopupMessage("Error", "Data Not Found");
                }
            },
            error: function (response) {
                PopupMessage("Error", "Data Not Found");
            }
        });
    });
}
function ClientPackageMappingGrid(data) {
    if (ClientMappingGrid != null) {
        ClientMappingGrid.destroy();
    }
    ClientMappingGrid = $('#ClientPackageMappingGrid').DataTable({
        data: data,
        scrollX: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        columns: [
            { title: "Sr.No.", width: 20, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; } },
            { title: "Orgnization Name", data: "ORGNIZATIO_NAME" },
            { title: "Package Name", data: "PACKAGE_NAME" },
            { title: "Validity", data: "TOTAL_VALIDITY" },
            { title: "Remaining Day", data: "TOTAL_REMAININGDAYS" },
            { title: "Total Amount", data: "TOTAL_AMOUNT", render: function (data, type, row, meta) { return "₹ " + parseFloat(row.TOTAL_AMOUNT).toFixed(2); }},
            { title: "No Of Users", data: "NO_OF_USERS_ALLOWED" },
            { title: "Technologies Name", data: "TECHNOLOGIES" },
            { title: "Module Name", data: "MODULE_NAME" },
        ],
    });
}