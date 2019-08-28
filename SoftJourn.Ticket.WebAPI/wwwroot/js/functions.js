function RangeValue() {
    var rangeElement = document.getElementById("customRange").value;
    var insertElement = document.getElementById("rangediv").innerHTML = "Value: " + rangeElement;
}
function GetResult() {
    var rangeElement = document.getElementById("customRange").value;

    $.ajax({
        type: 'POST',
        url: '/api/Messages',
        dataType: 'json',
        contentType: "application/json",
        data: JSON.stringify(rangeElement),
        statusCode: {
            200: function (value) {
                PrintLogs(value);
                document.getElementById("modalbodyid").innerHTML = 'Successfully sent!';
                $('#exampleModalCenter').modal('toggle');

            },
            500: function (value) {
                PrintLogs(value);
                document.getElementById("modalbodyid").innerHTML = 'Unsuccessful sending (error in the field)';
                $('#exampleModalCenter').modal('toggle');
            }
        }
    });
}

function PrintLogs(object) {
    var myTextarea = document.getElementById("exampleFormControlTextarea1");
    myTextarea.innerHTML = "";
    for (i = 0; i < object.length; i++) {
        if (object[i] !== null) {
            myTextarea.innerHTML += object[i] + "\n";
        }
    }
}