import React, { useState } from "react"
import "./BuySellModal.css"
import signalRManager from "../services/signalRManager"
import { useSelector } from "react-redux"
import { useNavigate } from 'react-router-dom'

const BuySellModal = ({ action, stock, onClose }) => {
  const navigate = useNavigate()
  const [quantity, setQuantity] = useState(1);
  const [limitPrice, setLimitPrice] = useState(stock.price);

  const { investorID, investorUniqueName, investorName, investorKind, countryCode, isLoggedIn } = useSelector(state => state.account)

  const handleConfirm = () => {
    if(!isLoggedIn){
      return navigate("/account")
    }
    // Parse values as numbers to ensure correct types
    const parsedQuantity = Number(quantity);
    const parsedLimitPrice = Number(limitPrice);
    console.log(`${action.toUpperCase()} ${parsedQuantity} ${stock.symbol} @ ₹${parsedLimitPrice}`);
    console.log(`InvestorID: ${investorID}, InvestorUniqueName: ${investorUniqueName}, Action: ${action}, stockID: ${stock.id}, Price: ${parsedLimitPrice}, Quantity: ${parsedQuantity}`);
    signalRManager.invoke("PlaceOrder", investorID, investorUniqueName, action, stock.id, stock.name, stock.symbol, parsedLimitPrice, parsedQuantity)
    onClose();
    return navigate("/")
  };

  return (
    <div className="modal-overlay">
      <div className="modal-box">
        <button className="modal-close" onClick={onClose}>×</button>
        <h3>{action === "buy" ? "Buy" : "Sell"} {stock.symbol}</h3>
        <label>Quantity:</label>
        <input
          type="number"
          value={quantity}
          onChange={e => setQuantity(e.target.value)}
          />

        <label>Limit Price:</label>
        <input
          type="number"
          value={limitPrice}
          onChange={e => setLimitPrice(e.target.value)}
          min={stock.price * 0.95}
          max={stock.price * 1.05}
        />

        <button className="confirm-button" onClick={handleConfirm} >
          Confirm {action}
        </button>
      </div>
    </div>
  );
};

export default BuySellModal;
