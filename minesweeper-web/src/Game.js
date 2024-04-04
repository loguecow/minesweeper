import React, { useEffect, useState } from 'react';
import axios from 'axios';

function Game() {
  const [game, setGame] = useState(null);

  useEffect(() => {
    axios.get('http://localhost:5000/game/1')
      .then(response => {
        setGame(response.data);
      });
  }, []);

  if (!game) {
    return 'Loading...';
  }

  return (
    <div>
      {/* Render your game data here */}
    </div>
  );
}

export default Game;