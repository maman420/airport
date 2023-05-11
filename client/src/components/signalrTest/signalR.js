import React, { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

function MyComponent() {
  const [data, setData] = useState([]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7196/airporthub")
      .build();

    connection.on("SendData", (d) => {
      setData(JSON.parse(d));
      console.log(d);
    });

    connection.start();

  }, []);

  // console.log(data);

  return (
    <div>
      <h1>My Data</h1>
      <ul>
        {
          data.map((item, index) =>
            <li key={index}>{item.Id}, {item.Name}, {item.LegLocation}</li>
          )
        }
      </ul>
    </div>
  );
}

export default MyComponent;