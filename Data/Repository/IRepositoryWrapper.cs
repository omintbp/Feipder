namespace Feipder.Data.Repository
{
    public interface IRepositoryWrapper
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ISizeRepository Sizes { get; }
        IColorRepository Colors { get; }
        IProductStorageRepository Storage { get; }

        void Save();
    }
}
