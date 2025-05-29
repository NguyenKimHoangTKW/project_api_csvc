$(".select2").select2();
let value_check = null;
$(document).ready(function () {
    load_data();
});
$(document).on("click", "#addnew", function () {
    $("#title_modal").text("Thêm mới tài khoản");
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
    $("#title_modal").text("Chỉnh sửa Account");
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
$(document).on("click", "#SaveEdit", function () {
    update_account(value_check);
});
$(document).on("click", "#SaveNew", function () {
    add_account();
});
async function get_info(value) {
    const res = await $.ajax({
        url: `${BASE_URL}/info-account`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_account: value
        }),
        headers: {
            Authorization: `Bearer ${token}`
        }
    });

    $("#ten-user-val").val(res.name).prop("readonly", true);
    $("#email-val").val(res.email).prop("readonly", true);
    $("#phan-quyen-val").val(res.ten_role).trigger("change");
}

async function add_account() {
    const email = $("#email-val").val();
    const role = $("#phan-quyen-val").val();
    const res = await $.ajax({
        url: `${BASE_URL}/create-account-email`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            email: email,
            ten_role: role,
        }),
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
    if (res.success) {
        load_data();
        Sweet_Alert("success", res.message);
    }
    else {
        Sweet_Alert("error", res.message);
    }
};
async function update_account(value) {
    const role = $("#phan-quyen-val").val();
    const res = await $.ajax({
        url: `${BASE_URL}/update-account`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_account: value,
            ten_role: role,
        }),
        headers: {
            Authorization: `Bearer ${token}`
        }
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
        url: `${BASE_URL}/get_full_account`,
        type: 'GET',
        headers: {
            Authorization: `Bearer ${token}`
        }
    });
    if ($.fn.DataTable.isDataTable('#data-table')) {
        $('#data-table').DataTable().clear().destroy();
    }
    const body = $("#data-table");
    body.empty();
    let html = ``;
    html =
        `
        <thead>
            <tr>
                <th>STT</th>
                <th>ID Account</th>
                <th>Tên Account</th>
                <th>Email</th>
                <th>Phân quyền</th>
                <th>Chức năng</th>
            </tr>
        </thead>
        <tbody>`;
    res.forEach((items, index) => {
        html +=
            `
                <tr>
                    <td class="formatSo">${index + 1}</td>
                    <td class="formatSo">${items.id_account}</td>
                    <td>${items.name}</td>
                    <td>${items.email}</td>
                    <td>${items.ten_role}</td>
                    <td>
                        <button class="btn btn-icon btn-hover btn-sm btn-rounded pull-right" id="btnEdit" data-id="${items.id_account}">
                            <i class="anticon anticon-edit"></i>
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