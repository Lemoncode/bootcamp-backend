using Polly;

using System.Net;
using System.Text.Json;

namespace AutoDialer.Infrastructure;

public class PhoneCallingAgent : IPhoneCallingAgent
{

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly string BaseApiUrl = "https://myfakeapi/v1/";

    public PhoneCallingAgent(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Guid> MakeCall(string phoneNumber, Stream audioMessageInMp3)
    {

        var retryPolicy = Policy
    .HandleResult<HttpResponseMessage>(r => r.StatusCode >= HttpStatusCode.InternalServerError)
    .WaitAndRetryAsync(10, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // Exponential back-off.
    );

        var httpClient = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await retryPolicy.ExecuteAsync(() =>
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(BaseApiUrl + "makeCall"),
                Content = new StringContent(JsonSerializer.Serialize(new { phoneNumber = phoneNumber, speech = this.ConvertStreamToBase64Async(audioMessageInMp3) }))
            };

            return httpClient.SendAsync(requestMessage);
        });
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return Guid.Parse(content);
    }

    public async Task<bool> CallIsComplete(Guid jobId)
    {
        var retryPolicy = Policy
    .HandleResult<HttpResponseMessage>(r => r.StatusCode >= HttpStatusCode.InternalServerError)
    .WaitAndRetryAsync(10, retryAttempt =>
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // Exponential back-off.
    );

        var httpClient = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await retryPolicy.ExecuteAsync(() =>
        {
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(BaseApiUrl + $"checkCallStatus?jobId={jobId}"),
            };

            return httpClient.SendAsync(requestMessage);
        });
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return bool.Parse(content);
    }

    private async Task<string> ConvertStreamToBase64Async(Stream input)
    {
        if (input.CanSeek)
        {
            input.Seek(0, SeekOrigin.Begin);
        }

        using (var memoryStream = new MemoryStream())
        {
            await input.CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();

            return Convert.ToBase64String(bytes);
        }
    }

}
