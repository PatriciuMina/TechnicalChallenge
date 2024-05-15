import React, { useState, useEffect, useRef } from 'react';
import axios from 'axios';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function OtpGenerate() {
    const [otp, setOtp] = useState('');
    const isMounted = useRef(true);  

    useEffect(() => {
        return () => {
            isMounted.current = false;  
        };
    }, []);

    const generateOtp = async () => {
        try {
            const response = await axios.get('https://localhost:44370/api/otp/generate');
            const { Otp, ToastDuration } = response.data;
            if (isMounted.current) {
                setOtp(Otp);
            }
            toast.success(`OTP Generated Successfully: ${Otp}`, {
                autoClose: ToastDuration,
                onClose: () => {
                    if (isMounted.current) {
                        console.log("Toast closed");
                    }
                }
            });
        } catch (error) {
            const message = error.response?.data?.Message || error.message;
            const duration = error.response?.data?.ToastDuration || 5000;
            toast.error(`Failed to generate OTP: ${message}`, {
                autoClose: duration,
                onClose: () => {
                    if (isMounted.current) {
                        console.log("Error Toast closed");
                    }
                }
            });
        }
    };

    return (
        <div>
            <button onClick={generateOtp}>Generate OTP</button>
            {otp && <div><strong>OTP: {otp}</strong></div>}
            <ToastContainer />
        </div>
    );
}

export default OtpGenerate;