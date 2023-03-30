﻿namespace shop.bll.Exceptions
{
    public class ValidationErrorException : Exception
    {
        public ValidationErrorException() { }
        public ValidationErrorException(string message) : base(message) { }
        public ValidationErrorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
