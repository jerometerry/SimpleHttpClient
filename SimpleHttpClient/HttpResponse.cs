namespace SimpleHttpClient
{
    public class HttpResponse
    {
        public string Raw { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }

        public static HttpResponse Parse(string response)
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
    }
}
