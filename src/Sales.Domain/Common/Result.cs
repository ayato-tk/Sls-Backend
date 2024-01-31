using System;
using System.Threading.Tasks;

namespace Sales.Domain.Common;

public class Result<T>
{
    public Result(T value)
    {
        IsSuccess = true;

        Value = value;
        Error = null;
    }

    public Result(Error error)
    {
        IsSuccess = false;

        Value = default;
        Error = error;
    }

    public T? Value { get; }

    public Error? Error { get; }

    public bool IsSuccess { get; }

    public bool IsFaulted => !IsSuccess;

    public R Match<R>(Func<T, R> onSuccess, Func<Error, R> onFailed)
    {
        return IsSuccess
            ? onSuccess(Value!)
            : onFailed(Error!);
    }

    public T IfFail(T defaultValue)
    {
        return IsSuccess
            ? Value!
            : defaultValue;
    }

    public T IfFail(Func<Error, T> func)
    {
        return IsFaulted
            ? func(Error!)
            : Value!;
    }

    public Result<T> IfFail(Action<Error> func)
    {
        if (IsFaulted)
        {
            func(Error!);
        }

        return this;
    }

    public Result<T> IfSuccess(Action<T> func)
    {
        if (IsSuccess)
            func(Value!);

        return this;
    }

    public Result<B> Map<B>(Func<T, B> func)
    {
        return IsFaulted
            ? new Result<B>(Error!)
            : new Result<B>(func(Value!));
    }

    public async Task<Result<B>> MapAsync<B>(Func<T, Task<B>> func)
    {
        return IsFaulted
            ? new Result<B>(Error!)
            : new Result<B>(await func(Value!));
    }

    public static implicit operator Result<T>(T value)
        => new(value);

    public static implicit operator Result<T>(Error error)
        => new(error);
}
