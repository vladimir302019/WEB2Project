import { configureStore, getDefaultMiddleware } from "@reduxjs/toolkit";
import userSlice from "./userSlice";
import adminSlice from "./adminSlice";

const store = configureStore({
    reducer:{user: userSlice, admin: adminSlice,},
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({ serializableCheck: false,}),
});

export default store;