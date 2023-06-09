import { useState, useEffect } from 'react';
import axios from 'axios';
import * as signalR from "@microsoft/signalr";
import styles from './table.module.css';

function Table() {
    const [table, setTable] = useState([]);

    const url = process.env.REACT_APP_API_URL;

    useEffect(() => {
        fetchData();

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(url + "airporthub")
            .build();

        connection.on("SendAllFlights", (d) => {
            setTable(JSON.parse(d).filter(item => item.LegLocation !== 0));
        });

        connection.start();
    }, []);

    const fetchData = async () => {
        try {
            const tableResponse = await axios.get(url);
            setTable(tableResponse.data.filter(item => item.LegLocation !== 0));
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
            <h1>active flights</h1>
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
        </div>
    );
}

export default Table;