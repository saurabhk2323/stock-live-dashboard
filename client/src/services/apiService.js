import axios from "axios";

export const getAllStocks = async () => {
    try {
        const response = await axios.get('http://localhost:5000/api/Stocks/get-all')
        return response.data
    } catch (err) {
        console.log("getAllStocks", err)
        return []
    }
}

export const saveInvestor = async (name, username, typeOfInvester, countryCode) => {
    try {
        const response = await axios.post('http://localhost:5000/api/Investors/add', {
            name: name,
            investorUniqueName: username,
            investorKind: typeOfInvester,
            countryCode: countryCode
        })
        return response.data
    } catch (err) {
        console.log("saveInvestor", err)
    }
}

export const getInvestor = async (username, countryCode) => {
    try {
        const response = await axios.get(`http://localhost:5000/api/Investors/get-by-username?username=${username}&countryCode=${countryCode}`)
        return response.data
    } catch (err) {
        console.log("getInvestor", err)
    }
}


export const getAllOrders = async (investorID) => {
    try {
        const response = await axios.get(`http://localhost:5000/api/Trades/get-all?investorID=${investorID}`)
        return response.data
    } catch (err) {
        console.log("getAllOrders", err)
    }
}

export const getStockDetailsByInvestor = async (investorID, stockID) => {
    try {
        const response = await axios.get(`http://localhost:5000/api/Stocks/get-stock-details-by-investor?investorID=${investorID}&stockID=${stockID}`)
        return response.data
    } catch (err) {
        console.log("getStockDetailsByInvestor", err)
    }
}