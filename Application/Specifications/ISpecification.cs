using System.Linq.Expressions;

namespace Application.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
    }
}