/*
    Author: DucBM
*/

CONFIG = {
    PATIENT_ID: 5, // hard code for dev-ing
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
    firebaseDB: {},
    sampleDtos: {},
    bookings: [],
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
            Controller.renderStep2Html();
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
            for (var i = 0; i < Model.sampleDtos.length; i++) {
                var sampleDto = Model.sampleDtos[i];
                if (sampleDto.IsSelected) {
                    var sampleGettingDto = {};
                    sampleGettingDto.SampleId = sampleDto.SampleId;

                    var trSample = $("tr[data-sample-index='" + sampleDto.Index + "']");

                    var gettingDate = trSample.find("[type=date]").val();
                    sampleGettingDto.GettingDate = gettingDate;

                    var slotId = trSample.find("select").val();
                    sampleGettingDto.SlotId = slotId;

                    sampleGettingDto.LabTestIds = [];
                    for (var j = 0; j < sampleDto.LabTests.length; j++) {
                        if (sampleDto.LabTests[j].IsChecked) {
                            var labTestId = sampleDto.LabTests[j].LabTestId;
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

        newAp2 = function () {
            $("#tr-2").hide();
            $("#no-3").html("2");
            $("#ap-2").fadeIn();
        }

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
            var checked = $(this).is(":checked"); 

            Model.sampleDtos[sampleIndex].LabTests[labTestIndex].IsChecked = checked;
            Model.sampleDtos[sampleIndex].nChecked += (checked ? 1 : -1);
            Model.sampleDtos[sampleIndex].IsSelected = Model.sampleDtos[sampleIndex].nChecked > 0;

            //console.log("checkbox change", sampleIndex, labTestIndex, checked)            
        });

        for (var i = 0; i < Model.sampleDtos.length; i++) {
            var sample = Model.sampleDtos[i];
            for (var j = 0; j < sample.LabTests.length; j++) {
                var labTest = sample.LabTests[j];
                labTest.FmPrice = labTest.Price.toLocaleString("VN-vi");
            }
        }
        //console.log(Model.sampleDtos);

        // config Firebase
        /*
        var config = {
            apiKey: "AIzaSyBmErEOrR3HvYOALMjpqmP4dwiYUuAPK7E",
            authDomain: "eltms-test1.firebaseapp.com",
            databaseURL: "https://eltms-test1.firebaseio.com",
            projectId: "eltms-test1",
            storageBucket: "eltms-test1.appspot.com",
            messagingSenderId: "1013768931631"
        };
        firebase.initializeApp(config);
        Model.firebaseDB = firebase.database().ref().child("/bookings/");

        Model.firebaseDB.on("value", function (snapshot) {
            Model.bookings = snapshot.val();
        });/**/
        
    }, // end Action
    renderStep2Html: function () {

        var data = { Samples: Model.sampleDtos };
        var template = $("#step-2-template").html();
        var htmlStep1 = Mustache.render(template, data);
        $("#step-2-form").html(htmlStep1);
        
        data = { Samples: Model.sampleDtos };
        template = $("#step-2-template").html();
        htmlStep1 = Mustache.render(template, data);
        $("#step-2-form").html(htmlStep1);

        $("#step-2-form table tbody tr:not(:last-child()) td:first-child()").each(function (index, el) {
            $(this).html(index + 1);
        });

        var totalPrice = 0;
        for (var i = 0; i < Model.sampleDtos.length; i++) {
            var sample = Model.sampleDtos[i];
            for (var j = 0; j < sample.LabTests.length; j++) {
                var labTest = sample.LabTests[j];
                console.log(labTest.IsChecked);
                if (labTest.IsChecked == true) {
                    totalPrice += labTest.Price;
                }
            }
        }
        var sTotalPrice = totalPrice.toLocaleString("VN-vi");
        $("#total-price").html(sTotalPrice);
        
    }, // end Action
    suggestSlots: function () {

        Model.comingDate = $("#coming-date").val();
        var hour = parseInt($("#coming-hour").val());
        var min = parseInt($("#coming-min").val());
        console.log(hour, min);
        if (hour != null && min != null) {
            Model.comingTime = hour * 60 * 60 + min * 60;
            // console.log(Model.sampleDtos);
            var result = AppointmentSuggestor.CalcTheBestTour(Model.slotDtos, Model.comingDate, Model.comingTime, Model.sampleDtos);
            Model.suggestResult = result;
            console.log(result);
            Controller.renderStep2bHtml();
        }

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
        if (sSampleDtos == null || true) {
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
            success: function (data) {
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
            },
            fail: function (data) {
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
            }
        }); // end AJAX define
        
    }, // end Action
}
Controller.init();
