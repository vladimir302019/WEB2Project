import React, { useEffect, useState } from 'react';
import { Grid, Button } from '@mui/material';
import Box from '@mui/material/Box';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import LocalMallIcon from '@mui/icons-material/LocalMall';
import ViewListIcon from '@mui/icons-material/ViewList';
import LogoutIcon from '@mui/icons-material/Logout';
import Stack from '@mui/material/Stack';
import { useNavigate} from 'react-router-dom';
import {logout} from '../../Store/userSlice';
import { useDispatch } from 'react-redux';
import { getImageAction, getUserInfoAction } from '../../Store/userSlice';

const Menu = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const [isInitial, setIsInitial] = useState(true);

    const profileClickHandler = (event) => {
        event.preventDefault();
        navigate('/profile');
    }
    const storeClickHandler = (event) => {
        event.preventDefault();

    }
    const ordersClickHandler = (event) => {
        event.preventDefault();

    }
    const logoutClickHandler = (event) => {
        event.preventDefault();
        navigate('/signin');
        dispatch(logout());
    }

    useEffect(()=> {
      if(!isInitial) {
        return;
      }
  
      const execute = async () => {
        await dispatch(getUserInfoAction());
        await dispatch(getImageAction());
      };
  
      execute();
      setIsInitial(false);
  
    }, [isInitial, dispatch]);

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
        <Box sx={{ boxShadow: '0px 3px 5px rgba(0, 0, 0, 0.25)', display: 'inline-block' }}>
          <Button style={{ width: '200px', height: '300px' }} onClick={profileClickHandler}>
            <AccountBoxIcon fontSize='large' style={{ width: '200px', height: '300px', color: "royalblue" }}/>
          </Button>
        </Box>
      </Grid>
      <Grid item xs='auto'>
        <Box sx={{ boxShadow: '0px 3px 5px rgba(0, 0, 0, 0.25)', display: 'inline-block' }}>
          <Button style={{ width: '200px', height: '300px' }} onClick={storeClickHandler}>
            <LocalMallIcon fontSize='large' style={{ width: '200px', height: '300px', color: "royalblue" }}/>
          </Button>
        </Box>
      </Grid>
      <Grid item xs='auto'>
        <Box sx={{ boxShadow: '0px 3px 5px rgba(0, 0, 0, 0.25)', display: 'inline-block' }}>
          <Button style={{ width: '200px', height: '300px' }} onClick={ordersClickHandler}>
            <ViewListIcon fontSize='large' style={{ width: '200px', height: '300px', color: "royalblue"  }}/>
          </Button>
        </Box>
      </Grid>
      <Grid item xs='auto'>
        <Box sx={{ boxShadow: '0px 3px 5px rgba(0, 0, 0, 0.25)', display: 'inline-block' }}>
          <Button style={{ width: '200px', height: '300px' }} onClick={logoutClickHandler}>
            <LogoutIcon fontSize='large' style={{ width: '200px', height: '300px', color: "darkred"  }}/>
          </Button>
        </Box>
      </Grid>
    </Stack>
    </Box>
  );
};

export default Menu;