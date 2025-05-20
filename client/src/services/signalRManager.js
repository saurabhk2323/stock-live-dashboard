import * as signalR from '@microsoft/signalr'

let connection = null

// keeps track of all events and callbacks
const handlers = new Map()

const startConnection = async (url) => {
    if (!connection) {
        connection = new signalR.HubConnectionBuilder()
            .withUrl(url, { withCredentials: true })
            .withAutomaticReconnect()
            .build();

        // Re-subscribe all events after reconnect
        connection.onreconnected(() => {
            console.log("Reconnected, re-binding events...");
            handlers.forEach((callback, eventName) => {
                connection.on(eventName, callback);
            });
        });

        connection.onclose((error) => {
            console.error("SignalR connection closed:", error);
        });

        try {
            await connection.start();
            console.log("SignalR connected.");
        } catch (err) {
            console.error("Connection failed:", err);
        }
    } else if (connection.state === signalR.HubConnectionState.Disconnected) {
        await connection.start()
        console.log("Reconnected existing SignalR connection.");
    }
}

const addEventListener = (eventName, callback) => {
    if (!connection) {
        console.warn("Connection not established yet.");
        startConnection("http://localhost:5001/stockshub");
        return;
    }

    connection.on(eventName, callback);
    handlers.set(eventName, callback); // Save for future reconnects
};

const removeEventListener = (eventName) => {
    if (connection) {
        connection.off(eventName);
        handlers.delete(eventName);
    }
};

const invoke = async (methodName, ...args) => {
    if (!connection || connection.state !== "Connected") {
        console.warn("Cannot invoke method; connection not ready.");
        await startConnection("http://localhost:5001/stockshub");
        return;
    }

    try {
        return await connection.invoke(methodName, ...args);
    } catch (err) {
        console.error(`Error invoking ${methodName}:`, err);
    }
};

const stopConnection = async () => {
    if (connection) {
        await connection.stop();
        connection = null;
        handlers.clear();
        console.log("SignalR connection stopped.");
    }
};

export default {
    startConnection,
    addEventListener,
    removeEventListener,
    invoke,
    stopConnection
}