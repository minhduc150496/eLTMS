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
    slotDtos: {},
    appointmentDto: {},
    suggestResult: null
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

        // fixing...
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
    renderStep1Html: function () {
        // render step 1:
        var data = { Samples: Model.sampleDtos };
        var template = $("#step-1-template").html();
        var htmlStep1 = Mustache.render(template, data);
        $("#step-1-form").html(htmlStep1);

        $("#step-1-form [type='checkbox']").change(function () {
            var sampleIndex = $(this).closest("[data-sample-index]").data("sample-index");
            var labTestIndex = $(this).data("labtest-index");
            var checked = $(this).val() == "on";

            Model.sampleDtos[sampleIndex].LabTests[labTestIndex].IsSelected = checked;
            Model.sampleDtos[sampleIndex].nChecked += (checked ? 1 : -1);
            
        });

        // step 2a
        Controller.renderStep2Html();

        // get avai. slots
        $.ajax({
            method: "GET",
            contentType: "application/json",
            url: "/api/slot/get-available-slots",
            dataType: "JSON",
            async: true,
        }).success(function (data) {
            Model.slotDtos = data;
            console.log(data);
        });

    }, // end Action
    renderStep2Html: function () {
        Model.comingDate = '2018-11-06';
        var data = {
            Date: '2018-11-06',
            Hours: [],
            Mins: ['00', '15', '30', '45']
        };
        for (var i = 4; i <= 17; i++) {
            data.Hours.push(i);
        }
        var template = $("#step-2a-template").html();
        var html = Mustache.render(template, data);
        $("#step-2a-form").html(html);

        $("#coming-date").off("change").change(function () {
            Controller.suggestSlots();
        });
        $("#coming-hour").off("change").change(function () {
            Controller.suggestSlots();
        });
        $("#coming-min").off("change").change(function () {
            Controller.suggestSlots();
        });


    }, // end Action
    suggestSlots: function () {

        Model.comingDate = $("#coming-date").val();
        var hour = parseInt($("#coming-hour").val());
        var min = parseInt($("#coming-min").val());
        console.log(hour, min);
        if (hour != null && min != null) {
            Model.comingTime = hour * 60 * 60 + min * 60;
            console.log(Model.sampleDtos);
            var result = AppointmentSuggestor.CalcTheBestTour(Model.slotDtos, Model.comingDate, Model.comingTime, Model.sampleDtos);
            Model.suggestResult = result;
            console.log(result);
            Controller.renderStep2bHtml();
        }

    }, // end Action
    renderStep2bHtml: function () {
        var data = {
            Samples: []
        };
        // FOR-EACH sampleDtos
        $(Model.sampleDtos).each(function (index, element) {
            var sample = {
                SampleId: element.SampleId,
                SampleGroupId: element.SampleGroupId,
                SampleName: element.SampleName,
                IsDisplay: element.nChecked > 0,
                Slots: []
            };
            // get suggest slot
            var selectedSlotId = -1;
            if (Model.suggestResult != null) {
                $(Model.suggestResult).each(function (index, element) {
                    if (element.SampleId == sample.SampleId) {
                        selectedSlotId = element.SlotId;
                        //console.log(sample.SampleId, selectedSlotId);
                    }
                });
            }
            // FOR-EACH slotDtos
            for (var j = 0; j < Model.slotDtos.length; j++) {
                var element = Model.slotDtos[j];
                if (element.Date == Model.comingDate && element.SampleGroupId == sample.SampleGroupId && element.StartTime > Model.comingTime) {
                    var slot = {
                        SlotId: element.SlotId,
                        StartTime: element.StartTime,
                        FmStartTime: Utils.formatTimeShort(element.StartTime),
                        FmFinishTime: Utils.formatTimeShort(element.FinishTime),
                        Date: element.Date,
                        IsSelected: false
                    };
                    if (slot.SlotId == selectedSlotId) {
                        //console.log(slot.SlotId);
                        slot.IsSelected = true
                    };
                    sample.Slots.push(slot);
                }
            }
            // sort Slots in dropdown
            sample.Slots.sort(function (a, b) {
                return a.StartTime - b.StartTime;
            });
            data.Samples.push(sample);
        });
        // sort Samples
        data.Samples.sort(function (sample1, sample2) {
            var startTime1;
            for (var i = 0; i < sample1.Slots.length; i++) {
                if (sample1.Slots[i].IsSelected) {
                    startTime1 = sample1.Slots[i].StartTime;
                }
            }
            var startTime2;
            for (var i = 0; i < sample2.Slots.length; i++) {
                if (sample2.Slots[i].IsSelected) {
                    startTime2 = sample1.Slots[i].StartTime;
                }
            }
            return startTime1 - startTime2;
        });

        var template = $("#step-2b-template").html();
        var html = Mustache.render(template, data);
        $("#step-2b-form").html(html);

        $("#step-2b-form").off("change").change(function () {

        });

    }, // end Action
    renderLabTestList: function () {
        // get all sampleDtos and LabTests
        var sSampleDtos = localStorage.getItem(CONFIG.SAMPLE_DTOS_KEY);
        var setIndex = function () {
            for (var i = 0; i < Model.sampleDtos.length; i++) {
                var sample = Model.sampleDtos[i];
                sample.Index = i;
                sample.nChecked = 0;
                for (var j = 0; j < sample.LabTests.length; j++) {
                    var labTest = sample.LabTests[j];
                    labTest.Index = j;
                }
            }
        }
        if (sSampleDtos == null && false) {
            $.ajax({
                url: "/api/sample/get-all"
            }).success(function (data) {
                //console.log(data);
                Model.sampleDtos = data; // CONFIG
                sSampleDtos = JSON.stringify(data);
                // save to local storage
                localStorage.setItem(CONFIG.SAMPLE_DTOS_KEY, sSampleDtos);
                setIndex();
                // render UI
                Controller.renderStep1Html();
            }) // end AJAX settings
        } else {
            // get data
            Model.sampleDtos = JSON.parse(sSampleDtos);
            setIndex();
            // render UI
            Controller.renderStep1Html();
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
