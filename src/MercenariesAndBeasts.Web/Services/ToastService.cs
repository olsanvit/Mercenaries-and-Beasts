using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MercenariesAndBeasts.Web.Services;

public enum ToastLevel
{
    Success,
    Error,
    Info,
    Warning
}

public record ToastMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public ToastLevel Level { get; }
    public string Title { get; }
    public string? Message { get; }
    public DateTime CreatedUtc { get; } = DateTime.UtcNow;

    // pro fade-out animaci (volitelné)
    public bool IsClosing { get; set; }

    public ToastMessage(ToastLevel level, string title, string? message = null)
    {
        Level = level;
        Title = title;
        Message = message;
    }   
}

public class ToastService
{
    
    private readonly List<ToastMessage> _toasts = new();

    public event Action? OnToastsUpdated;

    public IReadOnlyList<ToastMessage> Toasts => _toasts;

    public void Success(string title, string? message = null)
        => Show(ToastLevel.Success, title, message);

    public void Error(string title, string? message = null)
        => Show(ToastLevel.Error, title, message);

    public void Info(string title, string? message = null)
        => Show(ToastLevel.Info, title, message);

    public void Warning(string title, string? message = null)
        => Show(ToastLevel.Warning, title, message);

    public void Show(ToastLevel level, string title, string? message = null)
    {
        var toast = new ToastMessage(level, title, message);
        _toasts.Add(toast);
        OnToastsUpdated?.Invoke();

        // auto-dismiss po 5s
        _ = AutoRemoveAsync(toast);
    }

    public void Close(Guid id)
    {
        var toast = _toasts.FirstOrDefault(t => t.Id == id);
        if (toast == null)
            return;

        _toasts.Remove(toast);
        OnToastsUpdated?.Invoke();
    }

    private async Task AutoRemoveAsync(ToastMessage toast)
    {
        try
        {
            await Task.Delay(5000);

            toast.IsClosing = true;
            OnToastsUpdated?.Invoke();

            // čas na CSS fade-out (0.25s třeba)
            await Task.Delay(250);

            _toasts.Remove(toast);
            OnToastsUpdated?.Invoke();
        }
        catch
        {
            // ignore
        }
    }
}