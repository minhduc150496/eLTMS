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
    },
    registerEvent: function () {

        $('.btn-viewResult').off('click').on('click', function () {
            var code = $(this).data('id');
            $('#txtResultCodeView').val(code)
            $('#hiddenFormView').submit();

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
    loadDataResult: function (id, changePageSize) {
        $.ajax({
            url: '/appointment/AppDetail',
            type: 'GET',
            dataType: 'json',
            data: { app: id},
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtResultxx').val(data.Conclusion);
                    $('#txtAppCodexx').val(data.AppointmentCode);
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