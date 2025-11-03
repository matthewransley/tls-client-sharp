using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TlsClientWrapperSharp.Services;

public partial class TlsClientWrapper
{
    private const string DllPath = "DLLs/tls-client-windows-64-1.11.2.dll";
    
    /// <summary>
    ///     Imports the 'request' function from the TLS client DLL.
    /// </summary>
    /// <param name="requestPayload">The request payload as a byte array.</param>
    /// <param name="sessionId">The session ID.</param>
    /// <returns>A pointer to the response data.</returns>
    [LibraryImport(DllPath, EntryPoint = "request")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial IntPtr Request([In] byte[] requestPayload,
        [MarshalAs(UnmanagedType.LPWStr)] string sessionId);

    /// <summary>
    ///     Imports the 'freeMemory' function from the TLS client DLL.
    /// </summary>
    /// <param name="sessionId">The session ID.</param>
    [LibraryImport(DllPath, EntryPoint = "freeMemory")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial void FreeMemory([In] byte[] sessionId);

    /// <summary>
    ///     Wrapper method to invoke the 'Request' function from the TLS client DLL.
    /// </summary>
    public static async Task<IntPtr> SendRequest(byte[] requestPayload, string sessionId)
    {
        return await Task.Run(() => Request(requestPayload, sessionId));
    }

    /// <summary>
    ///     Wrapper method to invoke the 'FreeMemory' function from the TLS client DLL.
    /// </summary>
    public static async Task ReleaseMemory(byte[] sessionId)
    {
        await Task.Run(() => { FreeMemory(sessionId); });
    }
}