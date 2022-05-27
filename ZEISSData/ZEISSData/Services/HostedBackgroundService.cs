using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZEISSData.Models;

namespace ZEISSData.Services
{
    public class HostedBackgroundService : BackgroundService
    {
        private readonly IServiceProvider provider;
        private bool firstRun = true;

        public HostedBackgroundService(IServiceProvider provider)
        {
            this.provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var uri = new Uri("ws://machinestream.herokuapp.com/ws");
            var ws = new ClientWebSocket();            
            try
            {
                await ws.ConnectAsync(uri, CancellationToken.None);
                var buffer = new byte[1024 * 4];
                List<byte> data = new List<byte>();
                WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                int limitation = 0;
                while (!stoppingToken.IsCancellationRequested && !result.CloseStatus.HasValue)
                {
                    data.AddRange(buffer.Take(result.Count));
                    if (result.EndOfMessage)
                    {
                        string messageText = Encoding.UTF8.GetString(data.ToArray(), 0, data.Count);

                        //默认存储1000条，内存数据库容量限制
                        if (limitation++ > 1000)
                            continue;

                        using (IServiceScope scope = provider.CreateScope())
                        {                         
                            var messageBo = scope.ServiceProvider.GetService<IMessageBusinessObject>();
                            //this is for sqlite DB
                            //if (!Program.dbInitilized)
                            //{
                            //    messageBo.InitializeDbSchema();
                            //    Program.dbInitilized = true;
                            //}
                            var model = JsonConvert.DeserializeObject<MessageModel>(messageText);
                            messageBo.TransferMessage(model);
                            data = new List<byte>();
                        }
                    }
                }
            }
            catch (WebSocketException ex)
            {
                Debug.WriteLine($"WebSocketException happened :{ex.Message}");
                throw;
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"IOException happened :{ex.Message}");
                throw;
            }
            catch (PlatformNotSupportedException ex)
            {
                Debug.WriteLine($"PlatformNotSupportedException happened :{ex.Message}");
                throw;
            }
            catch (Exception ex) {
                Debug.WriteLine($"Unexpected exception happened :{ex.Message}");
                throw;
            }
        }
    }
}
