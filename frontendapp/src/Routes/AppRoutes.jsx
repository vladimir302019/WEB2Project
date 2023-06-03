import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { useSelector } from "react-redux";
import Register from "../Components/Register/Register"
import SignIn from "../Components/SignIn/SignIn"
import ProfilePage from "../Components/Profile/Profile";
import Menu from "../Components/Menu/Menu";

const AppRoutes = () => {
    const isLoggedIn = useSelector((state) => state.user.isLoggedIn);
    const user = useSelector((state) => state.user.user);
    const isApprovedSeller = user && user.type === "Seller" && user.Approved === 1;
    const isBuyer = user && user.type === "Buyer";
    const isAdmin = user && user.type === "Admin";

    return (
        <Routes>
            {!isLoggedIn && (
            <>
            <Route path="/register" element={<Register/>}></Route>
            <Route path="/signin" element={<SignIn/>}></Route>
            <Route path="*" element={<Navigate replace to={"/register"} />}></Route>
            </>
            )}
            {isLoggedIn && (
            <>
            <Route path="/profile" element={<ProfilePage/>}></Route>
            <Route path="/menu" element={<Menu/>}></Route>
            <Route path="" element={<Navigate replace to={"/menu"} />}></Route>
            </>
            )}
        </Routes>
    );
}

export default AppRoutes;