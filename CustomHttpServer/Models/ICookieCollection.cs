namespace CustomHttpServer.Models
{
    using System.Collections.Generic;

    public interface ICookieCollection : IEnumerable<Cookie>
    {
        int Count { get; }

        Cookie this[string cookieName] { get; set; }

        void AddCookie(Cookie cookie);

        void RemoveCookie(string cookieName);

        bool ContainsKey(string cookieName);
    }
}
