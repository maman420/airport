using System.Text;
using System.Text.Json;

Console.WriteLine("Hello, i'm the simulator. i will inject planes to the api!");

Random random = new Random();
var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
var client = new HttpClient(handler);

var url = "http://localhost:5014/";
await client.DeleteAsync(url + "deleteAll");
await Task.Delay(1000);

while(true){
    var newFlight = new {
        name = GenerateRandomName(10),
        legLocation = 0
    };
    var json = JsonSerializer.Serialize(newFlight);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var oneOrTwo = random.Next(1,3);
    if(oneOrTwo == 1)
        await client.PostAsync(url, content);
    else if(oneOrTwo == 2)
        await client.PostAsync(url + "fromTerminal", content);

    Console.WriteLine("plane " + newFlight.name + " is sent to the airport!");
    await Task.Delay(random.Next(1000,5000));
}
  
string GenerateRandomName(int length)
{
    const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();
    var chars = new char[length];

    for (int i = 0; i < length; i++)
    {
        chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
    }

    return new string(chars);
}
