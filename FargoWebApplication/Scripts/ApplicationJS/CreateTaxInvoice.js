$(function () {
    RegDdlClientChangeEvt();
    RegbtnSearchEvt();
    RegbtnCreateInvoiceEvt();
    RegbtnResetEvt();
    BindGrid();
    ClientList = JSON.parse($("#hdnClientList").val());
    BindClien(ClientList);
    $(".datetimepicker").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true
    });
});
function BindClien(ClientList) {
    $("#ddlClientName").html("");
    $("#ddlClientName").append($('<option></option>').val(null).html("Select Client Name"));
    $.each(ClientList, function (i, data) {
        $("#ddlClientName").append($('<option></option>').val(data.CR_ID).html(data.ORGNIZATIO_NAME));
    });
}
function RegDdlClientChangeEvt() {
    $("#ddlClientName").change(function () {
        debugger
        var ClientId = $(this).val();
        $.ajax({
            type: "POST",
            url: "/CreateTaxInvoice/GetQuotationNo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "ClientId": ClientId }),
            success: function (response) {
                debugger
                $("#ddlQuatationNo").html("");
                $("#ddlQuatationNo").append($('<option></option>').val(null).html("Select Quatation No"));
                if (response.length > 0) {
                    $.each(response, function (i, data) {
                        $("#ddlQuatationNo").append($('<option></option>').val(data.QUATATION_NO).html(data.QUATATION_NO));
                    });
                }
                else {
                    $("#ddlQuatationNo").append($('<option></option>').val("").html("No Quotation Found"));
                }
            },
            error: function (response) {
            }
        })
    })
}
function RegbtnSearchEvt() {
    $("#btnSearch").click(function () {
        var QuotationNo = $("#ddlQuatationNo").val();
        GetBilling_Details(QuotationNo);
    });
}
function GetBilling_Details(QuotationNo) {
    $.ajax({
        type: "POST",
        url: "/CreateTaxInvoice/Get_Tax_Invoice_Detail",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ "QuotationNo": QuotationNo }),
        success: function (response) {
            debugger
            Received_Billing_Details = response;
            var QuotationNo = $("#ddlQuatationNo :selected").text();
            if (QuotationNo != "") {
                $("#txtQUOTATION_PROF_NO").val(QuotationNo);
            }
            if (Received_Billing_Details.length > 0) {
                var body = ""; $("#tblTaxInvoice tbody").empty();
                $.each(Received_Billing_Details, function (i, item) {
                    body += "<tr>";
                    body += "<td width='5%'><input id='Paymentchk" + i + "' type='checkbox' class='ChkPayment' value='" + JSON.stringify(item) + "' onclick='PaymentChkClick(" + JSON.stringify(item) + "," + i + ")'></td>";
                    body += "<td width='15%'>" + item.RECEIPT_NO + "</td>";
                    body += "<td width='20%' style='text-align:right'>" + item.AMOUNT_RECEIVE + "</td>";
                    body += "<td width='15%' style='text-align:right'>" + item.TDS  + "</td>";
                    body += "<td width='15%' style='text-align:right'>" + item.DISCOUNT + "</td>";
                    body += "<td width='15%' style='text-align:center'>" + item.BILL_DATE + "</td>";
                    body += "<td width='15%' style='text-align:center'>" + item.PAYMENT_MODE + "</td>";
                });
                $("#tblTaxInvoice tbody").append(body);
                $("#paymentDiv").show();
            }
            else {
                $("#tblTaxInvoice tbody").empty();
                $("#paymentDiv").hide();
            }
        },
        error: function (response) {
            $("#paymentDiv").hide();
        }
    })
}
function PaymentChkClick(item, row) {
    debugger
    var PreAmount = parseFloat($("#txtTotalAmountChkCheck").val());
    if ($("#Paymentchk" + row).prop("checked") == true) {
        var Result = (item.AMOUNT_RECEIVE + item.DISCOUNT + item.TDS + PreAmount)
        $("#txtTotalAmountChkCheck").val("");
        $("#txtTotalAmountChkCheck").val(Result);
    }
    else {
        var Result = (PreAmount - (item.AMOUNT_RECEIVE + item.DISCOUNT + item.TDS))
        $("#txtTotalAmountChkCheck").val("");
        $("#txtTotalAmountChkCheck").val(Result);
    }
}
function RegbtnCreateInvoiceEvt() {
    $("#btnCreateInvoice").click(function () {
        debugger
        var spinner = $('#loader');
        spinner.show();
        var TaxInvoiceDate = moment($.trim($("#txtTaxInvoiceDate").val()), "DD/MM/YYYY").format("MM/DD/YYYY");
        var Total_Invoice_Amt = $("#txtTotalAmountChkCheck").val();
        var TBL_TAX_INVOICE = [];
        var TableLen = $("#tblTaxInvoice tbody tr").length;
        for (var j = 0; j <= TableLen; j++) {
            if ($("#Paymentchk" + j).prop("checked") == true) {
                var Tax_Invoice_Details = JSON.parse($('#Paymentchk' + j).val());
                item = {}
                item["CB_ID"] = Tax_Invoice_Details.CB_ID;
                item["RECEIPT_NO"] = Tax_Invoice_Details.RECEIPT_NO;
                item["QUOTATION_NO"] = Tax_Invoice_Details.QUATATION_NO;
                item["TAX_INVOICE_DATE"] = TaxInvoiceDate;
                item["TAX_INVOICE_AMT"] = Total_Invoice_Amt;
                TBL_TAX_INVOICE.push(item);
            }
        };
        if (TaxInvoiceDate == "Invalid date" || TaxInvoiceDate == null) {
            PopupMessage("Warning", "Please Select Tax Invoice Date");
            spinner.hide();
            return false;
        }
        else if (TBL_TAX_INVOICE.length == 0) {
            spinner.hide();
            PopupMessage("Warning", "Please Select Check Box");
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/CreateTaxInvoice/CreateTaxInoice",
            data: JSON.stringify(TBL_TAX_INVOICE),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                spinner.hide();
                if (data == "SUCCESS") {
                    spinner.hide();
                    Reset();
                    BindGrid();
                    PopupMessage("Success", "Invoice Create Successfully");
                }
                else {
                    spinner.hide();
                    PopupMessage("Error", "Invoice Not Create");
                }
            },
            error: function (data) {
                spinner.hide();
                PopupMessage("Error", "Some Error Throw");
            }
        });
    });
}
function RegbtnResetEvt() {
    $("#btnReset").click(function () {
        Reset();
    })
};
function Reset() {
    $("#ddlClientName").val(""); $("#ddlQuatationNo").val(""); $("#txtQUOTATION_PROF_NO").val("");
    $("#paymentDiv").hide(); $("#txtTotalAmountChkCheck").val(""); $("#txtTaxInvoiceDate").val("");
}
function BindGrid() {
    $.ajax({
        type: "POST",
        url: "/CreateTaxInvoice/BindGrid_Invoice_Tax",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (Grid_TaxInvoice_List) {
            if (Grid_TaxInvoice_List.length > 0) {
                var body = ""; 
                if ($.fn.DataTable.isDataTable('#tblGenerateTaxInvoice')) {
                    $('#tblGenerateTaxInvoice').DataTable().destroy();
                }
                $("#tblGenerateTaxInvoice tbody").empty();
                $.each(Grid_TaxInvoice_List, function (i, item) {
                    body += "<tr>";
                    body += "<td width='5%'>" + (++i) + "</td>";
                    body += "<td width='20%' style='text-align:center'>" + item.TAX_INVOICE_NO + "</td>";
                    body += "<td width='20%' style='text-align:center'>" + moment(item.TAX_INVOICE_DATE).format("DD/MM/YYYY") + "</td>";
                    body += "<td width='17%' style='text-align:right'>" + item.BASIC_AMOUNT + "</td>";
                    body += "<td width='16%' style='text-align:right'>" + item.GST_AMOUNT + "</td>";
                    body += "<td width='16%' style='text-align:right'>" + item.TOTAL_AMOUNT + "</td>";
                    body += "<td width='6%'><a href='/CreateTaxInvoice/ViewTaxInvoicePDF?TAX_INVOICE_NO=" + item.TAX_INVOICE_NO + "' target='_blank'><i class='fa fa-eye' aria-hidden='true' cursor:pointer;color:black;></i></a></td>";
                    body += "</tr>";
                });
                $("#tblGenerateTaxInvoice tbody").append(body);
                $('#tblGenerateTaxInvoice').DataTable({
                    "searching": true,
                    "paging": true,
                    "ordering": true,
                    "info": false
                });
            }
            else {
                $("#tblGenerateTaxInvoice tbody").empty();
            }
        }
    });
}