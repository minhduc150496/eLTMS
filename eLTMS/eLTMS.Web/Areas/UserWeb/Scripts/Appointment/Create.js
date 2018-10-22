﻿/*
    Author: DucBM
*/

CONFIG = {
    PATIENT_ID: 1, // hard code for dev-ing
    SAMPLE_DTOS_KEY: "SAMPLE_DTOS",
};

var Utils = {
    formatTimeShort: function (time) {
        var hour = Math.floor(time / 60 / 60);
        var min = Math.floor(time / 60) % 60;
        if (min < 10) {
            min = "0" + min;
        }
        return hour + ':' + min;
    }, // end function
    formatTimeLong: function (time) {
        var hour = Math.floor(time / 60 / 60);
        var min = Math.floor(time / 60) % 60;
        var sec = time % 60;
        if (min < 10) {
            min = "0" + min;
        }
        if (sec < 10) {
            sec = "0" + sec;
        }
        return hour + ':' + min + ":" + sec;
    } // end function
};

var Model = {
    sampleDtos: {},
    appointmentDto: {}
}

var Controller = {
    init: function () {
        Controller.registerEvent();
    }, // end Action
    registerEvent: function () {

        $(document).ready(function () {
            $("#step-2").hide(0);
            Controller.renderLabTestList();
        });

        $("#btn-next").click(function () {
            Controller.renderStep2Html(Model.sampleDtos);
            var duration = 200;
            $("#step-1").fadeOut(duration, function () {
                $("#step-2").fadeIn(duration);
            });
        });

        $("#btn-prev").click(function () {
            var duration = 200;
            $("#step-2").fadeOut(duration, function () {
                $("#step-1").fadeIn(duration);
            });
        });

        $("#btn-submit").click(function () {
            Model.appointmentDto = {
                PatientId: CONFIG.PATIENT_ID,
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
                    sampleDuration = parseInt(sampleDuration);
                    var startTime = $(el_Sample).find(":selected").val(); // double
                    startTime = parseInt(startTime);
                    var finishTime = startTime + sampleDuration;
                    // format to 07:00
                    startTime = Utils.formatTimeShort(startTime);
                    finishTime = Utils.formatTimeShort(finishTime);
                    var gettingDate = $(el_Sample).find("*[type='date']").val();
                    // assign to sampleGetting Dto
                    var sampleGettingDto = {
                        SampleId: sampleId,
                        LabTestIds: [],
                        GettingDate: gettingDate,
                        StartTime: startTime,
                        FinishTime: finishTime
                    };

                    var el_LabTests = $("#step-1-form [data-sampleid='" + sampleId + "'] input[data-labtestid]:checked");
                    //console.log(el_LabTests);
                    if (el_LabTests != null) {
                        for (var j = 0; j < el_LabTests.length; j++) {
                            var labTestId = $(el_LabTests[j]).data("labtestid");
                            //console.log(labTestId);
                            sampleGettingDto.LabTestIds.push(labTestId);
                        }
                    }
                    Model.appointmentDto.SampleGettingDtos.push(sampleGettingDto);
                }
            }
            // ajax for create new appointment
            var jsonData = JSON.stringify(Model.appointmentDto);
            Controller.sendToServer(jsonData);
        });

    }, // end Action
    renderStep1Html: function (sampleDtos) {
        // render step 1 - choosing Samples & LabTests
        var sampleHtml = "";
        if (sampleDtos != null) {
            for (var i = 0; i < sampleDtos.length; i++) {
                var sampleDto = sampleDtos[i];
                // append Sample Title: "1. Mau"
                sampleHtml += "<h3>" + (i + 1) + ". " + sampleDto.SampleName + "</h3>\n";
                sampleHtml += '<div data-sampleid="' + sampleDto.SampleId + '" class="row">\n';
                if (sampleDto.LabTests != null) {
                    for (var j = 0; j < sampleDto.LabTests.length; j++) {
                        // append Lab Tests in Sample
                        var labTest = sampleDto.LabTests[j];
                        sampleHtml += '<div class="col-md-3">' +
                            '<div class="pretty p-icon p-smooth p-bigger">' +
                            '<input type = "checkbox" data-labtestid="' + labTest.LabTestId + '" />' +
                            '<div class="state p-danger">' +
                            '<i class="icon fa fa-check"></i>' +
                            '<label>' + labTest.LabTestName + '</label>' +
                            '</div>' +
                            '</div >' +
                            '</div>\n';
                    }
                }
                sampleHtml += '</div>\n';
            }
        }
        $("#step-1-form").append(sampleHtml);
        // end rendering step 1
    }, // end Action
    renderStep2Html: function (sampleDtos) {
        $("#step-2-form").html("");
        // render step 2 - choosing Samples & LabTests
        var sampleHtml = "";
        if (sampleDtos != null) {
            for (var i = 0; i < sampleDtos.length; i++) {
                var sampleDto = sampleDtos[i];
                var sampleId = sampleDto.SampleId;
                var checkedLabTests = $("*[data-sampleid='" + sampleId + "'] input:checked");
                if (checkedLabTests == null || checkedLabTests.length == 0) {
                    continue;
                }
                // append Sample Title: "1. Mau"
                sampleHtml += "<h3>" + (i + 1) + ". " + sampleDto.SampleName + "</h3>\n";
                sampleHtml += '<div data-sampleid="' + sampleDto.SampleId + '" ' +
                    'data-sample-duration="' + sampleDto.SampleDuration + '" ' +
                    'data-open-time="' + sampleDto.OpenTime + '" ' +
                    'data-close-time="' + sampleDto.CloseTime + '">\n';
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
                sampleHtml += '<option value="">-- Vui lòng chọn một ca --</option>';
                var sampleDuration = sampleDto.SampleDuration;
                for (var time = sampleDto.OpenTime; time + sampleDuration <= sampleDto.CloseTime; time += 2 * sampleDuration) {
                    // print slots for sample
                    var startTime = Utils.formatTimeShort(time);
                    var finishTime = Utils.formatTimeShort(time + sampleDuration);
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
    },
    renderLabTestList: function () {
        // get all sampleDtos and LabTests
        var sSampleDtos = localStorage.getItem(CONFIG.SAMPLE_DTOS_KEY);
        if (sSampleDtos == null) {
            $.ajax({
                url: "/api/sample/get-all"
            }).success(function (data) {
                //console.log(data);
                Model.sampleDtos = data; // CONFIG
                sSampleDtos = JSON.stringify(data);
                // save to local storage
                localStorage.setItem(CONFIG.SAMPLE_DTOS_KEY, sSampleDtos);
                // render UI
                Controller.renderStep1Html(Model.sampleDtos);
            }) // end AJAX settings
        } else {
            // get data
            Model.sampleDtos = JSON.parse(sSampleDtos);
            // render UI
            Controller.renderStep1Html(Model.sampleDtos);
        }
    }, // end Action
    sendToServer: function (jsonData) {
        $("#processing-modal").modal('show');
        $.ajax({
            method: "POST",
            contentType: "application/json",
            url: "/api/appointment/create",
            dataType: "JSON",
            data: jsonData,
        }).success(function (data) {
            $("#processing-modal").on('shown.bs.modal', function () {
                $("#processing-modal").modal('hide');
            });
            $("#processing-modal").modal('hide');
            $("#processing-modal").on("hidden.bs.modal", function () {
                if (data.Success == true) {
                    $("#success-modal").modal({
                        show: true
                    });
                } else {
                    $("#fail-modal").modal({
                        show: true
                    });
                }
            });
        });
    }, // end Action
}
Controller.init();