namespace SimpleHttpClient.Test
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class HttpClientTest
    {
        [Test]
        public void Test()
        {
            var request = CreateRequest();
            var client = new HttpClient(10000, 20000, 20000);
            var response = client.Execute(request);
            Assert.AreEqual(200, response.Status);
        }

        [Test]
        public void Test2()
        {
            var request = CreateRequest2();
            var client = new HttpClient(10000, 20000, 20000);
            var response = client.Execute(request);
            Assert.AreEqual(200, response.Status);
        }

        [Test]
        public void Test_CreateRequest()
        {
            var request = CreateRequest();
            var raw = RawHttpRequestBuilder.Create(request);
            Console.WriteLine(raw);
            Assert.IsNotNullOrEmpty(raw);
        }

        private static HttpRequest CreateRequest()
        {
            var request = new HttpRequest();
            request.Path = "/";
            request.Domain = "www.google.ca";
            request.AddHeader("Host", "www.google.ca");
            request.AddHeader("Connection", "close");
            return request;
        }

        private static HttpRequest CreateRequest2()
        {
            var request = new HttpRequest();
            request.Path = "/";
            request.Domain = "127.0.0.1";
            request.AddHeader("Host", "127.0.0.1");
            request.AddHeader("Connection", "close");
            return request;
        }
    }
}
