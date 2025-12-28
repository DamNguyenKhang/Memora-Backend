using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Services;
using Application.DTOs.Response;
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
    }
}
