# Rock Paper Scissors Lizard Spock UI Game

## ✨ Features

- Clean, responsive UI built with React and Material UI
- Play against a computer opponent
- Get random choice suggestions
- View game rules and outcomes
- Visual feedback for wins, losses, and ties
- Dockerized for easy deployment

## 🚀 Quick Start

### Prerequisites

- Node.js 22+ and npm
- Docker and Docker Compose (for containerized deployment)

### Running Locally

1. Clone the repository:
   ```bash
   git clone https://github.com/username/rpsls-game.git
   cd rpsls-game
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

4. Open [http://localhost:3000](http://localhost:3000) in your browser.

### API Configuration

The app connects to a backend API that should be running on `http://localhost:6001`. You can configure a different API URL by setting the `REACT_APP_API_URL` environment variable:

```bash
REACT_APP_API_URL=https://your-api-url npm start
```

## 🔧 API Endpoints

The app interacts with the following API endpoints:

### Get All Choices
```
GET: Game/choices
```
Returns all available game choices:
```json
[
  { "id": 1, "name": "rock" },
  { "id": 2, "name": "paper" },
  { "id": 3, "name": "scissors" },
  { "id": 4, "name": "lizard" },
  { "id": 5, "name": "spock" }
]
```

### Get Random Choice
```
GET: game/choice
```
Returns a randomly generated choice:
```json
{
  "id": 3,
  "name": "scissors"
}
```

### Play a Round
```
POST: Game/play
```
Request body:
```json
{
  "player": 1
}
```
Response:
```json
{
  "results": "win",
  "player": 1,
  "computer": 3
}
```

## 🧩 Project Structure

```
rpsls-game/
├── public/              # Static files
├── src/
│   ├── App.js           # Main App component
│   ├── RPSLSGame.js     # Game component
│   └── index.js         # Entry point
├── Dockerfile           # Docker configuration
├── nginx.conf           # Nginx configuration for Docker
├── docker-compose.yml   # Docker Compose configuration
└── package.json         # Dependencies and scripts
```

## 🛠️ Technologies Used

- [React](https://reactjs.org/) - Frontend library
- [Material UI](https://mui.com/) - UI component library
- [Docker](https://www.docker.com/) - Containerization
- [Nginx](https://nginx.org/) - Web server for production build

## 🔄 Development Workflow

1. Make changes to the code
2. Test locally using `npm start`
