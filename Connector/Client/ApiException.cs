using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Connector.Client;

/// <summary>
/// Exception thrown when an API request fails
/// </summary>
public class ApiException : Exception
{
    /// <summary>
    /// Creates a new API exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="content">Response content stream</param>
    /// <param name="innerException">Inner exception if any</param>
    public ApiException(
        string message,
        HttpStatusCode statusCode,
        Stream? content = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = (int)statusCode;
        Content = content;
        
        if (content != null)
        {
            try
            {
                using var reader = new StreamReader(content);
                ErrorDetails = JsonSerializer.Deserialize<ApiErrorDetails>(reader.ReadToEnd());
            }
            catch
            {
                // Ignore deserialization errors
            }
        }
    }

    /// <summary>
    /// HTTP status code from the failed request
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Response content stream if available
    /// </summary>
    public Stream? Content { get; }

    /// <summary>
    /// Parsed error details if available
    /// </summary>
    public ApiErrorDetails? ErrorDetails { get; }
}

/// <summary>
/// Structured error details from the API
/// </summary>
public class ApiErrorDetails
{
    /// <summary>
    /// Error code from the API
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Detailed error message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Additional error details
    /// </summary>
    public object? Details { get; set; }
}