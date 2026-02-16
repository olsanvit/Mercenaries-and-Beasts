using Microsoft.Extensions.Logging;

namespace MercenariesAndBeasts.Infrastructure;
public interface IErrorService
{
    void Capture(Exception ex, string message, object? data = null);
}

public sealed class LogErrorService : IErrorService
{
    private readonly ILogger<LogErrorService> _logger;
    public LogErrorService(ILogger<LogErrorService> logger) => _logger = logger;

    public void Capture(Exception ex, string message, object? data = null)
        => _logger.LogError(ex, "{Message} | {@Data}", message, data);
}