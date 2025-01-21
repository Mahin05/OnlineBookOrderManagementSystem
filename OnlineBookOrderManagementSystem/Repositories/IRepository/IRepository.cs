using System.Linq.Expressions;

namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IEnumerable<T> GetAllForDropDown();
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<T> GetDetails(Expression<Func<T, bool>> filter);
        Task Add(T entity);
        Task Update(T entity);
        Task Remove(T entity);
        Task RemoveRange(T entity);
    }
}
