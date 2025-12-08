using MercenariesAndBeasts.Web.Services;

public class HttpInterceptorHandler : DelegatingHandler
{
    private readonly ErrorService _errors;

    public HttpInterceptorHandler(ErrorService errors)
    {
        _errors = errors;
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
                _errors.RaiseError(
                    $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}",
                    content);
            }

            return response;
        }
        catch (Exception ex)
        {
            _errors.RaiseError("Network error while calling API.", ex); // teď sedí na overload
            throw;
        }
    }
}