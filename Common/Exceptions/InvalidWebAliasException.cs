using System;

namespace Common.Exceptions
{
    public class InvalidWebAliasException : Exception
    {
        public InvalidWebAliasException()
        {

        }
        public InvalidWebAliasException(string message)
            : base(message)
        {
        }

        public InvalidWebAliasException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}