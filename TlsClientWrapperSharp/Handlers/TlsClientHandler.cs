using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using TlsClientWrapperSharp.Helpers;
using TlsClientWrapperSharp.Models;
using TlsClientWrapperSharp.Services;

namespace TlsClientWrapperSharp.Handlers
{
    /// <summary>
    /// Custom HttpClientHandler that uses a TLS client DLL to send HTTP requests.
    /// </summary>
    public partial class TlsClientHandler : DelegatingHandler
    {
        /// <summary>
        /// Gets or sets the proxy for the HTTP requests.
        /// </summary>
        public WebProxy? Proxy { get; set; }

        /// <summary>
        /// Gets or sets the session ID for the requests.
        /// </summary>
        public string SessionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the TLS client identifier.
        /// </summary>
        public string TlsClientIdentifier { get; set; } = "chrome_133";

        /// <summary>
        /// Sends an HTTP request asynchronously using the custom TLS client.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The HTTP response message.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri == null) throw new NullReferenceException("RequestUri cannot be null");

            var httpHeaders = (HttpHeaders)request.Headers;
            var headersDictionary = httpHeaders.NonValidated.ToDictionary(header => header.Key, header => header.Value.ToString());
            
            if (request.Content?.Headers != null)
                foreach (var header in request.Content.Headers)
                {
                    headersDictionary.Add(header.Key, header.Value.FirstOrDefault() ?? throw new InvalidOperationException());
                }

            var requestContent = request.Content != null ? await request.Content.ReadAsStringAsync(cancellationToken) : string.Empty;

            var sessionPayload = new TlsRequestMessage
            {
                SessionId = SessionId,
                TlsClientIdentifier = TlsClientIdentifier,
                ProxyUrl = Proxy == null ? string.Empty : ProxyHelper.ConvertWebProxyToString(Proxy),
                RequestMethod = request.Method.Method,
                RequestUrl = request.RequestUri.ToString(),
                Headers = headersDictionary,
                RequestBody = requestContent
            };

            var requestJson = JsonSerializer.Serialize(sessionPayload);
            var requestBytes = Encoding.UTF8.GetBytes(requestJson);

            var responsePtr = await TlsClientWrapper.SendRequest(requestBytes, SessionId);
            var responseJson = Marshal.PtrToStringAnsi(responsePtr) ?? throw new Exception("Failed to get response");

            var result = JsonSerializer.Deserialize<TlsResponseResponse>(responseJson) ?? throw new Exception("Failed to parse response");
            await TlsClientWrapper.ReleaseMemory(Encoding.UTF8.GetBytes(result.Id));

            var responseContent = Convert.FromBase64String(result.Body.Split("base64,")[1]);
            var responseStream = new MemoryStream(responseContent);

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(responseStream),
                StatusCode = (HttpStatusCode)result.Status,
            };

            foreach (var header in result.Headers)
            {
                httpResponseMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.FirstOrDefault());
            }

            return httpResponseMessage;
        }
    }
}
