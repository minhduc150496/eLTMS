var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allLabTesting: [],
    ImportExcel: false, 
    LoadFromDataBase: false
}
var homeController = {
    init: function () {
        homeController.registerEvent();
        homeController.loadData();
        homeController.loadDataLabTestingResultFail();
    },
    registerEvent: function () {
        $('.btn-deleteLabTestingResultFail').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteLabtesting(id);
            homeController.loadDataLabTestingResultFail();

        });
        $('.btn-viewResult').off('click').on('click', function () {
            var code = $(this).data('id');
            $('#txtResultCodeView').val(code)
            $('#hiddenFormView').submit();

        });
        $('#btnSaveResultLT').off('click').on('click', function () {
            var code = $('#txtAppCodeLT').val();
            var con = $('#txtResultLT').val();
            var cmt = $('#txtCMTLT').val();
            $.ajax({
                url: '/LabTest/UpdateResult',
                type: 'Post',
                dataType: 'json',
                data: { code: code, con: con,cmt,cmt },
                async: false,
                success: function (res) {
                    if (!res.success) {
                        toastr.success("Chẩn đoán không thành công.");

                    }
                    else {
                        toastr.success("Chẩn đoán thành công.");
                        homeController.loadDataLabTestingResult();
                    }
                }
            })
        });
        $('.btn-printResult').off('click').on('click', function () {
            var code = $(this).data('id');
            $('#txtResultCode').val(code)
            $('#hiddenForm').submit();

        });
        $('.btn-editResult').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật thông tin xét nghiệm');
             $('#myModal1').modal('show');
            var id = $(this).data('id');
            homeController.loadDataResult(id);
        });
    },
    deleteLabtesting: function (id) {
        try {
            $.ajax({
                url: '/LabTest/DeleteLabtesting',
                data: {
                    id: id
                },
                async: false,
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success == true) {
                        toastr.success("Xóa thành công.");
                    }
                    else {
                        toastr.error("Xóa không thành công.");
                    }
                }
            });

        }

        catch (err) {
            alert(err);
        }

    },
    loadDataResult: function (id, changePageSize) {
        $.ajax({
            url: '/appointment/AppDetail',
            type: 'GET',
            dataType: 'json',
            data: { app: id},
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtResultLT').val(data.Conclusion);
                    $('#txtAppCodeLT').val(data.AppointmentCode);
                    $('#txtCMTLT').val(data.DoctorComment);
                }
            }
        })
    },
    loadDataLabTestingResultFail: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllLabTestingsFail',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTestingResultFail-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabtestingID: item.LabTestingId,
                            Name: item.PatientName,
                            Phone: item.PatientPhone,
                            Code: item.AppointmentCode,
                            Sample: item.LabTestName,
                        });

                    });
                    $('#tblDataLabTestingResultFail').html(html);
                    homeController.registerEvent();
                }
            }
        })
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllResult',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTestingResult-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabTestingId: item.LabTestingId,
                            Name: item.LabTestName,
                            Status: item.Status,
                            Getting: item.AppointmentCode,
                            Group: item.SampleName,
                        });

                    });
                    $('#tblDataLabTestingResult').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadDataLabTesting();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },
    
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / homeconfig.pageSize);

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
                homeconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
 
}

homeController.init();