var SelectedFY = null; var GridCustomerDetails = null;

$(function () {
    $(document).ready(function () {
        RegddlFinancialYearChange();
        LoadFinancialYearList();
    });
});

function LoadFinancialYearList() {
    var ddlFy = $("#ddlFinancialYear").empty();
    var StartYear = 2020;
    let CurrentYear = (new Date()).getFullYear();
    let CurrentFY = (new Date()).getMonth() + 1 > 3 ? CurrentYear : CurrentYear - 1;
    let YearCount = CurrentFY - StartYear;
    for (let i = 0; i <= YearCount; i++) {
        var _FY = (CurrentFY - i) + '-' + ((CurrentYear - i) + 1);//.toString().substring(2, 4);
        if (i == 0)
            SelectedFY = _FY
        ddlFy.append($("<option></option>").val(_FY).text("FINANCIAL YEAR " + _FY))
    }
    LoadDashBoardData();
}

function RegddlFinancialYearChange() {
    $("#ddlFinancialYear").change(function () {
        SelectedFY = $(this).val();
        LoadDashBoardData();
    });
}

function LoadDashBoardData() {
    try {
        if (SelectedFY != null && SelectedFY != "") {
            $.ajax({
                type: "POST",
                url: "/Home/GetDashboardData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ "FinancialYear": $("#ddlFinancialYear").val() }),
                success: function (response) {
                    if (response != null) {
                        BindDashboardCounts(response.DashboardCount);
                        TotalRegisteredCustomersByPackage(response.PieChartSummary, response.PieChartData);
                        Total_Quot_Prof_Invoice_Receipt_ColumnContainer(response.StackedChartData);
                        Total_Quot_Prof_Amount_DonutContainer(response.DonutChartSummary, response.DonutChartData);
                        Total_Quot_Prof_Invoice_Receipt_LineContainer(response.LineChartData);
                        DisplayCustomerDetailsContainer(response.DashboardClientData);
                    }
                },
                error: function (response) {
                    PopupMessage("Error", "Unable to fetch data");
                }
            })
        } else {
            PopupMessage("Warning", "Please Select Financial Year");
            $("#ddlFinancialYear").focus();
        }
    } catch (ex) {
        PopupMessage("Error", "Unable to fetch data");
    }
}

function inrFormat(nStr) {
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

function BindDashboardCounts(DashboardCounts) {
    if (DashboardCounts != null) {
        $("#spanCustomerNumber").text(DashboardCounts.TOTAL_CUSTOMERS);
        $("#spanCustomerFy").text("FY " + SelectedFY);
        $("#spanQuotProformaNumber").text(DashboardCounts.TOTAL_PROFORMA);
        $("#spanQuotProformaAmount").text("₹ " + inrFormat(DashboardCounts.TOTAL_PROFORMA_AMOUNT.toFixed(2)));
        $("#spanTaxInvoiceNumber").text(DashboardCounts.TOTAL_INVOICES);
        $("#spanTaxInvoiceAmount").text("₹ " + inrFormat(DashboardCounts.TOTAL_INVOICE_AMOUNT.toFixed(2)));
        $("#spanReceiptsNumber").text(DashboardCounts.TOTAL_RECEIPTS);
        $("#spanReceiptsAmount").text("₹ " + inrFormat(DashboardCounts.TOTAL_RECEIPT_AMOUNT.toFixed(2)));
    }
}

function TotalRegisteredCustomersByPackage(PieChartSummary, PieChartData) {
    var PieChart_DrillDownData = [];
    $(PieChartSummary).each(function (i, head) {
        var data = [];
        var details = PieChartData.filter(function (item) { return item.PACKAGE_NAME == head.name });
        $(details).each(function (i, line) {
            data.push([line.MonthName, line.Total_Customer]);
        });
        PieChart_DrillDownData.push({
            "name": head.name,
            "id": head.name,
            "data": data
        });
    });

    Highcharts.chart('CustomerPackagePieContainer', {
        chart: {
            type: 'pie',
            //plotBackgroundColor: null,
            //plotBorderWidth: null,
            //plotShadow: false
        },
        title: {
            text: 'Total Registered Customers By Package'
        },
        accessibility: {
            announceNewData: {
                enabled: true
            },
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            series: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.name}: {point.y:.0f}'
                },
                showInLegend: true
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.0f}</b> of total<br/>'
        },
        series: [
            {
                name: "Packages",
                colorByPoint: true,
                data: PieChartSummary
            }
        ],
        drilldown: {
            series: PieChart_DrillDownData
        }
    });
}

function Total_Quot_Prof_Invoice_Receipt_ColumnContainer(StackedChartData) {
    var category = []; var Quot = []; var Invoice = []; var Receipt = [];
    $(StackedChartData).each(function (i, item) {
        category.push(item.MonthName);
        Quot.push(item.Total_Quot_Prof);
        Invoice.push(item.Total_Invoice);
        Receipt.push(item.Total_Receipts);
    });

    Highcharts.chart('Quot_Prof_Invoice_Receipt_ColumnContainer', {
        chart: {
            type: 'column'
        },
        //colors: ['#FF0000', '#00FF00', '#FFFF00'],
        title: {
            text: 'Stacked column chart'
        },
        xAxis: {
            categories: category
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Total Quot/Prof/Tax Invoice/Receipt',
                style: {
                    color: 'black'
                }
            },
            labels: {
                style: {
                    color: 'black'
                }
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: ( // theme
                        Highcharts.defaultOptions.title.style &&
                        Highcharts.defaultOptions.title.style.color
                    ) || 'black'
                }
            }
        },
        legend: {
            align: 'center',
            x: 0,
            verticalAlign: 'bottom',
            y: 23,
            floating: false,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || 'white',
            borderColor: '#CCC',
            borderWidth: 0,
            shadow: false
        },
        tooltip: {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: [{
            name: 'Quotation/Proforma',
            data: Quot
        }, {
            name: 'Tax Invoice',
            data: Invoice
        }, {
            name: 'Receipt',
            data: Receipt
        }],
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }
    });
}

function Total_Quot_Prof_Amount_DonutContainer(DonutChartSummary, DonutChartData) {
    var DonutChart_DrillDownData = [];
    $(DonutChartSummary).each(function (i, head) {
        var data = [];
        var details = DonutChartData.filter(function (item) { return item.PACKAGE_NAME == head.name });
        $(details).each(function (i, line) {
            data.push([line.MonthName, line.Total_Amount]);
        });
        DonutChart_DrillDownData.push({
            "name": head.name,
            "id": head.name,
            "data": data
        });
    });

    Highcharts.chart('Quot_Prof_Amount_DonutContainers', {
        chart: {
            type: 'pie',
            //plotBackgroundColor: null,
            //plotBorderWidth: null,
            //plotShadow: false
        },
        title: {
            text: 'Total Registered Customers By Package'
        },
        accessibility: {
            announceNewData: {
                enabled: true
            },
            point: {
                valueSuffix: '₹'
            }
        },
        plotOptions: {
            series: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.name}: ₹{point.y:.2f}'
                },
                showInLegend: true
            }
        },
        tooltip: {
            headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
            pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>₹{point.y:.2f}</b> of total<br/>'
        },
        series: [
            {
                name: "Packages",
                colorByPoint: true,
                data: DonutChartSummary
            }
        ],
        drilldown: {
            series: DonutChart_DrillDownData
        }
    });
}

function Total_Quot_Prof_Invoice_Receipt_LineContainer(LineChartData) {
    var category = []; var QuotAmt = []; var InvoiceAmt = []; var ReceiptAmt = [];
    $(LineChartData).each(function (i, item) {
        category.push(item.MonthName);
        QuotAmt.push(item.Total_Quot_Prof);
        InvoiceAmt.push(item.Total_Invoice);
        ReceiptAmt.push(item.Total_Receipts);
    });

    Highcharts.chart('Quot_Prof_Invoice_Receipt_LineContainers', {
        //title: {
        //    text: 'Solar Employment Growth by Sector, 2010-2016'
        //},
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'spline'
        },
        yAxis: {
            title: {
                text: 'Total Quot/Prof/Tax Invoice/Receipt Amount'
            },
            labels: {
                format: '₹{value:.0f}'
            }
        },
        xAxis: {
            title: {
                text: 'Months'
            },
            categories: category
        },
        legend: {
            align: 'center',
            x: 0,
            verticalAlign: 'bottom',
            y: 23,
            floating: false,
            backgroundColor:
                Highcharts.defaultOptions.legend.backgroundColor || 'white',
            borderColor: '#CCC',
            borderWidth: 0,
            shadow: false
        },
        plotOptions: {
            series: {
                label: {
                    connectorAllowed: false
                },
                //pointStart: 2010
            }
        },
        //tooltip: {
        //    headerFormat: '<b>{point.x}</b><br/>',
        //    pointFormat: '<li style="color:{point.color}">{series.name}</li>: <b>₹{point.y:.2f}</b>'
        //},
        series: [{
            name: 'Quotation/Proforma Amount',
            data: QuotAmt
        }, {
            name: 'Tax Invoice Amount',
            data: InvoiceAmt
        }, {
            name: 'Receipt Amount',
            data: ReceiptAmt
        }],
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        }
    });
}

function DisplayCustomerDetailsContainer(DashboardClientData) {
    if (GridCustomerDetails != null) {
        GridCustomerDetails.destroy();
    }
    GridCustomerDetails = $('#CustomerDetailsGrid').DataTable({
        data: DashboardClientData,
        scrollX: true,
        columns: [
            { title: "Sr.No.", width: 20, render: function (data, type, row, meta) { return meta.row + meta.settings._iDisplayStart + 1; } },
            { title: "Customer Name", data: "ORGNIZATIO_NAME" },
            //{ title: "Customer Name", data: "CONTACT_PERSON_NAME" },
            //{ title: "Corporate Id", data: "CORPORATE_ID" },
            { title: "Package Name", data: "PACKAGE_NAME" },
            //{ title: "VALID FROM", data: "VALID_FROM" },
            { title: "Expiry Date", data: "VALID_TO", render: function (data, type, row, meta) { return (moment(row.VALID_TO).format("DD-MMM-YYYY")); } },
            { title: "Validity", data: "VALIDITY" },
            { title: "Quot/Prof Amount", data: "Quot_Prof_Amount", render: function (data, type, row, meta) { return "₹ " + inrFormat(parseFloat(row.Quot_Prof_Amount).toFixed(2)); } },
            { title: "Receipt Amount", data: "Receipt_Amount", render: function (data, type, row, meta) { return "₹ " + inrFormat(parseFloat(row.Receipt_Amount).toFixed(2)); } },
            { title: "Tax Invoice Amount", data: "Invoice_Amount", render: function (data, type, row, meta) { return "₹ " + inrFormat(parseFloat(row.Invoice_Amount).toFixed(2)); } },
        ],
        fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            debugger
            if (aData.EXPIRES_IN > 30) {
                $(nRow).find('td:eq(4)').css({ 'background-color': 'green', 'font-weight': 'bold', 'color': 'black' });
                //$('td', nRow).css({ 'background-color': 'rgb(16 183 16)', 'font-weight': 'bold', 'color': 'black' });
            } else if (aData.EXPIRES_IN < 30 && aData.EXPIRES_IN > 10) {
                $(nRow).find('td:eq(4)').css({ 'background-color': 'yellow', 'font-weight': 'bold', 'color': 'black' });
                //$('td', nRow).css({ 'background-color': 'yellow', 'font-weight': 'bold', 'color': 'black' });
            } else if (aData.EXPIRES_IN < 10) {
                $(nRow).find('td:eq(4)').css({ 'background-color': 'red', 'font-weight': 'bold', 'color': 'black' });
                //$('td', nRow).css({ 'background-color': 'red', 'font-weight': 'bold', 'color': 'black' });
            }
        }
    });
}

