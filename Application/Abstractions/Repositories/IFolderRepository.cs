using Application.Specifications;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IFolderRepository : IRepository<Folder, long>
    {
        Task<(List<Folder>, int)> GetPagedAsync(ISpecification<Folder> spec, int page, int size);
    }
}