namespace CustomHttpServer.Models
{
    using System.Collections.Generic;
    using System.Text;
    using CustomHttpServer.Enums;

    public class Header
    {
        public Header(HeaderType type)
        {
            this.Type = type;
            this.ContentType = "text/html";
            //this.ContentLength = ?;
            this.OtherParameters = new Dictionary<string, string>();
            this.Cookies = new CookieCollection();
        }

        public HeaderType Type { get; set; }

        public string ContentType { get; set; }

        public string ContentLength { get; set; }

        public IDictionary<string, string> OtherParameters { get; set; }

        public ICookieCollection Cookies { get; private set; }

        public void AddCookie(Cookie cookie)
        {
            this.Cookies.AddCookie(cookie);
        }

        public override string ToString()
        {
            StringBuilder header = new StringBuilder();
            header.AppendLine("Content-type: " + this.ContentType);
            if (this.Cookies.Count > 0)
            {
                if (this.Type == HeaderType.HttpRequest)
                {
                    header.AppendLine("Cookie: " + this.Cookies.ToString());
                }
                else if (this.Type == HeaderType.HttpResponse)
                {
                    foreach (var cookie in this.Cookies)
                    {
                        header.AppendLine("Set-Cookie: " + cookie);
                    }
                }
            }

            if (this.ContentLength != null)
            {
                header.AppendLine("Content-Lenght: " + this.ContentLength);
            }

            foreach (var otherParameter in OtherParameters)
            {
                header.AppendLine($"{otherParameter.Key}: {otherParameter.Value}");
            }

            header.AppendLine();

            return header.ToString();
        }
    }
}
