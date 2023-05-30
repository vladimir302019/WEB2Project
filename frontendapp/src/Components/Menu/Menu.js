import React, { useEffect } from 'react';
import { Grid, Paper } from '@mui/material';
import Box from '@mui/material/Box';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import LocalMallIcon from '@mui/icons-material/LocalMall';
import ViewListIcon from '@mui/icons-material/ViewList';
import LogoutIcon from '@mui/icons-material/Logout';
import Stack from '@mui/material/Stack';
import { useNavigate, Link } from 'react-router-dom';

const Menu = () => {
    const navigate = useNavigate();

    const profileClickHandler = (event) => {
        event.preventDefault();
        
        console.log('profil');
    }
    const storeClickHandler = (event) => {
        event.preventDefault();

        console.log('store');
    }
    const ordersClickHandler = (event) => {
        event.preventDefault();

        console.log('orders');
    }
    const logoutClickHandler = (event) => {
        event.preventDefault();

        console.log('logout');
    }
    useEffect(()=>{
        //navigate("/profile");
       }, [navigate]);
  return (
    <Box
    sx={{
        marginTop: 15,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
      }}>
    <Stack
        direction={{ xs: 'row', sm: 'row' }}
        spacing={{ xs: 1, sm: 2, md: 4 }}
        >
      <Grid item xs='auto'>
        <Paper style={{ width: '200px', height: '300px' }} onClick={profileClickHandler}>
            <Link to="/profile">{"Profile"}</Link>
          <AccountBoxIcon fontSize='large' style={{ width: '200px', height: '300px' }}/>
        </Paper>
      </Grid>
      <Grid item xs='auto'>
        <Paper style={{ width: '200px', height: '300px' }} onClick={storeClickHandler}>
          <LocalMallIcon fontSize='large' style={{ width: '200px', height: '300px' }}/>
        </Paper>
      </Grid>
      <Grid item xs='auto'>
        <Paper style={{ width: '200px', height: '300px' }} onClick={ordersClickHandler}>
          <ViewListIcon fontSize='large' style={{ width: '200px', height: '300px' }}/>
        </Paper>
      </Grid>
      <Grid item xs='auto'>
        <Paper style={{ width: '200px', height: '300px' }} onClick={logoutClickHandler}>
          <LogoutIcon fontSize='large' style={{ width: '200px', height: '300px' }}/>
        </Paper>
      </Grid>
    </Stack>
    </Box>
  );
};

export default Menu;