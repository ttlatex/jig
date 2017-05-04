using System;

public class BusinessError : Exception
{
    public BusinessError()
    {
    }

    public BusinessError(string message)
        : base(message)
    {
    }

    public BusinessError(string message, Exception inner)
        : base(message, inner)
    {
    }
}
