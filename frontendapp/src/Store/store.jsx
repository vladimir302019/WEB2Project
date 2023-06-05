import { configureStore, getDefaultMiddleware } from "@reduxjs/toolkit";
import userSlice from "./userSlice";
import adminSlice from "./adminSlice";
import articleSlice from "./articleSlice";

const store = configureStore({
    reducer:{user: userSlice, admin: adminSlice, article: articleSlice,},
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({ serializableCheck: false,}),
});

export default store;