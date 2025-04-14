$("#multi-filter-select").DataTable({
    pageLength: 5,
    order: [],
    initComplete: function () {
        this.api()
            .columns()
            .every(function () {
                var column = this;
                var select = $(
                    '<select class="form-select"><option value=""></option></select>'
                )
                    .appendTo($(column.footer()).empty())
                    .on("change", function () {
                        var val = $.fn.dataTable.util.escapeRegex($(this).val());

                        column
                            .search(val ? "^" + val + "$" : "", true, false)
                            .draw();
                    });

                column
                    .data()
                    //.unique()
                    //.sort()
                    .each(function (d, j) {
                        select.append(
                            '<option value="' + d + '">' + d + "</option>"
                        );
                    });
            });
    },
});
WebFont.load({
    google: { families: ["Public Sans:300,400,500,600,700"] },
    custom: {
        families: [
            "Font Awesome 5 Solid",
            "Font Awesome 5 Regular",
            "Font Awesome 5 Brands",
            "simple-line-icons",
        ],
        urls: [window.location.origin + "/css/fonts.min.css"],
    },
    active: function () {
        sessionStorage.fonts = true;
    },
});

//document.addEventListener("DOMContentLoaded", function () {
//    var buttons = document.querySelectorAll(".confirm-btn");
//    buttons.forEach(function (btn) {
//        btn.addEventListener("click", function (e) {
//            e.preventDefault();
//            var url = btn.getAttribute("data-url");

//            swal({
//                title: "Emin misiniz?",
//                text: "Bu işlemi geri alamazsınız!",
//                icon: "warning",
//                buttons: ["İptal", "Evet, devam et"],
//                dangerMode: true
//            }).then(function (willDelete) {
//                if (willDelete) {
//                    window.location.href = url;
//                }
//            });
//        });
//    });
//});
$(document).ready(function () {
    // Sayfa yüklendiğinde, ve her sayfa geçişinde aşağıdaki kod çalışacak
    $(document).on("click", ".confirm-btn", function (e) {
        console.log("selam");
        e.preventDefault();
        var url = $(this).data("url");  // jQuery ile data-url'yi alıyoruz

        swal({
            title: "Emin misiniz?",
            text: "Bu işlemi geri alamazsınız!",
            icon: "warning",
            buttons: ["İptal", "Evet, devam et"],
            dangerMode: true
        }).then(function (willDelete) {
            if (willDelete) {
                window.location.href = url;  // Kullanıcı silmeyi onaylarsa yönlendirme yapıyoruz
            }
        });
    });
});


