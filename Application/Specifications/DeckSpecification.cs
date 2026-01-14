using Domain.Entities;

namespace Application.Specifications
{
    public class DeckSpecification : BaseSpecification<Deck>
    {
        public void WithOwner(long ownerId)
        {
            Add(d => d.OwnerId == ownerId);
        }

        // public void WithKeyword(string keyword)
        // {
        //     Add(d => d.Name.Contains(keyword));
        // }

        public void WithPublic(bool isPublic)
        {
            Add(d => d.IsPublic == isPublic);
        }

        public void NotDeleted()
        {
            Add(d => !d.IsDeleted);
        }

    }
}