import React, { useEffect, useState } from 'react';
import { Grid, Button } from '@mui/material';
import Box from '@mui/material/Box';
import AccountBoxIcon from '@mui/icons-material/AccountBox';
import LocalMallIcon from '@mui/icons-material/LocalMall';
import ViewListIcon from '@mui/icons-material/ViewList';
import AdminPanelSettingsIcon from '@mui/icons-material/AdminPanelSettings';
import LogoutIcon from '@mui/icons-material/Logout';
import Stack from '@mui/material/Stack';
import { useNavigate} from 'react-router-dom';
import { logout } from '../../Store/userSlice';
import { useDispatch, useSelector } from 'react-redux';
import { getImageAction, getUserInfoAction } from '../../Store/userSlice';

const Menu = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const user = useSelector((state) => state.user.user)
    const [isInitial, setIsInitial] = useState(true);

    const profileClickHandler = (event) => {
      event.preventDefault();
      navigate('/profile');
    }
    const storeClickHandler = (event) => {
      event.preventDefault();
      navigate('/store');
    }
    const ordersClickHandler = (event) => {
      event.preventDefault();

    }
    const adminPanelClickHandler = (event) => {
      event.preventDefault();
      navigate('/adminPanel');
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
    
    if(!user) {
      return <div>Loading...</div>;
    }

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
      {user.type !== 0 &&
      <Grid item xs='auto'>
        <Box sx={{ boxShadow: '0px 3px 5px rgba(0, 0, 0, 0.25)', display: 'inline-block' }}>
          <Button style={{ width: '200px', height: '300px' }} onClick={storeClickHandler}>
            <LocalMallIcon fontSize='large' style={{ width: '200px', height: '300px', color: "royalblue" }}/>
          </Button>
        </Box>
      </Grid>}
      <Grid item xs='auto'>
        <Box sx={{ boxShadow: '0px 3px 5px rgba(0, 0, 0, 0.25)', display: 'inline-block' }}>
          <Button style={{ width: '200px', height: '300px' }} onClick={ordersClickHandler}>
            <ViewListIcon fontSize='large' style={{ width: '200px', height: '300px', color: "royalblue"  }}/>
          </Button>
        </Box>
      </Grid>
      {user.type === 2 &&
      <Grid item xs='auto'>
        <Box sx={{ boxShadow: '0px 3px 5px rgba(0, 0, 0, 0.25)', display: 'inline-block' }}>
          <Button style={{ width: '200px', height: '300px' }} onClick={adminPanelClickHandler}>
            <AdminPanelSettingsIcon fontSize='large' style={{ width: '200px', height: '300px', color: "royalblue" }}/>
          </Button>
        </Box>
      </Grid>}
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