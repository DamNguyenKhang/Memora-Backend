namespace Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public ErrorCode ErrorCode { get; }
        public ErrorDetail Detail { get; }

        public ApplicationException(ErrorCode errorCode)
            : base(ErrorDetails.Get(errorCode).Message)
        {
            ErrorCode = errorCode;
            Detail = ErrorDetails.Get(errorCode);
        }

        public ApplicationException(ErrorCode errorCode, string customMessage)
            : base(customMessage)
        {
            ErrorCode = errorCode;
            Detail = new ErrorDetail(ErrorDetails.Get(errorCode).StatusCode, customMessage);
        }
    }
}
