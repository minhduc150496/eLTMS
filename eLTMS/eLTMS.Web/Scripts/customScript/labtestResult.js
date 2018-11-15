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
        $('#btnViewLabTesting').off('click').on('click', function () {
            $('#lblPopupTitle').text('Danh sách các yêu cầu xét nghiệm');
            $('#myModalLabTesting').modal('show');
            homeController.loadDataLabTesting(true);
            
        });      
        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
        $("#input").off('change').on("change", function () {
            var excelFile,
                fileReader = new FileReader();

            $("#result").hide();

            fileReader.onload = function (e) {
                var buffer = new Uint8Array(fileReader.result);
                $('.data-row').remove();
                $.ig.excel.Workbook.load(buffer, function (workbook) {
                    for (var a = 0; a < 10; a++) {
                    var column, row, newRow, cellValue, columnIndex, i,
                        worksheet = workbook.worksheets(a),
                        columnsNumber = 0,
                        gridColumns = [],
                        data = [],
                        worksheetRowsCount;

                    // Both the columns and rows in the worksheet are lazily created and because of this most of the time worksheet.columns().count() will return 0
                    // So to get the number of columns we read the values in the first row and count. When value is null we stop counting columns:
                    while (worksheet.rows(0).getCellValue(columnsNumber)) {
                        columnsNumber++;
                    }

                    // Iterating through cells in first row and use the cell text as key and header text for the grid columns
                    for (columnIndex = 0; columnIndex < columnsNumber; columnIndex++) {
                        column = worksheet.rows(0).getCellText(columnIndex);
                        switch (columnIndex) {
                            case 0:
                                column = "IndexName";
                                break;
                            case 1:
                                column = "IndexValue";
                                break;
                            case 2:
                                column = "Status";
                                break;
                            case 3:
                                column = "Normal";
                                break;
                            case 4:
                                column = "Unit";
                                break;
                        }
                        gridColumns.push({ headerText: column, key: column });
                    }

                    // We start iterating from 1, because we already read the first row to build the gridColumns array above
                    // We use each cell value and add it to json array, which will be used as dataSource for the grid
                    for (i = 1, worksheetRowsCount = worksheet.rows().count(); i < worksheetRowsCount; i++) {
                        newRow = {};
                        row = worksheet.rows(i);

                        for (columnIndex = 0; columnIndex < columnsNumber; columnIndex++) {
                            cellValue = row.getCellText(columnIndex);
                            newRow[gridColumns[columnIndex].key] = cellValue;
                        }

                        data.push(newRow);
                    }

                    homeconfig.ImportExcel = true;
                    $.each(data, function (i, item) {
                     
                        for (var i = 0; i < homeconfig.allLabTesting.length; i++) {
                           
                            if (homeconfig.allLabTesting[i].MachineSlot == (a+1)) {
                                item.LabTestingId = homeconfig.allLabTesting[i].LabTestingId;
                              
                                break;
                            }
                        }
                        var newRow = $('#template-row').clone();
                        $(newRow).insertAfter('#template-row');
                        $(newRow).addClass('data-row');
                        $(newRow).find('.colId').text(item.LabTestingId);
                        $(newRow).find('.colName').text(item.IndexName);
                        $(newRow).find('.colValue').text(item.IndexValue);
                        $(newRow).find('.colStatus').text(item.Status);
                        $(newRow).find('.colNomal').text(item.Normal);
                        $(newRow).find('.colUnit').text(item.Unit);
                        
                    });

                    var allRows = $('.data-row');

                    for (var i = 0; i < allRows.length; i++) {
                        $(allRows[i]).change();
                    }
                    $('.data-row').removeAttr('style');
                    homeconfig.ImportExcel = false;
                        $("#result").show();
                        $("#btnSaveLabTestingIndex").show();
                }
                }, function (error) {
                    $("#result").text("The excel file is corrupted.");
                   
                });
            }

            if (this.files.length > 0) {
                excelFile = this.files[0];
                if (excelFile.type === "application/vnd.ms-excel" || excelFile.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || (excelFile.type === "" && (excelFile.name.endsWith("xls") || excelFile.name.endsWith("xlsx")))) {
                    fileReader.readAsArrayBuffer(excelFile);
                } else {
                    $("#result").text("The format of the file you have selected is not supported. Please select a valid Excel file ('.xls, *.xlsx').");
                    $("#result").show(1000);
                }
            }

        });
        $('.btn-printResult').off('click').on('click', function () {
            var code = $(this).data('id');
            $('#txtResultCode').val(code)
            //homeController.print(code);
            $('#hiddenForm').submit();

        });
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
    loadDataLabTestingResultHaveCode: function (code) {
        $.ajax({
            url: '/LabTest/GetAllLabTestingHaveAppointmentCode',
            type: 'GET',
            dataType: 'json',
            data: { code:code },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTestingResult1-template').html();
                    var allIds = [];
                    $.each(data, function (i, item) {
                        allIds.push(item.LabTestingId);
                        html += Mustache.render(template, {
                            LabTestingId: item.LabTestingId,
                            Name: item.LabTestName,
                            Status: item.Status,
                            Getting: item.AppointmentCode,
                            Group: item.SampleName,

                        });


                    });
                    var allIdsString = JSON.stringify(allIds); 
                    console.log(allIdsString);

                    $('#btnAddNewResult').attr('data-ids', allIdsString);
                    console.log(html);
                    $('#tblDataLabTestingResult1').html(html);
                    $('#txtCode').val(code);
                    homeController.registerEvent();
                } $('#txtResult').val(''); $('#btnAddNewResult').hide();
            }
        })
    },
    print: function (code) {
        $.ajax({
            url: '/LabTest/ExportOrderDetailToPdf',
            data: {
                code: code
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success == true) {
                    toastr.success("Thành công.");
                }
                else {
                    toastr.error("Không thành công.");
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadDataResultId: function (id) {
        $.ajax({
            url: '/LabTest/GetAllLabTestingIndexHaveLabtestingId',
            type: 'GET',
            dataType: 'json',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataResult-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabTestingId: item.LabTestingId,
                            Name: item.IndexName,
                            Value: item.IndexValue,
                            Status: item.LowNormalHigh,
                            Nomal: item.NormalRange,
                            Unit: item.Unit,
                            changeColor: homeController.getColorByStatus(item.LowNormalHigh)
                        });
                        
                    });
                    $('#tblDataResult').html(html);
                    window.open('localhost:52406/LabTest/Result', '_blank');
                    homeController.registerEvent();
                }
            }
        })
    },
    loadAddLabTesting: function () {
        var i = 0;
        for (i; i < 10; i++) {

            var newRow = $('#template1-row').clone();
            $(newRow).addClass('data-row-lab-testing-import');
            console.log(newRow.html());
            var ddlData = "<select class='form-control ddlCode'>";
            ddlData += "<option value=''> --- Chọn vật tư --- </option>";
            $.each(homeconfig.allLabTesting, function (i, item) {

                ddlData += "<option value='" + item.LabTestingId + "' data-name='" + item.LabTestName + "'>" + item.AppointmentCode + " - " + item.LabTestName + "</option>"
                

            });
            ddlData += "</select>";
            $(newRow).find('.txtSlot').val(10 - i);
            $(newRow).find('.colCode').html(ddlData);
            $(newRow).insertAfter('#template1-row');
            $(newRow).removeAttr('style');
            homeController.registerEventForChangeDropDown();
            
        }

    },
    registerEventForChangeDropDown: function () {
        $('.ddlCode').off('change').on('change', function () {

            var value = $(this).find(':selected').val();       
            var allRows = $('.data-row-lab-testing-import');         
            var name = $(this).find(':selected').data('name');
            var curentRow = $(this).closest('tr');
            $(curentRow).find('.colName').text(name);
        });
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
    getColorByStatus: function (status) {
        if (status == 'L') {
            return 'background-color: yellow;'
        } else if (status == 'H') {
            return 'background-color: red;'
        } else {
            return "";
        }
                
    }
}

homeController.init();