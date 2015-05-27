namespace SimpleHttpClient
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RawHttpRequestBuilder
    {
        private StringBuilder m_builder = new StringBuilder();

        public void AddRequestLine(string method, string route, string version)
        {
            this.m_builder.AppendFormat("{0} {1} {2}{3}", method, route, version, Environment.NewLine);
        }

        public void AddHeader(string name, string value)
        {
            this.m_builder.AppendFormat("{0}: {1}{2}", name, value, Environment.NewLine);
        }

        public void AddBlankLine()
        {
            this.m_builder.AppendFormat("{0}", Environment.NewLine);
        }

        public override string ToString()
        {
            return this.m_builder.ToString();
        }

        public static string Create(HttpRequest request)
        {
            return Create(request.Method, request.Path, request.Headers);
        }

        public static string Create(
            string method,
            string route,
            Dictionary<string, string> headers)
        {
            var rb = new RawHttpRequestBuilder();
            rb.AddRequestLine(method, route, "HTTP/1.1");

            foreach (var key in headers.Keys)
            {
                rb.AddHeader(key, headers[key]);
            }

            rb.AddBlankLine();
            return rb.ToString();
        }
    }
}
