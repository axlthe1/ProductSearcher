using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ProductSearcher.Interfaces;
using ProductSearcher.Models.Extensions;
using ProductSearcher.Models.External;
using ProductSearcher.Models.Internal;

namespace SomeOtherGuyWorker.ExternalSourceRepositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class SomeOtherRepository : IExternalRepository
{
    List<TheTourGuyModel> InternalData { get; set; }
    public string SupplierName { get; set; }
    public string Destination { get; set; }
    
    private readonly IMapper _mapper;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILogger<SomeOtherRepository> _logger;

    public SomeOtherRepository(IMapper mapper,IRabbitMqService rabbitMqService,ILogger<SomeOtherRepository> logger)
    {
        SupplierName = "SomeOtherGuy";
        Destination = "Thailand";
        _mapper = mapper;
        _rabbitMqService = rabbitMqService;
        _logger = logger;
    }
    public async Task Configure()
    {
        await _rabbitMqService.ConnectExternalSearcher(this);
    }



    public async Task<IEnumerable<TheTourGuyModel>> GetExternalProducts(ProductFilter? request)
    {
        if (InternalData == null)
            await LoadProductsAsync();

        return InternalData.TheTourGuyFilter(request);

    }
    
    public List<TheTourGuyModel> LoadFromFileProducts(string filePath, string supplierName,string destination)
    {
        
        var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "JsonSource", filePath));
        JObject jsonObject = JObject.Parse(json);
        var products = jsonObject["Products"]?.ToObject<List<SomeOtherGuyModel>>();
        var remappedProducts = _mapper.Map<List<TheTourGuyModel>>(products);
        // Assign the supplier name and add to the main list
        foreach (var product in remappedProducts)
        {
            product.SupplierName = supplierName;
            product.Destination = destination;
            
        }
        
        return remappedProducts;
    }

    public async Task<List<TheTourGuyModel>> LoadProductsAsync()
    {
        try
        {
            InternalData = await Task.Run(() => LoadFromFileProducts("SomeOtherGuyData.json", SupplierName, Destination));
            return InternalData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading products");
        }

        return await Task.FromResult<List<TheTourGuyModel>>(null);
    }

    
}

