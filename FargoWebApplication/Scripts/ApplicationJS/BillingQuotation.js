var Billing_Detail = [];
$(function () {
    RegbtnSearchEvt();
    RegDdlClientChangeEvt();
    RegddlPaymentModeEvt();
    RegtxtAmountReceiveEvt();
    RegtxtDiscountEvt();
    RegtxtTDSEvt();
    RegBtnBILLINGEvt();
    RegBtnBResetEvt();
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
function RegddlPaymentModeEvt() {
    $("#ddlPaymentMode").change(function () {
        var paymentmode = $(this).val();
        if (paymentmode == "CASH") {
            $("#txtNEFT_RTGS_DD_ChequeNo").prop('readonly', true);
            $("#txtNEFT_RTGS_DD_Cheque_Date").prop('readonly', true);
        }
        else {
            $("#txtNEFT_RTGS_DD_ChequeNo").prop('readonly', false);
            $("#txtNEFT_RTGS_DD_Cheque_Date").prop('readonly', false);
        }
    })
}
function RegtxtAmountReceiveEvt() {
    $("#txtAmountReceive").change(function () {
        debugger
        var BalanceAmount = (Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT);
        var User_Enter_Received_Amt = parseFloat($(this).val());
        var User_Enter_Discount_Amt = parseFloat($("#txtDiscount").val());
        var User_Enter_TDS_Amt = parseFloat($("#txtTDS").val());
        var Total_Received_Amt_Dis_TDS = (User_Enter_Received_Amt + User_Enter_Discount_Amt + User_Enter_TDS_Amt);
        if (Total_Received_Amt_Dis_TDS > BalanceAmount) {
            $("#txtBalanceAmount").val(Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT);
            $("#txtAmountReceive").val(0);
            $("#txtDiscount").val(0);
            $("#txtTDS").val(0);
            PopupMessage("Warning", "Received Amount Should Not Greater Than Balance Amount");
            return false;
        }
        else {
            $("#txtBalanceAmount").val(Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT - Total_Received_Amt_Dis_TDS);
        }
    });
}
function RegtxtDiscountEvt() {
    $("#txtDiscount").change(function () {
        debugger
        var BalanceAmount = (Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT);
        var User_Enter_Discount_Amt = parseFloat($(this).val());
        var User_Enter_Received_Amt = parseFloat($("#txtAmountReceive").val());
        var User_Enter_TDS_Amt = parseFloat($("#txtTDS").val());
        var Total_Received_Amt_Dis_TDS = (User_Enter_Received_Amt + User_Enter_Discount_Amt + User_Enter_TDS_Amt);
        if (Total_Received_Amt_Dis_TDS > BalanceAmount) {
            $("#txtBalanceAmount").val(Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT);
            $("#txtAmountReceive").val(0);
            $("#txtDiscount").val(0);
            $("#txtTDS").val(0);
            PopupMessage("Warning", "Received Amount Should Not Greater Than Balance Amount");
            return false;
        }
        else {
            $("#txtBalanceAmount").val(Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT - Total_Received_Amt_Dis_TDS);
        }
    });
}
function RegtxtTDSEvt() {
    $("#txtTDS").change(function () {
        debugger
        var BalanceAmount = (Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT);
        var User_Enter_TDS_Amt = parseFloat($(this).val());
        var User_Enter_Discount_Amt = parseFloat($("#txtDiscount").val());
        var User_Enter_Received_Amt = parseFloat($("#txtAmountReceive").val());
        var Total_Received_Amt_Dis_TDS = (User_Enter_Received_Amt + User_Enter_Discount_Amt + User_Enter_TDS_Amt);
        if (Total_Received_Amt_Dis_TDS > BalanceAmount) {
            $("#txtBalanceAmount").val(Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT);
            $("#txtAmountReceive").val(0);
            $("#txtDiscount").val(0);
            $("#txtTDS").val(0);
            PopupMessage("Warning", "Received Amount Should Not Greater Than Balance Amount");
            return false;
        }
        else {
            $("#txtBalanceAmount").val(Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT - Total_Received_Amt_Dis_TDS);
        }
    });
}
function RegDdlClientChangeEvt() {
    $("#ddlClientName").change(function () {
        debugger
        var ClientId = $(this).val();
        $.ajax({
            type: "POST",
            url: "/Billing/GetQuotationNo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "ClientId": ClientId }),
            success: function (response) {
                debugger
                $("#ddlQuatationNo").html("");
                $("#ddlQuatationNo").append($('<option></option>').val(null).html("Select Quatation No"));
                if (response.length > 0) {
                    $.each(response, function (i, data) {
                        $("#ddlQuatationNo").append($('<option></option>').val(data.QUAT_PROF_NO).html(data.QUAT_PROF_NO));
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
        url: "/Billing/Get_Client_BillingQuot",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ "QuotationNo": QuotationNo }),
        success: function (response) {
            debugger
            Billing_Detail = response.Billing_Details;
            Received_Billing_Details = response.Received_Billing_Details;
                $("#btnBILLING").attr("data-id", Billing_Detail[0].QPID);
                var QuotationNo = $("#ddlQuatationNo :selected").text();
                if (QuotationNo != "") {
                    $("#txtQUOTATION_PROF_NO").val(QuotationNo);
                }
                $("#txtQuotationNo").val(Billing_Detail[0].QUAT_PROF_NO);
                $("#txtTotalAmount").val(Billing_Detail[0].TOTAL_AMOUNT);
                $("#txtBalanceAmount").val(Billing_Detail[0].TOTAL_AMOUNT - Billing_Detail[0].PAID_AMT);
                $("#txtAmountReceive").val(0);
                $("#txtDiscount").val(0);
                $("#txtTDS").val(0);
               
            $("#paymentDiv").show();
        },
        error: function (response) {
            $("#paymentDiv").hide();
        }
    })
}
function BILLING_Quotation_Model() {
    var model = {
        "QPID": 0,
        "RECEIPT_NO": "0",
        "FINANCIAL_YEAR": "0",
        "TOTAL_AMOUNT": $.trim($("#txtTotalAmount").val()),
        "CLIENTID": $("#ddlClientName").val(),
        "QUATATION_NO": $.trim($("#txtQuotationNo").val()),
        "BALANCE_AMOUNT": $.trim($("#txtBalanceAmount").val()),
        "AMOUNT_RECEIVE": $.trim($("#txtAmountReceive").val()),
        "BILL_DATE": moment($.trim($("#txtBillDate").val()), "DD/MM/YYYY").format("MM/DD/YYYY"),
        "BILL_NO": $.trim($("#txtBillNo").val()),
        "PAYMENT_MODE": $("#ddlPaymentMode").val(),
        "NEFT_RTGS_DD_CHEQUE_NO": $.trim($("#txtNEFT_RTGS_DD_ChequeNo").val()),
        "NEFT_RTGS_DD_CHEQUE_DATE": $.trim($("#txtNEFT_RTGS_DD_Cheque_Date").val()),
        "DISCOUNT": $.trim($("#txtDiscount").val()),
        "TDS": $.trim($("#txtTDS").val()),
        "REMARKS": $.trim($("#txtRemarks").val()),
    }
    return model;
}
function RegBtnBILLINGEvt() {
    $("#btnBILLING").click(function () {
        debugger
        var spinner = $('#loader');
        spinner.show();
        var model = BILLING_Quotation_Model();
        var QPID = ($(this).attr("data-id"));
        model.QPID = QPID;
        if (model.AMOUNT_RECEIVE == "" || model.AMOUNT_RECEIVE == null) {
            PopupMessage("Warning", "Please Enter Receive Amount");
            spinner.hide();
            return false;
        }
        else if (model.BILL_DATE == "" || model.BILL_DATE == null) {
            PopupMessage("Warning", "Please Enter Bill Date");
            spinner.hide();
            return false;
        }
        else if (model.BILL_NO == "" || model.BILL_NO == null) {
            PopupMessage("Warning", "Please Enter Bill No");
            spinner.hide();
            return false;
        }
        else if (model.PAYMENT_MODE == "" || model.PAYMENT_MODE == null) {
            PopupMessage("Warning", "Please Enter Payment Mode");
            spinner.hide();
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/Billing/CREATE_BILLING",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response == "Save Successfully") {
                    spinner.hide();
                    BindGrid()
                    Reset();
                    PopupMessage("Success", "Data Billig Created Successfully");
                }
                else {
                    spinner.hide();
                    Reset();
                    PopupMessage("Error", "Bill Not Created");
                    return false;
                }
            },
            error: function (response) {
                PopupMessage("Error", "Data Not Updated");
                spinner.hide();
                return false;
            }
        });
    })
}
function Reset() {
    debugger
    $("#ddlClientName").val("");
    $('#ddlQuatationNo').val("");
    $("#txtQUOTATION_PROF_NO").val("");
    $("#txtQuotationNo").val("");
    $("#txtTotalAmount").val("");
    $("#txtBalanceAmount").val("");
    $("#txtAmountReceive").val(0);
    $("#txtDiscount").val(0);
    $("#txtTDS").val(0);
    $("#txtBillDate").val("");
    $("#txtBillNo").val("");
    $("#ddlPaymentMode").val("");
    $("#txtNEFT_RTGS_DD_ChequeNo").val("");
    $("#txtNEFT_RTGS_DD_Cheque_Date").val("");
    $("#txtRemarks").val("");
}
function RegBtnBResetEvt() {
    $("#btnReset").click(function () {
        Reset();
    })
}
function BindGrid() {
    $.ajax({
        type: "POST",
        url: "/Billing/BindReceiptGrid",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (Received_Billing_Details) {
            if (Received_Billing_Details.length > 0) {
               var body = ""; $("#tblReceivedAmount tbody").empty();
                if ($.fn.DataTable.isDataTable('#tblReceivedAmount')) {
                    $('#tblReceivedAmount').DataTable().destroy();
                }
                $.each(Received_Billing_Details, function (i, item) {
                    body += "<tr>";
                    body += "<td width='5%'>" + (++i) + "</td>";
                    body += "<td width='10%'>" + item.RECEIPT_NO + "</td>";
                    body += "<td width='15%' style='text-align:center'>" + item.ORGNIZATIO_NAME + "</td>";
                    body += "<td width='10%' style='text-align:center'>" + item.QUATATION_NO + "</td>";
                    body += "<td width='15%' style='text-align:right'>" + item.AMOUNT_RECEIVE + "</td>";
                    body += "<td width='8%' style='text-align:right'>" + item.DISCOUNT + "</td>";
                    body += "<td width='7%' style='text-align:right'>" + item.TDS + "</td>";
                    body += "<td width='9%' style='text-align:center'>" + item.BILL_NO + "</td>";
                    //body += "<td width='9%' style='text-align:center'>" + item.BILL_DATE + "</td>";
                    body += "<td width='15%'>" + item.REMARKS + "</td>";
                    body += "<td width='7%'><a href='/Billing/ViewReceiptPDF?CB_ID=" + item.CB_ID + "' target='_blank'><i class='fa fa-eye' aria-hidden='true' cursor:pointer;color:black;></i></a></td>";
                    body += "</tr>";
                });
                $("#tblReceivedAmount tbody").append(body);
                $('#tblReceivedAmount').DataTable({
                    "searching": true,
                    "paging": true,
                    "ordering": true,
                    "info": false
                });
            }
            else {
                $("#tblReceivedAmount tbody").empty();
            }
        }
    });
}