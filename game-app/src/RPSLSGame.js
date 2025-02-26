import React, { useState, useEffect } from 'react';
import {
    Container,
    Typography,
    Button,
    Paper,
    Grid,
    CircularProgress,
    Card,
    CardContent,
    Box,
    Divider,
    Snackbar,
    Alert
} from '@mui/material';

// Icons for the choices
import ThumbUpAltIcon from '@mui/icons-material/ThumbUpAlt'; // Rock
import ArticleIcon from '@mui/icons-material/Article'; // Paper
import ContentCutIcon from '@mui/icons-material/ContentCut'; // Scissors
import PetsIcon from '@mui/icons-material/Pets'; // Lizard
import GestureIcon from '@mui/icons-material/Gesture'; // Spock

// Get API URL from environment variable or use default
const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:6001';

// Map ID to icons
const choiceIcons = {
    1: <ThumbUpAltIcon fontSize="large" />, // Rock
    2: <ArticleIcon fontSize="large" />, // Paper
    3: <ContentCutIcon fontSize="large" />, // Scissors
    4: <PetsIcon fontSize="large" />, // Lizard
    5: <GestureIcon fontSize="large" /> // Spock
};

function RPSLSGame() {
    const [choices, setChoices] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [playerChoice, setPlayerChoice] = useState(null);
    const [computerChoice, setComputerChoice] = useState(null);
    const [result, setResult] = useState(null);
    const [randomChoice, setRandomChoice] = useState(null);
    const [snackbar, setSnackbar] = useState({
        open: false,
        message: '',
        severity: 'info'
    });

    // Fetch all possible choices
    useEffect(() => {
        fetchChoices();
    }, []);

    const fetchChoices = async () => {
        try {
            setLoading(true);
            const response = await fetch(`${API_URL}/Game/choices`);

            if (!response.ok) {
                throw new Error(`Error fetching choices: ${response.statusText}`);
            }

            const data = await response.json();
            setChoices(data);
            setLoading(false);
        } catch (err) {
            setError(`Failed to fetch choices: ${err.message}`);
            setLoading(false);
            setSnackbar({
                open: true,
                message: `API Error: ${err.message}. Make sure your API is running on ${API_URL}`,
                severity: 'error'
            });
        }
    };

    // Get a random choice from API
    const getRandomChoice = async () => {
        try {
            const response = await fetch(`${API_URL}/Game/choice`);

            if (!response.ok) {
                throw new Error(`Error fetching random choice: ${response.statusText}`);
            }

            const data = await response.json();
            setRandomChoice(data);
            setSnackbar({
                open: true,
                message: `Random choice: ${data.name}`,
                severity: 'info'
            });
        } catch (err) {
            setError(`Failed to fetch random choice: ${err.message}`);
            setSnackbar({
                open: true,
                message: `API Error: ${err.message}`,
                severity: 'error'
            });
        }
    };

    // Play a round
    const playGame = async (choiceId) => {
        try {
            setPlayerChoice(choices.find(c => c.id === choiceId));
            setComputerChoice(null); // Reset previous computer choice
            setResult(null); // Reset previous result

            const response = await fetch(`${API_URL}/Game/play`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ player: choiceId })
            });

            if (!response.ok) {
                throw new Error(`Error playing game: ${response.statusText}`);
            }

            const data = await response.json();
            // Find computer choice from the ID returned by the API
            const computerChoiceObj = choices.find(c => c.id === data.computer);
            setComputerChoice(computerChoiceObj);
            setResult(data.results);

            // Show result notification
            setSnackbar({
                open: true,
                message: `You ${data.results}!`,
                severity: data.results === 'win' ? 'success' : data.results === 'lose' ? 'error' : 'info'
            });

        } catch (err) {
            setError(`Failed to play game: ${err.message}`);
            setSnackbar({
                open: true,
                message: `API Error: ${err.message}`,
                severity: 'error'
            });
        }
    };

    const handleCloseSnackbar = () => {
        setSnackbar(prev => ({ ...prev, open: false }));
    };

    // Render loading state
    if (loading) {
        return (
            <Container maxWidth="md" sx={{ textAlign: 'center', mt: 5 }}>
                <CircularProgress />
                <Typography variant="h6" mt={2}>Loading game choices...</Typography>
            </Container>
        );
    }

    // Render error state
    if (error) {
        return (
            <Container maxWidth="md" sx={{ textAlign: 'center', mt: 5 }}>
                <Alert severity="error">
                    {error}
                    <br />
                    <Button variant="outlined" color="error" onClick={fetchChoices} sx={{ mt: 2 }}>
                        Retry
                    </Button>
                </Alert>
            </Container>
        );
    }

    return (
        <Container maxWidth="md">
            <Paper elevation={3} sx={{ p: 4, mt: 4, mb: 4 }}>
                <Typography variant="h3" component="h1" gutterBottom sx={{ textAlign: 'center' }}>
                    Rock Paper Scissors Lizard Spock
                </Typography>

                <Typography variant="body1" paragraph sx={{ textAlign: 'center' }}>
                    Choose one of the options below to play against the computer:
                </Typography>

                {/* Game choices */}
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
                                onClick={() => playGame(choice.id)}
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

                {/* Game result */}
                {playerChoice && (
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
                )}

                <Divider sx={{ my: 4 }} />

                {/* Random choice section */}
                <Box sx={{ textAlign: 'center', mb: 3 }}>
                    <Typography variant="h5" gutterBottom>
                        Get Random Choice
                    </Typography>

                    <Button
                        variant="contained"
                        color="secondary"
                        onClick={getRandomChoice}
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

                {/* Game rules section */}
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
            </Paper>

            {/* Snackbar for notifications */}
            <Snackbar
                open={snackbar.open}
                autoHideDuration={3000}
                onClose={handleCloseSnackbar}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
            >
                <Alert onClose={handleCloseSnackbar} severity={snackbar.severity}>
                    {snackbar.message}
                </Alert>
            </Snackbar>
        </Container>
    );
}

export default RPSLSGame;