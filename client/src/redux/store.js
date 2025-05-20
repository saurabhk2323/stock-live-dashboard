import { configureStore } from "@reduxjs/toolkit"
import notificationReducer from "./notificationSlice"
import accountReducer from "./accountSlice"

const store = configureStore({
    reducer: {
        notification: notificationReducer,
        account: accountReducer
    }
})

export default store