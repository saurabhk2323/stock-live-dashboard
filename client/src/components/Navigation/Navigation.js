import React from "react";

import { Link } from "react-router-dom";

import './Navigation.css';

const Navigation = () => {
    return (
        <nav>
            <ul className="nav-left">
                <li>
                    <Link to="/">StockLiveDashboard</Link>
                </li>
                <li>
                    <Link to="/orders">Orders</Link>
                </li>
            </ul>
            <ul className="nav-right">
                <li>
                    <Link to="/account">Account</Link>
                </li>
            </ul>
        </nav>
    )
}

export default Navigation;