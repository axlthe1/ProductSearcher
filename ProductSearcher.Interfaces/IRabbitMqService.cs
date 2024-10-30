using ProductSearcher.Interfaces;

namespace ProductSearcher.Interfaces;

public interface IRabbitMqService
{
    string RequestQueue { get; }
    bool Configured { get; }
    Task ConnectExternalSearcher(IExternalRepository repository);
}