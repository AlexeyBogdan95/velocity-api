using System;

namespace Velocity.Api;

public class InvalidModelException: Exception
{
    public InvalidModelException(string message) : base(message){}
}