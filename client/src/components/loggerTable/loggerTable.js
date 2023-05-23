import { useState, useEffect } from 'react';
import axios from 'axios';
import * as signalR from "@microsoft/signalr";
import styles from './loggerTable.module.css';


function LoggerTable() {
    const [flightLogger, setFlightLogger] = useState([]);

    const url = process.env.REACT_APP_API_URL;

    useEffect(() => {
        fetchData();

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(url + "airporthub")
            .build();

        connection.on("SendAllFlightsLogger", (d) => {
            setFlightLogger(JSON.parse(d));
        });

        connection.start();
    }, []);

    const fetchData = async () => {
        try {
            const flightLoggerResponse = await axios.get(url + "flightLogger");
            setFlightLogger(flightLoggerResponse.data);
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <div>
            <h1>logger</h1><br />
            <table className={styles.styledTable}>
                <thead>
                    <tr>
                        <th>flight logger</th>
                        <th>name</th>
                        <th>location</th>
                        <th>Airline</th>
                        <th>exit time</th>
                    </tr>
                </thead>
                <tbody>
                    {flightLogger.map((item, index) => (
                        <tr key={index} className={styles.activeRow}>
                            <td>{item.Flight.Id}</td>
                            <td>{item.Flight.Name}</td>
                            <td>{item.Flight.LegLocation}</td>
                            <td>{item.Flight.AirLine}</td>
                            <td>{new Date(item.Exit).toLocaleString('en-US', {
                                year: 'numeric',
                                month: '2-digit',
                                day: '2-digit',
                                hour: '2-digit',
                                minute: '2-digit'
                            })}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default LoggerTable;