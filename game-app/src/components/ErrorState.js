import React from 'react';
import { Container, Alert, Button } from '@mui/material';

export const ErrorState = ({ error, onRetry }) => (
    <Container maxWidth="md" sx={{ textAlign: 'center', mt: 5 }}>
        <Alert severity="error">
            {error}
            <br />
            <Button variant="outlined" color="error" onClick={onRetry} sx={{ mt: 2 }}>
                Retry
            </Button>
        </Alert>
    </Container>
);