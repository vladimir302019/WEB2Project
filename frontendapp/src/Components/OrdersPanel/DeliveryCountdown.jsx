import React, { useEffect, useState } from 'react';
import Typography from '@mui/material/Typography';

const DeliveryCountdown = ({ deliveryTime }) => {
  const [countdown, setCountdown] = useState({ minutes: 0, seconds: 0 });

  useEffect(() => {
    const calculateCountdown = () => {
      const currentTime = new Date().getTime();
      const deliveryTimeValue = new Date(deliveryTime).getTime();

      const remainingTime = deliveryTimeValue - currentTime;
      const remainingSeconds = Math.floor((remainingTime / 1000) % 60);
      const remainingMinutes = Math.floor((remainingTime / (1000 * 60)) % 60);

      setCountdown({ minutes: remainingMinutes, seconds: remainingSeconds });
    };

    calculateCountdown();

    const timer = setInterval(calculateCountdown, 1000);

    // Clean up the timer when the component unmounts
    return () => clearInterval(timer);
  }, [deliveryTime]);

  return (
    <Typography component="div">
      {countdown.minutes}:{countdown.seconds < 10 ? '0' : ''}
      {countdown.seconds}
    </Typography>
  );
};


  export default DeliveryCountdown;