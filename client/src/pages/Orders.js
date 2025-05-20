import React, { useEffect, useState } from "react";
import "./Orders.css";
import { getAllOrders } from "../services/apiService";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";

// Sample mock orders for demo
const mockOrders = [
  { id: 1, stock: "RELIANCE", symbol: "RELI", type: "Buy", quantity: 10, price: 2720, time: "14:10" },
  { id: 2, stock: "TCS", symbol: "TCS", type: "Sell", quantity: 5, price: 3650, time: "13:45" },
  { id: 3, stock: "INFY", symbol: "INFY", type: "Buy", quantity: 20, price: 1450, time: "12:30" },
];

const Orders = () => {
  const navigate = useNavigate()
  const { investorID, investorUniqueName, investorName, investorKind, countryCode, isLoggedIn } = useSelector(state => state.account)
  const [orders, setOrders] = useState([]);

  useEffect(() => {
    if (!isLoggedIn) {
      return navigate("/account")
    }
    // In real app, fetch today's orders from backend
    const loadData = async () => {
      const placedOrders = await getAllOrders(investorID)
      if (Array.isArray(placedOrders)) {
        // Sorting by descending time
        const sorted = [...placedOrders].sort((a, b) => (b.trade_timestamp > a.trade_timestamp ? 1 : -1))
        setOrders(sorted)
      } else {
        console.warn("Expected an array, but got:", placedOrders);
      }
    }
    loadData()
  }, []);

  return (
    <div className="orders-page">
      <h2>Today's Orders</h2>
      {orders.length === 0 ? (
        <p>No orders placed today.</p>
      ) : (
        <ul className="orders-list">
          {orders.map(order => (
            <li key={order.id} className={`order-card ${order.buyOrSell.toLowerCase()}`}>
              <div className="top-row">
                <span className="stock">{order.stockName} ({order.stockSymbol})</span>
                <span className="type">{order.buyOrSell.toUpperCase()}</span>
              </div>
              <div className="details">
                <div className="info-block">
                  <div><strong>Qty:</strong> {order.totalOrderedQuantity}</div>
                  <div><strong>Price:</strong> ₹{order.price}</div>
                </div>
                <div className="status-block">
                  <span className={`status-label ${order.status.toLowerCase() === "success" ? "completed" : "pending"}`}>
                    {order.status.toLowerCase() === 'success' ? 'Completed' : 'Pending'}
                  </span>
                  <div className="timestamp">
                    {new Date(order.tradeTimestamp).toLocaleString()}
                  </div>
                </div>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default Orders;
