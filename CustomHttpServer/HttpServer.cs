namespace CustomHttpServer
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using CustomHttpServer.Models;

    public class HttpServer
    {
        public HttpServer(int port, IEnumerable<Route> routes)
        {
            this.Port = port;
            this.Processor = new HttpProcessor(routes);
            this.IsActive = true;
        }

        public TcpListener Listener { get; private set; }

        public int Port { get; }

        public HttpProcessor Processor { get; }

        public bool IsActive { get; }

        public void Listen()
        {
            this.Listener = new TcpListener(IPAddress.Any, this.Port);
            this.Listener.Start();
            while (this.IsActive)
            {
                TcpClient client = this.Listener.AcceptTcpClient();
                Thread thread = new Thread(() =>
                {
                    this.Processor.HandleClient(client);
                });
                thread.Start();
                Thread.Sleep(1);
            }
        }
    }
}
