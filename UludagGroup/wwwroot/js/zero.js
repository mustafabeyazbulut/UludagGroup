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