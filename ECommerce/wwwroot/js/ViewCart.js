$(() => {
    let cartId = $("table").data('cart-id');
    ClearAndPopulateTable(cartId);

    function ClearAndPopulateTable(cartId) {
        $("table tr:gt(0)").remove();
        $.post('/home/GetCartItems', { cartId }, function (items) {
            items.forEach(AddCartItems);
        });
    };

    const AddCartItems = item => {
        const itemId = item.id;
        $("table").append(`<tr>
            <td width="200"> <img src="/itemsImages/${item.image}" width="200" style="padding:0;" /></td>
            <td>${item.name}</td>
            <td>${item.description}</td>
            <td>${item.price}</td>
            <td id=${item.id}>             
                <button class="btn btn-danger pull-left delete" data-item-id="${item.id}">Delete</button>
            </td >
            <td>$${(item.price * item.quantity).toFixed(2)}</td>
        </tr > `
        );
        var select = createSelect(item.quantity, item.id);
        $(`#${item.id}`).append(select);       
    };

    function createSelect(quantity, itemId) {
        var select = document.createElement("select");
        select.setAttribute('data-item-id', itemId);
        select.className="quantity"
        for (let i = 1; i <= 10; i++) {
            var option = document.createElement("option");
            option.value = i;
            option.text = i;
            option.style = "form-control";
            if (quantity === i) {
                option.selected = true;
            }
            select.add(option);            
        }
        return select;        
    }       

    $("table").on('click', '.delete', function () {
        const itemId = $(this).data('item-id');
        console.log(itemId);
        $.post('/home/DeleteItem', { cartId: cartId, itemId: itemId }, function (items) {
            ClearAndPopulateTable(cartId);
        });
    });

    $("table").on('change', '.quantity', function () {
        const itemId = $(this).data('item-id');
        const quantity = $(this).val();
        console.log(itemId);
        console.log(quantity);
        $.post('/home/UpdateItem', { cartId, itemId, quantity }, function (items) {
            ClearAndPopulateTable(cartId);
        });
    });
});