import React, { useState, useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { login, logout } from "../redux/accountSlice"
import { saveInvestor, getInvestor } from "../services/apiService"
import "./Account.css"

const Account = () => {
  const [name, setName] = useState("")
  const [username, setUsername] = useState("")
  const [typeOfInvester, setTypeOfInvester] = useState("Retailer")
  const [userCountryCode, setUserCountryCode] = useState("IN")
  const [showSignIn, setShowSignIn] = useState(true)

  const dispatch = useDispatch()
  const { investorID, investorUniqueName, investorName, investorKind, countryCode, isLoggedIn } = useSelector(state => state.account)

  const handleSignIn = async () => {
    const response = await getInvestor(username, userCountryCode)
    if (response) {
      dispatch(
        login({
          investorID: response.id,
          investorName: response.name,
          investorUniqueNam: response.investorUniqueName,
          investorKind: response.investorKind,
          countryCode: response.countryCode,
        })
      )
    }
  }

  const handleSignUp = async () => {
    const response = await saveInvestor(name, username, typeOfInvester, userCountryCode)
    if (response)
      dispatch(
        login({
          investorID: response.ID,
          investorName: name,
          investorUniqueNam: username,
          investorKind: typeOfInvester,
          countryCode: userCountryCode,
        })
      )
  }

  const handleSignOut = () => {
    dispatch(logout())
  }

  return (
    <div className="account-container">
      <h2>Account</h2>

      {isLoggedIn ? (
        <div className="user-info">
          <p><strong>Name:</strong> {investorName}</p>
          <p><strong>Username:</strong> {investorUniqueName}</p>
          <p><strong>Investor Kind:</strong> {investorKind}</p>
          <p><strong>Country Code:</strong> {countryCode}</p>
          <button className="signout-button" onClick={handleSignOut}>Sign Out</button>
        </div>
      ) :
        showSignIn ? (
          <div className="signin-form">
            <input
              type="text"
              placeholder="Enter a unique username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
            <select
              value={userCountryCode}
              onChange={(e) => setUserCountryCode(e.target.value)}
            >
              <option value="IN">IN</option>
              <option value="CN">CN</option>
              <option value="JP">JP</option>
              <option value="UK">UK</option>
              <option value="US">US</option>
            </select>
            <button onClick={handleSignIn}>Sign In</button>
            <button onClick={() => setShowSignIn(false)}>Click for Sign-Up Page</button>
          </div>
        ) : (
          <div className="signin-form">
            <input
              type="text"
              placeholder="Enter your name"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
            <input
              type="text"
              placeholder="Enter a unique username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
            <select
              value={typeOfInvester}
              onChange={(e) => setTypeOfInvester(e.target.value)}
            >
              <option value="FII">FII</option>
              <option value="DII">DII</option>
              <option value="Retailer">Retailer</option>
              <option value="Others">Others</option>
            </select>
            <select
              value={userCountryCode}
              onChange={(e) => setUserCountryCode(e.target.value)}
            >
              <option value="IN">IN</option>
              <option value="CN">CN</option>
              <option value="JP">JP</option>
              <option value="UK">UK</option>
              <option value="US">US</option>
            </select>
            <button onClick={handleSignUp}>Sign Up</button>
            <button onClick={() => setShowSignIn(true)}>Click for Sign-In Page</button>
          </div>
        )}
    </div>
  );
};

export default Account;
