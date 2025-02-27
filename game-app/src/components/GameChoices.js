import React from 'react';
import { Grid, Card, CardContent, Box, Typography } from '@mui/material';

export const GameChoices = ({ choices, choiceIcons, playerChoice, onChoiceSelect }) => (
    <Grid container spacing={2} justifyContent="center" sx={{ mb: 4 }}>
        {choices.map((choice) => (
            <Grid item key={choice.id} xs={6} sm={4} md={2}>
                <Card
                    elevation={3}
                    sx={{
                        textAlign: 'center',
                        cursor: 'pointer',
                        transition: 'transform 0.2s',
                        '&:hover': {
                            transform: 'scale(1.05)',
                            boxShadow: 6
                        },
                        backgroundColor: playerChoice?.id === choice.id ? '#e3f2fd' : 'white'
                    }}
                    onClick={() => onChoiceSelect(choice.id)}
                >
                    <CardContent>
                        <Box sx={{ fontSize: 40, mb: 1 }}>
                            {choiceIcons[choice.id]}
                        </Box>
                        <Typography variant="h6" component="div">
                            {choice.name.charAt(0).toUpperCase() + choice.name.slice(1)}
                        </Typography>
                    </CardContent>
                </Card>
            </Grid>
        ))}
    </Grid>
);