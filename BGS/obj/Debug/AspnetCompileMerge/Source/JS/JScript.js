$(function () {
    //debugger;
    $("[id$=txtQuarter]").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'WebService/AutoComplete.asmx/GetQuarters',
                data: "{ 'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split('-')[0],
                            val: item.split('-')[1]
                        }
                    }))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("[id$=hfQuarterId]").val(i.item.val);
        },
        minLength: 1
    });
});
function checkAll(objRef) {
    var GridView = objRef.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    for (var i = 0; i < inputList.length; i++) {
        var row = inputList[i].parentNode.parentNode;
        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
            if (objRef.checked) {
                inputList[i].checked = true;
            }
            else {
                inputList[i].checked = false;
            }
        }
    }
}





