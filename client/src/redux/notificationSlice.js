import { createSlice } from "@reduxjs/toolkit";

const initialState = {
    message: "Welcome to the StockLiveDashboard Application",
    type: "info"
}

const notificationSlice = createSlice({
    name: "notification",
    initialState,
    reducers: {
        showNotification: (state, action) => {
            state.message = action.payload.message
            state.type = action.payload.type || "info"
        },
        clearNotification: (state) => {
            state.message = null
        }
    }
})

export const {showNotification, clearNotification} = notificationSlice.actions

export default notificationSlice.reducer