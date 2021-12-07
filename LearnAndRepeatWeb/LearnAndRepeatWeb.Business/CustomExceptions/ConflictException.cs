using System;

namespace LearnAndRepeatWeb.Business.CustomExceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message, Exception inner = null) : base(message, inner)
        {
        }
    }
}
