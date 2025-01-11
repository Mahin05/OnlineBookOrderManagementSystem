namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryReposiory Category { get; }
        IProductReposiory Product { get; }
        //void Save();
        Task Save();
    }
}
