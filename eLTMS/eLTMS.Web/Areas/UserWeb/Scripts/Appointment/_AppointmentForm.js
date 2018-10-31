/*
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
            $("#success-modal").modal({
                backdrop: "static",
                keyboard: false,
                show: false,
            });
        });

        $("#btn-next").click(function () {
            // VALIDATION:
            var $checks = $("#step-1-form input[type='checkbox']:checked");
            //console.log($checks);
            var noChecked = $checks.length == 0;
            if (noChecked) { // user did not check any checkbox, not allow to go to step 2
                $("html, body").animate({
                    scrollTop: $("#step-1").offset().top,
                }, 0);
                $("#alert-required-labtests").fadeIn(500);
                return;
            } // end VALIDATION
            // GO TO STEP 2:
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
            // VALIDATION: required fill all fields
            var hasEmptyField = false;
            $("#step-2 [type='date']").each(function (index, element) {
                if ($(this).val() == '') {
                    hasEmptyField = true;
                }
            });
            $("#step-2 select").each(function (index, element) {
                if ($(this).val() == '') {
                    hasEmptyField = true;
                }
            });
            if (hasEmptyField) {
                $("html, body").animate({
                    scrollTop: $("#step-2").offset().top,
                }, 0);
                $("#alert-required-datetime").fadeIn(500);
                return;
            } // end VALIDATION

            Model.appointmentDto = {
                PatientId: CONFIG.PATIENT_ID,
                SampleGettingDtos: []
            };
            // create SampleGettingDtos and assign to Model
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
        }); // end event handler

    }, // end Action
    renderOptions: function () {

    }, // end Action
    renderStep1Html: function (sampleDtos) {
        // render step 1 - choosing Samples & LabTests
        var sampleHtml = "";
        if (sampleDtos != null) {
            for (var i = 0; i < sampleDtos.length; i++) {
                var sampleDto = sampleDtos[i];
                // append Sample Title: "1. Mau"
                sampleHtml += "<div class='sample-box'>";
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
                sampleHtml += '</div>\n';
            }
        }
        $("#step-1-form").append(sampleHtml);
        // end rendering step 1
    }, // end Action
    renderStep2Html: function (sampleDtos, argLastFinishTime) {
        $("#step-2-form").html("");
        // render step 2 - choosing Samples & LabTests
        var sampleHtml = "";
        var firstSelect = "id='first-select'";
        if (sampleDtos != null) {
            var lastFinishTime = 0;
            if (argLastFinishTime != null && argLastFinishTime != undefined) {
                lastFinishTime = argLastFinishTime;
            }
            for (var i = 0; i < sampleDtos.length; i++) {
                var sampleDto = sampleDtos[i];
                var sampleId = sampleDto.SampleId;
                var checkedLabTests = $("*[data-sampleid='" + sampleId + "'] input:checked");
                if (checkedLabTests == null || checkedLabTests.length == 0) {
                    continue;
                }
                // append Sample Title: "1. Mau"
                sampleHtml += "<h3>" + sampleDto.SampleName + "</h3>\n";
                sampleHtml += '<div data-sampleid="' + sampleDto.SampleId + '" ' +
                    'data-sample-duration="' + sampleDto.SampleDuration + '" ' +
                    'data-open-time="' + sampleDto.OpenTime + '" ' +
                    'data-close-time="' + sampleDto.CloseTime + '">\n';
                var today = new Date();
                today.setDate(today.getDate() + 1);
                var year = today.getFullYear();
                var month = today.getMonth()+1;
                if (month < 10) {
                    month = "0" + month;
                }
                var date = today.getDate();
                if (date < 10) {
                    date = "0" + date;
                }
                var sToday = "" + year + "-" + month + "-" + date;
                sampleHtml += '<input type="date" value="' + sToday + '" />\n';
                sampleHtml += '<select style="overflow-y: scroll" ' + firstSelect + '>\n';
                sampleHtml += '<option value="">-- Vui lòng chọn một ca --</option>';
                var sampleDuration = sampleDto.SampleDuration;
                var firstOption = true;
                for (var time = sampleDto.OpenTime; time + sampleDuration <= sampleDto.CloseTime; time += 2 * sampleDuration) {
                    // print slots for sample
                    var startTime = Utils.formatTimeShort(time);
                    var finishTime = Utils.formatTimeShort(time + sampleDuration);
                    var selected = "";
                    if (time > lastFinishTime) {
                        if (firstOption) {
                            selected = "selected";
                            firstOption = false;
                            lastFinishTime = time + sampleDuration;
                        }
                    }
                    sampleHtml += '<option value="' + time + '" ' + selected + ' >' +
                        startTime + ' - ' + finishTime +
                        '</option>\n';
                }
                sampleHtml += '</select>\n';
                sampleHtml += '</div>\n';
                if (firstSelect != "") {
                    firstSelect = ""
                }
            }
        }
        $("#step-2-form").append(sampleHtml);
        
        var selects = $("#step-2-form select");
        for (var i = 0; i < selects.length; i++) {
            var options = $(selects[i]).children("option");
            var sampleDuration = sampleDtos[i].SampleDuration;
            for (var k = 1; k < options.length; k++) {
                var time = parseInt($(options[k]).val());
                var bDisable = false;
                for (var j = 0; j < selects.length; j++) {
                    if (i != j) {
                        var start = parseInt($(selects[j]).children("option:selected").val());
                        var end = start + sampleDtos[j].SampleDuration;
                        var IsIntersect = function(A, B, a, b) {
                            var boundary = Math.max(B, b) - Math.min(A, a);
                            var sum = (B - A) + (b - a);
                            return boundary <= sum;
                        }
                        if (IsIntersect(start, end, time, time + sampleDuration)) {
                            bDisable = true;
                            //console.log("dis")
                        }
                    }
                }
                if (bDisable) {
                    $(options[k]).attr("disabled", "disabled");
                } else {
                    $(options[k]).removeAttr("disabled");
                }
            }
        }

        $("#first-select").on("change", function () {
            var value = $(this).val();
            Controller.renderStep2Html(sampleDtos, value - 1);
        })

        // end rendering step 2
    },
    renderLabTestList: function () {
        // get all sampleDtos and LabTests
        var sSampleDtos = localStorage.getItem(CONFIG.SAMPLE_DTOS_KEY);
        sSampleDtos = null; // TEMPORARILY
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
        // show processing popup
        $("#processing-modal").modal('show');
        var openingProcessingModal = true;
        $("#processing-modal").on("shown.bs.modal", function (e) {
            openingProcessingModal = false;
        });
        // func: 
        var showModalWithMessage = function ($modal, message) {
            $modal.modal('show');
            $modal.find(".out-message").html(message);
        }
        // call AJAX to create a new Appointment
        $.ajax({
            method: "POST",
            contentType: "application/json",
            url: "/api/appointment/create",
            dataType: "JSON",
            async: true,
            data: jsonData,
        }).success(function (data) {
            console.log("response: ");
            console.log(data);
            // 
            var checkResult = function (data) {
                $("#processing-modal").modal('hide');
                if (data.Success == true) {
                    showModalWithMessage($("#success-modal"), data.Message);
                } else {
                    showModalWithMessage($("#fail-modal"), data.Message);
                }
            }
            if (openingProcessingModal) {
                $("#processing-modal").on("shown.bs.modal", function (e) {
                    e.stopPropagation();
                    openingProcessingModal = false;
                    checkResult(data);
                });
            } else {
                checkResult(data);
            }
        }).fail(function (data) {
            console.log("response: ");
            console.log(data);
            if (openingProcessingModal) {
                $("#processing-modal").on("shown.bs.modal", function (e) {
                    e.stopPropagation();
                    openingProcessingModal = false;
                    $("#processing-modal").modal('hide');
                    showModalWithMessage($("#fail-modal"), "Có lỗi xảy ra."); // Lỗi 500
                });
            } else {
                $("#processing-modal").modal('hide');
                showModalWithMessage($("#fail-modal"), "Có lỗi xảy ra");
            }
        });

        //}, 1000);


    }, // end Action
}
Controller.init();