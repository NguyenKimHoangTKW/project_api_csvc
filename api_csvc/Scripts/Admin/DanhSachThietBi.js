$(".select2").select2();
let value_check = null;
$(document).ready(function () {
    load_data();
});

$(document).on("click", "#addnew", function () {
    $("#title_modal").text("Thêm mới thiết bị");
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
$(document).on("click", "#SaveNew", function () {
    add_new();
});
$(document).on("click", "#SaveEdit", function () {
    update_thiet_bi(value_check);
});
$(document).on("click", "#btnDelete", function () {
    const value = $(this).data("id");
    Swal.fire({
        title: "Bạn đang thao tác xóa thiết bị?",
        text: "Bằng việc đổng ý, bạn sẽ xóa tất cả dữ liệu liên quan đến thiết bị này, bạn có đồng ý không?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Có, tôi đồng ý!"
    }).then((result) => {
        if (result.isConfirmed) {
            delete_thiet_bi(value);
        }
    })
});
async function get_info(value) {
    const res = await $.ajax({
        url: `${BASE_URL}/get-info-thiet-bi`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_thiet_bi: value
        })
    });
    $("#thiet-bi-val").val(res.ten_thiet_bi);
    $("#thong-so-val").val(res.thong_so);
    $("#so-luong-val").val(res.so_luong);
    $("#mo-ta-val").val(res.mo_ta);
    $("#thuong-hieu-val").val(res.ten_thuong_hieu).trigger("change");
    $("#don-vi-tinh-val").val(res.ten_don_vi_tinh).trigger("change");
    $("#phan-loai-val").val(res.ten_phan_loai).trigger("change");
}
async function delete_thiet_bi(value) {
    const res = await $.ajax({
        url: `${BASE_URL}/delete-thiet-bi`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_thiet_bi: value
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
async function add_new() {
    const thiet_bi = $("#thiet-bi-val").val();
    const thong_so = $("#thong-so-val").val();
    const so_luong = $("#so-luong-val").val();
    const mo_ta = $("#mo-ta-val").val();
    const thuong_hieu = $("#thuong-hieu-val").val();
    const don_vi_tinh = $("#don-vi-tinh-val").val();
    const phan_loai = $("#phan-loai-val").val();
    const res = await $.ajax({
        url: `${BASE_URL}/update-thiet-bi`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            ten_thiet_bi: thiet_bi,
            thong_so: thong_so,
            ten_thuong_hieu: thuong_hieu,
            ten_don_vi_tinh: don_vi_tinh,
            ten_phan_loai: phan_loai,
            so_luong: so_luong,
            mo_ta: mo_ta
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
async function update_thiet_bi(value) {
    const thiet_bi = $("#thiet-bi-val").val();
    const thong_so = $("#thong-so-val").val();
    const so_luong = $("#so-luong-val").val();
    const mo_ta = $("#mo-ta-val").val();
    const thuong_hieu = $("#thuong-hieu-val").val();
    const don_vi_tinh = $("#don-vi-tinh-val").val();
    const phan_loai = $("#phan-loai-val").val();
    const res = await $.ajax({
        url: `${BASE_URL}/sua-thiet-bi`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_thiet_bi: value,
            ten_thiet_bi: thiet_bi,
            thong_so: thong_so,
            ten_thuong_hieu: thuong_hieu,
            ten_don_vi_tinh: don_vi_tinh,
            ten_phan_loai: phan_loai,
            so_luong: so_luong,
            mo_ta: mo_ta
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
        url: `${BASE_URL}/get_full_thiet_bi`,
        type: 'GET'
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
                <th>ID thiết bị</th>
                <th>Tên thiết bị</th>
                <th>Thông số</th>
                <th>Tên thương hiệu</th>
                <th>Số lượng</th>
                <th>Mô tả</th>
                <th>Tên phân loại</th>
                <th>Tên trạng thái</th>
                <th>Đơn vị tính</th>
                <th>Chức năng</th>
            </tr>
        </thead>
        <tbody>`;
    res.forEach((items, index) => {
        html +=
            `
                <tr>
                    <td class="formatSo">${index + 1}</td>
                    <td class="formatSo">${items.id_thiet_bi}</td>
                    <td>${items.ten_thiet_bi}</td>
                    <td class="formatSo">${items.thong_so}</td>
                    <td>${items.ten_thuong_hieu}</td>
                    <td class="formatSo">${items.so_luong}</td>
                    <td>${items.mo_ta == null ? "" : items.mo_ta}</td>
                    <td>${items.ten_phan_loai}</td>
                    <td>${items.ten_trang_thaii}</td>
                    <td>${items.ten_don_vi_tinh}</td>
                    <td>
                        <button class="btn btn-icon btn-hover btn-sm btn-rounded pull-right" id="btnEdit" data-id="${items.id_thiet_bi}">
                            <i class="anticon anticon-edit"></i>
                        </button>
                        <button class="btn btn-icon btn-hover btn-sm btn-rounded pull-right" id="btnDelete" data-id="${items.id_thiet_bi}">
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