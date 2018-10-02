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
            var type = parseFloat($('#ddlSupplyType').val());
            var supplyId = $('#txtSupplyId').val();
            var unit = $('#ddlSupplyType').val();
            var note = $('#txtNote').val();
            var supply = {
                SupplyId: supplyId,
                SuppliesCode: code,
                SuppliesName: name,
                SuppliesTypeName: type,
                //Quantity:quantity,
                Unit: unit,
                Note: note
            }
            if (supply.supplyId == 0) {
                $.ajax({
                    url: '/WareHouse/AddSupply',
                    type: 'Post',
                    dataType: 'json',
                    data: supply,
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
                    url: '/WareHouse/AddSupply',
                    type: 'Post',
                    dataType: 'json',
                    data: supply,
                    success: function (res) {
                        if (!res.sucess) {
                            if (res.validation && res.validation.Errors) {
                                toastr.error(res.validation.Errors[0].ErrorMessage);
                            }

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



        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới vật tư');
            homeController.resetForm();
            $('#myModal').modal('show');
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
            $('#lblPopupTitle').text('Cập nhật dịch vụ');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Are you sure to delete this employee?", function (result) {
                homeController.deleteEmployee(id);
            });
        });

    },
    deleteEmployee: function (id) {
        $.ajax({
            url: '/Home/Delete',
            data: {
                id: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    bootbox.alert("Delete Success", function () {
                        homeController.loadData(true);
                    });
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
    loadDetail: function (id) {
        $.ajax({
            url: '/Supplier/Service/GetSupplyById',
            data: {
                serviceId: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.result;
                    $('#txtServiceId').val(data.ServiceId);
                    $('#txtServiceName').val(data.Name);
                    $('#txtPrice').val(data.Price);
                    $('#ddlType').val(data.ServiceTypeId).change();
                    $('#ddlStatusId').val(data.ServiceStatusId).change();
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
        $('#txtServiceId').val('0');
        $('#txtPrice').val('');
        $('#ddlStatusId').val(1);
        $('#ddlServiceTypeId').val(1);
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Warehouse/GetAllSupplies',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            SuppliesCode: item.SuppliesCode,
                            SupplyName: item.SuppliesName,
                            SupplyTypeName: item.SuppliesTypeName,
                            Quantity: item.Quantity,
                            Unit: item.Unit,
                            //Status: (item.ServiceStatusId === 1) ? "<span class=\"label label-success\">Hoạt động</span>" : "<span class=\"label label-danger\">Tạm ngưng</span>"
                            Note: item.Note,

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
    }
}
homeController.init();