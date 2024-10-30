using ProductSearcher.Models.Internal;

namespace ProductSearcher;

public interface IProductRepository
{
    void LoadProducts();
    Task<IEnumerable<TheTourGuyModel>> SearchProducts(ProductFilter filter);
}