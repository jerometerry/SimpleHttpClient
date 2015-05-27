namespace SimpleHttpClient
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    public class HttpClient
    {
        private int m_connectionTimeout;
        private int m_sendTimeout;
        private int m_recieveTimeout;

        public HttpClient()
            : this(60000, 60000, 60000)
        {
        }

        public HttpClient(int connectionTimeout, int sendTimeout, int receiveTimeout)
        {
            m_connectionTimeout = connectionTimeout;
            m_sendTimeout = sendTimeout;
            m_recieveTimeout = receiveTimeout;
        }

        public HttpResponse Execute(HttpRequest request)
        {
            Socket socket = null;
            
            try
            {
                socket = CreateSocket();
                Connect(socket, request);
                Send(socket, request);
                return Receive(socket);               
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
        }

        private Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                SendTimeout = m_sendTimeout,
                ReceiveTimeout = m_recieveTimeout
            };
        }

        private void Connect(Socket socket, HttpRequest request)
        {
            var ipAddress = IpResolver.Resolve(request.Domain);
            var result = socket.BeginConnect(ipAddress, request.Port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(m_connectionTimeout, true);
            if (!success)
            {
                throw new ApplicationException("Failed to connect server.");
            }
        }

        private static void Send(Socket socket, HttpRequest request)
        {
            var httpRequestBytes = request.GetBytes();
            socket.Send(httpRequestBytes, httpRequestBytes.Length, 0);
        }

        private static HttpResponse Receive(Socket socket)
        {
            var responseBuffer = new byte[1024];
            var sb = new StringBuilder();
            int bytesRead;

            do
            {
                bytesRead = socket.Receive(responseBuffer, responseBuffer.Length, 0);
                var data = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
                sb.Append(data);
            }
            while (bytesRead > 0);

            var response = sb.ToString();
            return HttpResponse.Parse(response);
        }
    }
}