namespace CustomHttpServer.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CookieCollection : ICookieCollection
    {
        private readonly Dictionary<string, Cookie> innerMap;

        public CookieCollection()
        {
            this.innerMap = new Dictionary<string, Cookie>();
            this.Count = 0;
        }

        public int Count { get; private set; }

        public Cookie this[string cookieName]
        {
            get { return this.innerMap[cookieName]; }

            set
            {
                if (!this.innerMap.ContainsKey(cookieName))
                {
                    this.innerMap.Add(cookieName, value);
                    this.Count++;
                }

                this.innerMap[cookieName] = value;
            }
        }

        public void AddCookie(Cookie cookie)
        {
            var nameKey = cookie.Name;
            if (!this.innerMap.ContainsKey(nameKey))
            {
                this.innerMap.Add(nameKey, cookie);
                this.Count++;
            }

            this.innerMap[cookie.Name] = cookie;
        }

        public void RemoveCookie(string cookieName)
        {
            if (!this.innerMap.ContainsKey(cookieName))
            {
                throw new ArgumentException("Collection does no contain a pair with this key!");
            }

            this.innerMap.Remove(cookieName);
            this.Count--;
        }

        public bool ContainsKey(string cookieName)
        {
            return this.innerMap.ContainsKey(cookieName);
        }

        public IEnumerator<Cookie> GetEnumerator()
        {
            return this.innerMap.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join("; ", this.innerMap.Values);
        }
    }
}
