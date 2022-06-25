var PMM_ID = 0; var PackageList = [], TechnologyList = [], ModuleList = [], SubModuleList = [];
$(function () {
    RegBtnSubmitEvt();
    RegDdlPackageChangeEvt();
    PackageList = JSON.parse($("#hdnPackageList").val());
    TechnologyList = JSON.parse($("#hdnTechnologyList").val());
    ModuleList = JSON.parse($("#hdnModuleList").val());
    SubModuleList = JSON.parse($("#hdnSubModuleList").val());
    BindPackageList();
    BindMappings();
    RegSubmoduleChkEvt();
});

function RegBtnSubmitEvt() {
    $("#btnSubmit").click(function () {
        var spinner = $('#loader');
        spinner.show();
        var PMID = $("#ddlPackageName").val();
        if (PMID == "" || PMID == null) {
            PopupMessage("Warning", "Please Select Package Name");
            spinner.hide();
            return false;
        }
        var ModuleList = [];
        $('.chkEmp:checked').each(function () {
            debugger
            ModuleList.push({
                PMM_ID: PMM_ID,
                PMID: PMID,
                SMM_ID: $(this).val(),
                MM_ID: $(this).attr('MM_ID'),
            });
        });
        if (ModuleList.length == 0) {
            PopupMessage("Warning", "Please Select Atleast One Module");
            spinner.hide();
            return false;
        }
        $.ajax({
            type: "POST",
            url: "/PACKAGE_MODULE_MAPPINGS/SaveMapping",
            data: JSON.stringify(ModuleList),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                spinner.hide();
                if (response == "SUCCESS") {
                    if (PMM_ID == 0)
                       
                        PopupMessage("Success", "Data Save Successfully");
                    else
                        PopupMessage("Success", "Data Updated Successfully");
                    ResetMappingForm(1);
                }
                else {
                    if (PMM_ID == 0)
                        PopupMessage("Error", "Data Not Saved");
                    else
                        PopupMessage("Error", "Data Not updated");
                }
            },
            error: function (response) {
                spinner.hide();
                if (PMM_ID == 0)
                    PopupMessage("Error", "Data Not Saved");
                else
                    PopupMessage("Error", "Data Not Updated");
            }
        });
    });
}
function RegDdlPackageChangeEvt() {
    $("#ddlPackageName").change(function () {
        debugger
        LoadPackageMappingGrid($(this).val());
    });
}
function LoadPackageMappingGrid(PMID) {
    $.ajax({
        type: "POST",
        url: "/PACKAGE_MODULE_MAPPINGS/GET_PACKAGE_MODULE_MAPPING",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ "PMID": PMID }),
        success: function (response) {
            BindPackageModuleMapping(response);
        },
        error: function (response) {
        }
    });
}
function BindPackageModuleMapping(data) {
    ResetMappingForm(2);
    if (data != null && data.length > 0) {
        debugger
        PMM_ID = data[0].PMM_ID;
        $("#ddlPackageName").val(data[0].PMID);
        $.each(data, function (i, item) {
            var chkbox = $("input[type='checkbox'][value='" + item.SMM_ID + "']").prop('checked', true);
            SubModuleChk(chkbox);
        });
    }
}
function ResetMappingForm(flag) {
    PMM_ID = 0;
    if (flag == 1)
    $("#ddlPackageName").val("");
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
function BindPackageList() {
    $("#ddlPackageName").html("");
    $("#ddlPackageName").append($('<option></option>').val(null).html("Select Package Name"));
    $.each(PackageList, function (i, data) {
        $("#ddlPackageName").append($('<option></option>').val(data.PMID).html(data.PACKAGE_NAME));
    });
}
function BindMappings() {
    var spinner = $('#loader');
    spinner.show();
    var WebModules = ModuleList.filter(function (filtered) {
        return filtered.TM_ID == 1;
    });
    $.each(WebModules, function (i, data) {
        var TechdnyContent = "";
        TechdnyContent += "<div>"
        TechdnyContent += "<a href=#Module" + data.MM_ID + " class='btn form-control' data-toggle='collapse' style='background-color:#6d6aa2;border:1px solid #6d6aa2;font-weight:bold; color:white;text-align:left!important'>  " + (i + 1) + ".&nbsp;&nbsp; <input type='checkbox' onclick='event.stopPropagation();Check_Uncheck_SubModule(this," + data.MM_ID + ")' id='ChkAll" + data.MM_ID + "' />&nbsp;&nbsp;" + data.MODULE_NAME + "</a>"
        TechdnyContent += "</div>"
        $("#tab_WebApplication").append(TechdnyContent);
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