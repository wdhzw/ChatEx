using ChatEx.Models.ReqModels;
using System.Text.Json;
using System.Text;

namespace ChatEx.Services
{
    public class StreamParserService
    {
        public StreamParserService()
        {
            
        }

        public async IAsyncEnumerable<StreamingResponse> ParseEventStreamAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            var eventData = new StringBuilder();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                {
                    if (eventData.Length > 0)
                    {
                        var response = ProcessEventData(eventData.ToString());
                        if (response != null)
                            yield return response;

                        eventData.Clear();
                    }
                    continue;
                }

                if (line.StartsWith("data: "))
                {
                    var data = line["data: ".Length..];
                    if (data == "[DONE]") 
                        break;

                    eventData.AppendLine(data);
                }
            }
        }

        private StreamingResponse? ProcessEventData(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<StreamingResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                return null;
            }
        }
    }
}
