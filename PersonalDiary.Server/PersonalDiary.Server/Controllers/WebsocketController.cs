using BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedData.Models;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalDiary.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private readonly ILogger<WebSocketController> _logger;
        private readonly NeuralNetworkService _neuralNetworkService;

        public WebSocketController(ILogger<WebSocketController> logger, NeuralNetworkService neuralNetworkService)
        {
            _logger = logger;
            _neuralNetworkService = neuralNetworkService;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _logger.Log(LogLevel.Information, "WebSocket connection established");
                await Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task Echo(WebSocket webSocket)
        {
            byte[] buffer = new byte[30000 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string base64String = "";
            _logger.Log(LogLevel.Information, "Message received from Client");

            while (!result.CloseStatus.HasValue)
            {
                string receivedJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Message receivedMsg = JsonSerializer.Deserialize<Message>(receivedJson);
                MessageTypes messageTypeSend = MessageTypes.ResultLoginFace;
                bool isSucceed = false;

                switch (receivedMsg.Type)
                {
                    case MessageTypes.LoginFace:
                        base64String = _neuralNetworkService.ProcessLoginFace(receivedMsg.Content, receivedMsg.UserId);
                        messageTypeSend = MessageTypes.ResultLoginFace;
                        break;
                    case MessageTypes.RegisterFace:
                        base64String = _neuralNetworkService.ProcessRegisterFace(receivedMsg.Content, receivedMsg.UserId, receivedMsg.ShouldSave);
                        messageTypeSend = MessageTypes.ResultRegisterFace;
                        break;
                    default:
                        break;
                }

                switch (messageTypeSend)
                {
                    case MessageTypes.ResultLoginFace:
                        _neuralNetworkService.usersLoginSucceed.TryGetValue(receivedMsg.UserId.ToString(), out bool isSucceedLogin);
                        isSucceed = isSucceedLogin;
                        break;
                    case MessageTypes.ResultRegisterFace:
                        _neuralNetworkService.usersRegisterSucceed.TryGetValue(receivedMsg.UserId.ToString(), out bool isSucceedRegister);
                        isSucceed = isSucceedRegister;
                        break;
                }

                Message sendObject = new Message
                {
                    Type = messageTypeSend,
                    Content = base64String,
                    Succeed = isSucceed
                };
                string sendJson = JsonSerializer.Serialize<Message>(sendObject);
                var sendMsg = Encoding.UTF8.GetBytes(sendJson);


                await webSocket.SendAsync(new ArraySegment<byte>(sendMsg, 0, sendMsg.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                _logger.Log(LogLevel.Information, "Message sent to Client");

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                _logger.Log(LogLevel.Information, "Message received from Client");

            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _logger.Log(LogLevel.Information, "WebSocket connection closed");
        }
    }
}
