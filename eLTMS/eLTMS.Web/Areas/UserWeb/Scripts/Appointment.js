GLOBAL = {};
Utils = {};
GLOBAL.START_HOUR = 4;
GLOBAL.END_HOUR = 14;
GLOBAL.PATIENT_ID = 1; // hard code for dev-ing

$(document).ready(function () {
    $("#step-2").hide(0);
    $("#btn-next").click(function () {
        renderStep2Html(GLOBAL.sampleDtos);
        var duration = 200;
        $("#step-1").fadeOut(duration, function () {
            $("#step-2").fadeIn(duration);
        });
    })
    $("#btn-prev").click(function () {
        var duration = 200;
        $("#step-2").fadeOut(duration, function () {
            $("#step-1").fadeIn(duration);
        });
    })

    // btn submit
    $("#btn-submit").click(function () {
        GLOBAL.appointmentDto = {
            PatientId: GLOBAL.PATIENT_ID,
            SampleGettingDtos: []
        };
        var el_Samples = $("#step-2-form *[data-sampleid]");
        if (el_Samples != null) {
            for (var i = 0; i < el_Samples.length; i++) {
                // create new SampleGetingDto
                var el_Sample = el_Samples[i];
                // get date, start time & finish time
                var sampleId = $(el_Sample).data("sampleid");
                var sampleDuration = $(el_Sample).data("sample-duration");
                sampleDuration = parseFloat(sampleDuration);
                var startTime = $(el_Sample).find(":selected").val(); // double
                startTime = parseFloat(startTime);
                var finishTime = startTime + sampleDuration / 60;
                var sDate = $(el_Sample).find("*[type='date']").val();
                startTime = sDate + " " + Utils.formatTime(startTime);
                finishTime = sDate + " " + Utils.formatTime(finishTime);// + finishTime;
                // assign to sampleGetting Dto
                var sampleGettingDto = {
                    SampleId: sampleId,
                    LabTestIds: [],
                    StartTime: startTime,
                    FinishTime: finishTime
                };

                var el_LabTests = $("#step-1-form [data-sampleid='" + sampleId + "'] input[data-labtestid]:checked");
                console.log(el_LabTests);
                if (el_LabTests != null) {
                    for (var j = 0; j < el_LabTests.length; j++) {
                        var labTestId = $(el_LabTests[j]).data("labtestid");
                        console.log(labTestId);
                        sampleGettingDto.LabTestIds.push(labTestId);
                    }
                }
                GLOBAL.appointmentDto.SampleGettingDtos.push(sampleGettingDto);
            }
        }
        // ajax for create new appointment
        var jsonData = JSON.stringify(GLOBAL.appointmentDto);
        console.log(jsonData);
        $.ajax({
            method: "POST",
            contentType: "application/json",
            url: "/api/appointment/create",
            dataType: "JSON",
            data: jsonData,
        }).success(function (data) {
            if (data == true) {
                $("#success-modal").modal({
                    show: true
                });
            }
        })
    })

    // ajax for get all sampleDtos and labtests
    $.ajax({
        url: "/api/sample/get-all"
    }).success(function (data) {
        GLOBAL.sampleDtos = data; // global
        //console.log(data);
        renderStep1Html(GLOBAL.sampleDtos);
    }) // end AJAX settings


}) // end ready

function renderStep1Html(sampleDtos) {
    // render step 1 - choosing Samples & LabTests
    var sampleHtml = "";
    if (sampleDtos != null) {
        for (var i = 0; i < sampleDtos.length; i++) {
            var sampleDto = sampleDtos[i];
            // append Sample Title: "1. Mau"
            sampleHtml += "<h3>" + (i + 1) + ". " + sampleDto.sampleName + "</h3>\n";
            sampleHtml += '<div data-sampleid="' + sampleDto.sampleId + '">\n';
            if (sampleDto.labTests != null) {
                for (var j = 0; j < sampleDto.labTests.length; j++) {
                    // append Lab Tests in Sample
                    var labTest = sampleDto.labTests[j];
                    sampleHtml += '<label><input type="checkbox" data-labtestid="' + labTest.LabTestId + '" /> ';
                    sampleHtml += labTest.LabTestName + '</label>\n';
                    sampleHtml += '<br>\n';
                }
            }
            sampleHtml += '</div>\n';
        }
    }
    $("#step-1-form").append(sampleHtml);
    // end rendering step 1
}

function renderStep2Html(sampleDtos) {
    $("#step-2-form").html("");
    // render step 2 - choosing Samples & LabTests
    var sampleHtml = "";
    if (sampleDtos != null) {
        for (var i = 0; i < sampleDtos.length; i++) {
            var sampleDto = sampleDtos[i];
            var sampleId = sampleDto.sampleId;
            var checkedLabTests = $("*[data-sampleid='" + sampleId + "'] input:checked");
            if (checkedLabTests == null || checkedLabTests.length == 0) {
                continue;
            }
            // append Sample Title: "1. Mau"
            sampleHtml += "<h3>" + (i + 1) + ". " + sampleDto.sampleName + "</h3>\n";
            sampleHtml += '<div data-sampleid="' + sampleDto.sampleId + '" data-sample-duration="' + sampleDto.sampleDuration + '">\n';
            var today = new Date();
            var year = today.getFullYear();
            var month = today.getMonth();
            if (month < 10) {
                month = "0" + month;
            }
            var date = today.getDate();
            if (date < 10) {
                date = "0" + date;
            }
            var sToday = "" + year + "-" + month + "-" + date;
            sampleHtml += '<input type="date" value="' + sToday + '" />\n';
            sampleHtml += '<select style="overflow-y: scroll">\n';
            var sampleDuration = sampleDto.sampleDuration / 60;
            for (var time = GLOBAL.START_HOUR; time + sampleDuration <= GLOBAL.END_HOUR; time += 2 * sampleDuration) {
                // print slots for sample
                var startTime = Utils.formatTime(time);
                var finishTime = Utils.formatTime(time + sampleDuration);
                sampleHtml += '<option value="' + time + '">' +
                    startTime + ' - ' + finishTime +
                    '</option>\n';
            }
            sampleHtml += '</select>\n';
            sampleHtml += '</div>\n';
        }
    }
    $("#step-2-form").append(sampleHtml);
    // end rendering step 2
}

Utils.formatTime = function (time) {
    var hour = Math.trunc(time);
    var min = (time - hour) * 60;
    min = Math.round(min);
    if (min == 60) { // khử trường hợp: 7:60
        hour++;
        min = 0;
    }
    if (hour < 10) {
        hour = '0' + hour;
    }
    if (min < 10) {
        min = '0' + min;
    }
    return hour + ':' + min;
} // end function

