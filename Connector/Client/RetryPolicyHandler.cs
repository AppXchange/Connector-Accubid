using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Connector.Client;

/// <summary>
/// Handles automatic retries for failed HTTP requests
/// </summary>
public class RetryPolicyHandler : DelegatingHandler
{
    private readonly ILogger<RetryPolicyHandler> _logger;
    private readonly ConnectorRegistrationConfig _config;

    public RetryPolicyHandler(
        ConnectorRegistrationConfig config,
        ILogger<RetryPolicyHandler> logger)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(
            "Request to {Method} {Url}", 
            request.Method, 
            request.RequestUri);

        for (int i = 0; i <= _config.ApiClient.MaxRetries; i++)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
                    if (i > 0)
                    {
                        _logger.LogInformation(
                            "Request succeeded after {Attempt} attempts",
                            i + 1);
                    }
                    return response;
                }

                if (i == _config.ApiClient.MaxRetries)
                {
                    _logger.LogError(
                        "Request failed permanently after {MaxAttempts} attempts with status code {StatusCode}",
                        _config.ApiClient.MaxRetries + 1,
                        response.StatusCode);
                    return response;
                }

                _logger.LogWarning(
                    "Request failed with status code {StatusCode}. Attempt {Attempt} of {MaxAttempts}",
                    response.StatusCode,
                    i + 1,
                    _config.ApiClient.MaxRetries + 1);

                await DelayWithJitter(i, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                if (i == _config.ApiClient.MaxRetries)
                {
                    _logger.LogError(
                        ex,
                        "Request failed permanently after {MaxAttempts} attempts",
                        _config.ApiClient.MaxRetries + 1);
                    throw;
                }

                _logger.LogWarning(
                    ex,
                    "Request failed. Attempt {Attempt} of {MaxAttempts}",
                    i + 1,
                    _config.ApiClient.MaxRetries + 1);

                await DelayWithJitter(i, cancellationToken);
            }
        }

        throw new InvalidOperationException("Should not reach this point");
    }

    private static async Task DelayWithJitter(int attempt, CancellationToken cancellationToken)
    {
        // Exponential backoff with jitter to prevent thundering herd
        var baseDelay = Math.Pow(2, attempt);
        var jitter = Random.Shared.NextDouble() * 0.5 + 0.5; // 50-100% of base delay
        var delay = TimeSpan.FromSeconds(baseDelay * jitter);
        await Task.Delay(delay, cancellationToken);
    }
} 