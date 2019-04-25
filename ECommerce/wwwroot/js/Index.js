$(() => {
    $("#add-To-Cart").on('click', function () {
        const quanity = $("#quantity").val();
        $("#selected-quantity").val(quanity);
    });
    });