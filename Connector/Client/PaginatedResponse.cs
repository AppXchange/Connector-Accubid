using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Connector.Client
{
    /// <summary>
    /// Represents a paginated response from the API
    /// </summary>
    /// <typeparam name="TResult">Type of items in the response</typeparam>
    public class PaginatedResponse<TResult>
    {
        /// <summary>
        /// Current page number (0-based)
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; init; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; init; }

        /// <summary>
        /// Total number of records available
        /// </summary>
        [JsonPropertyName("totalRecords")]
        public int TotalRecords { get; init; }

        /// <summary>
        /// Total number of pages available
        /// </summary>
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; init; }

        /// <summary>
        /// Items in the current page
        /// </summary>
        [JsonPropertyName("items")]
        public IEnumerable<TResult> Items { get; init; } = Array.Empty<TResult>();

        /// <summary>
        /// Whether there are more pages available
        /// </summary>
        public bool HasNextPage => Page < TotalPages - 1;

        /// <summary>
        /// Whether this is the first page
        /// </summary>
        public bool IsFirstPage => Page == 0;

        /// <summary>
        /// Whether this is the last page
        /// </summary>
        public bool IsLastPage => Page == TotalPages - 1;
    }
}