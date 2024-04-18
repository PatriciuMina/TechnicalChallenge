import React, { useEffect } from 'react';

function TestApiCall() {
    useEffect(() => {
        fetch('https://localhost:44370/api/values', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error('Error fetching data:', error));
    }, []);

    return <div>Check the console for API call response.</div>;
}

export default TestApiCall;
