var CPD_ID = 0; var ClientList = [], PackageList = [], TechnologyList = [], ModuleList = [], SubModuleList = [];
$(function () {
    RegBtnSubmitEvt();
    RegDdlClientChangeEvt();
    RegDdlPackageChangeEvt();
    $(".datetimepicker").datepicker({
        dateFormat: 'dd/mm/yy',
        changeMonth: true,
        changeYear: true
    });
    ClientList = JSON.parse($("#hdnClientList").val());
    PackageList = JSON.parse($("#hdnPackageList").val());
    TechnologyList = JSON.parse($("#hdnTechnologyList").val());
    ModuleList = JSON.parse($("#hdnModuleList").val());
    SubModuleList = JSON.parse($("#hdnSubModuleList").val());
    BindClientList();
    BindPackageList();
    Bind_Client_Package_Mappings();
    RegSubmoduleChkEvt();
});
function GetClientPackageModuleMappingModel() {
    var model = {
        "CPD_ID": CPD_ID,
        "CLIENT_ID": $("#ddlClientName").val(),
        "PM_ID": $("#ddlPackageName").val(),
        "VALID_FROM": moment($.trim($("#txtValidFrom").val()), "DD/MM/YYYY").format("MM-DD-YYYY"),
        "VALID_TO": moment($.trim($("#txtValidTo").val()), "DD/MM/YYYY").format("MM-DD-YYYY"),
        "NO_OF_USERS_ALLOWED": $.trim($("#txtNoOfUsers").val()),
        "_CLIENT_PACKAGE_MODULE_MAPPING": null,
    }
    return model;
}
function RegBtnSubmitEvt() {
    $("#btnSubmit").click(function () {
        debugger
        var spinner = $('#loader');
        spinner.show();
        var model = GetClientPackageModuleMappingModel();
        var ModuleList = [];
        $('.chkEmp:checked').each(function () {
            ModuleList.push({
                SMM_ID: $(this).val(),
                MM_ID: $(this).attr('MM_ID'),
            });
        });
        model._CLIENT_PACKAGE_MODULE_MAPPING = ModuleList;
        if (model.CLIENT_ID == "" || model.CLIENT_ID == null) {
            PopupMessage("Warning", "Please Select Client Name");
            spinner.hide();
            return false;
        }
        else if (model.PM_ID == "" || model.PM_ID == null) {
            PopupMessage("Warning", "Please Select Package Name");
            return false;
        }
        else if (model.VALID_FROM == "" || model.VALID_FROM == null) {
            PopupMessage("Warning", "Please Select Valid From");
            spinner.hide();
            return false;
        }
        else if (model.VALID_TO == "" || model.VALID_TO == null) {
            PopupMessage("Warning", "Please Select Valid To");
            spinner.hide();
            return false;
        }
        else if (model.NO_OF_USERS_ALLOWED == "" || model.NO_OF_USERS_ALLOWED == null) {
            PopupMessage("Warning", "Please Select No Of Users");
            spinner.hide();
            return false;
        }
        else if (model._CLIENT_PACKAGE_MODULE_MAPPING.length == 0) {
            PopupMessage("Warning", "Please Select Atleast One Module");
            spinner.hide();
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/ClientPackageMapping/SaveMapping",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                spinner.hide();
                if (response == "SUCCESS") {
                    if (model.CPD_ID == 0)
                        PopupMessage("Success", "Data Save Successfully");
                    else
                        PopupMessage("Success", "Data updated Successfully");
                    ResetMappingForm(1);
                }
                else {
                    if (model.CPD_ID == 0)
                        PopupMessage("Error", "Data Not Saved");
                    else
                        PopupMessage("Error", "Data Not updated");
                }
            },
            error: function (response) {
                spinner.hide();
                if (model.CPD_ID == 0)
                    PopupMessage("Error", "Data Not Saved");
                else
                    PopupMessage("Error", "Data Not Updated");
            }
        });
    });
}
function LoadClientMappingGrid(ClientId) {
    $.ajax({
        type: "POST",
        url: "/ClientPackageMapping/GetClientMappingList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ "CLIENT_ID": ClientId }),
        success: function (response) {
            BindClientModuleMapping(response);
        },
        error: function (response) {
        }
    });
}
function BindClientModuleMapping(data) {
    ResetMappingForm(2);
    if (data != null && data.length > 0) {
        debugger
        CPD_ID = data[0].CPD_ID;
        $("#ddlClientName").val(data[0].CLIENT_ID);
        $("#ddlPackageName").val(data[0].PM_ID);
        $("#txtValidFrom").val(moment(data[0].VALID_FROM).format("DD/MM/YYYY"));
        $("#txtValidTo").val(moment(data[0].VALID_TO).format("DD/MM/YYYY"));
        $("#txtNoOfUsers").val(data[0].NO_OF_USERS_ALLOWED);
        $.each(data, function (i, item) {
            var chkbox =  $("input[type='checkbox'][value='" + item.SMM_ID + "']").prop('checked', true);
            SubModuleChk(chkbox);
        });
    }
}
function ResetMappingForm(flag) {
    CPD_ID = 0;
    if (flag == 1)
        $("#ddlClientName").val("");
    $("#ddlPackageName").val("");
    $("#txtValidFrom").val("");
    $("#txtValidTo").val("");
    $("#txtNoOfUsers").val("");
    $('input:checkbox:checked').prop('checked', false);
}
function RegDdlClientChangeEvt() {
    $("#ddlClientName").change(function () {
        LoadClientMappingGrid($(this).val());
    });
}
function RegDdlPackageChangeEvt() {
    $("#ddlPackageName").change(function () {
        LoadPackageMappingGrid($(this).val());
    });
}
function LoadPackageMappingGrid(PMID) {
    $.ajax({
        type: "POST",
        url: "/ClientPackageMapping/GET_PACKAGE_MODULE_MAPPING",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ "PM_ID": PMID }),
        success: function (response) {
            BindPackageModuleMapping(response);
        },
        error: function (response) {
        }
    });
}
function BindPackageModuleMapping(data) {
    ResetPackageMappingForm(2);
    if (data != null && data.length > 0) {
        debugger
        $.each(data, function (i, item) {
            var chkbox = $("input[type='checkbox'][value='" + item.SMM_ID + "']").prop('checked', true);
            SubModuleChk(chkbox);
        });
    }
}
function ResetPackageMappingForm() {
    $('input:checkbox:checked').prop('checked', false);
}
function Check_Uncheck_SubModule(that, MM_ID) {
    if ($(that).is(":checked"))
        $("#Module" + MM_ID + " :checkbox").prop("checked", true);
    else
        $("#Module" + MM_ID + " :checkbox").prop('checked', false);
}
function SubModuleChk(that) {
    var MM_ID = $(that).attr("MM_ID");
    var allChk = $("#Module" + MM_ID + " :checkbox").length;
    var CheckedChk = $("#Module" + MM_ID + " :checkbox:checked").length;
    if (allChk == CheckedChk)
        $("#ChkAll" + MM_ID).prop("checked", true);
    else
        $("#ChkAll" + MM_ID).prop('checked', false);
}
function BindClientList() {
    $("#ddlClientName").html("");
    $("#ddlClientName").append($('<option></option>').val(null).html("Select Client Name"));
    $.each(ClientList, function (i, data) {
        $("#ddlClientName").append($('<option></option>').val(data.CR_ID).html(data.ORGNIZATIO_NAME));
    });
}
function BindPackageList() {
    $("#ddlPackageName").html("");
    $("#ddlPackageName").append($('<option></option>').val(null).html("Select Package Name"));
    $.each(PackageList, function (i, data) {
        $("#ddlPackageName").append($('<option></option>').val(data.PMID).html(data.PACKAGE_NAME));
    });
}
function Bind_Client_Package_Mappings() {
    debugger
    var spinner = $('#loader');
    spinner.show();
    var WebModules = ModuleList.filter(function (filtered) {
        return filtered.TM_ID == 1;
    });
    $.each(WebModules, function (i, data) {
        var TechdnyContent = "";
        TechdnyContent += "<div>"
        TechdnyContent += "<a href=#Module" + data.MM_ID + " class='btn form-control' data-toggle='collapse' style='background-color:#6d6aa2;border:1px solid #6d6aa2;font-weight:bold; color:white;text-align:left!important'>" + (i + 1) + ".&nbsp;&nbsp; <input type='checkbox' onclick='event.stopPropagation();Check_Uncheck_SubModule(this," + data.MM_ID + ")' id='ChkAll" + data.MM_ID + "' />&nbsp;&nbsp;  " + data.MODULE_NAME + "</a>"
        TechdnyContent += "</div>"
        $("#tab_WebApplication").append(TechdnyContent);
        TechdnyContent = "";
        var FilteredModuleList = SubModuleList.filter(function (filtered) {
            return filtered.MM_ID == data.MM_ID;
        });
        TechdnyContent += "<div id=Module" + data.MM_ID + " class='collapse in' style='padding:10px!important;'>"
       TechdnyContent += "<br/>"
        $.each(FilteredModuleList, function (j, SMlist) {
            TechdnyContent += "<div class='col-md-3'>"
            TechdnyContent += "<div class='form-group'><label>"
            TechdnyContent += "<input type='checkbox' class='chkEmp' value=" + SMlist.SMM_ID + " MM_ID=" + data.MM_ID + " />"
            TechdnyContent += "<span class='lbl' style='color:black;'> " + SMlist.SUB_MODULE_NAME + "</span>"
            TechdnyContent += "</label>"
            TechdnyContent += "</div></div>"
        });
        TechdnyContent += " </div >"
        $("#tab_WebApplication").append(TechdnyContent);
    });
    var WebModules = ModuleList.filter(function (filtered) {
        return filtered.TM_ID == 2;
    });
    $.each(WebModules, function (i, data) {
        var TechdnyContent = "";
        TechdnyContent += "<div>"
        TechdnyContent += "<a href=#Module" + data.MM_ID + " class='btn form-control' data-toggle='collapse' style='background-color:#6d6aa2;border:1px solid #6d6aa2;font-weight:bold; color:white;text-align:left!important'>" + (i + 1) + ".&nbsp;&nbsp; <input type='checkbox' onclick='event.stopPropagation();Check_Uncheck_SubModule(this," + data.MM_ID + ")' id='ChkAll" + data.MM_ID + "' />&nbsp;&nbsp; " + data.MODULE_NAME + "</a>"
        TechdnyContent += "</div>"
        $("#divmobMobules").append(TechdnyContent);
        TechdnyContent = "";
        var FilteredModuleList = SubModuleList.filter(function (filtered) {
            return filtered.MM_ID == data.MM_ID;
        });
        TechdnyContent += "<div id=Module" + data.MM_ID + " class='collapse in' style='padding:10px!important;'>"
        $.each(FilteredModuleList, function (j, SMlist) {
            TechdnyContent += "<div class='col-md-3'>"
            TechdnyContent += "<div class='form-group'><label>"
            TechdnyContent += "<input type='checkbox' class='chkEmp' value=" + SMlist.SMM_ID + " MM_ID=" + data.MM_ID + " />"
            TechdnyContent += "<span class='lbl' style='color:black;'> " + SMlist.SUB_MODULE_NAME + "</span>"
            TechdnyContent += "</label>"
            TechdnyContent += "</div></div>"
        });
        TechdnyContent += " </div >"
        $("#divmobMobules").append(TechdnyContent);
    });
    spinner.hide();
}
function RegSubmoduleChkEvt() {
    $(".chkEmp").click(function () {
        debugger
        SubModuleChk($(this));
    });
}