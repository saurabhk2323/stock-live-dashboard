import React, { useEffect, useState } from "react";
import { getAllStocks } from "../services/apiService"
import StockCard from "../components/Stock/StockCard";
import StockDetail from "../components/StockDetail";
import "./StockLiveDashboard.css";
import signalRManager from "../services/signalRManager";

const StockLiveDashboard = () => {
    const [stocks, setStocks] = useState([]);
    const [selectedStock, setSelectedStock] = useState(null);

    useEffect(() => {
        console.log("again")
        const initSignalR = async () => {
            await signalRManager.startConnection("http://localhost:5001/stockshub");
            signalRManager.addEventListener("ReceiveStockUpdate", onStockUpdate)
        };
        const loadData = async () => {
            console.log("load data")
            const stockData = await getAllStocks();
            onStockUpdate(stockData)
        }
        initSignalR()
        loadData()
    }, []);

    function onStockUpdate(stock) {
        console.log(stock)
        if (Array.isArray(stock)) {
            setStocks(stock)
        } else {
            setStocks(prev => {
                const updated = [...prev]
                const index = updated.findIndex(s => s.id === stock.stockID)
                if (index >= 0) {
                    // Replace the object immutably
                    updated[index] = { ...updated[index], price: stock.price };
                } else {
                    updated.push(stock);
                }
                return updated;
            });
        }
    }

    return (
        <div className="dashboard-container">
            <div className="stock-grid">
                {stocks.map(stock => (
                    <StockCard key={stock.symbol} stock={stock} onSelect={setSelectedStock} />
                ))}
            </div>

            {selectedStock && (
                <StockDetail stock={selectedStock} onClose={() => setSelectedStock(null)} />
            )}
        </div>
    );
};

export default StockLiveDashboard;
