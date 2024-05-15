import React from 'react';
import OtpGenerate from './OtpGenerate';
import OtpValidate from './OtpValidate';
import SetValidityTime from './SetValidityTime';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import './App.css'; 

function App() {
  return (
    <div className="App">
      <div className="container">
        <h1>OTP Management System</h1>
        <div className="module">
          <h2>Generate OTP</h2>
          <OtpGenerate />
        </div>
        <div className="module">
          <h2>Validate OTP</h2>
          <OtpValidate />
        </div>
        <div className="module">
          <h2>Set OTP Validity Period</h2>
          <SetValidityTime />
        </div>
      </div>
    </div>
  );
}

export default App;