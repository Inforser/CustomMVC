namespace CustomHttpServer.Utilities
{
    using System;
    using CustomHttpServer.Models;

    public static class SessionCreator
    {
        public static HttpSession Create()
        {
            var sessionId = new Random().Next().ToString();
            var session = new HttpSession(sessionId);
            return session;
        }
    }
}
