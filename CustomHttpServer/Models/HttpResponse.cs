namespace CustomHttpServer.Models
{
    using System;
    using System.Text;
    using CustomHttpServer.Enums;

    /// <summary>
    /// NOTE: two consequences of this simplified response model are:
    /// (a) it's not possible to send 8-bit clean responses (like file content)
    /// (b) it must be loaded into memory in the the Content property. If you want to send large files,
    /// this has to be reworked so a handler can write to the output stream instead.
    /// </summary>
    public class HttpResponse
    {
        public HttpResponse()
        {
            this.Header = new Header(HeaderType.HttpResponse);
            this.Content = new byte[0];
        }

        public ResponseStatusCode StatusCode { get; set; }

        public byte[] Content { get; set; }

        public Header Header { get; set; }

        public string StatusMessage => Enum.GetName(typeof(ResponseStatusCode), this.StatusCode);

        public string ContentAsUtf8
        {
            set { this.Content = Encoding.UTF8.GetBytes(value); }
        }

        public override string ToString()
        {
            return $"HTTP/1.0 {(int)this.StatusCode} {this.StatusMessage}\r\n{this.Header}";
        }
    }
}
