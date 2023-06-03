import React from "react";
import { Backdrop, CircularProgress } from "@mui/material";

const LoadingModal = (props) => {
  return (
    <Backdrop
      open={props.show}
      sx={{ color: "#fff", zIndex: (theme) => theme.zIndex.drawer + 1 }}
    >
      <CircularProgress color="inherit" size="4rem" />
    </Backdrop>
  );
};

export default LoadingModal;