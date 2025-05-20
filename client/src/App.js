import { HubConnectionBuilder } from "@microsoft/signalr";
import signalRManager from "./services/signalRManager";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import StockLiveDashboard from "./pages/StockLiveDashboard";
import Orders from "./pages/Orders";
import Account from "./pages/Account";
import Header from "./components/Header";
import NotificationBar from "./components/Notification/NotificationBar";
import { useEffect } from "react";

function App() {
  useEffect(() => {
    const initSignalR = async () => {
      await signalRManager.startConnection("http://localhost:5001/stockshub");
    };
    initSignalR();
  }, []);
  return (
    <Router>
      <div className="App">
        <Header />
        <NotificationBar />
        <Routes>
          <Route path="" element={<StockLiveDashboard />} />
          <Route path="/orders" element={<Orders />} />
          <Route path="/account" element={<Account />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;