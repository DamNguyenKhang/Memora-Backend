using Application.Abstractions.Services;
using Application.DTOs.Request.Folder;
using Application.DTOs.Response;
using Application.DTOs.Response.Folder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FolderController(IFolderService folderService) : ControllerBase
    {
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse>> CreateUser([FromBody] CreateFolderRequest request)
        {
            await folderService.CreateFolderAsync(request);
            return new ApiResponse
            {
                Message = "Create new folder successfully"
            };
        }

        [Authorize]
        [HttpGet("get-folders/{userId}")]
        public async Task<ActionResult<ApiResponse<GetListFolderResponse>>> GetFolderByUserId(long userId, [FromQuery] GetListFolderRequest request)
        {
            var response = await folderService.GetFolderByUserId(userId, request);
            return new ApiResponse<GetListFolderResponse>
            {
                Result = response,
                Message = "Get list folders successfully"
            };
        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult<ApiResponse<FolderResponse>>> GetFolderById(long Id)
        {
            var response = await folderService.GetFolderByIdAsync(Id);
            return new ApiResponse<FolderResponse>
            {
                Result = response,
                Message = "Get folder detail successfully"
            };
        }
    }
}