using System.Linq.Expressions;

namespace StudentTrackingSystem.Models
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProps = null);
        T Get(Expression<Func<T, bool>> filtre, string? includeProps = null);
        void Add(T entitiy);
        void Delete(T entitiy);
        void DeleteRange(IEnumerable<T> entities);

    }
}
