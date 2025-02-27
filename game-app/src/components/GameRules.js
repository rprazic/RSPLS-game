import React from 'react';
import { Box, Typography, Grid } from '@mui/material';

export const GameRules = () => (
    <Box sx={{ mt: 4 }}>
        <Typography variant="h5" gutterBottom sx={{ textAlign: 'center' }}>
            Game Rules
        </Typography>

        <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
                <Typography variant="body1">
                    <strong>Rock:</strong> crushes Scissors, crushes Lizard
                </Typography>
                <Typography variant="body1">
                    <strong>Paper:</strong> covers Rock, disproves Spock
                </Typography>
                <Typography variant="body1">
                    <strong>Scissors:</strong> cuts Paper, decapitates Lizard
                </Typography>
            </Grid>
            <Grid item xs={12} md={6}>
                <Typography variant="body1">
                    <strong>Lizard:</strong> eats Paper, poisons Spock
                </Typography>
                <Typography variant="body1">
                    <strong>Spock:</strong> vaporizes Rock, smashes Scissors
                </Typography>
            </Grid>
        </Grid>
    </Box>
);