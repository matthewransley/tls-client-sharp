using TlsClientWrapperSharp.Handlers;
using TlsClientWrapperSharp.Helpers;
using TlsClientWrapperSharp.Models;

// Ensure the TLS client library is downloaded and available
await TlsLibraryLoader.EnsureLibraryExistsAsync();

var tlsClientHandler = new TlsClientHandler
{
    TlsClientIdentifier = ClientIdentifier.Chrome133
};

var httpClient = new HttpClient(tlsClientHandler);

httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB,en;q=0.9");

var responseContent = await httpClient.GetStringAsync(@"https://tls.browserleaks.com/tls");

Console.WriteLine(responseContent);
