import React from 'react';
import { AppBar, Toolbar, Typography, IconButton, Button } from '@mui/material';
import { ShoppingBasket, Assignment, AccountCircle, ExitToApp } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { logout } from '../../Store/userSlice';

const Navbar = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    const logoutClickHandler = (event) => {
        event.preventDefault();
        navigate('/signin');
        dispatch(logout());
        //console.log('logout');
    }
  return (
    <AppBar position="static">
      <Toolbar>
        <IconButton color="inherit">
          <ShoppingBasket fontSize="large"/>
        </IconButton>
        <IconButton color="inherit">
          <Assignment fontSize="large"/>
        </IconButton>
        <IconButton color="inherit">
          <AccountCircle fontSize="large"/>
        </IconButton>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
        </Typography>
        <Button color="inherit" endIcon={<ExitToApp fontSize="large"/>} sx={{fontSize: '1.2rem', padding: '12px'}} onClick={logoutClickHandler}>
          Logout
        </Button>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;
