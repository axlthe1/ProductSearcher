using ProductSearcher.Models.Internal;

namespace ProductSearcher;

public interface IRabbitMqExchangeService
{
    void Connect();
    Task<IEnumerable<TheTourGuyModel>> SearchExternalProducts(string supplier, ProductFilter filter);
}