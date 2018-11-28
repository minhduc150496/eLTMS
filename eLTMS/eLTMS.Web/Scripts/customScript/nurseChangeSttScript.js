var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        var dateNow = homeController.formatDate(new Date());
        document.getElementById("select-date").value = dateNow;
        homeController.loadDataBySample();
        homeController.registerEvent();
    },
    checkIsGot: function (SampleGettingId) {
        homeController.ChangeIsGot(SampleGettingId);
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
            homeController.loadDataBySample();
        });
        
        $("#select-date").change(function () {
            homeController.loadDataBySample();
        });

        //$('#cbIsGot').change(function () {
        //    $('#cbIsGot').val($(this).is(':checked'));
        //});

        $('#btnSearch').off('click').on('click', function () {
            homeController.loadDataBySample(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.loadData(true);
        });


    },
    
    //loadDetail: function (id) {
    //    $.ajax({
    //        url: '/WareHouse/SupplyDetail',
    //        data: {
    //            id: id
    //        },
    //        type: 'GET',
    //        dataType: 'json',
    //        success: function (response) {
    //            if (response.sucess) {
    //                var data = response.data;
    //                $('#txtSupplyId').val(data.SuppliesId);
    //                $('#txtCode').val(data.SuppliesCode);
    //                $('#txtName').val(data.SuppliesName);
    //                $('#ddlSupplyType').val(data.SuppliesTypeId).change();
    //                $('#ddlSupplyUnit').val(data.Unit).change();
    //                $('#txtNote').val(data.Note);

    //            }
    //            else {
    //                bootbox.alert(response.message);
    //            }
    //        },
    //        error: function (err) {
    //            console.log(err);
    //        }
    //    });
    //},
    saveData: function () {

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


    loadDataBySample: function (changePageSize) {
        var selectedSample = $("#select-sample").children("option:selected").val();
        var selectDate = $("#select-date").val();

        $.ajax({
            url: '/nurse/GetAppBySample',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, sampleId: selectedSample, date: selectDate },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            SgId: item.SampleGettingId,
                            FullName: item.PatientName,
                            SampleName: item.SampleName,
                            StartTime: item.StartTime,
                            Date: item.Date,
                            IsGot: item.IsGot,
                            ReadOnly: (item.IsGot === true) ? "return false;" : "",
                            Checked: (item.IsGot === true) ?  "checked" : ""
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

    //change IsGot
    ChangeIsGot: function (SampleGettingId) {
        $.ajax({
            url: '/nurse/IsGot',
            type: 'POST',
            dataType: 'json',
            data: { sampleGettingId: SampleGettingId },
            success: function (response) {
                if (response.success === true) {
                    toastr.success('Đổi trạng thái thành công');
                    homeController.loadDataBySample();
                }
                
            }
        })
    }
}
homeController.init();