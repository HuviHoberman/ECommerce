$(() => {
    let categoryId = $(".categories").data('default-category');
    GetItems(categoryId);

    $(".category").on('click', function () {
        categoryId = $(this).data('id');
        GetItems(categoryId);
    });

    function GetItems(categoryId) {
        $.get('/home/GetItems', { categoryId }, function (items) {
            $(".items").empty();
            items.forEach(addItemToDiv);
        });
    };

    const addItemToDiv = item => {
        $(".items").append(`<div class="col-sm-4 col-lg-4 col-md-4">
                 <div class="thumbnail">
                     <a href="/home/item?id=${item.id}">
                        <img src="/itemsImages/${item.image}" alt="">
                        <div class="caption">
                            <h4 class="pull-right">$${item.price.toFixed(2)}</h4>
                            <h4>${item.name}</h4>
                            <p>${item.description}</p>
                        </div>
                     </a>
                 </div>
            </div>`);
    }

    $("#add-To-Cart").on('click', function () {
        const quanity = $("#quantity").val();
        $("#selected-quantity").val(quanity);
    });
});