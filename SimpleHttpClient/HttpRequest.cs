namespace SimpleHttpClient
{
    using System.Collections.Generic;
    using System.Text;

    public class HttpRequest
    {
        public int Port = 80;
        public string Domain;
        public string Method = "GET";
        public string Path;

        public Dictionary<string, string> Headers = new Dictionary<string, string>();

        public override string ToString()
        {
            return RawHttpRequestBuilder.Create(this);
        }

        public byte[] GetBytes()
        {
            return Encoding.ASCII.GetBytes(this.ToString());
        }

        public void AddHeader(string name, string value)
        {
            Headers.Add(name, value);
        }
    }
}
