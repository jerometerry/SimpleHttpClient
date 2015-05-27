namespace SimpleHttpClient
{
    using System;
    using System.Net;

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

            return hostEntry.AddressList[0];
        }
    }
}
