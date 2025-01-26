namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryReposiory Category { get; }
        IProductReposiory Product { get; }
        ICompanyReposiory Company { get; }
        //void Save();
        Task Save();
    }
}
