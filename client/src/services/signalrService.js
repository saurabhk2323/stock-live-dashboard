import * as signalR from "@microsoft/signalr"

let connection = null;

export const startSignalR = (onStockUpdate) => {
    if (!connection) {
        connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5001/stockshub", {
                withCredentials: true
            })
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveStockUpdate", onStockUpdate);

        connection.start()
            .then(() => console.log("SignalR connected."))
            .catch(console.error);
    } else if (connection.state === signalR.HubConnectionState.Disconnected) {
        connection.start()
            .then(() => console.log("SignalR reconnected."))
            .catch(console.error);
    }
}