using Domain.Entities;

namespace Application.Specifications
{
    public class DeckSpecification : BaseSpecification<Deck>
    {
        public void WithOwner(long ownerId)
        {
            Add(d => d.OwnerId == ownerId);
        }

        public void WithFolder(long folderId)
        {
            Add(d => d.FolderId == folderId);
        }

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