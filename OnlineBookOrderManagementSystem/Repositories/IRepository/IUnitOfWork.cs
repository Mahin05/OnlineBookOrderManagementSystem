namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryReposiory Category { get; }
        void Save();
    }
}
