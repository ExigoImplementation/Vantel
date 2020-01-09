using System;

namespace Common.Exceptions
{
    public class WebAliasRequiredException : Exception
    {
        public WebAliasRequiredException()
        {

        }
        public WebAliasRequiredException(string message) 
            : base(message)
        {
        }

        public WebAliasRequiredException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}