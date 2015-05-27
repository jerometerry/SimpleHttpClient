namespace SimpleHttpClient
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public static class IpResolver
    {
        public static IPAddress Resolve(string hostNameOrAddress)
        {
            var hostEntry = Dns.GetHostEntry(hostNameOrAddress);
            if (hostEntry == null || hostEntry.AddressList.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName");
            }

            foreach (var addr in hostEntry.AddressList)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    return addr;
                }
            }

            throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName");
        }
    }
}
