$(".select2").select2();
let value_check = null;
$(document).ready(function () {
    load_data();
});
$(document).on("click", "#btnEdit", function () {
    const value = $(this).data("id");
    $("#title_modal").text("Duyệt mượn thiết bị");
    let html = ``;
    let footer_modal = $(".modal-footer");
    footer_modal.empty();
    html =
        `
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" id="SaveEdit">Duyệt</button>
        `;
    footer_modal.html(html);
    get_info(value);
    value_check = value;
    $(".bd-example-modal-lg").modal("show");
});
$(document).on("click", "#SaveEdit", function () {
    update_thiet_bi(value_check);
});
async function get_info(value) {
    const res = await $.ajax({
        url: `${BASE_URL}/info-danh-sach-muon`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_danh_sach_muon: value
        })
    });
    $("#thiet-bi-val").val(res.ten_thiet_bi);
    $("#mo-ta-val").val(res.ly_do_huy);
};
async function update_thiet_bi(value) {
    const thiet_bi = $("#thiet-bi-val").val();
    const mo_ta = $("#mo-ta-val").val();
    const trang_thai = $("#trang_thai-val").val();
    const res = await $.ajax({
        url: `${BASE_URL}/duyet-muon-user`,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            id_danh_sach_muon: value,
            ten_thiet_bi: thiet_bi,
            ten_trang_thai: trang_thai,
            ly_do_huy: mo_ta
        })
    });
    load_data();
    Sweet_Alert("success", res.message);
};
async function load_data() {
    const res = await $.ajax({
        url: `${BASE_URL}/get-full-thiet-bi-muon`,
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
                <th>ID thiết bị mượn</th>
                <th>Tên cán bộ viên chức mượn</th>
                <th>Tên phòng học mượn</th>
                <th>Tên thiết bị</th>
                <th>Số lượng mượn</th>
                <th>Yêu cầu của người mượn</th>
                <th>Trạng thái</th>
                <th>Ngày đăng ký mượn</th>
                <th>Ngày mượn</th>
                <th>Ngày trả</th>
                <th>Ngày hủy</th>
                <th>Lý do hủy</th>
                <th>Chức năng</th>
            </tr>
        </thead>
        <tbody>`;
    res.forEach((items, index) => {
        html +=
            `
                <tr>
                    <td class="formatSo">${index + 1}</td>
                    <td class="formatSo">${items.id_danh_sach_muon}</td>
                    <td>${items.name_CBVC}</td>
                    <td class="formatSo">${items.ten_phong_hoc}</td>
                    <td>${items.ten_thiet_bi}</td>
                    <td class="formatSo">${items.so_luong_muon}</td>
                    <td>${items.yeu_cau == null ? "" : items.yeu_cau}</td>
                    <td>${items.ten_trang_thaii}</td>
                    <td>${items.ngay_dang_ky_muon != null ? unixTimestampToDate(items.ngay_dang_ky_muon) : ""}</td>
                    <td>${items.ngay_muon != null ? unixTimestampToDate(items.ngay_muon) : ""}</td>
                    <td>${items.ngay_tra != null ? unixTimestampToDate(items.ngay_tra) : ""}</td>
                    <td>${items.ngay_huy != null ? unixTimestampToDate(items.ngay_huy) : ""}</td>
                    <td>${items.ly_do_huy == null ? "" : items.ly_do_huy}</td>
                    <td>
                        <button class="btn btn-icon btn-hover btn-sm btn-rounded pull-right" id="btnEdit" data-id="${items.id_danh_sach_muon}">
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
                title: 'Danh sách thiết bị mượn'
            },
            {
                extend: 'print',
                title: 'Danh sách thiết bị mượn'
            }
        ]
    });
}
function unixTimestampToDate(unixTimestamp) {
    var date = new Date(unixTimestamp * 1000);
    var weekdays = ['Chủ Nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
    var dayOfWeek = weekdays[date.getDay()];
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var day = ("0" + date.getDate()).slice(-2);
    var year = date.getFullYear();
    var hours = ("0" + date.getHours()).slice(-2);
    var minutes = ("0" + date.getMinutes()).slice(-2);
    var seconds = ("0" + date.getSeconds()).slice(-2);
    var formattedDate = dayOfWeek + ', ' + day + "-" + month + "-" + year + " " + hours + ":" + minutes + ":" + seconds;
    return formattedDate;
}