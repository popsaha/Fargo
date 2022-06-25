window.globalCounter = 1; var ClientDetails = []; var PrintClientDetail = [];
$(function () {
    RegDdlClientChangeEvt();
    RegDdlQuatationNoChangeEvt();
    RegbtnSearchEvt();
    RegBtnSubmitEvt();
    RegBtnResetEvt();
    RegDeleteTblRow();
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
        var ClientId = $(this).val();
        $.ajax({
            type: "POST",
            url: "/ReviseQuotation/GetQuotationNo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "ClientId": ClientId }),
            success: function (response) {
                debugger
                $("#ddlQuatationNo").html("");
                $("#ddlQuatationNo").append($('<option></option>').val(null).html("Select Quatation No"));
                if (response.length > 0) {
                    $.each(response, function (i, data) {
                        $("#ddlQuatationNo").append($('<option></option>').val(data.QPID).html(data.QUAT_PROF_NO));
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
function RegDdlQuatationNoChangeEvt() {
    $("#ddlQuatationNo").change(function () {
        var QuotationNo = $("#ddlQuatationNo :selected").text();
        GetQuotationDetails(QuotationNo);
    })
}
function RegbtnSearchEvt() {
    $("#btnSearch").click(function () {
        var QuotationNo = $("#txtQUOTATION_PROF_NO").val();
        GetQuotationDetails(QuotationNo);
    });
}
function GetQuotationDetails(QuotationNo) {
    $.ajax({
        type: "POST",
        url: "/ReviseQuotation/Get_Client_ReviseQuot",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ "QuotationNo": QuotationNo }),
        success: function (response) {
            BindReviseQuotDetails(response);
            ClientDetails = response;
        },
        error: function (response) {
        }
    })
}
function BindReviseQuotDetails(details) {
    debugger
    var body = ""; window.globalCounter = 1;
    $("#tblReviseQuotationProforma tbody").empty();
    $("#ddlClientName").val(details[0].CLIENT_ID);
    var QuotationNo = $("#ddlQuatationNo :selected").text();
    if (QuotationNo != "") {
        $("#txtQUOTATION_PROF_NO").val(QuotationNo);
    }
    $("#txtdate").val(moment(details[0].QP_FROM).format("DD/MM/YYYY"));
    $("#txtRemarks").val(details[0].REMARK);
    $.each(details, function (i, item) {
        body += "<tr>";
        body += "<td width='5%'>" + window.globalCounter + "</td>";
        body += "<td width='35%' style='display:none'><input type='text' id='txtSUBQP_ID" + window.globalCounter + "' class='form-control txtSUBQP_ID' value='" + item.SUBQP_ID + "'/></td>";
        body += "<td width='35%'><input type='text' id='txtParticularName" + window.globalCounter + "' class='form-control txtParticularName' value='" + item.PARTICULAR_NAME + "'/></td>";
        body += "<td width='25%'><input type='number' min='0' onKeyDown='if (this.value.length == 7) return false;' oninput='this.value =!!this.value && Math.abs(this.value) >= 0 ? Math.abs(this.value) : null' id='txtAmount" + window.globalCounter + "' class='form-control txtAmount' value='" + item.AMOUNT + "'/></td>";
        body += "<td width='25%'><select class='form-control ddlIdtaxable select2' id='ddlIdtaxable" + window.globalCounter + "'><option value='1'>Taxable</option><option value='0'>Non-Taxable</option></td>";
        body += "<td width='10%' style='text-align:center;'><span class='delete'><i class='fa fa-trash' aria-hidden='true' style='font-size:18px; cursor:pointer'></i></span></td>";
        body += "</tr>";
        window.globalCounter++;
    });
    $("#tblReviseQuotationProforma tbody").append(body);
}
function btnAddRow() {
    debugger
    var body = "";
    body += "<tr>";
    body += "<td width='5%'>" + window.globalCounter + "</td>";
    body += "<td width='35%' style='display:none'><input type='text' id='txtSUBQP_ID" + window.globalCounter + "' class='form-control txtSUBQP_ID' value='0'/></td>";
    body += "<td width='35%'><input type='text' id='txtParticularName" + window.globalCounter + "' class='form-control txtParticularName' value=''/></td>";
    body += "<td width='25%'><input type='number' min='0' onKeyDown='if (this.value.length == 7) return false;' oninput='this.value =!!this.value && Math.abs(this.value) >= 0 ? Math.abs(this.value) : null' id='txtAmount" + window.globalCounter + "' class='form-control txtAmount' value=''/></td>";
    body += "<td width='25%'><select class='form-control ddlIdtaxable select2' id='ddlIdtaxable" + window.globalCounter + "'><option value='1'>Taxable</option><option value='0'>Non-Taxable</option></td>";
    body += "<td width='10%' style='text-align:center;'><span class='delete'><i class='fa fa-trash' aria-hidden='true' style='font-size:18px; cursor:pointer'></i></span></td>";
    body += "</tr>";
    $("#tblReviseQuotationProforma tbody").append(body);
    window.globalCounter++;
}
function RegDeleteTblRow() {
    $("#tblReviseQuotationProforma").on("click", ".delete", function (ev) {
        var $currentTableRow = $(ev.currentTarget).parents('tr')[0];
        $currentTableRow.remove();
        window.globalCounter--;
        $("#tblReviseQuotationProforma tbody").find('tr').each(function (index) {
            var firstTDDomEl = $(this).find('td')[0];
            var $firstTDJQObject = $(firstTDDomEl);
            $firstTDJQObject.text(index + 1);
            var SecondTDDomEl = $(this).find('.txtParticularName');
            $(SecondTDDomEl).attr("id", "txtParticularName" + (index + 1))
            var ThirdTDDomEl = $(this).find('.txtAmount');
            $(ThirdTDDomEl).attr("id", "txtAmount" + (index + 1))
            var FourthTDDomEl = $(this).find('.ddlIdtaxable');
            $(FourthTDDomEl).attr("id", "ddlIdtaxable" + (index + 1))
        });
    });
}
function ReviseQuotationProformaModel() {
    var model = {
        "QPID": 0,
        "CLIENT_ID": $("#ddlClientName").val(),
        "QUAT_PROF_NO": $("#txtQUOTATION_PROF_NO").val(),
        "FINANCIAL_YEAR": "",
        "QP_FROM": moment($.trim($("#txtdate").val()), "DD/MM/YYYY").format("MM/DD/YYYY"),
        "REMARK": $.trim($("#txtRemarks").val()),
        "_QUOT_PROF_SUB_TBL": null,
        "PRICE_WO_GST": 0,
        "GST_AMOUNT": 0,
        "TOTAL_AMOUNT": 0,
        "GST_PERCENTAGE": 0,
    }
    return model;
}
function RegBtnSubmitEvt() {
    $("#btnUpdate").click(function () {
        debugger
        var spinner = $('#loader');
        spinner.show();
        var model = ReviseQuotationProformaModel();
        var QUOTE_PROF_List = []; var _CalTotalGST_Amt = 0; var _PRICE_WO_GST = 0;
        var QuotationRowlist = $("#tblReviseQuotationProforma tbody tr").length;
        for (var j = 1; j <= QuotationRowlist; j++) {
            item = {}
            item["SUBQP_ID"] = $("#txtSUBQP_ID" + j).val();
            item["PARTICULAR_NAME"] = $("#txtParticularName" + j).val();
            item["AMOUNT"] = $("#txtAmount" + j).val();
            item["IS_TAXABLE"] = parseFloat($("#ddlIdtaxable" + j).val());
            QUOTE_PROF_List.push(item);
            _PRICE_WO_GST += parseFloat($("#txtAmount" + j).val());
            if ($("#ddlIdtaxable" + j).val() == "1") {
                _CalTotalGST_Amt += parseFloat($("#txtAmount" + j).val());
            }
        };
        model.PRICE_WO_GST = _PRICE_WO_GST;
        model.GST_AMOUNT = ((_CalTotalGST_Amt * ClientDetails[0].GST_PERCENTAGE) / 100);
        model.TOTAL_AMOUNT = model.GST_AMOUNT + model.PRICE_WO_GST;
        model.GST_PERCENTAGE = ClientDetails[0].GST_PERCENTAGE;
        model.QPID = ClientDetails[0].QPID;
        model._QUOT_PROF_SUB_TBL = QUOTE_PROF_List;
        if (model.CLIENT_ID == "" || model.CLIENT_ID == null) {
            PopupMessage("Warning", "Please Select Client Name");
            spinner.hide();
            return false;
        }
        else if (model.QUAT_PROF_NO == "" || model.QUAT_PROF_NO == null) {
            PopupMessage("Warning", "Please Enter Quotation No");
            spinner.hide();
            return false;
        }
        else if (model.QP_FROM == "" || model.QP_FROM == null) {
            PopupMessage("Warning", "Please Select Date");
            spinner.hide();
            return false;
        }
        //else if (model.REMARK == "" || model.REMARK == null) {
        //    PopupMessage("Warning", "Please Select Remark");
        //    spinner.hide();
        //    return false;
        //}
        $.ajax({
            type: "POST",
            url: "/ReviseQuotation/UpdateProforma",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                spinner.hide();
                if (data == "SUCCESS") {
                    ShowQuotation();
                }
                else {
                    PopupMessage("Error", "Data Not Saved");
                }
            },
            error: function (data) {
                spinner.hide();
                PopupMessage("Error", "Data Not Saved");
            }
        });
    });
}
function RegBtnResetEvt() {
    $("#btnReset").click(function () {
        location.reload();
    });
}
function inrFormat(nStr) { // nStr is the input string
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    var z = 0;
    var len = String(x1).length;
    var num = parseInt((len / 2) - 1);

    while (rgx.test(x1)) {
        if (z > 0) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        else {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
            rgx = /(\d+)(\d{2})/;
        }
        z++;
        num--;
        if (num == 0) {
            break;
        }
    }
    return x1 + x2;
}
function ShowQuotation() {
    debugger
    var ClientId = ClientDetails[0].CLIENT_ID;
    $.ajax({
        type: "POST",
        url: "/QuoAndProfa/ClientPackageDetails",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ "ClientId": ClientId }),
        success: function (PrintClientDetail) {
            $("#tblParticulars tbody").empty();
            $("#tblGST").empty();
            var Date = $('#txtdate').val();
            var Remarks = $('#txtRemarks').val();
            var QUOTATION_PROF_NO = $('#txtQUOTATION_PROF_NO').val();
            $("#lblQuotationNo").text(QUOTATION_PROF_NO);
            $("#lblDated").text(Date);
            $('#lblRemarks').text(Remarks);
            $("#lblClientName").text(PrintClientDetail[0].ORGNIZATIO_NAME);
            $("#lblClientAddress").text(PrintClientDetail[0].PRIMARY_ADDRESS);
            $("#lblClientState").text(PrintClientDetail[0].STATE_NAME);
            $("#lblClientGSTIN").text(PrintClientDetail[0].GST_NO);
            $("#lblClientPAN").text(PrintClientDetail[0].PAN_NO);
            $("#lblApplicationURL").text(PrintClientDetail[0].CLIENT_APPLICATION_URL);
            $("#lblClientContactNo").text(PrintClientDetail[0].PRIMARY_CONTACT_NO);
            var array = [];
            var QuotationRowlist = $("#tblReviseQuotationProforma tbody tr").length;
            for (var j = 1; j <= QuotationRowlist; j++) {
                item = {}
                item["Particular"] = $("#txtParticularName" + j).val();
                item["Amount"] = $("#txtAmount" + j).val();
                item["Is_Taxable"] = $("#ddlIdtaxable" + j).val();
                array.push(item);
            };
            var MiddleString = ""; var LastString = ""; var otherTaxableValue = 0; var TotalAmountWithTax = 0; var otherNon_TaxableValue = 0;

            $.each(array, function (i, item) {
                debugger
                if (item.Is_Taxable == "1") {
                    MiddleString += "<tr> \
                <td style='font-size:12px;border-left:1px solid #000;border-right:1px solid #000;font-size:11px;'>&nbsp;&nbsp;<span style='font-weight:bold;'>" + item.Particular + "</span></td> \
                <td style='text-align:center;font-size:11px;border-right:1px solid #000;'><span style='font-weight:bold;'></span></td> \
                <td style='text-align:right;font-size:11px;border-right:1px solid #000;'><span id='lblAmount' style='font-weight:bold;margin-right:2px;font-size:11px'>" + inrFormat(parseFloat(item.Amount)) + "</span></td> \
                </tr>";
                    otherTaxableValue += parseFloat(item.Amount);
                }
                else {
                    MiddleString += "<tr> \
                <td style='font-size:12px;border-left:1px solid #000;border-right:1px solid #000;font-size:11px;'>&nbsp;&nbsp;<span>" + item.Particular + "</span></td> \
                <td style='text-align:center;font-size:11px;border-right:1px solid #000;'><span style='font-weight:bold;'></span></td> \
                <td style='text-align:right;font-size:11px;border-right:1px solid #000;'><span id='lblAmount' style='margin-right:2px;font-size:11px;'>" + inrFormat(parseFloat(item.Amount)) + "</span></td> \
                </tr>";
                    otherNon_TaxableValue += parseFloat(item.Amount);
                }
            });
            var TaxableValue = (parseFloat(otherTaxableValue)) * parseFloat(PrintClientDetail[0].GST_PERCENT) / 100;
            var Amount_decimal = parseFloat(otherTaxableValue).toFixed(2);
            if (PrintClientDetail[0].STATE_NAME == "Maharashtra") {
                var halfRate = parseFloat(PrintClientDetail[0].GST_PERCENT) / 2;
                var halfTaxableValue = TaxableValue / 2;

                LastString = "<tr id='rowSGST'> \
                                 <td style='text-align:right;font-size:11px;border-left:1px solid #000;border-right:1px solid #000;font-weight:bold;'>Output SGST @ <span id='lblSGSTaxRate' style='margin-right:2px;' >" + halfRate + " %</span></td> \
                                 <td style='text-align:right;font-size:11px;border-right:1px solid #000;'>&nbsp;</td> \
                                 <td style='text-align:right;font-size:11px;border-right:1px solid #000;'><span id='lblSGSTAmount' style='font-weight:bold;margin-right:2px;'>" + inrFormat(halfTaxableValue.toFixed(2)) + "</span></td> \
                              </tr> \
                              <tr id='rowCGST'> \
                                 <td style='text-align:right;font-size:11px;border-left:1px solid #000;border-right:1px solid #000;font-weight:bold;'>Output CGST @ <span id='lblCGSTaxRate' style='margin-right:2px;'>" + halfRate + " %</span></td> \
                                 <td style='text-align:right;font-size:11px;border-right:1px solid #000;'>&nbsp;</td> \
                                 <td style='text-align:right;font-size:11px;border-right:1px solid #000;'><span id='lblCGSTAmount' style='font-weight:bold;margin-right:2px;'>" + inrFormat(halfTaxableValue.toFixed(2)) + "</span></td> \
                              </tr> \
                              <tr style='border-bottom:1px solid #000;'> \
                                 <td style='text-align:right;font-size:11px;border-left:1px solid #000;border-right:1px solid #000;font-weight:bold;'>&nbsp;</td> \
                                 <td style='text-align:right;font-size:11px;border-right:1px solid #000;'>&nbsp;</td> \
                                 <td style='text-align:right;font-size:11px;border-right:1px solid #000;'>&nbsp;</td> \
                              </tr>";
                TotalAmountWithTax = parseFloat(otherTaxableValue) + parseFloat(halfTaxableValue) + parseFloat(halfTaxableValue);

                var fullServiceGstAmt = parseFloat(otherTaxableValue) * parseFloat(PrintClientDetail[0].GST_PERCENT) / 100;
                var halfServiceGstAmt = parseFloat(fullServiceGstAmt) / 2;

                var GstFirstString = "<tr style='border-bottom:1px solid #000;'> \
                                          <td rowspan='2' width='40%' style='border-right:1px solid #000;border-left:1px solid #000;text-align:center;font-weight:bold;'>HSN/SAC</td> \
                                          <td rowspan='2' width='15%' style='border-right:1px solid #000;text-align:center;font-weight:bold;'>Taxable Value</td> \
                                          <td colspan='2' width='15%' style='border-right:1px solid #000;text-align:center;font-weight:bold;'>Central Tax</td> \
                                          <td colspan='2' width='15%' style='border-right:1px solid #000;text-align:center;font-weight:bold;'>State Tax</td> \
                                          <td rowspan='2' width='15%' style='border-right:1px solid #000;text-align:center;font-weight:bold;'>Total Tax Amount</td> \
                                      </tr> \
                                      <tr style='border-bottom:1px solid #000;'> \
                                          <td style='border-right:1px solid #000;text-align:center;font-weight:bold;'>Rate</td> \
                                          <td style='border-right:1px solid #000;text-align:center;font-weight:bold;'>Amount</td> \
                                          <td style='border-right:1px solid #000;text-align:center;font-weight:bold;'>Rate</td> \
                                          <td style='border-right:1px solid #000;text-align:center;font-weight:bold;'>Amount</td> \
                                      </tr> ";
                var GstSecondString = "";
                if (QuotationRowlist != 0) {
                    $.each(array, function (i, item) {
                        if (item.Is_Taxable == "1") {
                            var fullTaxAmt = parseFloat(item.Amount) * parseFloat(PrintClientDetail[0].GST_PERCENT) / 100;
                            var halfTaxAmt = parseFloat(fullTaxAmt) / 2;
                            GstSecondString += "<tr style='border-bottom:1px solid #000;'> \
                                            <td style='border-right:1px solid #000;border-left:1px solid #000;font-weight:bold;'><span style='margin-left:2px;'>" + item.Particular + "</span></td> \
                                            <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span style='margin-right:2px;'>" + inrFormat(parseFloat(item.Amount)) + "</span></td> \
                                            <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span style='margin-right:2px;'>" + halfRate + " %</span></td> \
                                            <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span style='margin-right:2px;'>" + inrFormat(parseFloat(halfTaxAmt).toFixed(2)) + "</span></td> \
                                            <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span style='margin-right:2px;'>" + halfRate + " %</span></td> \
                                            <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span style='margin-right:2px;'>" + inrFormat(parseFloat(halfTaxAmt).toFixed(2)) + "</span></td> \
                                            <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span style='margin-right:2px;'>" + inrFormat(parseFloat(fullTaxAmt).toFixed(2)) + "</span></td> \
                                      </tr>";
                        }
                    })
                }
                var totTaxableAmt = parseFloat(otherTaxableValue);
                var GstThirdString = "<tr style='border-bottom:1px solid #000;'> \
                                          <td style='border-right:1px solid #000;border-left:1px solid #000;text-align:right;font-weight:bold;'>Total</td> \
                                          <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span id='lblAmount3' style='margin-right:2px;'>" + inrFormat(totTaxableAmt.toFixed(2)) + "</span></td> \
                                          <td style='border-right:1px solid #000;'></td> \
                                          <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span id='lblCGSTAmount3' style='margin-right:2px;'>" + inrFormat(halfTaxableValue.toFixed(2)) + "</span></td> \
                                          <td style='border-right:1px solid #000;'></td> \
                                          <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span id='lblSGSTAmount3' style='margin-right:2px;'>" + inrFormat(halfTaxableValue.toFixed(2)) + "</span></td> \
                                          <td style='border-right:1px solid #000;text-align:right;font-weight:bold;'><span id='lblTotalTaxableValue2' style='margin-right:2px;'>" + inrFormat(TaxableValue.toFixed(2)) + "</span></td> \
                                      </tr>";
                $("#tblGST").append(GstFirstString + GstSecondString + GstThirdString);
            }
            $("#tblParticulars tbody").append(MiddleString + LastString);
            var roundOff = Math.round(TotalAmountWithTax + otherNon_TaxableValue);
            document.getElementById('lblTaxAmountInWords').innerHTML = number2text(TaxableValue);
            document.getElementById('lblTotalAmountWithTax').innerHTML = inrFormat(roundOff.toFixed(2));
            document.getElementById('lblAmountChargeableInWords').innerHTML = number2text(roundOff);
            $('#btnOpenMail').hide();
            $('#ModalPreview').modal('show');
        },
        error: function (respo) {
        }
    })
}
function number2text(value) {
    var fraction = Math.round(frac(value) * 100);
    var f_text = "";

    if (fraction > 0) {
        f_text = " AND " + convert_number(fraction) + " PAISE";
    }

    return convert_number(value) + "" + f_text + " ONLY";
}
function frac(f) {
    return f % 1;
}
function convert_number(number) {
    if ((number < 0) || (number > 999999999)) {
        return "NUMBER OUT OF RANGE!";
    }
    var Gn = Math.floor(number / 10000000);
    number -= Gn * 10000000;
    var kn = Math.floor(number / 100000);
    number -= kn * 100000;
    var Hn = Math.floor(number / 1000);
    number -= Hn * 1000;
    var Dn = Math.floor(number / 100);
    number = number % 100;
    var tn = Math.floor(number / 10);
    var one = Math.floor(number % 10);
    var res = "";

    if (Gn > 0) {
        res += (convert_number(Gn) + " CRORE");
    }
    if (kn > 0) {
        res += (((res == "") ? "" : " ") +
            convert_number(kn) + " LAKH");
    }
    if (Hn > 0) {
        res += (((res == "") ? "" : " ") +
            convert_number(Hn) + " THOUSAND");
    }
    if (Dn) {
        res += (((res == "") ? "" : " ") +
            convert_number(Dn) + " HUNDRED");
    }
    var ones = Array("", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN");
    var tens = Array("", "", "TWENTY", "THIRTY", "FOURTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY");

    if (tn > 0 || one > 0) {
        if (!(res == "")) {
            res += " AND ";
        }
        if (tn < 2) {
            res += ones[tn * 10 + one];
        }
        else {

            res += tens[tn];
            if (one > 0) {
                res += ("-" + ones[one]);
            }
        }
    }

    if (res == "") {
        res = "zero";
    }
    return res;
}
function ClosePreview() {
    $('#ModalPreview').modal('hide');
}
function PrintDiv(divName) {
    var contents = document.getElementById(divName).innerHTML;

    var frame1 = document.createElement('iframe');
    frame1.name = "frame1";
    frame1.style.position = "absolute";
    frame1.style.top = "-1000000px";
    document.body.appendChild(frame1);
    var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
    frameDoc.document.open();
    frameDoc.document.write('<html><head><title>DIV Contents</title>');
    frameDoc.document.write('<link rel="stylesheet" href="http://cdn.datatables.net/1.10.2/css/jquery.dataTables.min.css"></style>');
    frameDoc.document.write('<link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet">');
    frameDoc.document.write('<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">');
    frameDoc.document.write('<link rel="stylesheet" href="/resources/demos/style.css">');
    frameDoc.document.write('</head><body>');
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        document.body.removeChild(frame1);
    }, 500);
    return false;
}