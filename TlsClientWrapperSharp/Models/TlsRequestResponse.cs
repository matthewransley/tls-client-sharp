using System.Text.Json.Serialization;

namespace TlsClientWrapperSharp.Models
{
    /// <summary>
    /// Represents the response received from the TLS client.
    /// </summary>
    public class TlsResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the response.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the body of the response, typically encoded in Base64.
        /// </summary>
        [JsonPropertyName("body")]
        public string? Body { get; set; }

        /// <summary>
        /// Gets or sets the cookies returned in the response.
        /// </summary>
        [JsonPropertyName("cookies")]
        public Dictionary<string, string>? Cookies { get; set; }

        /// <summary>
        /// Gets or sets the headers returned in the response.
        /// </summary>
        [JsonPropertyName("headers")]
        public Dictionary<string, List<string>>? Headers { get; set; }

        /// <summary>
        /// Gets or sets the session ID associated with the response.
        /// </summary>
        [JsonPropertyName("sessionId")]
        public string? SessionId { get; set; }

        /// <summary>
        /// Gets or sets the status code of the response.
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the target URL of the response.
        /// </summary>
        [JsonPropertyName("target")]
        public string? Target { get; set; }

        /// <summary>
        /// Gets or sets the protocol used for the response.
        /// </summary>
        [JsonPropertyName("usedProtocol")]
        public string? UsedProtocol { get; set; }
    }
}