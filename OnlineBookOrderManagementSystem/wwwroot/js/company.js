//$(document).ready(function () {
//    loadDataTable();
//});


//function loadDataTable() {
//    $('#tblData').DataTable({
//        "ajax": { ur:'/admin/product/getall'},
//        "columns": [
//            { data: 'name',"width":"15%" },
//            { data: 'position', "width": "15%" },
//            { data: 'salary', "width": "15%" },
//            { data: 'office', "width": "15%" }
//        ]
//    });
//};
//$(document).ready(function () {
//    $.ajax({
//        url: '/Admin/Company/GetAll',
//        method: 'GET',
//        dataType: 'json',
//        success: function (response) {
//            console.log("responsedata", response);
//            if (response.data) {
//                console.log(response.data);
//                //Build Tabulator
//                var table = new Tabulator("#example-table", {
//                    height: "311px",
//                    data: response.data,
//                    layout: "fitColumns",
//                    placeholder: "No Data Set",
//                    pagination: "local",
//                    paginationSize: 6,
//                    paginationSizeSelector: [3, 6, 8, 10],
//                    movableColumns: true,
//                    paginationCounter: "rows",
//                    columns: [
//                        { title: "Name", field: "name", sorter: "string", width: 200 },
//                        { title: "StreetAddress", field: "streetAddress", sorter: "string", width: 200 },
//                        { title: "Isbn", field: "isbn", sorter: "string", width: 200 },
//                        { title: "City", field: "city", sorter: "string", width: 200 },
//                        { title: "State", field: "state", sorter: "string", width: 200 },
//                        { title: "PostalCode", field: "postalCode", sorter: "string", width: 200 },
//                        { title: "PhoneNumber", field: "phoneNumber", sorter: "string", width: 200 },
//                        {
//                            title: "Actions", field: "", sorter: "string", width: 200, formatter: function (cell, formatterParams, onRendered) {
//                                var id = cell.getRow().getData().id;
//                                return `<a href='/Admin/Products/Upsert/${id}' class='btn btn-sm btn-info'>Edit</a> <a href='/Admin/Products/Details/${id}' class='btn btn-sm btn-info'>Details</a> <a href='/Admin/Products/Delete/${id}' class='btn btn-sm btn-primary'>Delete</a>
//                                `;
//                            }
//                        }
//                    ],
//                });
//            }
//            else {
//                console.log("No data found.");
//            }
//        },
//        error: function (xhr, status, error) {
//            console.error("An error occurred: " + error);
//        }
//    });
//});


$.ajax({
    url: '/Admin/Company/GetAllCompany',
    method: 'GET',
    dataType: 'json',
    success: function (response) {
        console.log("responsedata", response);
        if (response.data) {
            console.log(response.data);
            //Build Tabulator
            var table = new Tabulator("#example-table", {
                height: "311px",
                data: response.data,
                layout: "fitColumns",
                placeholder: "No Data Set",
                pagination: "local",
                paginationSize: 6,
                paginationSizeSelector: [3, 6, 8, 10],
                movableColumns: true,
                paginationCounter: "rows",
                columns: [
                    { title: "Name", field: "name", sorter: "string", width: 200 },
                    { title: "StreetAddress", field: "streetAddress", sorter: "string", width: 200 },
                    { title: "City", field: "city", sorter: "string", width: 200 },
                    { title: "State", field: "state", sorter: "string", width: 200 },
                    { title: "PostalCode", field: "postalCode", sorter: "string", width: 200 },
                    { title: "PhoneNumber", field: "phoneNumber", sorter: "string", width: 200 },
                    {
                        title: "Actions", field: "", sorter: "string", width: 200, formatter: function (cell, formatterParams, onRendered) {
                            var id = cell.getRow().getData().id;
                            return `<a href='/Admin/Company/Edit/${id}' class='btn btn-sm btn-info'>Edit</a> <a href='/Admin/Company/Details/${id}' class='btn btn-sm btn-info'>Details</a> <a href='/Admin/Company/Delete/${id}' class='btn btn-sm btn-primary'>Delete</a>
                                `;
                        }
                    }
                ],
            });
        }
        else {
            console.log("No data found.");
        }
    },
    error: function (xhr, status, error) {
        console.error("An error occurred: " + error);
    }
});