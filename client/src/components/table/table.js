import { useState, useEffect } from 'react';
import axios from 'axios';
import * as signalR from "@microsoft/signalr";

function Table() {
    const [table, setTable] = useState([]);
    const url = "https://localhost:7196/";

    useEffect(() => {
        fetchTable();

        const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7196/airporthub")
        .build();
  
      connection.on("SendData", (d) => {
        setTable(JSON.parse(d));
      });
  
      connection.start();
  
    }, []);

    const fetchTable = async () => {
        try {
            const response = await axios.get(url, { withCredentials: true });
            setTable(response.data);
        } catch (error) {
            console.error(error);
        }
    };


    return (
        <div>
            <h1>Table</h1>
            {table.map((item, index) => (
                <p key={index}>flight: {item.Id}, name: {item.Name}, location: {item.LegLocation}</p>
            ))}
        </div>
    );

}

export default Table;
