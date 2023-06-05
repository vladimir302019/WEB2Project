import React, { useEffect, useState } from 'react';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';

const Cart = ({cartArticles}) => {
  const [cartItems, setCartItems] = useState([]);
  useEffect(()=> {
    setCartItems([...cartArticles]);

  }, [cartArticles]);

  const calculateTotalPrice = () => {
    return cartItems.reduce((total, item) => total + item.price * item.quantity, 0);
  };

  return (
    <Grid item xs={12} md={4} lg={3}>
      <Paper sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
        <Typography variant="h6" sx={{ mb: 2 }}>
          Cart
        </Typography>
        {cartItems.map((article) => (
          <Grid item key={article.id} xs={12} sx={{ mb: 2 }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 1 }}>
              <div style={{ display: 'flex', flexDirection: 'row', textAlign: 'left', width: '30%' }}>
                <Typography>{article.name}</Typography>
              </div>
              <div style={{ display: 'flex', flexDirection: 'row', textAlign: 'center', width: '30%' }}>
                <Typography variant="body2">Price: {article.price}</Typography>
              </div>
              <div style={{ textAlign: 'right', width: '30%' }}>
                <Typography variant="body2"
                  sx={{ textAlign: 'right' }}>Quantity: {article.quantity}</Typography>
              </div>
              <div>
                <Button color='error'>Remove</Button>
              </div>
            </div>
          </Grid>
        ))}
        <Grid container>
          <Grid item xs={8}>
            <Typography variant="h5" textAlign="left">
              Total price:
            </Typography>
          </Grid>
          <Grid item xs={4} textAlign="right">
            <Typography variant="h5">{calculateTotalPrice()}</Typography>
          </Grid>
          <Grid item xs={12}>
            <Typography variant="body2" color="textSecondary" textAlign="left">
              Shipping is not included in price
            </Typography>
          </Grid>
        </Grid>

        <Button variant="contained" color="primary" sx={{ mt: 2 }}>
          Checkout
        </Button>
      </Paper>
    </Grid>
  );
};

export default Cart;