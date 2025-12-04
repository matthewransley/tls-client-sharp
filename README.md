# TLS Client Wrapper Sharp

[![NuGet Version](https://img.shields.io/nuget/v/tls-client-sharp.svg?style=flat-square)](https://www.nuget.org/packages/tls-client-sharp/1.0.0)
[![NuGet Downloads](https://img.shields.io/nuget/dt/tls-client-sharp.svg?style=flat-square)](https://www.nuget.org/packages/tls-client-sharp/1.0.0)
[![License](https://img.shields.io/github/license/matthewransley/tls-client-sharp.svg?style=flat-square)](https://github.com/matthewransley/tls-client-sharp/blob/main/LICENSE.txt)

A C# wrapper for `bogdanfinn/tls-client` that provides a custom `HttpClientHandler`. This allows you to make HTTP requests that mimic the TLS fingerprints of popular browsers, helping to bypass anti-bot protections.

## Installation

The wrapper automatically checks for and downloads the required `tls-client` library from GitHub Releases upon the first run. It stores the library in the user's temporary folder to ensure portability. You do **not** need to manually download the DLL.

### Via .NET CLI
```bash
dotnet add package tls-client-sharp
```

### Via Package Manager Console (Visual Studio)
```powershell
Install-Package tls-client-sharp
```

### Via PackageReference
Add the following to your .csproj file:
```xml
<ItemGroup>
  <PackageReference Include="tls-client-sharp" Version="1.0.0" />
</ItemGroup>
```

## Usage

```csharp
using TlsClientWrapperSharp.Handlers;
using TlsClientWrapperSharp.Helpers;
using TlsClientWrapperSharp.Models;

// 1. Ensure the library is downloaded (checks for updates or missing file)
await TlsLibraryLoader.EnsureLibraryExistsAsync();

// 2. Initialize the handler with a specific browser identifier
var tlsClientHandler = new TlsClientHandler
{
    TlsClientIdentifier = ClientIdentifier.Chrome133
};

// 3. Create a standard HttpClient using the handler
var httpClient = new HttpClient(tlsClientHandler);

// (Optional) Add headers as needed
httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-GB,en;q=0.9");

// 4. Make requests
var response = await httpClient.GetStringAsync("https://tls.browserleaks.com/tls");
Console.WriteLine(response);
```

## Features

-   **Automatic Library Management:** Automatically downloads the correct `tls-client` binary for your OS (Windows, Linux, macOS).
-   **TLS Fingerprint Spoofing:** Mimic Chrome, Firefox, Safari, and Opera to blend in with normal traffic.
-   **Standard HttpClient Integration:** Works seamlessly with existing C# codebases by extending `DelegatingHandler`.
-   **Proxy Support:** Configure proxies directly on the handler.
