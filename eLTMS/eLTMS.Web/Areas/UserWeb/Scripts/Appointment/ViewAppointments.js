var CONFIG = {
    pageSize: 10,
    pageIndex: 1,
    PatientId: 71
};

var Controller = {
    init: function () {
        Controller.loadData();
        Controller.registerEvent();
    },
    registerEvent: function () {

        toastr.options = {
            "debug": false,
            "positionClass": "toast-bottom-left",
            "onclick": null,
            "fadeIn": 300,
            "fadeOut": 1000,
            "timeOut": 5000,
            "extendedTimeOut": 1000
        }

        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật vật tư');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            Controller.loadDetail(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm({
                message: "Bạn có chắc chắn muốn xóa cuộc hẹn này?",
                buttons: {
                    confirm: {
                        label: "Có"
                    },
                    cancel: {
                        label: "Không"
                    }
                },
                callback: function (result) {
                    if (result) {
                        Controller.deleteAppointment(id);
                    }
                }
            });
        });
    },
    deleteAppointment: function (id) {
        $.ajax({
            url: '/api/appointment/delete-appointment?appointmentId=' + id,
            type: 'DELETE',
            success: function (response) {
                if (response.Success == true) {
                    toastr["success"]("Hủy lịch thành công.");
                    Controller.loadData(true);
                }
                else {
                    toastr["error"](response.Message);
                }
            },
            error: function (err) {
                toastr["error"](err);
            }
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/WareHouse/SupplyDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtSupplyId').val(data.SuppliesId);
                    $('#txtCode').val(data.SuppliesCode);
                    $('#txtName').val(data.SuppliesName);
                    $('#ddlSupplyType').val(data.SuppliesTypeId).change();
                    $('#ddlSupplyUnit').val(data.Unit).change();
                    $('#txtNote').val(data.Note);

                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    resetForm: function () {
        $('#txtSupplyId').val('0');
        $('#txtCode').val('');
        $('#txtName').val('')
        $('#ddlSupplyType').val('').change();
        $('#ddlSupplyUnit').val('').change();
        $('#txtNote').val('')
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/api/appointment/get-appointments-by-patient-id?patientId=' + CONFIG.PatientId,
            type: 'GET',
            dataType: 'json',
            //data: { page: CONFIG.pageIndex, pageSize: CONFIG.pageSize, suppliesCode: $('#txtSearch').val() },
            success: function (data) {
                console.log(data);
                var html = '';
                var template = $('#data-template').html();
                $.each(data, function (index, item) {
                    var totalPrice = 0;
                    var labTests = [];
                    for (var i = 0; i < item.SampleGettingDtos.length; i++) {
                        var sampleGetting = item.SampleGettingDtos[i];
                        for (var j = 0; j < sampleGetting.LabTests.length; j++) {
                            var labTest = sampleGetting.LabTests[j];
                            labTest.sPrice = labTest.Price.toLocaleString("VN");
                            labTests.push(labTest);
                            totalPrice += parseInt(labTest.Price);
                        }
                    }
                    html += Mustache.render(template, {
                        Index: (index + 1),
                        AppointmentId: item.AppointmentId,
                        AppointmentCode: item.AppointmentCode,
                        SampleGettings: item.SampleGettingDtos,
                        LabTests: labTests,
                        TotalPrice: totalPrice.toLocaleString("VN"),
                        IsNew: item.Status == "NEW",
                        IsProcess: item.Status != "NEW" && item.Status != "DONE",
                        IsDone: item.Status == "DONE"
                    });
                });
                //console.log(html);
                $('#tblData').html(html);
                /*Controller.paging(response.total, function () {
                    Controller.loadData();
                }, changePageSize);*/
                Controller.registerEvent();
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / CONFIG.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                CONFIG.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}

Controller.init();