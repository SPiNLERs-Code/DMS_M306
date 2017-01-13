
if($("#physicalStorageSelction").val() !== 1)
{
    $("#physicalStorageInfos").hide();
}

$("#physicalStorageSelction").change(function () {

    if ($("#physicalStorageSelction").val() === 1) {
        $("#physicalStorageInfos").show("slow");
    } else {
        $("#physicalStorageInfos").hide("slow");
    }
})