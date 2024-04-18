import React, { useState } from 'react';
import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function OtpValidate() {
    const [otp, setOtp] = useState('');
    const [validationMessage, setValidationMessage] = useState('');

    const validateOtp = async () => {
        try {
            const response = await axios.post('https://localhost:44370/api/otp/validate', { otp }, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            const message = response.data.Message || response.data;
            setValidationMessage(message);
            toast.success("OTP Validation: " + message);
        } catch (error) {
          
            let errorMessage = "Failed to validate OTP";
            if (error.response && error.response.data) {
                errorMessage = error.response.data.Message || JSON.stringify(error.response.data);
            }
            setValidationMessage(errorMessage);
            toast.error("OTP Validation Error: " + errorMessage);
        }
    };

    return (
        <div>
            <input
                type="text"
                value={otp}
                onChange={e => setOtp(e.target.value)}
                placeholder="Enter your OTP"
            />
            <button onClick={validateOtp}>Validate OTP</button>
            
            <ToastContainer />
        </div>
    );
}

export default OtpValidate;
