
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace GuidGenerator
{
    public class GetGuid
    {
        private readonly ILogger _logger;

        public GetGuid(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetGuid>();
        }

        [Function("GetGuid")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("Started the GetGuid function call");

            string? numberOfGuidsText = req.Query["count"];

            List<string> guids = new();


            if (numberOfGuidsText is not null && int.TryParse(numberOfGuidsText, out int numberOfGuids))
            {
                _logger.LogInformation($"Number of guids requested: {numberOfGuids}");
            }
            else
            {
                _logger.LogInformation($"Unknown number of guids requested. Using 1.");
                numberOfGuids = 1;

            }

            for (int i = 0; i < numberOfGuids; i++)
            {
                guids.Add(Guid.NewGuid().ToString());
            }

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(guids);



            return response;
        }
    }
}
