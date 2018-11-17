var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        //homeController.loadData();
        homeController.loadDataBySample();
        homeController.registerEvent();
    },
    checkIsPaid: function (SampleGettingId) {
        homeController.ChangeIsPaid(SampleGettingId);
    },
    loadIsPaid: function (IsPaid, SampleGettingId) {
        var a = 1;
    },
    registerEvent: function () {
        
        $("#select-sample").change(function () {
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
                url: '/receptionist/AddApp',
                type: 'Post',
                dataType: 'json',
                data: item,
                success: function (res) {
                    if (!res.sucess) {
                        if (res.validation && res.validation.Errors) {
                            toastr.error(res.validation.Errors[0].ErrorMessage);
                        }

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
            $('#lblPopupTitle').text('Thêm mới vật tư');
            homeController.resetForm();
            $('#myModal').modal('show');
        });


        $('#btnSearch').off('click').on('click', function () {
            homeController.loadDataBySample(true);
        });

        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật vật tư');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteSupply(id);

        });

    },
    //deleteSupply: function (id) {
    //    $.ajax({
    //        url: '/WareHouse/Delete',
    //        data: {
    //            supplyId: id
    //        },
    //        type: 'POST',
    //        dataType: 'json',
    //        success: function (response) {
    //            if (response.success == true) {
    //                toastr.success("Xóa thành công.");
    //                homeController.loadData(true);
    //            }
    //            else {
    //                toastr.error("Xóa không thành công.");
    //            }
    //        },
    //        error: function (err) {
    //            console.log(err);
    //        }
    //    });
    //},
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
    saveData: function () {

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
        var selectedSample = $(this).children("option:selected").val();
        $.ajax({
            url: '/receptionist/GetAllAppointment',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, sampleId: selectedSample },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        var sample = "";
                        $.each(item.SampleGettingDtos, function (e, etem) {
                            sample = sample + etem.SampleName + ": " + etem.StartTime + " ";
                        });
                        html += Mustache.render(template, {
                            AppCode: item.AppointmentCode,
                            FullName: item.PatientName,
                            Phone: item.Phone,
                            Address: item.Address,
                            SampleName: sample /*+ item.SampleGettingDtos.StartTime +"/n"*/,
                            //StartTime: item.Unit,
                            //Note: item.Note,

                        });

                    });
                    console.log(html);
                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
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


    loadDataBySample: function () {
        var selectedSample = $("#select-sample").children("option:selected").val();
        $.ajax({
            url: '/receptionist/GetAllSampleGettingsBySampleGroupId',
            type: 'GET',
            dataType: 'json',
            data: {
                page: homeconfig.pageIndex,
                pageSize: homeconfig.pageSize,
                sampleGroupId: selectedSample
            },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, item);
                    });
                    //console.log(html);
                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadDataBySample();
                    }, changePageSize);
                    //homeController.registerEvent();
                }
            }
        })
    },
    ChangeIsPaid: function (SampleGettingId) {
        $.ajax({
            url: '/receptionist/IsPaid',
            type: 'POST',
            dataType: 'json',
            data: { sampleGettingId: SampleGettingId },
            success: function (response) {
                if (response.success) {
                    homeController.registerEvent();
                }
                homeController.loadDataBySample();
            }
        })
    }
}
homeController.init();