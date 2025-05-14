$(".select2").select2();
let value_check = null;
$(document).ready(function () {
    load_data();
});
$(document).on("click", "#addnew", function () {
    $("#title_modal").text("Thêm mới cán bộ viên chức");
    let html = ``;
    let footer_modal = $(".modal-footer");
    footer_modal.empty();
    html =
        `
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" id="SaveNew">Save changes</button>
        `;
    footer_modal.html(html);
    $(".bd-example-modal-lg").modal("show");
});
$(document).on("click", "#btnEdit", function () {
    const value = $(this).data("id");
    $("#title_modal").text("Chỉnh sửa thiết bị");
    let html = ``;
    let footer_modal = $(".modal-footer");
    footer_modal.empty();
    html =
        `
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" id="SaveEdit">Save changes</button>
        `;
    footer_modal.html(html);
    get_info(value);
    value_check = value;
    $(".bd-example-modal-lg").modal("show");
});
$(document).on("click", "#btnDelete", function () {
    const value = $(this).data("id");
    Swal.fire({
        title: "Bạn đang thao tác xóa cán bộ viên chức?",
        text: "Bằng việc đổng ý, bạn sẽ xóa tất cả dữ liệu liên quan đến cán bộ viên chức này, bạn có đồng ý không?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Có, tôi đồng ý!"
    }).then((result) => {
        if (result.isConfirmed) {
            delete_cbvc(value);
        }
    })
});
$(document).on("click", "#SaveNew", function () {
    add_new();
});
$(document).on("click", "#SaveEdit", function () {
    update_cbvc(value_check);
});
async function delete_cbvc(value) {
    const res = await $.ajax({
        url: `${BASE_URL}/delete-cbvc`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_CBVC: value
        })
    });
    if (res.success) {
        load_data();
        Sweet_Alert("success", res.message);
    }
    else {
        Sweet_Alert("error", res.message);
    }
}
async function get_info(value) {
    const res = await $.ajax({
        url: `${BASE_URL}/get-info-cbvc`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_CBVC: value
        })
    });
    $("#ten-cbvc-val").val(res.name_CBVC);
    $("#email-val").val(res.email);
}
async function update_cbvc(value) {
    const ten_cbvc = $("#ten-cbvc-val").val();
    const email = $("#email-val").val();
    const res = await $.ajax({
        url: `${BASE_URL}/update-cbvc`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_CBVC: value,
            name_CBVC: ten_cbvc,
            email: email
        })
    });
    if (res.success) {
        load_data();
        Sweet_Alert("success", res.message);
    }
    else {
        Sweet_Alert("error", res.message);
    }
};
async function add_new() {
    const ten_cbvc = $("#ten-cbvc-val").val();
    const email = $("#email-val").val();
    const res = await $.ajax({
        url: `${BASE_URL}/them-moi-cbvc`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            name_CBVC: ten_cbvc,
            email: email
        })
    });
    if (res.success) {
        load_data();
        Sweet_Alert("success", res.message);
    }
    else {
        Sweet_Alert("error", res.message);
    }
};
async function load_data() {
    const res = await $.ajax({
        url: `${BASE_URL}/get-full-cbvc`,
        type: 'GET'
    });
    if ($.fn.DataTable.isDataTable('#data-table')) {
        $('#data-table').DataTable().clear().destroy();
    }
    const body = $("#data-table");
    body.empty();
    let html = ``;
    if (res.success) {
        html =
            `
        <thead>
            <tr>
                <th>STT</th>
                <th>ID cán bộ viên chức</th>
                <th>Tên cán bộ viên chức</th>
                <th>Email</th>
                <th>Chức năng</th>
            </tr>
        </thead>
        <tbody>`;
        res.data.forEach((items, index) => {
            html +=
                `
                <tr>
                    <td class="formatSo">${index + 1}</td>
                    <td class="formatSo">${items.id_CBVC}</td>
                    <td>${items.name_CBVC}</td>
                    <td>${items.email}</td>
                    <td>
                        <button class="btn btn-icon btn-hover btn-sm btn-rounded pull-right" id="btnEdit" data-id="${items.id_CBVC}">
                            <i class="anticon anticon-edit"></i>
                        </button>
                        <button class="btn btn-icon btn-hover btn-sm btn-rounded pull-right" id="btnDelete" data-id="${items.id_CBVC}">
                            <i class="anticon anticon-delete"></i>
                        </button>
                    </td>
                </tr>
            `;
        });
        html +=
            `
         </tbody>
        `;
        body.html(html);
        $('#data-table').DataTable({
            pageLength: 7,
            lengthMenu: [5, 10, 25, 50, 100],
            ordering: true,
            searching: true,
            autoWidth: false,
            responsive: true,
            language: {
                paginate: {
                    next: "Next",
                    previous: "Previous"
                },
                search: "Search",
                lengthMenu: "Show _MENU_ entries"
            },
            dom: "Bfrtip",
            buttons: [
                {
                    extend: 'excel',
                    title: 'Danh sách thiết bị'
                },
                {
                    extend: 'print',
                    title: 'Danh sách thiết bị'
                }
            ]
        });
    }
    else {
        Sweet_Alert("error", res.message);
    }
}