using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Connector.Client;

public class AuthenticationHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // For now, just pass through since auth is handled by OAuth2CodeFlow
        return base.SendAsync(request, cancellationToken);
    }
} 