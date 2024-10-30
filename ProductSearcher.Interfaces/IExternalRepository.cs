using ProductSearcher.Models.Internal;

namespace ProductSearcher.Interfaces;

public interface IExternalRepository
{
    string SupplierName { get; set; }
    string Destination { get; set; }
    Task Configure();
    Task<IEnumerable<TheTourGuyModel>> GetExternalProducts(ProductFilter? request);

    Task<List<TheTourGuyModel>> LoadProductsAsync();
}