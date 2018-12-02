﻿var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        //homeController.loadData();
        var dateNow = homeController.formatDate(new Date());
        document.getElementById("select-date").value = dateNow;
        homeController.loadDataBySample();
        homeController.registerEvent();
    },
    checkIsPaid: function (SampleGettingId) {
        homeController.ChangeIsPaid(SampleGettingId);
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
        

        $('#btnSave').off('click').on('click', function () {
            var name = $('#txtName').val();
            var address = $('#txtAddress').val();
            var phone = $('#txtPhone').val();
            var dateOfBirth = $('#txtDateOfBirth').val();
            var cmnd = $('#txtCMND').val();
            var mau = false;
            var nuocTieu = false;
            var teBaoHoc = false;
            var phan = false;
            var dich = false;

            if ($('#checkBox_loaiXetNghiem1').prop('checked') === true || $('#checkBox_loaiXetNghiem2').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem3').prop('checked') === true
                || $('#checkBox_loaiXetNghiem4').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem5').prop('checked') === true || $('#checkBox_loaiXetNghiem6').prop('checked') === true
                || $('#checkBox_loaiXetNghiem7').prop('checked') === true || $('#checkBox_loaiXetNghiem8').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem9').prop('checked') === true
                || $('#checkBox_loaiXetNghiem10').prop('checked') === true || $('#checkBox_loaiXetNghiem11').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem12').prop('checked') === true
                || $('#checkBox_loaiXetNghiem13').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem14').prop('checked') === true || $('#checkBox_loaiXetNghiem15').prop('checked') === true
                || $('#checkBox_loaiXetNghiem16').prop('checked') === true || $('#checkBox_loaiXetNghiem17').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem18').prop('checked') === true
                || $('#checkBox_loaiXetNghiem19').prop('checked') === true || $('#checkBox_loaiXetNghiem20').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem21').prop('checked') === true
                || $('#checkBox_loaiXetNghiem22').prop('checked') === true)
                mau = true;
            if ($('#checkBox_loaiXetNghiem23').prop('checked') === true || $('#checkBox_loaiXetNghiem24').prop('checked') === true)
                nuocTieu = true;
            if ($('#checkBox_loaiXetNghiem25').prop('checked') === true || $('#checkBox_loaiXetNghiem26').prop('checked') === true)
                teBaoHoc = true;
            if ($('#checkBox_loaiXetNghiem27').prop('checked') === true || $('#checkBox_loaiXetNghiem28').prop('checked') === true)
                phan = true;
            if ($('#checkBox_loaiXetNghiem29').prop('checked') === true || $('#checkBox_loaiXetNghiem30').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem31').prop('checked') === true
                || $('#checkBox_loaiXetNghiem32').prop('checked') === true || $('#checkBox_loaiXetNghiem33').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem34').prop('checked') === true)
                dich = true;

            var item = {
                Name: name,
                Phone: phone,
                Address: address,
                DateOfBirth: dateOfBirth,
                IdentityCardNumber: cmnd,
                Mau: mau,
                NuocTieu: nuocTieu,
                TeBaoHoc: teBaoHoc,
                Phan: phan,
                Dich: dich
            };
            $.ajax({
                url: '/cashier/AddApp',
                type: 'Post',
                dataType: 'json',
                data: item,
                success: function (res) {
                    if (!res.success) {
                        toastr.error("Tạo mới thất bại.");
                        $('#myModal').modal('hide');
                    }
                    else {
                        toastr.success("Tạo mới thành công.");
                        $('#myModal').modal('hide');
                        homeController.loadDataBySample();
                    }
                }
            });

        });

        $('#cbIsPaid').change(function () {
            $('#cbIsPaid').val($(this).is(':checked'));
        });

        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới cuộc hẹn');
            homeController.resetForm();
            $('#myModal').modal('show');
            var selectedSample = $(".Sample").children("option:selected").val();
            if (selectedSample == 1) {
                $('#mauCheckGroup').show();
                $('#nuocTieuCheckGroup').hide();
                $('#teBaoHocCheckGroup').hide();
                $('#phanCheckGroup').hide();
                $('#dichCheckGroup').hide();
            }
            else if (selectedSample == 2) {
                $('#mauCheckGroup').hide();
                $('#nuocTieuCheckGroup').show();
                $('#teBaoHocCheckGroup').hide();
                $('#phanCheckGroup').hide();
                $('#dichCheckGroup').hide();
            }
            else if (selectedSample == 3) {
                $('#mauCheckGroup').hide();
                $('#nuocTieuCheckGroup').hide();
                $('#teBaoHocCheckGroup').show();
                $('#phanCheckGroup').hide();
                $('#dichCheckGroup').hide();
            }
            else if (selectedSample == 4) {
                $('#mauCheckGroup').hide();
                $('#nuocTieuCheckGroup').hide();
                $('#teBaoHocCheckGroup').hide();
                $('#phanCheckGroup').show();
                $('#dichCheckGroup').hide();
            }
            else if (selectedSample == 5) {
                $('#mauCheckGroup').hide();
                $('#nuocTieuCheckGroup').hide();
                $('#teBaoHocCheckGroup').hide();
                $('#phanCheckGroup').hide();
                $('#dichCheckGroup').show();
            }
        });


        $('#btnSearch').off('click').on('click', function () {
            homeController.loadDataBySample(true);
        });

    },
    
    
    resetForm: function () {
        $('#txtDateOfBirth').val('');
        $('#txtName').val('');
        $('#txtCMND').val('');
        $('#txtPhone').val('');
        $('#txtAddress').val('');
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



    loadDataBySample: function (changePageSize) {
        //chi lấy dữ liệu mà select
        var selectedSample = $("#select-sample").children("option:selected").val();
        var selectDate = $("#select-date").val();
        var searchData = $("#txtSearch").val();
        $.ajax({
            url: '/cashier/GetAppBySample',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, sampleId: selectedSample, date: selectDate, search: searchData },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    //do du lieu qua html
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            AppCode: item.AppointmentCode,
                            FullName: item.PatientName,
                            Phone: item.Phone,
                            Address: item.Address,
                            StartTime: item.StartTime,
                            OrderNumber: item.OrderNumber,
                            Date: item.Date,
                            Table: item.Table,
                            SampleGettingId: item.SampleGettingId,
                            IsPaid: item.IsPaid,
                            ReadOnly: (item.IsPaid === true) ? "return false;" : "", 
                            Checked: (item.IsPaid === true) ?  "checked" : ""
                        });

                    });
                    console.log(html);
                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadDataBySample();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },


    ChangeIsPaid: function (SampleGettingId) {
        $.ajax({
            url: '/cashier/IsPaid',
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