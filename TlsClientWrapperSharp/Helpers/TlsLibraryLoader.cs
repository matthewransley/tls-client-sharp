using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace TlsClientWrapperSharp.Helpers;

public static class TlsLibraryLoader
{
    private const string RepoOwner = "bogdanfinn";
    private const string RepoName = "tls-client";
    
    private static readonly string TempFolder = Path.Combine(Path.GetTempPath(), "TlsClientWrapperSharp");
    
    public const string LibraryName = "tls-client";

    private static bool _resolverRegistered = false;
    private static readonly object _lock = new();

    public static async Task EnsureLibraryExistsAsync(bool forceUpdate = false)
    {
        var libraryFileName = GetLibraryFileName();
        var destinationPath = Path.Combine(TempFolder, libraryFileName);
        
        if (!Directory.Exists(TempFolder))
        {
            Directory.CreateDirectory(TempFolder);
        }
        
        if (forceUpdate || !File.Exists(destinationPath))
        {
            try
            {
                Console.WriteLine("Checking for latest tls-client release...");
                
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("TlsClientWrapperSharp");

                var releaseUrl = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";
                var releaseJson = await httpClient.GetStringAsync(releaseUrl);
                
                using var document = JsonDocument.Parse(releaseJson);
                var assets = document.RootElement.GetProperty("assets");

                string? downloadUrl = null;
                var assetNamePattern = GetAssetNamePattern();

                foreach (var asset in assets.EnumerateArray())
                {
                    var name = asset.GetProperty("name").GetString();
                    if (name != null && name.Contains(assetNamePattern) && (name.EndsWith(".dll") || name.EndsWith(".so") || name.EndsWith(".dylib")))
                    {
                        downloadUrl = asset.GetProperty("browser_download_url").GetString();
                        Console.WriteLine($"Found matching asset: {name}");
                        break;
                    }
                }

                if (string.IsNullOrEmpty(downloadUrl))
                {
                    throw new Exception($"No matching asset found for pattern '{assetNamePattern}' in the latest release.");
                }

                Console.WriteLine($"Downloading {downloadUrl}...");
                var libraryBytes = await httpClient.GetByteArrayAsync(downloadUrl);
                
                await File.WriteAllBytesAsync(destinationPath, libraryBytes);
                Console.WriteLine($"Successfully downloaded tls-client library to {destinationPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download tls-client library: {ex.Message}");
                throw;
            }
        }

        RegisterDllImportResolver(destinationPath);
    }

    private static void RegisterDllImportResolver(string libraryPath)
    {
        lock (_lock)
        {
            if (_resolverRegistered) return;

            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), (libraryName, assembly, searchPath) =>
            {
                if (libraryName == LibraryName)
                {
                    return NativeLibrary.Load(libraryPath);
                }
                
                return IntPtr.Zero;
            });
            
            _resolverRegistered = true;
        }
    }

    private static string GetLibraryFileName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return $"{LibraryName}.dll";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return $"lib{LibraryName}.so";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return $"lib{LibraryName}.dylib";
        
        throw new PlatformNotSupportedException("Unsupported operating system.");
    }

    private static string GetAssetNamePattern()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Environment.Is64BitProcess ? "windows-64" : "windows-32";
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "linux-arm64" : "linux-ubuntu-amd64";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
             return RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "darwin-arm64" : "darwin-amd64";
        }

        throw new PlatformNotSupportedException("Unsupported operating system.");
    }
}