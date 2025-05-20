import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    investorID: "",
    investorUniqueName: "",
    investorName: "",
    investorKind: "",
    countryCode: "",
    isLoggedIn: false
}

const accountSlice = createSlice({
    name: "account",
    initialState,
    reducers: {
        login: (state, action) => {
            state.isLoggedIn = true
            state.investorID = action.payload.investorID
            state.investorUniqueName = action.payload.investorUniqueNam
            state.investorName = action.payload.investorName
            state.investorKind = action.payload.investorKind
            state.countryCode = action.payload.countryCode
        },
        logout: (state, action) => {
            state.isLoggedIn = false
            state.investorID = ""
            state.investorUniqueName = ""
            state.investorName = ""
            state.investorKind = ""
            state.countryCode = ""
        }
    }
})

export const { login, logout } = accountSlice.actions

export default accountSlice.reducer 