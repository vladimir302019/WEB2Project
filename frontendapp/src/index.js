import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import { BrowserRouter as Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import store from "./Store/store";
import { ToastContainer } from 'react-toastify';
import App from './App';
import { GoogleOAuthProvider } from '@react-oauth/google';
import { Beforeunload } from 'react-beforeunload';

const removeApplicationData = () =>{
  if(typeof window !== 'undefined'){
    localStorage.clear();
  }
};

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <Beforeunload onBeforeunload={removeApplicationData}>
    <Router>
      <GoogleOAuthProvider clientId={process.env.REACT_APP_GOOGLE_CLIENT_ID || ""}>
      <Provider store={store}>
        <App/>
      </Provider>
      </GoogleOAuthProvider>
      <ToastContainer/>
    </Router>
  </Beforeunload>
);

