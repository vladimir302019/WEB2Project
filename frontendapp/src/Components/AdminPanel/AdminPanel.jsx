import React, { useEffect, useState } from 'react';
import { Grid, Paper, Button } from '@mui/material';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Title from './Title/Title';
import { useNavigate} from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import Navbar from '../Navbar/Navbar';
import { getInactiveSellersAction, getActiveSellersAction, approveSellerAction } from '../../Store/adminSlice';

export default function AdminPanel() {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const inactiveSellers = useSelector((state) => state.admin.inactiveSellers);
    const allSellers = useSelector((state) => state.admin.allSellers);
    const [isInitial, setIsInitial] = useState(true);

    const handleApprove = (id) => {
        const requestBody = {
            id: id,
            isActive: true,
        } 
        verify(requestBody);
        console.log(`Approve user with id ${id}`);
    };

    
    const handleDeny = (id) => {
        const requestBody = {
            id: id,
            isActive: false,
        } 
        verify(requestBody);
        console.log(`Deny user with id ${id}`);
    };

    const verify = async (requestBody) => {
        await dispatch(approveSellerAction(requestBody));
        setIsInitial(true);
    }

  useEffect(()=> {
    if(!isInitial) {
      return;
    }

    const execute = async () => {
      await dispatch(getInactiveSellersAction());
      await dispatch(getActiveSellersAction());
    };

    execute();
    setIsInitial(false);

  }, [isInitial, dispatch]);

  if(!inactiveSellers){
    return <div>Loading...</div>
  }

  return (
    <React.Fragment>
        <Navbar/>
        <Grid container justifyContent="center" mt={4}>
        <Grid item xs={12} sm={8} md={6} lg={6}>
        <Paper sx={{p: 2, display: 'flex', flexDirection: 'column' }}>
      <Title>Pending verification</Title>
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Full name</TableCell>
            <TableCell>Username</TableCell>
            <TableCell>Email</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {inactiveSellers.map((seller) => (
            <TableRow key={seller.id} >
              <TableCell>{seller.fullName}</TableCell>
              <TableCell>{seller.username}</TableCell>
              <TableCell>{seller.email}</TableCell>
              <TableCell align="right">
                <Button
                    variant="contained"
                    color="primary"
                    className="approveButton"
                    onClick={() => handleApprove(seller.id)}
                    >
                    Approve
                </Button>
                <Button
                    sx={{ml:2}}
                    variant="contained"
                    color="secondary"
                    onClick={() => handleDeny(seller.id)}
                    >
                    Deny
                    </Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
      </Paper>
      </Grid>
      </Grid>
      <Grid container justifyContent="center" sx={{ mt: 4 }}>
        <Grid item xs={12} sm={8} md={6} lg={6}>
          <Paper sx={{ p: 2, display: 'flex', flexDirection: 'column' }}>
            <Title>Verified/denied sellers</Title>
            <Table size="small">
              <TableHead>
                <TableRow>
                    <TableCell>Full name</TableCell>
                    <TableCell>Username</TableCell>
                    <TableCell>Email</TableCell>
                    <TableCell>Approve status</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {allSellers.map((seller) => (
                  <TableRow key={seller.id}>
                    <TableCell>{seller.fullName}</TableCell>
                    <TableCell>{seller.username}</TableCell>
                    <TableCell>{seller.email}</TableCell>
                    <TableCell>{seller.approved ? 'APPROVED' : (seller.denied ? 'DENIED' : 'PENDING') }</TableCell>  
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </Paper>
        </Grid>
      </Grid>
    </React.Fragment>
  );
}