// See https://aka.ms/new-console-template for more information
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Serilog;

Console.WriteLine("Hello, World!");

static async Task Main(string[] args)
{   
    ConfigureLogging();

    using var client = new ClientWebSocket();
    await client.ConnectAsync(new Uri("ws://localhost:5000/"), default);
    Log.Information("Connected to the server.");

    // Example usage
    await LoginAsync(client, "device123");

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

static void ConfigureLogging()
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.File("logs/client_log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

static async Task LoginAsync(ClientWebSocket client, string deviceId)
{
    var loginRequest = new
    {
        Type = "Login",
        Payload = new { DeviceId = deviceId }
    };

    await SendMessageAsync(client, loginRequest);
}

static async Task UpdateResourcesAsync(ClientWebSocket client, string deviceId, string resourceType, int resourceValue)
{
    var updateResourcesRequest = new
    {
        Type = "UpdateResources",
        Payload = new { DeviceId = deviceId, ResourceType = resourceType, ResourceValue = resourceValue }
    };

    await SendMessageAsync(client, updateResourcesRequest);
}

static async Task SendGiftAsync(ClientWebSocket client, string deviceId, string friendPlayerId, string resourceType, int resourceValue)
{
    var sendGiftRequest = new
    {
        Type = "SendGift",
        Payload = new { DeviceId = deviceId, FriendPlayerId = friendPlayerId, ResourceType = resourceType, ResourceValue = resourceValue }
    };

    await SendMessageAsync(client, sendGiftRequest);
}

static async Task SendMessageAsync(ClientWebSocket client, object message)
{
    var messageJson = JsonSerializer.Serialize(message);
    var messageBuffer = Encoding.UTF8.GetBytes(messageJson);
    await client.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

    var responseBuffer = new byte[1024 * 4];
    var response = await client.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None);
    var responseJson = Encoding.UTF8.GetString(responseBuffer, 0, response.Count);

    Log.Information("Received response: {Response}", responseJson);
}