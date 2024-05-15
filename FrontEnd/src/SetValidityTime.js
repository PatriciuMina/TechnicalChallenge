import React, { useState } from 'react';
import axios from 'axios';

function SetValidityTime() {
    const [period, setPeriod] = useState('');
    const [message, setMessage] = useState('');

    const updateValidityPeriod = async () => {
        try {
            const response = await axios.post('https://localhost:44370/api/otp/set-validity', period, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            setMessage(`Server response: ${response.data}`);
        } catch (error) {
            setMessage(`Error updating validity period: ${error.response?.data || error.message}`);
        }
    };

    return (
        <div>
            <input 
                type="number" 
                value={period} 
                onChange={e => setPeriod(e.target.value)} 
                placeholder="Enter new validity period (minutes)" 
            />
            <button onClick={updateValidityPeriod}>Update Validity Period</button>
            {message && <div>{message}</div>}
        </div>
    );
}

export default SetValidityTime;