// See https://aka.ms/new-console-template for more information
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Serilog;

Console.WriteLine("Hello, World!");

 
ConfigureLogging();

using var client = new ClientWebSocket();
await client.ConnectAsync(new Uri("ws://localhost:5000/"), default);
Log.Information("Connected to the server.");
var result = await LoginAsync(client, "device144456792121212121");
Log.Information($"User id is: {result}");
var newBalance = await UpdateResourcesAsync(client, "coins", 122);
Log.Information($"New balance is {newBalance}");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();


static void ConfigureLogging()
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File("logs/client_log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

static async Task<string> LoginAsync(ClientWebSocket client, string deviceId)
{
    var loginRequest = new
    {
        Metadata = new
        {
            Id = Guid.NewGuid().ToString(),
            Version = 1,
            Timestamp = DateTime.UtcNow
        },
        MessageType = "Login",
        DeviceId = deviceId
    };

    return await SendMessageAsync(client, loginRequest);
}

static async Task<string> UpdateResourcesAsync(ClientWebSocket client, string resourceType, int resourceValue)
{
    var updateResourcesRequest = new
    {
        Metadata = new
        {
            Id = Guid.NewGuid().ToString(),
            Version = 1,
            Timestamp = DateTime.UtcNow
        },
        MessageType = "UpdateResources",
        ResourceType = resourceType, 
        ResourceValue = resourceValue
    };

    return await SendMessageAsync(client, updateResourcesRequest);
}

static async Task<string> SendMessageAsync(ClientWebSocket client, object message)
{
    var messageJson = JsonSerializer.Serialize(message);
    var messageBuffer = Encoding.UTF8.GetBytes(messageJson);
    await client.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

    var responseBuffer = new byte[1024 * 4];
    var response = await client.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None);
    var responseJson = Encoding.UTF8.GetString(responseBuffer, 0, response.Count);

    Log.Information("Received response: {Response}", responseJson);

    return responseJson;
}