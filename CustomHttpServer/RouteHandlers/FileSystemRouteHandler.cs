namespace CustomHttpServer.RouteHandlers
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using CustomHttpServer.Enums;
    using CustomHttpServer.Models;
    using CustomHttpServer.Utilities;

    public class FileSystemRouteHandler
    {
        public string BasePath { get; set; }

        public string RouteUrlRegex { get; set; }

        public HttpRequest Request { get; set; }

        public HttpResponse Handle(HttpRequest request)
        {
            this.Request = request;
            string urlPart;
            // extract the path if there is one

            var match = Regex.Match(request.Url, this.RouteUrlRegex);
            if (match.Groups.Count > 1)
            {
                urlPart = match.Groups[1].Value;
            }
            else
            {
                urlPart = request.Url;
            }
            //var urlPart = request.Path;

            urlPart = SanitarizePath(urlPart);

            // make sure the first part of the path is not "/" or "\"
            if (urlPart.Length > 0)
            {
                var firstChar = urlPart.ElementAt(0);
                if (firstChar == '/' || firstChar == '\\')
                {
                    urlPart = "." + urlPart;
                }
            }
            var localPath = Path.Combine(this.BasePath, urlPart);

            if (Directory.Exists(localPath))
            {
                return this.HandleDirectory(localPath);
            }
            else if (File.Exists(localPath))
            {
                return HandleFile(localPath);
            }
            else
            {
                return HttpResponseBuilder.NotFound();
            }
        }

        private static string SanitarizePath(string urlPart)
        {
            urlPart = urlPart.Replace("\\..\\", "\\");
            urlPart = urlPart.Replace("/../", "/");
            urlPart = urlPart.Replace("//", "/");
            urlPart = urlPart.Replace(@"\\", @"\");
            urlPart = urlPart.Replace(":", "");
            urlPart = urlPart.Replace("/", Path.DirectorySeparatorChar.ToString());
            return urlPart;
        }

        private static HttpResponse HandleFile(string localPath)
        {
            var fileExtension = Path.GetExtension(localPath);

            var response = new HttpResponse
            {
                Header = {ContentType = QuickMimeTypeMapper.GetMimeType(fileExtension)},
                StatusCode = ResponseStatusCode.OK,
                Content = File.ReadAllBytes(localPath)
            };

            response.Header.ContentLength = response.Content.Length.ToString();
            return response;
        }

        private HttpResponse HandleDirectory(string localPath)
        {
            var output = new StringBuilder();
            output.Append($"<h3> Directory: {this.Request.Url} </h3>");
            output.Append("<ul>");
            var trailPath = localPath.Replace(this.BasePath, "") + "\\";
            foreach (var folder in Directory.GetDirectories(localPath).OrderBy(d => d))
            {
                var dirInfo = new DirectoryInfo(folder);
                output.Append(string.Format("<li><a href=\"{0}{1}\">{1}/</a></li>", trailPath, dirInfo.Name));
            }

            foreach (var entry in Directory.GetFiles(localPath).OrderBy(f => f))
            {
                var fileInfo = new FileInfo(entry);
                output.Append(string.Format("<li><a href=\"{0}{1}\">{1}</a></li>",trailPath , fileInfo.Name));
            }

            output.Append("</ul>");

            return new HttpResponse()
            {
                StatusCode = ResponseStatusCode.OK,
                ContentAsUtf8 = output.ToString(),
            };
        }
    }
}
