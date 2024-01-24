using AutoDialer.Infrastructure;

using Moq;
using Moq.Protected;

using System.Net;

namespace AutoDialer.Tests;

public class PhoneCallingAgentShould
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly PhoneCallingAgent _phoneCallingAgent;

    public PhoneCallingAgentShould()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://myfakeapi/v1/")
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        _phoneCallingAgent = new PhoneCallingAgent(_mockHttpClientFactory.Object);
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task MakeCall_RetryOn5xxResponse(HttpStatusCode httpStatusCode)
    {
        // Configura el mock para devolver un error 5xx en las primeras llamadas y luego un éxito
        _mockHttpMessageHandler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage { StatusCode = httpStatusCode }) // Primera respuesta fallida
            .ReturnsAsync(new HttpResponseMessage { StatusCode = httpStatusCode }) // Segunda respuesta fallida
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(Guid.NewGuid().ToString()) }); // Tercera respuesta exitosa

        // Llama al método que quieres probar
        var result = await _phoneCallingAgent.MakeCall("123456789", new MemoryStream());

        // Verifica que el método SendAsync del handler fue llamado al menos tres veces
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.AtLeast(3),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task CallIsComplete_RetryOn5xxResponse(HttpStatusCode httpStatusCode)
    {
        // Configura el mock para devolver un error 5xx en las primeras llamadas y luego un éxito
        _mockHttpMessageHandler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage { StatusCode = httpStatusCode }) // Primera respuesta fallida
            .ReturnsAsync(new HttpResponseMessage { StatusCode = httpStatusCode }) // Segunda respuesta fallida
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(true.ToString()) }); // Tercera respuesta exitosa

        // Llama al método que quieres probar
        var result = await _phoneCallingAgent.CallIsComplete(Guid.NewGuid());

        // Verifica que el método SendAsync del handler fue llamado al menos tres veces
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.AtLeast(3),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}
