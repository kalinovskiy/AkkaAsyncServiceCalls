var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();
connection.start().catch(err => console.error(err.toString()));

connection.on("EmailMessage", function (message) {
    var text = $('#txtEmailMessages').text();
    var appendText;
    if (text === undefined) {
        appendText = message;
    } else {
        appendText = "\n" + message;
    }
    $('#txtEmailMessages').append(appendText);
});

connection.on("SmsMessage", function (message) {
    var text = $('#txtSmsMessages').text();
    var appendText;
    if (text === undefined) {
        appendText = message;
    } else {
        appendText = "\n" + message;
    }
    $('#txtSmsMessages').append(appendText);
});

$("#btnToggleEmailState").click(function (e) {
    e.preventDefault();

    $.ajax({
        type: "POST",
        url: "/Home/ToggleEmailState/",
        data: {
        },
        success: function (newState) {
            $('#imgEmailRed').toggleClass('hidden');
            $('#imgEmailGreen').toggleClass('hidden');

            var btnText;
            if (newState === true) {
                btnText = "Выключить обработку Email";
            } else {
                btnText = "Включить обработку Email";
            }
            $("#btnToggleEmailState").attr('value', btnText);
        },
        error: function (result) {
            console.error(result);
        }
    });
});

$("#btnToggleSmsState").click(function (e) {
    e.preventDefault();

    $.ajax({
        type: "POST",
        url: "/Home/ToggleSmsState/",
        data: {
        },
        success: function (newState) {
            $('#imgSmsRed').toggleClass('hidden');
            $('#imgSmsGreen').toggleClass('hidden');

            var btnText;
            if (newState === true) {
                btnText = "Выключить обработку Sms";
            } else {
                btnText = "Включить обработку Sms";
            }
            $("#btnToggleSmsState").attr('value', btnText);
        },
        error: function (result) {
            console.error(result);
        }
    });
});