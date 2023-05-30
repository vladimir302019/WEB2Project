import React from 'react';
import { Avatar, Typography, Grid, TextField} from '@mui/material';
import { DatePicker, LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';

const ProfilePage = () => {

  return (
    <div>
      <Grid container spacing={2} alignItems="center" justify="center">
        <Grid item>
          <Avatar  src="/path/to/profile-image.jpg" alt="Profile Picture" />
        </Grid>
        <Grid item>
          <Typography variant="h5" component="h1">
            John Doe
          </Typography>
          <Typography variant="subtitle1" color="textSecondary">
            Web Developer
          </Typography>
          <TextField
            id="username"
            label="Username"
            variant="outlined"
            margin="normal"
            fullWidth
            value="johndoe"
          />
          <TextField
            id="password"
            label="Password"
            type="password"
            variant="outlined"
            margin="normal"
            fullWidth
            value="**"
          />
          <TextField
            id="email"
            label="Email"
            type="email"
            variant="outlined"
            margin="normal"
            fullWidth
            value="johndoe@example.com"
          />
          <LocalizationProvider dateAdapter={AdapterDayjs}>
                    <DatePicker 
                        required
                        fullWidth
                        name="birthdate"
                        label="Birth Date *"
                        id="birthdate"
                    />
                </LocalizationProvider>
          <TextField
            id="address"
            label="Address"
            variant="outlined"
            margin="normal"
            fullWidth
            multiline
            rows={3}
            value="123 Main St, City, Country"
          />
        </Grid>
      </Grid>
    </div>
  );
};

export default ProfilePage;