import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import Register from './Components/Register/Register';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import SignIn from './Components/SignIn/SignIn';
import { Provider } from 'react-redux';
import store from "./Store/store";
import { ToastContainer } from 'react-toastify';
import ProfilePage from './Components/Profile/Profile';
import Menu from './Components/Menu/Menu';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <Router>
      <Provider store={store}>
      <Routes>
        <Route path="/register" element={<Register/>}></Route>
        <Route path="/signin" element={<SignIn/>}></Route>
        <Route path="/profile" element={<ProfilePage/>}></Route>
        <Route path="/menu" element={<Menu/>}></Route>
        <Route path="*" element={<Navigate replace to={"/register"} />}></Route>
      </Routes>
      </Provider>
      <ToastContainer/>
    </Router>
);

