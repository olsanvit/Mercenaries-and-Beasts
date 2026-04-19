using SharedServices.Services;

public class HttpInterceptorHandler : DelegatingHandler
{
    private readonly AlertService _alerts;

    public HttpInterceptorHandler(AlertService alerts)
    {
        _alerts = alerts;
    }

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
