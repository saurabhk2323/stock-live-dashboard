import React, { useState, useEffect } from "react"
import { startSignalR } from "../services/signalrService"
import { getAllStocks } from "../services/apiService"

export default function StockList() {
    const [stocks, setStocks] = useState([])

    useEffect(() => {
        startSignalR(onStockUpdate)

        const loadData = async () => {
            const stockData = await getAllStocks();
            if (Array.isArray(stockData)) {
                onStockUpdate(stockData)
            } else {
                console.warn("Expected an array, but got:", stockData);
            }
        }
        loadData()
    }, [])

    function onStockUpdate(stock) {
        setStocks(prev => {
            const updated = [...prev];
    
            stock.forEach(element => {
                const index = updated.findIndex(s => s.symbol === element.symbol);
                if (index >= 0) {
                    // Replace the object immutably
                    updated[index] = { ...updated[index], price: element.price };
                } else {
                    updated.push(element);
                }
            });
    
            return updated;
        });
    }

    return (
        <div>
            <h2>Live Stock Prices</h2>
            <ul>
                {stocks.map(s => (
                    <li key={s.symbol}>{s.symbol} - {s.price}</li>
                ))}
            </ul>
        </div>
    )
}