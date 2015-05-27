namespace SimpleHttpClient
{
    using System;
    using System.Text;

    public class HttpRequestBuilder
    {
        StringBuilder m_builder = new StringBuilder();

        public void AddBlankLine()
        {
            this.m_builder.AppendFormat("{0}", Environment.NewLine);
        }

        public void AddRequestLine(string method, string route, string version)
        {
            this.m_builder.AppendFormat("{0} {1} {2}{3}", method, route, version, Environment.NewLine);
        }

        public void AddHeader(string name, string value)
        {
            this.m_builder.AppendFormat("{0}: {1}{2}", name, value, Environment.NewLine);
        }

        public override string ToString()
        {
            return this.m_builder.ToString();
        }
    }
}
