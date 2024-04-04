import React, { useState } from 'react';
import axios from 'axios';
import logo from './logo.svg';
import './App.css';

function App() {
  const [apiResponse, setApiResponse] = useState(null);

  const fetchData = () => {
    const data = { level: 0 };
  
    axios.post('http://localhost:5008/games/', {
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      },
      body: JSON.stringify(data),
    })
      .then(response => response.json())
      .then(data => setApiResponse(JSON.stringify(data, null, 2)))
      .catch(error => setApiResponse(error.toString()));
  };

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <button onClick={fetchData}>Fetch Data</button>
        <pre>{apiResponse}</pre>
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