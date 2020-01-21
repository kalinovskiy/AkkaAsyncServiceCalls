var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();
connection.start().catch(err => console.error(err.toString()));

connection.on("TraceMessage", function (message) {
    var text = $('#blockOutput').text();
    var appendText;
    if (text === undefined) {
        appendText = message;
    } else {
        appendText = "</br>" + message;
    }
    $('#blockOutput').append(appendText);
});

connection.on("Finish", function () {
    $('#btnSend').removeAttr('disabled');
    $('#btnStop').attr('disabled', 'disabled');
});

$("#btnSend").click(function (e) {
    e.preventDefault();

    $('#btnSend').attr('disabled', 'disabled');

    $('#blockOutput').text("");

    $.ajax({
        type: "POST",
        url: "/Home/SendMessage/",
        data: {
            messages: $("#txtMessages").val()
        },
        success: function () {
            $('#btnStop').removeAttr('disabled');
        },
        error: function (result) {
            $('#btnSend').removeAttr('disabled');
            console.error(result);
        }
    });
});

$("#btnStop").click(function (e) {
    e.preventDefault();

    $('#btnStop').attr('disabled', 'disabled');

    $.ajax({
        type: "POST",
        url: "/Home/StopMessages/",
        data: {
        },
        success: function () {
            $('#btnSend').removeAttr('disabled');
        },
        error: function (result) {
            $('#btnStop').removeAttr('disabled');

            console.error(result);
        }
    });
});