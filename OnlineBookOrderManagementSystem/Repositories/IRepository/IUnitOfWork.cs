namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryReposiory Category { get; }
        IProductReposiory Product { get; }
        ICompanyReposiory Company { get; }
        IShoppingCartReposiory ShoppingCart { get; }
        IOrderHeaderReposiory OrderHeader { get; }
        IOrderDetailReposiory OrderDetail { get; }
        //void Save();
        //Task Save();
        void Save();
    }
}
