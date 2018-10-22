var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            var code = $('#txtCode').val();
            var name = $('#txtName').val();
            var type = parseInt($('#ddlType').val());
            var labTestId = parseInt($('#txtLabTestId').val());
            var price = parseInt($('#txtPrice').val());
            var description = $('#txtDescription').val();
            var isDeleted = "False";
            var labTest = {
                LabTestId: labTestId,
                LabTestCode: code,
                LabTestName: name,
                SampleId: type,
                Price: price,
                Description: description,
                IsDeleted: isDeleted
            }
            if (labTest.LabTestId == 0) {
                $.ajax({
                    url: '/LabTest/AddLabTest',
                    type: 'Post',
                    dataType: 'json',
                    data: labTest,
                    success: function (res) {
                        if (!res.sucess) {
                            if (res.validation && res.validation.Errors) {
                                toastr.error(res.validation.Errors[0].ErrorMessage);
                            }

                        }
                        else {
                            toastr.success("Tạo mới thành công.");
                            $('#myModal').modal('hide');
                            homeController.loadData();
                        }
                    }
                })
            } else {
                $.ajax({
                    url: '/LabTest/UpdateLabTest',
                    type: 'Post',
                    dataType: 'json',
                    data: labTest,
                    success: function (res) {
                        if (!res.sucess) {
                           
                                toastr.error("Cập nhật không thành công");
                            

                        }
                        else {
                            toastr.success("Cập nhật thành công.");
                            $('#myModal').modal('hide');
                            homeController.loadData();
                        }
                    }
                })
            }
          
        })
        $('#btnSaveSample').off('click').on('click', function () {
            var name = $('#txtSampleName').val();
            var type = parseInt($('#ddlSampleType').val());
            var description = $('#txtSampleDescription').val();
            var isDeleted = "False";
            var sample = {
                SampleName: name,
                SampleGroupId: type,
                Description: description,
                IsDeleted: isDeleted
            }
            
                $.ajax({
                    url: '/LabTest/AddSample',
                    type: 'Post',
                    dataType: 'json',
                    data: sample,
                    success: function (res) {
                        if (!res.sucess) {
                            if (res.validation && res.validation.Errors) {
                                toastr.error(res.validation.Errors[0].ErrorMessage);
                            }

                        }
                        else {
                            toastr.success("Tạo mới thành công.");
                            $('#myModal1').modal('hide');
                            homeController.loadDataSample();
                            $('#myModal3').modal('show');
                        }
                    }
                })
            

        })
        $('#btnSaveSampleGroup').off('click').on('click', function () {
            var name = $('#txtGroupName').val();
            var duration = parseInt($('#txtTime').val());
            var start = parseInt($('#txtStart').val());
            var close = parseInt($('#txtClose').val());
            var isDeleted = "False";
            var sampleGroup = {
                GroupName: name,
                GettingDuration: duration,
                OpenTime: start,
                CloseTime: close,
                IsDeleted: isDeleted
            }

            $.ajax({
                url: '/LabTest/AddSampleGroup',
                type: 'Post',
                dataType: 'json',
                data: sampleGroup,
                success: function (res) {
                    if (!res.sucess) {
                        if (res.validation && res.validation.Errors) {
                            toastr.error(res.validation.Errors[0].ErrorMessage);
                        }

                    }
                    else {
                        toastr.success("Tạo mới thành công.");
                        $('#myModal2').modal('hide');
                        homeController.loadDataSampleGroup();
                        $('#myModal4').modal('show');
                    }
                }
            })


        })
        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới dịch vụ xét nghiệm');
            homeController.resetForm();
            $('#myModal').modal('show');
        });
        $('#btnAddNewSampleGroup').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới nhóm xét nghiệm');
            $('#myModal4').modal('hide');
            $('#myModal2').modal('show');
        });
        $('#btnAddNewSample').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới xét nghiệm');
            $('#myModal3').modal('hide');
            $('#myModal1').modal('show');
        });
        $('#btnViewSample').off('click').on('click', function () {
            $('#lblPopupTitle').text('Danh sách các loại xét nghiệm');
            $('#myModal3').modal('show');
            homeController.loadDataSample(true);
        });
        $('#btnViewSampleGroup').off('click').on('click', function () {
            $('#lblPopupTitle').text('Danh sách các nhóm xét nghiệm');
            $('#myModal4').modal('show');
           homeController.loadDataSampleGroup(true);
        });
        $('#btnSearch').off('click').on('click', function () {
            homeController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật xét nghiệm');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.delete(id);
            
        });
        $('.btn-deleteSample').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteSample(id);


        });
        $('.btn-deleteSampleGroup').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteSampleGroup(id);
        });
    },
    delete: function (id) {
        $.ajax({
            url: '/LabTest/DeleteLabTest',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.success == true) {
                    toastr.success("Xóa thành công.");
                    homeController.loadData(true);
                }
                else {
                    toastr.error("Xóa không thành công.");
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    deleteSample: function (id) {
        try {
            $.ajax({
                url: '/LabTest/DeleteSample',
                data: {
                    id: id
                },
                async: false,
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success == true) {
                        toastr.success("Xóa thành công.");
                        // homeController.loadDataSample(true);
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
    deleteSampleGroup: function (id) {
        try {
            $.ajax({
                url: '/LabTest/DeleteSampleGroup',
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
    loadDetail: function (id) {
        $.ajax({
            url: '/LabTest/LabTestDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtLabTestId').val(data.LabTestId);
                    $('#txtCode').val(data.LabTestCode);
                    $('#txtName').val(data.LabTestName);
                    $('#ddlType').val(data.SampleId).change();
                    $('#txtPrice').val(data.Price);
                    $('#txtDescription').val(data.Description);
                   
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
        $('#txtLabTestId').val('0');
        $('#txtCode').val('');
        $('#txtName').val('');
        $('#ddlType').val('').change();
        $('#txtPrice').val('');
        $('#txtDescription').val('');
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllLabTests',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, code: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabTestId: item.LabTestId,
                            Code: item.LabTestCode,
                            Name: item.LabTestName,
                            Type: item.SampleName,
                            Price: item.Price,
                            Description: item.Description,
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
    loadDataSample: function () {
        $.ajax({
            url: '/LabTest/GetAllSamples',
            type: 'GET',
            dataType: 'json',
            data: {  },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataSample-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            SampleId: item.SampleId,
                            Name: item.SampleName,
                            Type: item.SampleGroupName,
                            Description: item.Description,
                        });

                    });
                    console.log(html);
                    $('#tblDataSample').html(html);
                    
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataSampleGroup: function () {
        $.ajax({
            url: '/LabTest/GetAllSampleGroups',
            type: 'GET',
            dataType: 'json',
            data: {},
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataSampleGroup-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            SampleGroupId: item.SampleGroupId,
                            Name: item.GroupName,
                            Start: item.OpenTime,
                            Duration: item.GettingDuration,
                            Close: item.CloseTime,
                        });

                    });
                    console.log(html);
                    $('#tblDataSampleGroup').html(html);

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
    }
}
homeController.init();