import React, { useEffect, useState } from "react";
import BuySellModal from "./BuySellModal";
import { useSelector } from "react-redux";
import { getStockDetailsByInvestor } from "../services/apiService"
import "./StockDetail.css";

const StockDetail = ({ stock, onClose }) => {
  const { investorID, investorUniqueName, investorName, investorKind, countryCode, isLoggedIn } = useSelector(state => state.account)
  const [action, setAction] = useState(null)
  const [buyersCount, setBuyersCount] = useState(null)
  const [sellersCount, setSellersCount] = useState(null)
  const [holdings, setHoldings] = useState(null)

  useEffect(() => {
    const loadStockData = async () => {
      const stockDetail = await getStockDetailsByInvestor(investorID, stock.id)
      setBuyersCount(stockDetail.buyers)
      setSellersCount(stockDetail.sellers)
      setHoldings(stockDetail.holding)
    }
    if (isLoggedIn)
      loadStockData()
  }, [])

  return (
    <div className="overlay-container">
      <div className="overlay-box">
        <button className="close-button" onClick={onClose}>×</button>
        <h2>{stock.name} ({stock.symbol})</h2>
        <p className="description">{stock.description}</p>
        <p className="live-price">Live Price: ₹ {stock.price.toFixed(2)}</p>

        {isLoggedIn && (
          <div className="stock-info-grid">
            <div className="volume-card">
              <h4>Buyers / Sellers</h4>
              <div className="volume-bars">
                <div className="bar buyers">
                  <span>Buyers</span>
                  <span>{buyersCount}</span>
                </div>
                <div className="bar sellers">
                  <span>Sellers</span>
                  <span>{sellersCount}</span>
                </div>
              </div>
            </div>

            <div className="holdings-card">
              <h4>Your Holdings</h4>
              <p>You own <strong>{holdings}</strong> shares.</p>
            </div>
          </div>
        )}

        <div className="action-buttons">
          <button className="buy-button" onClick={() => setAction("buy")}>Buy</button>
          <button className="sell-button" onClick={() => setAction("sell")}>Sell</button>
        </div>

        {action && (
          <BuySellModal
            action={action}
            stock={stock}
            onClose={() => setAction(null)}
          />
        )}
      </div>
    </div>
  );
};

export default StockDetail;
