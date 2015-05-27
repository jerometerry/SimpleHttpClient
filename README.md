# SimpleHttpClient
Very simple HTTP client built for .NET 2.0

It's surprising how complicated the existing HTTP clients are for .NET (HttpWebRequest, WebClient). All I wanted was to be able to create a simple GET request, and specify the HOST and Authorization headers. 

I could not find a simple library that does this, that works reliable across different servers with different versions of .NET installed.

So I created this basic library that uses Sockets to send and receive HTTP data, and compiled it for .NET 2.0. 
