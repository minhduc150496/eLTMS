var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allSupply: []
}
var homeController = {
    init: function () {
        //homeController.loadData();
        homeController.registerEvent();
        homeController.loadAllSuply();
    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            var code = $('#txtCode').val();
            var name = $('#txtName').val();
            var type = parseInt($('#ddlSupplyType').val());
            var supplyId = $('#txtSupplyId').val();
            var unit = $('#ddlSupplyUnit').val();
            var note = $('#txtNote').val();
            var quantity = '0';
            var supply = {
                SuppliesId: supplyId,
                SuppliesCode: code,
                SuppliesName: name,
                SuppliesTypeId: type,
                Quantity:quantity,
                Unit: unit,
                Note: note
            }
            if (supply.SuppliesId == 0) {
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
                    url: '/WareHouse/UpdateSupply',
                    type: 'Post',
                    dataType: 'json',
                    data: supply,
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



        $('#btnAddNew').off('click').on('click', function () {
            var newRow = $('#template-row').clone();
            $(newRow).addClass('data-row');
            console.log(newRow.html());
            var ddlData = "<select class='form-control ddlCode'>";
            $.each(homeconfig.allSupply, function (i, item) {
                ddlData += "<option value='" + item.SuppliesId + "' data-name='" + item.SuppliesName + "' data-unit='" + item.Unit + "'>" + item.SuppliesCode + "</option>"

            });           
            ddlData += "</select>";
            var codeColumn = $(newRow).find('.colCode').html(ddlData);
            $(newRow).insertAfter('#template-row');
            $(newRow).removeAttr('style');
            homeController.registerEventForChangeDropDown();
            
        });
        $('#btnView').off('click').on('click', function () {
            $('#lblPopupTitle').text('Danh sách phiếu nhập kho');
            homeController.resetForm();
            $('#myModal').modal('show');
        });

        $('#btnSaveImport').off('click').on('click', function () {
            var allRows = $('.data-row');
            var tmpData = [];
            $.each(allRows, function (i, item) {
                var detail = {};
                detail.SuppliesId = $(item).find('.ddlCode').val();
                detail.Quantity = $(item).find('.txtQuantity').val();
                detail.Note = $(item).find('.txtNote').val();
                detail.Unit = $(item).find('.colUnit').text();
                tmpData.push(detail);
            });
            var data = {
                ImportPaperCode: $('#txtImportCode').val(),
                Note: $('#txtNote').val(),
                ImportPaperDetails: tmpData
            };
            $.ajax({
                url: '/WareHouse/AddImportPaper',
                type: 'Post',
                dataType: 'json',
                data: data,
                async: false,
                success: function (res) {
                    if (!res.success) {
                        toastr.success("Tạo mới không thành công.");

                    }
                    else {
                        toastr.success("Tạo mới thành công.");
                        $('#myModal').modal('hide');
                        homeController.loadData();
                    }
                }
            })
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
            $('#myModal').modal('hide');
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
            url: '/WareH/Delete',
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
            url: '/WareHouse/LoadPaperImportDetailId',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    $('#txtImportCode').val(data.ImportPaperCode);
                    $('#txtNote').val(data.Note);
                    $('#txtCreateDate').val(data.CreateDate);
                    $('#txtCreateDate').removeAttr('style');
                    homeController.loadDataDetail(id);
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
        $.ajax({
            url: '/Warehouse/GetAllImportPapers',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, createDate: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ImportPaperId: item.ImportPaperId,
                            ImportPaperCode: item.ImportPaperCode,
                            CreateDate: item.CreateDate,
                        });

                    });
                    console.log(html);
                    $('#tblData1').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataDetail: function (id) {
        $.ajax({
            url: '/Warehouse/LoadPaperImportDetailId',
            type: 'GET',
            dataType: 'json',
            data: {  id: id },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ImportPaperId: item.ImportPaperId,
                            ImportPaperCode: item.Unit,
                            CreateDate: item.Note,
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
    loadAllSuply: function () {
        $.ajax({
            url: '/WareHouse/GetAllSupply',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    homeconfig.allSupply = data;

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
    registerEventForChangeDropDown: function () {
        $('.ddlCode').off('change').on('change', function () {

            var name = $(this).find(':selected').data('name');
            var unit = $(this).find(':selected').data('unit');
            var curentRow = $(this).closest('tr');
            $(curentRow).find('.colName').text(name);
            $(curentRow).find('.colUnit').text(unit);
        });
    }
}
homeController.init();