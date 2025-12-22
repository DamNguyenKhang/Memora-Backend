using Microsoft.AspNetCore.Http;

namespace Application.Exceptions
{
    public enum ErrorCode
    {
        USER_NOT_FOUND,
        INVALID_CREDENTIALS,
        UNAUTHORIZED,
        INTERNAL_ERROR,
        INVALID_REFRESH_TOKEN
    }

    public record ErrorDetail(int StatusCode, string Message);

    public static class ErrorDetails
    {
        public static readonly Dictionary<ErrorCode, ErrorDetail> Map = new()
    {
        { ErrorCode.USER_NOT_FOUND, new ErrorDetail(StatusCodes.Status400BadRequest, "User not found") },
        { ErrorCode.INVALID_CREDENTIALS, new ErrorDetail(StatusCodes.Status401Unauthorized, "Invalid credentials") },
        { ErrorCode.UNAUTHORIZED, new ErrorDetail(StatusCodes.Status403Forbidden, "Unauthorized access") },
        { ErrorCode.INTERNAL_ERROR, new ErrorDetail(StatusCodes.Status500InternalServerError, "Internal server error") },
        { ErrorCode.INVALID_REFRESH_TOKEN, new ErrorDetail(StatusCodes.Status500InternalServerError, "Invalid refresh token")}
    };

        public static ErrorDetail Get(ErrorCode code) => Map[code];
    }
}
