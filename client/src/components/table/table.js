import { useState, useEffect } from 'react';
import axios from 'axios';
import * as signalR from "@microsoft/signalr";
import styles from './table.module.css';

function Table() {
    const [table, setTable] = useState([]);
    const [flightLogger, setFlightLogger] = useState([]);

    const url = "http://localhost:5014/";

    useEffect(() => {
        fetchData();

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5014/airporthub")
            .build();

        connection.on("SendAllFlights", (d) => {
            setTable(JSON.parse(d).filter(item => item.LegLocation != 0));
        });
        connection.on("SendAllFlightsLogger", (d) => {
            setFlightLogger(JSON.parse(d));
        });

        connection.start();
    }, []);

    const fetchData = async () => {
        try {
            const tableResponse = await axios.get(url);
            setTable(tableResponse.data);

            const flightLoggerResponse = await axios.get(url + "flightLogger");
            setFlightLogger(flightLoggerResponse.data);
        } catch (error) {
            console.error(error); 
        } 
    };

    const onDeleteHandler = (id) => {
        axios.delete(url + "deleteFlight/" + id)
            .then(response => {
                console.log(response);
            })
            .catch(error => {
                console.log(error);
            });
    }

    return (
        <div>
            <table className={styles.styledTable}>
                <thead>
                    <tr>
                        <th>flight</th>
                        <th>name</th>
                        <th>location</th>
                        <th>Airline</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {table.map((item, index) => (
                        <tr key={index} className={styles.activeRow}>
                            <td>{item.Id}</td>
                            <td>{item.Name}</td>
                            <td>{item.LegLocation}</td>
                            <td>{item.AirLine}</td>
                            <td><button onClick={() => onDeleteHandler(item.Id)}>delete</button></td>
                        </tr>
                    ))} 
                </tbody>
            </table><br/>
            <h1>logger</h1><br/>
            <table className={styles.styledTable}>
                <thead>
                    <tr>
                        <th>flight logger</th>
                        <th>name</th>
                        <th>location</th>
                        <th>Airline</th>
                    </tr>
                </thead>
                <tbody>
                    {flightLogger.map((item, index) => (
                        <tr key={index} className={styles.activeRow}>
                            <td>{item.Flight.Id}</td>
                            <td>{item.Flight.Name}</td>
                            <td>{item.Flight.LegLocation}</td>
                            <td>{item.Flight.AirLine}</td>
                        </tr>
                    ))} 
                </tbody>
            </table>
        </div>
    );
}

export default Table;