
if($("#physicalStorageSelction").val() != 1)
{
    $("#physicalStorageInfos").hide();
    $("#fileToUploadFields").show();
} else {
    $("#physicalStorageInfos").show();
    $("#fileToUploadFields").hide();
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

