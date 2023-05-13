import { useState, useEffect } from 'react';
import axios from 'axios';
import * as signalR from "@microsoft/signalr";
import styles from './table.module.css';
import { Link } from 'react-router-dom';

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
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {table.map((item, index) => (
                        <tr key={index} className={styles.activeRow}>
                            <td>{item.Id}</td>
                            <td>{item.Name}</td>
                            <td>{item.LegLocation}</td>
                            <td><button onClick={() => onDeleteHandler(item.Id)}>delete</button></td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default Table;