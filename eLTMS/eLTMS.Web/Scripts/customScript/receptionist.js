var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        var dateNow = homeController.formatDate(new Date());
        document.getElementById("select-date").value = dateNow;
        homeController.loadPatientByDate();
        homeController.registerEvent();
    },

    checkIsPaid: function (PatientId) {
        homeController.loadPrice(PatientId);
        var modalConfirm = function (callback) {

            $("#mi-modal").modal('show');

            $("#modal-btn-si").on("click", function () {
                callback(true);
                $("#mi-modal").modal('hide');
            });

            $("#modal-btn-no").on("click", function () {
                callback(false);
                $("#mi-modal").modal('hide');
            });
        };

        modalConfirm(function (confirm) {
            if (confirm) {
                homeController.ChangeIsPaid(PatientId);
            }
            else {
                homeController.loadPatientByDate();
            }
        });

    },

    formatDate: function (date) {
        var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    },

    registerEvent: function () {

        $("#select-sample").change(function () {
            homeController.loadPatientByDate();
        });
        
        $("#select-date").change(function () {
            homeController.loadPatientByDate();
        });

        $('#btnSearch').off('click').on('click', function () {
            homeController.loadPatientByDate(true);
        });

        $('.btn-editResult').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.loadDataByPatientId(id);
            $('#lblPopupTitle').text('Thông tin các cuộc hẹn');
            $('#myModal1').modal('show');
        });
    },
    
    //change page
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

   
    loadPatientByDate: function (changePageSize) {
        var selectedSample = $("#select-sample").children("option:selected").val();
        var selectDate = $("#select-date").val();
        var searchData = $("#txtSearch").val();
        $.ajax({
            url: '/receptionist/GetPatientByDate',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, sampleId: selectedSample, date: selectDate, search: searchData },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            OrderNumber: i+1,
                            FullName: item.PatientName,
                            PatientID: item.PatientID,
                            ID: item.IdentityCardNumber,
                            Phone: item.Phone,
                            Address: item.Address,
                            DOBirth: item.DateOfBirth,
                        });

                    });
                    console.log(html);
                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        //homeController.loadDataBySample();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },

    //ispaid
    ChangeIsPaid: function (PatientId) {
        var selectDate = $("#select-date").val();
        $.ajax({
            url: '/receptionist/IsPaid',
            type: 'POST',
            dataType: 'json',
            data: { patientId: PatientId, date: selectDate  },
            success: function (response) {
                //                homeController.loadPrice(SampleGettingId);
                if (response.success === true) {
                    toastr.success('Đổi trạng thái thành công');
                    homeController.loadDataBySample();
                }

            }
        })
    },

    //load data in modal
    loadDataByPatientId: function (id, changePageSize) {
        var selectDate = $("#select-date").val();
        $.ajax({
            url: '/receptionist/GetAppByPatientId',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, patientId: id, date: selectDate },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template1').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            OrderNum: item.OrderNumber,
                            SgId: item.SampleGettingId,
                            SampleName: item.SampleName,
                            //LabTestName: item.LabTestName,
                            StartTime: item.StartTime,
                            EnterDate: item.EnterDate,
                            EnterTime: item.EnterTime
                        });

                    });
                    console.log(html);
                    $('#tblData1').html(html);
                    homeController.paging(response.total, function () {
                        //homeController.loadDataBySample();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },

    //load giá
    loadPrice: function (id, changePageSize) {
        var selectDate = $("#select-date").val();
        $.ajax({
            url: '/receptionist/GetPriceByPatient',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, patientId: id, date: selectDate },
            success: function (response) {
                //debugger;
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template2').html();
                    $.each(data.PriceListItemDto, function (i, item) {
                        html += Mustache.render(template, {
                            OrderNumber: i + 1,
                            LabtestName: item.LabtestName,
                            Price: item.Price,
                        });
                    });
                    html += Mustache.render(template, {
                        TotalPrice: data.TotalPrice,
                    });
                    console.log(html);
                    $('#tblPriceData').html(html);
                    homeController.paging(response.total, function () {
                        //homeController.loadDataBySample();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },
}
homeController.init();