import React, { useState } from 'react';
import axios from 'axios';
import logo from './logo.svg';
import './App.css';

import { ApiResponse } from './models';

function App() {
  const [apiResponse, setApiResponse] = useState<ApiResponse | null>(null);


  const fetchData = () => {
    const data = { level: 0 };
  
    axios.post<ApiResponse>('http://localhost:5008/games/', {
      body: JSON.stringify(data),
    })
      .then(response => response.data)
      .then(data => setApiResponse(data))
      .catch(error => setApiResponse(error.toString()));
  };

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Minesweeep
        </p>
        <button onClick={fetchData}>Fetch Data</button>
        <pre>{apiResponse?.gameId}</pre>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;