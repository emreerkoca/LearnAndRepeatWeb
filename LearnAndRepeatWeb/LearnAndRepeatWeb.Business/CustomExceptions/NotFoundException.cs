using System;

namespace LearnAndRepeatWeb.Business.CustomExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message, Exception inner = null) : base(message, inner)
        {
        }
    }
}
