import React, { useState } from 'react'
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';
import Paper from '@mui/material/Paper';
import Products from './Products';
import Navbar from '../Navbar/Navbar';
import Cart from './Cart';
import {toast} from "react-toastify";

export default function StorePanel() {
    const [cartArticles, setCartArticles] = useState([]);

    const addArticlesToCart = (article) => {
        if (cartArticles.find(a => a.id === article.id)) {
            const updatedCartArticles = cartArticles.map(a => {
              if (a.id === article.id && a.maxQuantity < a.quantity+article.quantity) {
                toast.error("Maximum quantity is ${a.maxQuantity}, you already added ${a.quantity}!",{
                    position: "top-center",
                    autoClose: 2500,
                    closeOnClick: true,
                    pauseOnHover: false,
                });
              }
              return {...a, quantity: a.quantity + article.quantity };
            });
        setCartArticles(updatedCartArticles);
        }else{
            setCartArticles([...cartArticles, article]);
        }
    };

    if(cartArticles === []){
        return <div>Loading...</div>
    }
    else
    return (
        <React.Fragment>
            <Navbar></Navbar>
            <Container maxWidth="xl" sx={{ mt: 4, mb: 4 }}>
            <Grid container spacing={3}>
                <Grid item xs={12} md={8} lg={9}>
                <Paper
                    sx={{
                    p: 2,
                    display: 'flex',
                    flexDirection: 'column',
                    }}
                >
                    <Products cartArticles={cartArticles} addArticlesToCart={addArticlesToCart} />
                </Paper>
                </Grid>
                <Cart cartArticles={cartArticles}/>
            </Grid>
            </Container>
        </React.Fragment>
    )
}