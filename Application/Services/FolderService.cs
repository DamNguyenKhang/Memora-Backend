using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.DTOs.Request.Folder;
using Application.DTOs.Response.Folder;
using Application.Exceptions;
using Application.Specifications;
using AutoMapper;
using Domain.Entities;
using ApplicationException = Application.Exceptions.ApplicationException;

namespace Application.Services
{
    public class FolderService(IFolderRepository folderRepository, IMapper mapper, ICurrentUserService currentUser) : IFolderService
    {
        public async Task CreateFolderAsync(CreateFolderRequest request)
        {
            var userId = currentUser.UserId ?? throw new ApplicationException(ErrorCode.USER_NOT_FOUND);
            var folder = new Folder
            {
                Name = request.Name,
                UserId = userId,
            };
            await folderRepository.AddAsync(folder);
        }

        public async Task<GetListFolderResponse> GetFolderByUserId(long userId, GetListFolderRequest request)
        {
            var spec = new FolderSpecification();
            spec.WithUser(userId);
            spec.NotDeleted();

            var (items, totalItems) = await folderRepository.GetPagedAsync(spec, request.Page, request.Size);
            return new GetListFolderResponse()
            {
                Page = request.Page,
                Size = request.Size,
                TotalItems = totalItems,
                Folders = mapper.Map<IEnumerable<FolderResponse>>(items)
            };
        }

        public async Task<FolderResponse?> GetFolderByIdAsync(long folderId)
        {
            var deck = await folderRepository.GetByIdAsync(folderId, d => d.Decks, d => d.User) ?? throw new ApplicationException(ErrorCode.DECK_NOT_FOUND);
            var folderResponse = mapper.Map<FolderResponse>(deck);
            return folderResponse;
        }
    }
}