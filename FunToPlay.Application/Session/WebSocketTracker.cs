using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace FunToPlay.Application.Session;

public class WebSocketTracker
{
    public WebSocket WebSocket { get; set; }
    public string DeviceId { get; set; }
}

