using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Linq;

namespace Connector.Client;

/// <summary>
/// Configuration for the API client
/// </summary>
public class ApiClientConfig
{
    /// <summary>
    /// Maximum number of retry attempts for failed requests
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Base timeout for requests in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}

/// <summary>
/// Client for making HTTP requests to the Accubid API
/// </summary>
public class ApiClient
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the ApiClient
    /// </summary>
    /// <param name="httpClient">The HTTP client to use for requests</param>
    /// <param name="baseUrl">Base URL for the API</param>
    public ApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    /// <summary>
    /// Tests the API connection using the oauth/me endpoint
    /// </summary>
    public async Task<ApiResponse> TestConnection(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync("oauth/me", cancellationToken)
            .ConfigureAwait(false);

        return new ApiResponse
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            RawResult = await response.Content.ReadAsStreamAsync(cancellationToken)
        };
    }

    #region Private Utilities

    /// <summary>
    /// Builds a query string from a dictionary of parameters
    /// </summary>
    private static string BuildQueryString(Dictionary<string, string>? parameters)
    {
        if (parameters?.Any() != true)
        {
            return string.Empty;
        }

        var queryString = HttpUtility.ParseQueryString(string.Empty);
        foreach (var param in parameters)
        {
            queryString[param.Key] = param.Value;
        }

        return queryString.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Gets a paginated list of records from the specified endpoint
    /// </summary>
    public async Task<ApiResponse<PaginatedResponse<T>>> GetRecords<T>(
        string relativeUrl, 
        int page,
        Dictionary<string, string>? additionalParams = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "page", page.ToString() }
        };

        if (additionalParams != null)
        {
            foreach (var param in additionalParams)
            {
                queryParams[param.Key] = param.Value;
            }
        }

        var queryString = BuildQueryString(queryParams);
        var fullUrl = $"{relativeUrl}?{queryString}";

        return await GetAsync<PaginatedResponse<T>>(fullUrl, cancellationToken);
    }

    #endregion

    #region Base HTTP Methods

    /// <summary>
    /// Makes a GET request to the specified endpoint
    /// </summary>
    public async Task<ApiResponse<T>> GetAsync<T>(
        string relativeUrl,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .GetAsync(relativeUrl, cancellationToken)
            .ConfigureAwait(false);

        var apiResponse = new ApiResponse<T>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            apiResponse.Data = await response.Content
                .ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            apiResponse.RawResult = await response.Content
                .ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        return apiResponse;
    }

    /// <summary>
    /// Makes a POST request to the specified endpoint
    /// </summary>
    public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(
        string relativeUrl,
        TRequest content,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient
            .PostAsJsonAsync(relativeUrl, content, cancellationToken)
            .ConfigureAwait(false);

        var apiResponse = new ApiResponse<TResponse>
        {
            IsSuccessful = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode
        };

        if (response.IsSuccessStatusCode)
        {
            apiResponse.Data = await response.Content
                .ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            apiResponse.RawResult = await response.Content
                .ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        return apiResponse;
    }

    #endregion

    #region Change Order Endpoints

    /// <summary>
    /// Gets contract cost distribution information
    /// </summary>
    public Task<ApiResponse<List<T>>> GetContractCostDistribution<T>(
        string databaseToken,
        string contractId,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>(
            $"anywhere/changeorder/v1/ContractCostDistribution/{databaseToken}/{contractId}", 
            cancellationToken);

    /// <summary>
    /// Gets a list of contracts
    /// </summary>
    public Task<ApiResponse<List<T>>> GetContracts<T>(
        string databaseToken,
        string projectId,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>(
            $"anywhere/changeorder/v1/Contracts/{databaseToken}/{projectId}", 
            cancellationToken);

    /// <summary>
    /// Gets a PCO by ID
    /// </summary>
    public Task<ApiResponse<T>> GetPCO<T>(
        string databaseToken,
        string pcoId,
        CancellationToken cancellationToken = default)
        => GetAsync<T>(
            $"anywhere/changeorder/v1/PCO/{databaseToken}/{pcoId}", 
            cancellationToken);

    /// <summary>
    /// Gets a list of PCOs
    /// </summary>
    public Task<ApiResponse<List<T>>> GetPCOs<T>(
        string databaseToken,
        string contractId,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>(
            $"anywhere/changeorder/v1/PCOs/{databaseToken}/{contractId}", 
            cancellationToken);

    /// <summary>
    /// Gets contract quote labels for a specific contract
    /// </summary>
    public Task<ApiResponse<List<T>>> GetContractQuoteLabels<T>(
        string databaseToken,
        string contractId,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>(
            $"anywhere/changeorder/v1/ContractQuoteLabels/{databaseToken}/{contractId}", 
            cancellationToken);

    /// <summary>
    /// Gets contract statuses for a specific contract
    /// </summary>
    public Task<ApiResponse<List<T>>> GetContractStatuses<T>(
        string databaseToken,
        string contractId,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>(
            $"anywhere/changeorder/v1/ContractStatuses/{databaseToken}/{contractId}", 
            cancellationToken);

    /// <summary>
    /// Gets contract subcontract labels for a specific contract
    /// </summary>
    public Task<ApiResponse<List<T>>> GetContractSubcontractLabels<T>(
        string databaseToken,
        string contractId,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>(
            $"anywhere/changeorder/v1/ContractSubcontractLabels/{databaseToken}/{contractId}", 
            cancellationToken);

    #endregion

    #region Closeout Endpoints

    /// <summary>
    /// Gets final price details for a bid summary
    /// </summary>
    public Task<ApiResponse<T>> GetFinalPrice<T>(
        string databaseToken,
        string bidSummaryId,
        CancellationToken cancellationToken = default)
        => GetAsync<T>(
            $"anywhere/closeout/v1/FinalPrice/{databaseToken}/{bidSummaryId}", 
            cancellationToken);

    #endregion

    #region Database Endpoints

    /// <summary>
    /// Gets a list of all accessible databases
    /// </summary>
    public Task<ApiResponse<List<T>>> GetDatabases<T>(CancellationToken cancellationToken = default)
        => GetAsync<List<T>>("anywhere/database/v1/Databases", cancellationToken);

    #endregion

    #region Estimate Endpoints

    /// <summary>
    /// Gets a list of estimates for a project
    /// </summary>
    public Task<ApiResponse<List<T>>> GetEstimates<T>(
        string databaseToken,
        string projectId,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>(
            $"anywhere/estimate/v1/Estimates/{databaseToken}/{projectId}", 
            cancellationToken);

    /// <summary>
    /// Gets details for a specific estimate
    /// </summary>
    public Task<ApiResponse<T>> GetEstimate<T>(
        string databaseToken,
        string estimateId,
        CancellationToken cancellationToken = default)
        => GetAsync<T>(
            $"anywhere/estimate/v1/Estimate/{databaseToken}/{estimateId}", 
            cancellationToken);

    /// <summary>
    /// Gets estimates by due date
    /// </summary>
    public Task<ApiResponse<List<T>>> GetEstimatesByDueDate<T>(
        string databaseToken,
        string startDate,
        string endDate,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>(
            $"anywhere/estimate/v1/EstimatesByDueDate/{databaseToken}/{startDate}/{endDate}", 
            cancellationToken);

    /// <summary>
    /// Gets extension item details file URL via SignalR
    /// </summary>
    public Task<ApiResponse<T>> GetExtensionItemDetailsFileSignalR<T>(
        string databaseToken,
        string estimateId,
        string connectionId,
        string? bidSummaryId = null,
        CancellationToken cancellationToken = default)
    {
        var endpoint = string.IsNullOrEmpty(bidSummaryId)
            ? $"anywhere/estimate/v1/ExtensionItemDetailsFileSignalR/{databaseToken}/{estimateId}/{connectionId}"
            : $"anywhere/estimate/v1/ExtensionItemDetailsFileSignalR/{databaseToken}/{estimateId}/{bidSummaryId}/{connectionId}";

        return GetAsync<T>(endpoint, cancellationToken);
    }

    /// <summary>
    /// Tests SignalR notifications with a given connection ID
    /// </summary>
    public Task<ApiResponse<T>> GetNotificationTest<T>(
        string connectionId,
        CancellationToken cancellationToken = default)
        => GetAsync<T>(
            $"anywhere/estimate/v1/NotificationTest/{connectionId}", 
            cancellationToken);

    #endregion

    #region Project Endpoints

    /// <summary>
    /// Gets a list of all projects for a database
    /// </summary>
    public Task<ApiResponse<List<T>>> GetProjects<T>(
        string databaseToken,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>($"anywhere/project/v1/Projects/{databaseToken}", cancellationToken);

    /// <summary>
    /// Gets details for a specific project
    /// </summary>
    public Task<ApiResponse<T>> GetProject<T>(
        string databaseToken,
        string projectId,
        CancellationToken cancellationToken = default)
        => GetAsync<T>($"anywhere/project/v1/Project/{databaseToken}/{projectId}", cancellationToken);

    /// <summary>
    /// Gets recently accessed projects for a database
    /// </summary>
    public Task<ApiResponse<List<T>>> GetLastProjects<T>(
        string databaseToken,
        CancellationToken cancellationToken = default)
        => GetAsync<List<T>>(
            $"anywhere/project/v1/LastProjects/{databaseToken}", 
            cancellationToken);

    #endregion
}