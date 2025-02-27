// src/components/GameResult.js
import React from 'react';
import { Box, Typography, Grid, Card, CardContent, CircularProgress } from '@mui/material';

export const GameResult = ({ playerChoice, computerChoice, result, choiceIcons }) => (
    <Box sx={{ mb: 4 }}>
        <Typography variant="h5" gutterBottom sx={{ textAlign: 'center' }}>
            Game Result
        </Typography>

        <Grid container spacing={2} justifyContent="center" alignItems="center">
            <Grid item xs={12} sm={5}>
                <Card>
                    <CardContent sx={{ textAlign: 'center' }}>
                        <Typography variant="h6" component="div" gutterBottom>
                            Your Choice
                        </Typography>
                        <Box sx={{ fontSize: 60, my: 2 }}>
                            {choiceIcons[playerChoice.id]}
                        </Box>
                        <Typography variant="h5" component="div">
                            {playerChoice.name.toUpperCase()}
                        </Typography>
                    </CardContent>
                </Card>
            </Grid>

            <Grid item xs={12} sm={2}>
                <Box sx={{ textAlign: 'center' }}>
                    <Typography variant="h4" component="div">
                        VS
                    </Typography>
                </Box>
            </Grid>

            <Grid item xs={12} sm={5}>
                <Card>
                    <CardContent sx={{ textAlign: 'center' }}>
                        <Typography variant="h6" component="div" gutterBottom>
                            Computer's Choice
                        </Typography>
                        {computerChoice ? (
                            <>
                                <Box sx={{ fontSize: 60, my: 2 }}>
                                    {choiceIcons[computerChoice.id]}
                                </Box>
                                <Typography variant="h5" component="div">
                                    {computerChoice.name.toUpperCase()}
                                </Typography>
                            </>
                        ) : (
                            <CircularProgress sx={{ my: 4 }} />
                        )}
                    </CardContent>
                </Card>
            </Grid>
        </Grid>

        {result && (
            <Box sx={{ textAlign: 'center', mt: 3 }}>
                <Typography
                    variant="h4"
                    component="div"
                    color={
                        result === 'win' ? 'success.main' :
                            result === 'lose' ? 'error.main' :
                                'text.secondary'
                    }
                >
                    {result === 'win' && 'You Win! 🎉'}
                    {result === 'lose' && 'You Lose! 😞'}
                    {result === 'tie' && "It's a Tie! 🤝"}
                </Typography>
            </Box>
        )}
    </Box>
);
