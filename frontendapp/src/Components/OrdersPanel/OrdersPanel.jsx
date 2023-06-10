import React, {useEffect, useState} from 'react'
import Orders from './Orders'
import Navbar from '../Navbar/Navbar'
import { useDispatch, useSelector } from 'react-redux'
import { getSellerNewOrdersAction, getUndeliveredOrdersAction, getUserOrdersAction, getOrdersAction, getSellerOldOrdersAction } from '../../Store/orderSlice';

export default function OrdersPanel() {
    const dispatch = useDispatch();
    const deliveredOrders = JSON.parse(localStorage.getItem("deliveredOrders"));
    const undeliveredOrders = useSelector((state) => state.order.undeliveredOrders);
    const user = useSelector((state) => state.user.user);
    const [isInitial, setIsInitial] = useState(true);

    let isAdmin = false;
    if(user.type === 2){
        isAdmin = true;
    }
    let isBuyer = false;
    if(user.type === 1)
    {
        isBuyer = true;
    }
    
  useEffect(() => {
    const waitForUser = async () => {
      while (user === null) {
        await new Promise((resolve) => setTimeout(resolve, 100)); // Wait for 100 milliseconds
      }
      execute();
    
      setIsInitial(false);
    };
  
    const execute = () => {
      if (user.type === 2) {
         dispatch(getOrdersAction());
      } else if (user.type === 1) {
         dispatch(getUserOrdersAction());
         dispatch(getUndeliveredOrdersAction());
      } else {
         dispatch(getSellerNewOrdersAction());
         dispatch(getSellerOldOrdersAction());
      }
    };
  
    if (isInitial) {
      waitForUser();
    } else {
      return;
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [isInitial,user]);
  
  const handleUpdateOrders = () =>
  {
    dispatch(getUndeliveredOrdersAction());
  }

  return (
  <React.Fragment>
    <Navbar/>
    {!isAdmin && undeliveredOrders !== null &&<Orders orders={undeliveredOrders} undelivered={true} header={"Undelivered orders"} isBuyer={isBuyer} handleUpdateOrders = {handleUpdateOrders}/>}
    {deliveredOrders !== null  && <Orders orders={deliveredOrders}/>}
 </React.Fragment>
  )
}