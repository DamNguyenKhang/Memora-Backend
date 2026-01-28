using Application.DTOs.Request.Folder;
using Application.DTOs.Response.Folder;

namespace Application.Abstractions.Services
{
    public interface IFolderService
    {
        Task CreateFolderAsync(CreateFolderRequest request);
        Task<GetListFolderResponse> GetFolderByUserId(long userId, GetListFolderRequest request);
        Task<FolderResponse?> GetFolderByIdAsync(long folderId);
    }
}