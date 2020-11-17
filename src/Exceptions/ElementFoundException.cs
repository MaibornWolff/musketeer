using System;

public class ElementFoundException : Exception
{
    public ElementFoundException()
    {
    }

    public ElementFoundException(string message) : base(message)
    {
    }

}