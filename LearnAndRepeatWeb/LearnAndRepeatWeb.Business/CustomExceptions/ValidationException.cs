using System;

namespace LearnAndRepeatWeb.Business.CustomExceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, Exception inner = null) : base(message, inner)
        {
        }
    }
}
