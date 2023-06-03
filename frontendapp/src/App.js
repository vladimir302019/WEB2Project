import logo from './logo.svg';
import './App.css';
import AppRoutes from './Routes/AppRoutes';
import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { logout } from './Store/userSlice';

function App() {
  const dispatch = useDispatch();
  useEffect(() => {
    const handleUnload = () => {
    dispatch(logout());
  };

  window.addEventListener('beforeunload', handleUnload);

  return() => {
    window.removeEventListener('beforeunload', handleUnload);
  };
  
},[dispatch]);

  return (
    <div className="App">
      <AppRoutes/>
    </div>
  );
}

export default App;
