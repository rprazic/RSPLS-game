import React from 'react';
import { Box, Typography, Button } from '@mui/material';

export const RandomChoice = ({ randomChoice, choiceIcons, onGetRandomChoice }) => (
    <Box sx={{ textAlign: 'center', mb: 3 }}>
        <Typography variant="h5" gutterBottom>
            Get Random Choice
        </Typography>

        <Button
            variant="contained"
            color="secondary"
            onClick={onGetRandomChoice}
            sx={{ mb: 2 }}
        >
            Get Random Choice
        </Button>

        {randomChoice && (
            <Box sx={{ mt: 2 }}>
                <Typography variant="body1">
                    Random choice: <strong>{randomChoice.name.toUpperCase()}</strong>
                </Typography>
                <Box sx={{ fontSize: 40, my: 1 }}>
                    {choiceIcons[randomChoice.id]}
                </Box>
            </Box>
        )}
    </Box>
);