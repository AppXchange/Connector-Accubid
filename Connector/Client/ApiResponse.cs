using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Connector.Client;

/// <summary>
/// Base response from an API request
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Whether the request was successful (2xx status code)
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// HTTP status code from the response
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Raw response content stream
    /// </summary>
    public Stream? RawResult { get; set; }

    /// <summary>
    /// Error details if the request failed
    /// </summary>
    public ApiErrorDetails? ErrorDetails { get; set; }

    /// <summary>
    /// Checks if the response has a specific status code
    /// </summary>
    public bool HasStatusCode(HttpStatusCode statusCode) => StatusCode == (int)statusCode;

    /// <summary>
    /// Throws an ApiException if the request was not successful
    /// </summary>
    public virtual void EnsureSuccessStatusCode()
    {
        if (!IsSuccessful)
        {
            throw new ApiException(
                ErrorDetails?.Message ?? $"Request failed with status code {StatusCode}",
                (HttpStatusCode)StatusCode,
                RawResult);
        }
    }
}

/// <summary>
/// Typed API response with deserialized data
/// </summary>
/// <typeparam name="TResult">Type of the deserialized response data</typeparam>
public class ApiResponse<TResult> : ApiResponse
{
    /// <summary>
    /// Deserialized response data
    /// </summary>
    public TResult? Data { get; set; }

    /// <summary>
    /// Gets the data or throws if request failed
    /// </summary>
    public TResult GetDataOrThrow()
    {
        EnsureSuccessStatusCode();
        return Data ?? throw new ApiException(
            "Response data was null",
            HttpStatusCode.InternalServerError);
    }

    /// <summary>
    /// Gets the data or returns default value
    /// </summary>
    public TResult GetDataOrDefault(TResult defaultValue)
        => IsSuccessful ? Data ?? defaultValue : defaultValue;

    /// <summary>
    /// Gets enumerable data or empty collection
    /// </summary>
    public IEnumerable<T> GetDataOrEmpty<T>() where T : class
        => (Data as IEnumerable<T>)?.Where(x => x != null) ?? Enumerable.Empty<T>();
}