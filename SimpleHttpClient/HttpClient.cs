namespace SimpleHttpClient
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class HttpClient
    {
        public HttpResponse Get(
            string route, 
            string hostNameOrAddress, 
            int port, 
            string host,
            string userName,
            string password)
        {
            var request = CreateHttpRequest(route, host, userName, password);
            var response = GetResponse(hostNameOrAddress, port, request);
            return ParseHttpResponse(response);
        }

        private static string GetResponse(string hostNameOrAddress, int port, string request)
        {
            Socket socket = null;
            string response;

            try
            {
                var ipAddress = ParseIPAddress(hostNameOrAddress);
                var ip = CreateEndPoint(ipAddress, port);
                socket = CreateSocket(ip.AddressFamily);
                socket.Connect(ip);

                var httpRequestBytes = GetBytes(request);

                socket.Send(httpRequestBytes, httpRequestBytes.Length, 0);
                response = GetHttpResponse(socket);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }

            return response;
        }

        private static string CreateHttpRequest(
            string route, 
            string host, 
            string userName, 
            string password)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("GET {0} ", route);
            sb.AppendFormat("HTTP/1.1{0}", Environment.NewLine);
            sb.AppendFormat("Host: {0}{1}", host, Environment.NewLine);

            if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(password))
            {
                var authInfo = GetAuthHeader(userName, password);
                sb.AppendFormat("Authorization: {0}{1}", authInfo, Environment.NewLine);
            }

            sb.AppendFormat("Connection: Close{0}", Environment.NewLine);
            sb.AppendFormat("{0}", Environment.NewLine);
            var request = sb.ToString();
            return request;
        }

        private static string GetAuthHeader(string userName, string password)
        {
            var authInfo = string.Format("{0}:{1}", userName, password);
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            authInfo = string.Format("Basic {0}", authInfo);
            return authInfo;
        }

        private static string GetHttpResponse(Socket socket)
        {
            var responseBuffer = new byte[1024];
            var response = string.Empty;

            var bytes = socket.Receive(responseBuffer, responseBuffer.Length, 0);
            response = response + GetString(responseBuffer, bytes);

            while (bytes > 0)
            {
                bytes = socket.Receive(responseBuffer, responseBuffer.Length, 0);
                response = response + GetString(responseBuffer, bytes);
            }

            return response;
        }

        private static HttpResponse ParseHttpResponse(string response)
        {
            var httpResponse = new HttpResponse { Raw = response };

            if (string.IsNullOrEmpty(response))
            {
                return httpResponse;
            }

            response = response.Replace("\r", string.Empty);
            var lines = response.Split(new[] { '\n' });
            if (lines == null || lines.Length == 0)
            {
                return httpResponse;
            }

            var firstLine = lines[0];
            var tokens = firstLine.Split(' ');
            if (tokens.Length >= 2)
            {
                httpResponse.Status = int.Parse(tokens[1]);
            }

            var firstBlankLine = -1;
            for (var i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    firstBlankLine = i;
                    break;
                }
            }

            if (firstBlankLine < 0 || firstBlankLine >= lines.Length - 1)
            {
                return httpResponse;
            }

            httpResponse.Content = lines[firstBlankLine + 1];
            return httpResponse;
        }

        private static IPAddress ParseIPAddress(string hostNameOrAddress)
        {
            var hostEntry = Dns.GetHostEntry(hostNameOrAddress);
            if (hostEntry == null || hostEntry.AddressList.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName");
            }

            return hostEntry.AddressList[0];
        }

        private static Socket CreateSocket(AddressFamily addressFamily)
        {
            return new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        private static IPEndPoint CreateEndPoint(IPAddress address, int port)
        {
            return new IPEndPoint(address, port);
        }

        private static string GetString(byte[] receiveByte, int bytes)
        {
            return Encoding.ASCII.GetString(receiveByte, 0, bytes);
        }

        private static byte[] GetBytes(string request)
        {
            return Encoding.ASCII.GetBytes(request);
        }
    }
}
