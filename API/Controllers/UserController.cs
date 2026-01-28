using System.Security.Claims;
using Application.Abstractions.Services;
using Application.DTOs.Response;
using Application.DTOs.Response.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("get-all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserResponse>>>> GetAllUsers()
        {
            return new ApiResponse<IEnumerable<UserResponse>>
            {
                Result = await userService.GetAllAsync(),
                Message = "Get all users successfully"
            };
        }

        [HttpGet("my-info")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetMyInfo()
        {
            return new ApiResponse<UserResponse>
            {
                Result = await userService.GetCurrentUserAsync(),
                Message = "Get my information successfully"
            };
        }
    }
}
