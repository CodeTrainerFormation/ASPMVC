$(document).ready(function () {

    var loading = "Loading...";

    $("td").on("click", "button.preview", function (e) {
        updateModal(); 

        var id = $(this).attr('data-id');
        $.ajax({
            url: "/Student/AjaxDetail/" + id,
            method: 'GET'
        }).then(function (student) {
            updateModal(student.name, "Age : " + student.age + " | Classe : " + student.classroom);
        }, function (error) {

        })
    });

    var updateModal = function (title, content) {
        if (title === undefined || content === undefined) {
            title = loading;
            content = loading;
        } 

        $("#detail-modal-title").html(title);
        $("#detail-modal-body").html(content);

        $(".detail-modal").modal('show');
    }

});