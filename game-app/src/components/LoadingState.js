import React from 'react';
import { Container, CircularProgress, Typography } from '@mui/material';

export const LoadingState = () => (
    <Container maxWidth="md" sx={{ textAlign: 'center', mt: 5 }}>
        <CircularProgress />
        <Typography variant="h6" mt={2}>Loading game choices...</Typography>
    </Container>
);