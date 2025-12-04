namespace TlsClientWrapperSharp.Models
{
    /// <summary>
    /// Represents the message used for a TLS request.
    /// </summary>
    public class TlsRequestMessage
    {
        /// <summary>
        /// Gets or sets the TLS client identifier. Default is Chrome 133.
        /// </summary>
        public string TlsClientIdentifier { get; set; } = ClientIdentifier.Chrome133;

        /// <summary>
        /// Gets or sets a value indicating whether to follow redirects. Default is true.
        /// </summary>
        public bool FollowRedirects { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to skip server certificate validation. Default is true.
        /// </summary>
        public bool InsecureSkipVerify { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to send the request without a cookie jar. Default is false.
        /// </summary>
        public bool WithoutCookieJar { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to use the default cookie jar. Default is false.
        /// </summary>
        public bool WithDefaultCookieJar { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the request is a byte request. Default is false.
        /// </summary>
        public bool IsByteRequest { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to force HTTP/1. Default is false.
        /// </summary>
        public bool ForceHttp1 { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the response is a byte response. Default is true.
        /// </summary>
        public bool IsByteResponse { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to enable debug information. Default is false.
        /// </summary>
        public bool WithDebug { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to catch panics. Default is false.
        /// </summary>
        public bool CatchPanics { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to use random TLS extension order. Default is true.
        /// </summary>
        public bool WithRandomTlsExtensionOrder { get; set; } = true;

        /// <summary>
        /// Gets or sets the session ID. Default is "Nada".
        /// </summary>
        public string SessionId { get; set; } = "Nada";

        /// <summary>
        /// Gets or sets the timeout in seconds. Default is 100 seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 100;

        /// <summary>
        /// Gets or sets the timeout in milliseconds. Default is 0 milliseconds.
        /// </summary>
        public int TimeoutMilliseconds { get; set; } = 0;

        /// <summary>
        /// Gets or sets the certificate pinning hosts.
        /// </summary>
        public Dictionary<string, string> CertificatePinningHosts { get; set; } = new();

        /// <summary>
        /// Gets or sets the proxy URL.
        /// </summary>
        public string ProxyUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether to use a rotating proxy. Default is false.
        /// </summary>
        public bool IsRotatingProxy { get; set; } = false;

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new();

        /// <summary>
        /// Gets or sets the order of the headers.
        /// </summary>
        public List<string> HeaderOrder { get; set; } = null!;

        /// <summary>
        /// Gets or sets the request URL.
        /// </summary>
        public string RequestUrl { get; set; } = null!;

        /// <summary>
        /// Gets or sets the request method (GET, POST, etc.).
        /// </summary>
        public string RequestMethod { get; set; } = null!;

        /// <summary>
        /// Gets or sets the request body.
        /// </summary>
        public string RequestBody { get; set; } = null!;

        /// <summary>
        /// Gets or sets the request cookies.
        /// </summary>
        public List<TlsCookie> RequestCookies { get; set; } = new();
    }

    /// <summary>
    /// Represents a cookie used in a TLS request.
    /// </summary>
    public class TlsCookie
    {
        /// <summary>
        /// Gets or sets the cookie name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the cookie value.
        /// </summary>
        public string Value { get; set; } = null!;
    }
}
