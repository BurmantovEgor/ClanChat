const connection = new signalR.HubConnectionBuilder()
    .withUrl("/messageHub")  // URL хаба
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Подключаемся к хабу
connection.start().then(function () {
    console.log("Connected to SignalR hub");

    // Присоединяемся к группе с ID клана, чтобы получать сообщения
    const clanId = "43f5aaf4-3cac-4fda-974c-beeee90e5cce"; // Пример ID клана
    connection.invoke("JoinGroup", `clan-${clanId}`).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    console.error("Connection failed: " + err.toString());
});

// Обработчик получения новых сообщений
connection.on("ReceiveNewMessage", function (message) {
    console.log("Received new message:", message);
    // Обновление UI с новым сообщением (например, добавление в список)
});
