import {createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import {Login, Register} from "../Services/UserService";
import {toast} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

const initialState = {
    token: localStorage.getItem("token"),
    isLoggedIn: localStorage.getItem("token") != null,
    user:  localStorage.getItem("user") !== null ? JSON.parse( localStorage.getItem("user")) : null,
};

export const loginAction = createAsyncThunk(
    "user/login",
    async (data, thunkApi) =>{
        try{
            const response = await Login(data);
            return thunkApi.fulfillWithValue(response.data);
        } catch (error) {
            return thunkApi.rejectWithValue(error.response.data.error);
        }
    }
)

export const registerAction = createAsyncThunk(
    "user/register",
    async(data, thunkApi) => {
        try{
            console.log(data);
            const response = await Register(data);
            console.log(response);
            return thunkApi.fulfillWithValue(response.data);
        } catch (error) {
            return thunkApi.rejectWithValue(error.response.data.error);
        }
    }
)

const userSlice = createSlice({
    name: "user",
    initialState,
    reducers:{
        logout(state) {
            state.token = null;
            state.user = null;
            state.isLoggedIn = false;

            localStorage.removeItem("token");
        },
    },
    extraReducers: (builder) => {
        builder.addCase(loginAction.fulfilled, (state, action) => {
            const token = action.payload.token;
            state.token = token;
            state.isLoggedIn = true;
            console.log(token.toString());
            localStorage.setItem("token", token);
        });
        builder.addCase(loginAction.rejected, (state,action) =>{
            let error = "Login error!";
            if(typeof action.payload === "string"){
                error = action.payload;
            }

            toast.error(error, {
                position: "top-center",
                autoClose: 2500,
                closeOnClick: true,
                pauseOnHover: true,
            });
        });
        builder.addCase(registerAction.fulfilled, (state, action) =>{
            toast.success("You have registered successfully!",{
            position: "top-center",
            autoClose: 2500,
            closeOnClick: true,
            pauseOnHover: true,
            });
        });
        builder.addCase(registerAction.rejected, (state, action) => {
            let error = "Registration error!";
            if(typeof action.payload === "string"){
                error = action.payload;
            }

            toast.error(error, {
            position: "top-center",
            autoClose: 2500,
            closeOnClick: true,
            pauseOnHover: true,
            });
        });
    }
});

export const {logout} = userSlice.actions;

export default userSlice.reducer;