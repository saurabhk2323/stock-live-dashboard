import React from "react";
import "./StockCard.css";

const StockCard = ({ stock, onSelect }) => {
  return (
    <div className="stock-card" onClick={() => onSelect(stock)}>
      <div className="stock-title">
        {stock.name} ({stock.symbol})
      </div>
      <div className="stock-price">₹ {stock.price.toFixed(2)}</div>
    </div>
  );
};

export default StockCard;
