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
            var accountId = parseInt($('#txtAccountId').val());
            var name = $('#txtName').val();
            var gender = $('#ddlGender').val();
            var patientId = parseInt($('#txtPatientId').val());
            var phone = $('#txtPhoneNumber').val();
            var homeAddress = $('#txtHomeAddress').val();
            var companyAddress = $('#txtCompanyAddress').val();
            var date = '06-10-2018';
            var isDeleted = "False";
            var patient = {
                PatientId: patientId,
                PatientCode: code,
                AccountId: accountId   ,
                FullName: name,
                Gender: gender,
                DateOfBirth: date,
                PhoneNumber: phone,
                HomeAddress: homeAddress,
                CompanyAddress: companyAddress,
                IsDeleted: isDeleted,
                AvatarUrl: $('#avatar').attr('src')
            }
            console.log(patient);
            if (patient.PatientId == 0) {
                $.ajax({
                    url: '/Patient/AddPatient',
                    type: 'Post',
                    dataType: 'json',
                    data: patient,
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
                    url: '/Patient/UpdatePatient',
                    type: 'Post',
                    dataType: 'json',
                    data: patient,
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
            $('#lblPopupTitle').text('Thêm mới bệnh nhân');
            //homeController.resetForm();
            $('#myModal').modal('show');
        });   
        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật thông tin bệnh nhân');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deletePatient(id);
            
        });
        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
    },
    deletePatient: function (id) {
        $.ajax({
            url: '/Patient/DeletePatient',
            data: {
                patientId: id
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
    loadDetail: function (id) {
        $.ajax({
            url: '/Patient/PatientDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    //console.log(data);
                    $('#txtPatientId').val(data.PatientId);
                    $('#txtAccountId').val(data.AccountId);
                    $('#txtCode').val(data.PatientCode);
                    $('#txtName').val(data.FullName);
                    $('#ddlGender').val(data.Gender).change();
                    $('#txtPhoneNumber').val(data.PhoneNumber.trim());
                    $('#txtHomeAddress').val(data.HomeAddress);
                    $('#txtCompanyAddress').val(data.CompanyAddress);
                    $('#avatar').attr('src', data.Avatar);
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
        $('#txtPatientId').val('0');
        $('#txtCode').val('');
        $('#txtAccountId').val('');
        $('#txtName').val('');
        $('#ddlGender').val('').change();
        $('#txtPhoneNumber').val('');
        $('#txtHomeAddress').val('');
        $('#txtCompanyAddress').val('');
        $('#avatar').attr('src', data.Avatar);
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Patient/GetAllPatients',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, phoneNumber: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            PatientId: item.PatientId,
                            PatientCode: item.PatientCode,
                            FullName: item.FullName,
                            PhoneNumber: item.PhoneNumber,
                            HomeAddress: item.HomeAddress,
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