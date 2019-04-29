$(() => {
    let categoryId = $(".categories").data('default-category')
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
                <img src="/itemsImages/${item.image}" alt="">
                    <div class="caption">
                        <h4 class="pull-right">$${item.price.toFixed(2)}</h4>
                        <h4>
                            <a href="/home/item?id=${item.id}">${item.name}</a>
                        </h4>
                        <p>${item.description}</p>
                    </div>
                        </div>

            </div>`);
    }

    $("#add-To-Cart").on('click', function () {
        const quanity = $("#quantity").val();
        $("#selected-quantity").val(quanity);
    });

    $(".delete-item").on('click', function () {
        const cartId =$("#cart-items").data('cart-id')
        const itemId = $(this).data('item-id');
        $.post('/home/DeleteAndGetItems', { cartId: cartId, itemId: itemId }, function (items) {
            $("#cart-items tr:gt(1)").remove();
            items.forEach(AddCartItems);
        });
    });

    const AddCartItems = item => {
        `<tr>
            <td width="200"> <img src="/itemsImages/${item.image}" width="200" style="padding:0;" /></td>
            <td> ${item.Name}</td>
            <td> ${item.Description}</td>
            <td> ${item.Price}</td>
            <td>
                <select id="quantity" class="form-control" style="width:70px;">
                    @for (int x = 1; x <= 10; x++)
                    {
                        if (x == i.Quantity)
                        {
                        <option selected="selected" data-id="@x)">@x</option>}
                    <option data-id="@x)">@x</option>
                    }
                </select>
                <button class="btn btn-danger pull-left delete-item" data-item-id="${item.Id}">Delete</button>
            </td>
            <td> $${(item.Price * i.Quantity)}</td>
        </tr>`
    };

});