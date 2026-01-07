using Microsoft.AspNetCore.Http;

namespace Application.Exceptions
{
    public enum ErrorCode
    {
        USER_NOT_FOUND,
        INVALID_CREDENTIALS,
        UNAUTHORIZED,
        INTERNAL_ERROR,
        INVALID_REFRESH_TOKEN,
        REFRESH_TOKEN_NOT_FOUND,
        EMAIL_VERIFICATION_TOKEN_NOT_FOUND,
        USED_EMAIL_VERIFICATION_TOKEN,
        EMAIL_VERIFICATION_TOKEN_EXPIRED,
        EMAIL_NOT_MATCH,
        EMAIL_NOT_VERIFY,
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
        { ErrorCode.INVALID_REFRESH_TOKEN, new ErrorDetail(StatusCodes.Status403Forbidden, "Invalid refresh token")},
        { ErrorCode.REFRESH_TOKEN_NOT_FOUND, new ErrorDetail(StatusCodes.Status404NotFound, "Refresh token not found")},
        { ErrorCode.EMAIL_VERIFICATION_TOKEN_NOT_FOUND, new ErrorDetail(StatusCodes.Status404NotFound, "Email verification token not found")},
        { ErrorCode.USED_EMAIL_VERIFICATION_TOKEN, new ErrorDetail(StatusCodes.Status409Conflict, "Email verification token has been used")},
        { ErrorCode.EMAIL_VERIFICATION_TOKEN_EXPIRED, new ErrorDetail(StatusCodes.Status410Gone, "Email verification token has expired")},
        { ErrorCode.EMAIL_NOT_MATCH, new ErrorDetail(StatusCodes.Status400BadRequest, "The verification email does not match the registered email")},
        { ErrorCode.EMAIL_NOT_VERIFY, new ErrorDetail(StatusCodes.Status403Forbidden, "Email hasn't been verified")},
    };

        public static ErrorDetail Get(ErrorCode code) => Map[code];
    }
}
