import { useState, useEffect } from 'react';
import axios from 'axios';

function Table() {
    const [table, setTable] = useState([]);
    const url = "https://localhost:7196/";

    useEffect(() => {
        fetchTable();
    }, []);

    const fetchTable = async () => {
        try {
            const response = await axios.get(url, { withCredentials: true });
            console.log(response.data)
            setTable(response.data);
        } catch (error) {
            console.error(error);
        }
    };


    return (
        <div>
            <h1>Table</h1>
            {table.map(row => (
                <p key={row.id}>flight: {row.id}, name: {row.name}, location: {row.legLocation}</p>
            ))}
        </div>
    );

}

export default Table;
