using System.Text;
using System.Text.Json;
using simulator.models;

Console.WriteLine("Hello, i'm the simulator. i will inject planes to the api!");

Random random = new Random();
var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
var client = new HttpClient(handler);

var url = "http://localhost:5014/";
await client.DeleteAsync(url + "deleteAll");
await Task.Delay(1000);

while(true){
    var newFlight = new FlightDto();
    var json = JsonSerializer.Serialize(newFlight);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var oneOrTwo = random.Next(1,3);
    if(oneOrTwo == 1)
        await client.PostAsync(url, content);
    else if(oneOrTwo == 2)
        await client.PostAsync(url + "fromTerminal", content);

    Console.WriteLine("plane " + newFlight.Name + " is sent to the airport!");
    await Task.Delay(random.Next(1000,3000));
}
  

