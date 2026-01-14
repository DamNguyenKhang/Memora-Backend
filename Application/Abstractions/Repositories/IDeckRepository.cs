using Application.Specifications;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IDeckRepository : IRepository<Deck, long>
    {
        Task<(List<Deck>, int)> GetPagedAsync(ISpecification<Deck> spec, int page,int size);
    }
}