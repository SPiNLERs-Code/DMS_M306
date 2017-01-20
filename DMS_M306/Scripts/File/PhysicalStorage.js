var physicalStorageValue = $("#physicalStorageSelction").val()

if (physicalStorageValue != 1 && physicalStorageValue != "PhysicalStorage")
{
    $("#physicalStorageInfos").hide();
    $("#fileToUploadFields").show();
    $("#fileTypeInfo").show();
} else {
    $("#physicalStorageInfos").show();
    $("#fileToUploadFields").hide();
    $("#fileTypeInfo").hide();
}

$("#physicalStorageSelction").change(function () {

    if ($("#physicalStorageSelction").val() == 1) {
        $("#physicalStorageInfos").show("slow");
        $("#fileToUploadFields").hide("slow");
    } else {
        $("#physicalStorageInfos").hide("slow");
        $("#fileToUploadFields").show("slow");
    }
})

