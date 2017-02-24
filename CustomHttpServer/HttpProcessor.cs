// ReSharper disable MemberCanBeMadeStatic.Local

namespace CustomHttpServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using CustomHttpServer.Enums;
    using CustomHttpServer.Models;
    using CustomHttpServer.RouteHandlers;
    using CustomHttpServer.Utilities;

    public class HttpProcessor
    {
        private readonly IList<Route> routes;
        private HttpRequest request;
        private HttpResponse response;

        public HttpProcessor(IEnumerable<Route> routes)
        {
            this.routes = new List<Route>(DefaultRoutes.AppendDefaultRoutes(routes));
        }

        public void HandleClient(TcpClient tcpClient)
        {
            using (var stream = tcpClient.GetStream())
            {
                this.request = this.GetRequest(stream);
                this.response = this.RouteRequest();
                Console.WriteLine("-RESPONSE-------------");
                Console.WriteLine(this.response.Header);
                //Console.WriteLine(Encoding.UTF8.GetString(this.response.Content));
                Console.WriteLine("----------------------");
                StreamUtils.WriteResponse(stream, this.response);
            }
        }

        private HttpRequest GetRequest(Stream inputStream)
        {
            // 1. Read request line
            string requestLine = StreamUtils.ReadLine(inputStream);
            string[] tokens = requestLine.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != 3)
            {
                throw new Exception("Invalid http request line");
            }

            var method = (RequestMethod) Enum.Parse(typeof(RequestMethod), tokens[0].ToUpper());
            string url = tokens[1];
            //string protocolVersion = tokens[2];

            Header header = this.ReadHeader(inputStream);

            var content = this.ReadContent(inputStream, header);

            var resultRequest = new HttpRequest()
            {
                Method = method,
                Url = url,
                Header = header,
                Content = content
            };

            Console.WriteLine("-REQUEST-----------------------------");
            Console.WriteLine(resultRequest);
            Console.WriteLine("-------------------------------------");

            return resultRequest;
        }

        private Header ReadHeader(Stream inputStream)
        {
            var header = new Header(HeaderType.HttpRequest);
            string line;
            while ((line = StreamUtils.ReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    break;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("Invalid http header line: " + line);
                }

                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++;
                }

                var name = line.Substring(0, separator);
                var value = line.Substring(pos, line.Length - pos);

                if (name.ToLower() == "cookie")
                {
                    string[] cookieSaves = value.Split(';');
                    foreach (var cookieSave in cookieSaves)
                    {
                        string[] cookiePair = cookieSave.Split('=').Select(pp => pp.Trim()).ToArray();
                        var cookie = new Cookie(cookiePair[0], cookiePair[1]);
                        header.AddCookie(cookie);
                    }
                }
                else if (name.ToLower() == "content-length")
                {
                    header.ContentLength = value;
                }
                else
                {
                    header.OtherParameters.Add(name, value);
                }
            }

            return header;
        }

        private string ReadContent(Stream inputStream, Header header)
        {
            if (header.ContentLength == null) return null;

            int totalBytes = Convert.ToInt32(header.ContentLength);
            int bytesLeft = totalBytes;
            byte[] bytes = new byte[totalBytes];

            while (bytesLeft > 0)
            {
                byte[] buffer = new byte[bytesLeft > 1024 ? 1024 : bytesLeft];
                int n = inputStream.Read(buffer, 0, buffer.Length);
                buffer.CopyTo(bytes, totalBytes - bytesLeft);

                bytesLeft -= n;
            }

            string content = Encoding.ASCII.GetString(bytes);
            return content;
        }

        private HttpResponse RouteRequest()
        {
            var matchedRoutes = this.routes
                .Where(r => Regex.Match(this.request.Url, r.UrlRegex).Success)
                .ToList();

            if (!matchedRoutes.Any())
            {
                return HttpResponseBuilder.NotFound();
            }

            var route = matchedRoutes.FirstOrDefault(r => r.Method == this.request.Method);

            if (route == null)
            {
                return new HttpResponse()
                {
                    StatusCode = ResponseStatusCode.MethodNotAllowed
                };
            }
            
            try
            {
                return route.Callable(this.request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return HttpResponseBuilder.InternalServerError();
            }
        }
    }
}
