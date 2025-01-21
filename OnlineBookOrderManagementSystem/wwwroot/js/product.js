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
$(document).ready(function () {
    $.ajax({
        url: '/Admin/Products/GetAll',
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
                        { title: "Title", field: "title", sorter: "string", width: 200 },
                        { title: "Discription", field: "discription", sorter: "string", width: 200 },
                        { title: "Isbn", field: "isbn", sorter: "string", width: 200 },
                        { title: "Author", field: "author", sorter: "string", width: 200 },
                        { title: "Category", field: "category.name", sorter: "string", width: 200 },
                        { title: "List Price", field: "listPrice", sorter: "string", width: 200 },
                        { title: "Price 1-50", field: "price", sorter: "string", width: 200 },
                        { title: "Price for 50+", field: "price50", sorter: "string", width: 200 },
                        { title: "Price for 100+", field: "price100", sorter: "string", width: 200 },
                        {
                            title: "Actions", field: "", sorter: "string", width: 200, formatter: function (cell, formatterParams, onRendered) {
                                var id = cell.getRow().getData().id;
                                return `<a href='/Admin/Products/Upsert/${id}' class='btn btn-sm btn-info'>Edit</a> <a href='/Admin/Products/Details/${id}' class='btn btn-sm btn-info'>Details</a> <a href='/Admin/Products/Delete/${id}' class='btn btn-sm btn-primary'>Delete</a>
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
});


