import React, {useState} from 'react';
import PropTypes from 'prop-types';
import Box from '@mui/material/Box';
import Collapse from '@mui/material/Collapse';
import IconButton from '@mui/material/IconButton';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Typography from '@mui/material/Typography';
import Paper from '@mui/material/Paper';
import KeyboardArrowDownIcon from '@mui/icons-material/KeyboardArrowDown';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';
import { Button, Grid } from '@mui/material';
import TablePagination from '@mui/material/TablePagination';
import { cancelOrderAction } from '../../Store/orderSlice';
import { useDispatch } from 'react-redux';
import DeliveryCountdown from './DeliveryCountdown'

function Row(props) {
  const dispatch = useDispatch();
  const { row } = props;
  const [open, setOpen] = React.useState(false);
  const [remainingHours, setRemainingHours] = useState(0); // State variable to store the remaining hours

  const handleCountdownComplete = () => {
    // Countdown completed, update the remaining hours in the parent component
    setRemainingHours(0);
  };
  
  const handleRemainingHoursChange = (hours) => {
    // Update the remaining hours in the parent component
    setRemainingHours(hours);
  };
const handleCancelClick = (id) => {
  const data = new FormData();
  data.append("orderId", id);
  dispatch(cancelOrderAction(data)).then(() => {
    props.handleUpdateOrders(); // Call the callback function
  });
};
  return (
    <React.Fragment>
      {row.confirmed && (<>
      <TableRow sx={{ '& > *': { borderBottom: 'unset' } }}>
        <TableCell>
          <IconButton aria-label="expand row" size="small" onClick={() => setOpen(!open)}>
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
          </IconButton>
        </TableCell>
        <TableCell component="th" scope="row">
          {row.id}
        </TableCell>
        <TableCell>{row.comment}</TableCell>
        <TableCell>{row.address}</TableCell>
        <TableCell align="right">{row.totalPrice}</TableCell>
        <TableCell>{row.confirmed ? 'Confirmed' : 'Not Confirmed'}</TableCell>
        <TableCell>{new Date(row.deliveryDate).toLocaleString()}</TableCell>
      {props.undelivered &&  <TableCell>{<DeliveryCountdown deliveryTime ={new Date(row.deliveryDate)}  onCountdownComplete={handleCountdownComplete} onRemainingHoursChange={handleRemainingHoursChange}/>}</TableCell>}
        {props.isBuyer && (<TableCell><Button disabled={remainingHours < 1}
        color='error' variant='contained' onClick={() => handleCancelClick(row.id, props.handleUpdateOrders)}>Cancel order</Button></TableCell>)}
      </TableRow>
      <TableRow>
        <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={9}>
          <Collapse in={open} timeout="auto" unmountOnExit>
            <Box sx={{ margin: 1 }}>
              <Typography variant="h6" gutterBottom component="div">
                Order Items
              </Typography>
              <Table size="small" aria-label="order-items">
                <TableHead>
                  <TableRow>
                    <TableCell>Item Name</TableCell>
                    <TableCell align="center">Item Price</TableCell>
                    <TableCell align="right">Quantity</TableCell>
                    <TableCell align="right">Total Price</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {row.orderItems.length > 0 &&
                  row.orderItems.map((item) => (
                    <TableRow key={item.id}>
                      <TableCell component="th" scope="row">
                        {item.name}
                      </TableCell>
                      <TableCell align="center" scope="row">
                        {item.price}
                      </TableCell>
                      <TableCell align="right">{item.quantity}</TableCell>
                      <TableCell align="right">{item.quantity*item.price}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </Box>
          </Collapse>
        </TableCell>
      </TableRow> </>)}
    </React.Fragment>
  );
}

Row.propTypes = {
  row: PropTypes.shape({
    id: PropTypes.number.isRequired,
    comment: PropTypes.string.isRequired,
    address: PropTypes.string.isRequired,
    totalPrice: PropTypes.number.isRequired,
    confirmed: PropTypes.bool.isRequired,
    deliveryDate: PropTypes.string.isRequired,
    orderItems: PropTypes.arrayOf(
      PropTypes.shape({
        id: PropTypes.number.isRequired,
        name: PropTypes.string.isRequired,
        quantity: PropTypes.number.isRequired,
      })
    ).isRequired,
  }).isRequired,
  handleUpdateOrders: PropTypes.func.isRequired,
};

export default function Orders({orders, header, isBuyer, handleUpdateOrders, undelivered}) {
  const[page, setPage] = useState(0);
  const[rowsPerPage, setRowsPerPage] = useState(5);
  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };
  
  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const emptyRows = rowsPerPage - Math.min(rowsPerPage, orders.length - page * rowsPerPage);

  return (
    <React.Fragment>  
    <Grid container justifyContent={'center'} sx={{ maxWidth: '80%', margin: '0 auto', mt:10 }}>
      <TableContainer component={Paper}>
        <Table aria-label="collapsible table" size="large">
          <TableHead>
            <TableRow>
                <TableCell colSpan={9}>
                    <Typography variant='h4' align="center">{header ? header : "Orders"}</Typography>
                </TableCell>
            </TableRow>
            <TableRow>
              <TableCell />
              <TableCell>Id</TableCell>
              <TableCell>Comment</TableCell>
              <TableCell>Address</TableCell>
              <TableCell align="right">Total Price</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Delivery Date</TableCell>
              {undelivered && <TableCell>Time to delivery</TableCell>}
              {(undelivered && isBuyer)&& <TableCell>Cancel</TableCell>}
            </TableRow>
          </TableHead>
          <TableBody>
          {orders.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={9}>
                    <Typography variant="body1" align="center">No orders found.</Typography>
                  </TableCell>
                </TableRow>
              ) : (
            (rowsPerPage > 0
              ? orders.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
              : orders
            ).map((row) => (
              <Row key={row.id} undelivered={undelivered} row={row} isBuyer={isBuyer} handleUpdateOrders = {handleUpdateOrders}/>
            )))}

            {emptyRows > 0 && (
              <TableRow style={{ height: 53 * emptyRows }}>
                <TableCell colSpan={9} />
              </TableRow>
            )}
          </TableBody>
        </Table>
        <TablePagination
          rowsPerPageOptions={[5, 10, 25]}
          component="div"
          count={orders.length}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </TableContainer>
    </Grid>
    </React.Fragment>
  );
}
