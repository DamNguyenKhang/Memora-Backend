using Domain.Entities;

namespace Application.Specifications
{
    public class FolderSpecification : BaseSpecification<Folder>
    {
        public void WithUser(long userId)
        {
            Add(d => d.UserId == userId);
        }

        // public void WithKeyword(string keyword)
        // {
        //     Add(d => d.Name.Contains(keyword));
        // }

        public void NotDeleted()
        {
            Add(d => !d.IsDeleted);
        }

    }
}