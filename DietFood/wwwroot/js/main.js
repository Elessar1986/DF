DecimalConvert();
ConstOrInterval();

function DecimalConvert() {
    $(".decimal").focusout(function () {
        $(this).val(decimalFormating($(this).val(), 2));
    });

    $(".percent").focusout(function () {
        $(this).val(decimalFormating($(this).val(), 3));
    });

    function decimalFormating(num, digits) {
        if (num != "") {
            num = num.toString().replace(",", ".");
            var n = parseFloat(num);
            if (isNaN(n)) return "";
            var moneyNum = n.toFixed(digits);
            moneyNum = moneyNum.toString();
            moneyNum = moneyNum.replace(".", ",");
            return moneyNum;
        } else {
            return "";
        }
    };
}

function AddDish(data) {
    if (data.status == "ok") {
        window.location = "/Main/EditMeal?mealId=" + data.mealId
    } else {
        $("#panel").html(data);
        ConstOrInterval();
    }
    
}

function ConstOrInterval() {
    $('.radio-ConstOrInterval').change(function () {

        if ($(this).val() == 'false') {
            console.log($(this).val());
            $('#MinWeight, #MaxWeight').prop('disabled', true);//.hide();
            $('#ConstWeight').prop('disabled', false);//.show();
        } else {
            console.log($(this).val());
            $('#MinWeight, #MaxWeight').prop('disabled', false);//.show();
            $('#ConstWeight').prop('disabled', true);//.hide();
        }
    })
}