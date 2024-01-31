namespace Sales.Domain.Common;

public record Error
{
    public Error(string reason, object? content)
    {
        Reason = reason;
        Content = content;
    }

    public string Reason { get; }

    public object? Content { get; }

    public static Error CreateError(string reason, object? content)
    {
        return new(reason, content);
    }
}
