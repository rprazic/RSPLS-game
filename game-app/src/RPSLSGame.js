import React, { useState, useEffect } from 'react';
import {
    Container,
    Paper,
    Typography,
    Divider,
    Snackbar,
    Alert
} from '@mui/material';

import { LoadingState } from './components/LoadingState';
import { ErrorState } from './components/ErrorState';
import { GameChoices } from './components/GameChoices';
import { GameResult } from './components/GameResult';
import { RandomChoice } from './components/RandomChoice';
import { GameRules } from './components/GameRules';

// Icons for the choices
import ThumbUpAltIcon from '@mui/icons-material/ThumbUpAlt';
import ArticleIcon from '@mui/icons-material/Article';
import ContentCutIcon from '@mui/icons-material/ContentCut';
import PetsIcon from '@mui/icons-material/Pets';
import BackHandIcon from '@mui/icons-material/BackHand';

const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:6001';

const choiceIcons = {
    1: <ThumbUpAltIcon fontSize="large" />,
    2: <ArticleIcon fontSize="large" />,
    3: <ContentCutIcon fontSize="large" />,
    4: <PetsIcon fontSize="large" />,
    5: <BackHandIcon fontSize="large" />
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

    useEffect(() => {
        fetchChoices();
    }, []);

    const fetchChoices = async () => {
        try {
            setLoading(true);
            const response = await fetch(`${API_URL}/Game/choices`);
            if (!response.ok) throw new Error(`Error fetching choices: ${response.statusText}`);
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

    const getRandomChoice = async () => {
        try {
            const response = await fetch(`${API_URL}/Game/choice`);
            if (!response.ok) throw new Error(`Error fetching random choice: ${response.statusText}`);
            const data = await response.json();
            setRandomChoice(data);
            playGame(data.id);
            setSnackbar({
                open: true,
                message: `Random choice selected: ${data.name}`,
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

    const playGame = async (choiceId) => {
        try {
            setPlayerChoice(choices.find(c => c.id === choiceId));
            setComputerChoice(null);
            setResult(null);

            const response = await fetch(`${API_URL}/Game/play`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ player: choiceId })
            });

            if (!response.ok) throw new Error(`Error playing game: ${response.statusText}`);

            const data = await response.json();
            const computerChoiceObj = choices.find(c => c.id === data.computer);
            setComputerChoice(computerChoiceObj);
            setResult(data.results);

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

    if (loading) return <LoadingState />;
    if (error) return <ErrorState error={error} onRetry={fetchChoices} />;

    return (
        <Container maxWidth="md">
            <Paper elevation={3} sx={{ p: 4, mt: 4, mb: 4 }}>
                <Typography variant="h3" component="h1" gutterBottom sx={{ textAlign: 'center' }}>
                    Rock Paper Scissors Lizard Spock
                </Typography>

                <Typography variant="body1" paragraph sx={{ textAlign: 'center' }}>
                    Choose one of the options below to play against the computer:
                </Typography>

                <GameChoices
                    choices={choices}
                    choiceIcons={choiceIcons}
                    playerChoice={playerChoice}
                    onChoiceSelect={playGame}
                />

                {playerChoice && (
                    <GameResult
                        playerChoice={playerChoice}
                        computerChoice={computerChoice}
                        result={result}
                        choiceIcons={choiceIcons}
                    />
                )}

                <Divider sx={{ my: 4 }} />

                <RandomChoice
                    randomChoice={randomChoice}
                    choiceIcons={choiceIcons}
                    onGetRandomChoice={getRandomChoice}
                />

                <GameRules />
            </Paper>

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