using SharedServices.Services;

/// <summary>
/// A delegating HTTP handler that intercepts all outgoing requests and surfaces
/// non-success HTTP responses or network errors as application alerts via <see cref="AlertService"/>.
/// Register this handler in the DI container and attach it to named <see cref="System.Net.Http.HttpClient"/> instances.
/// </summary>
public class HttpInterceptorHandler : DelegatingHandler
{
    private readonly AlertService _alerts;

    /// <summary>
    /// Initialises a new instance of <see cref="HttpInterceptorHandler"/> with the required alert service.
    /// </summary>
    /// <param name="alerts">The alert service used to raise error notifications visible to the user.</param>
    public HttpInterceptorHandler(AlertService alerts)
    {
        _alerts = alerts;
    }

    /// <summary>
    /// Sends the HTTP request to the inner handler and raises an alert if the response
    /// indicates failure or if a network exception occurs. The exception is re-thrown after alerting.
    /// </summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the request.</param>
    /// <returns>The <see cref="HttpResponseMessage"/> returned by the inner handler, even when the status code indicates failure.</returns>
    /// <exception cref="Exception">Re-throws any exception raised by the inner handler after surfacing it as an alert.</exception>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                _alerts.RaiseError(
                    $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}",
                    content);
            }

            return response;
        }
        catch (Exception ex)
        {
            _alerts.RaiseError("Network error while calling API.", ex);
            throw;
        }
    }
}
