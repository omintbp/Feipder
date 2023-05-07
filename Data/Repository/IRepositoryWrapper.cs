namespace Feipder.Data.Repository
{
    public interface IRepositoryWrapper
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ISizeRepository Sizes { get; }
        IProductStorageRepository Storage { get; }

        void Save();
    }
}
