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
        
        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật vật tư');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            Controller.loadDetail(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            Controller.deleteAppointment(id);
        });
    },
    deleteAppointment: function (id) {
        $.ajax({
            url: '/api/appointment/delete-appointment?appointmentId='+id,
            type: 'PUT',
            success: function (response) {
                if (response.Success == true) {
                    alert("Xóa thành công.");
                    Controller.loadData(true);
                }
                else {
                    alert(response.Message);
                }
            },
            error: function (err) {
                console.log(err);
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
            url: '/api/appointment/get-appointments-by-patient-id?patientId='+CONFIG.PatientId,
            type: 'GET',
            dataType: 'json',
            //data: { page: CONFIG.pageIndex, pageSize: CONFIG.pageSize, suppliesCode: $('#txtSearch').val() },
            success: function (data) {
                var html = '';
                var template = $('#data-template').html();
                $.each(data, function (index, item) {
                    var gettingDates = [];
                    for (var i = 0; i < item.SampleGettingDtos.length; i++) {
                        var date = item.SampleGettingDtos[i].GettingDate;
                        date = new Date(date);
                        var fmDate = date.getDate() + "-" + date.getMonth() + "-" + date.getFullYear();
                        if (gettingDates.indexOf(fmDate) == -1) {
                            gettingDates.push(fmDate);
                        }
                    }
                    var vieStatus = "";
                    if (item.Status == "NEW") {
                        vieStatus = "Mới tạo";
                    } else {
                        vieStatus = "Hoàn tất";
                    }
                    html += Mustache.render(template, {
                        Index: (index + 1),
                        AppointmentId: item.AppointmentId,
                        AppointmentCode: item.AppointmentCode,
                        DoctorName: item.DoctorName,
                        TestPurpose: item.TestPurpose,
                        GettingDates: gettingDates,
                        Conclusion: item.Conclusion,
                        Status: vieStatus,
                        IsNew: item.Status == "NEW",
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