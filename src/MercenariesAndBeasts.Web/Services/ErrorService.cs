using System;

namespace MercenariesAndBeasts.Web.Services
{
    public class ErrorService   // nebo AlertService, název si nech svůj
{
    public event Action? OnAlertsChanged;

    private readonly List<AlertMessage> _alerts = new();

    public IReadOnlyList<AlertMessage> Alerts => _alerts;

    public void Success(string msg) => Add(msg, AlertLevel.Success);
    public void Info(string msg)    => Add(msg, AlertLevel.Info);
    public void Warning(string msg) => Add(msg, AlertLevel.Warning);
    public void Error(string msg)   => Add(msg, AlertLevel.Error);

    private void Add(string msg, AlertLevel level)
    {
        _alerts.Add(new AlertMessage
        {
            Level = level,
            Message = msg
        });

        OnAlertsChanged?.Invoke();
    }

    public void Dismiss(Guid id)
    {
        var i = _alerts.FindIndex(x => x.Id == id);
        if (i >= 0)
        {
            _alerts.RemoveAt(i);
            OnAlertsChanged?.Invoke();
        }
    }
    public event Action<string, string?>? OnError;

    /// <summary>
    /// Vyvolá globální chybu – používá se z HttpInterceptoru, catch bloků atd.
    /// </summary>
    public void RaiseError(string message, string? details = null)
    {
        OnError?.Invoke(message, details);
    }

    // overload pro Exception, aby fungovaly tvé původní volání RaiseError("msg", ex)
    public void RaiseError(string message, Exception ex)
    {
        OnError?.Invoke(message, ex.ToString());
    }
}
public enum AlertLevel
{
    Info,
    Success,
    Warning,
    Error
}
public sealed class AlertMessage
{
    public Guid Id {get;set;}
    public AlertLevel Level { get; set; }

    public string Message { get; set; } = string.Empty;

    public string? Title { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
}