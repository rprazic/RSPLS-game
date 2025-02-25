import React from 'react';
import { createTheme, ThemeProvider, CssBaseline, AppBar, Toolbar, Typography } from '@mui/material';
import RPSLSGame from './RPSLSGame';

// Create a theme
const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#9c27b0',
    },
    background: {
      default: '#f5f5f5',
    },
  },
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            RPSLS Game
          </Typography>
        </Toolbar>
      </AppBar>
      <RPSLSGame />
    </ThemeProvider>
  );
}

export default App;