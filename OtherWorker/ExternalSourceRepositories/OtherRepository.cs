﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ProductSearcher.Interfaces;
using ProductSearcher.Models.Extensions;
using ProductSearcher.Models.External;
using ProductSearcher.Models.Internal;


namespace TheBigGuyWorker.ExternalSourceRepositories;

public class OtherRepository : IExternalRepository
{
    private readonly IMapper _mapper;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILogger<OtherRepository> _logger;
    public string SupplierName { get; set; }
    public string Destination { get; set; }
    
    public OtherRepository(IMapper mapper,IRabbitMqService rabbitMqService,ILogger<OtherRepository> logger)
    {
        SupplierName = "Other";
        Destination = "Island";
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

    public List<TheTourGuyModel> InternalData { get; set; }

    public async Task<List<TheTourGuyModel>> LoadProductsAsync()
    {
        try
        {
            InternalData = await Task.Run(() => LoadFromFileProducts("TheBigGuy.json", SupplierName, Destination));
            return InternalData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading products");
        }

        return await Task.FromResult<List<TheTourGuyModel>>(null);
    }
    
    public List<TheTourGuyModel> LoadFromFileProducts(string filePath, string supplierName,string destination)
    {
        
        var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "JsonSource", filePath));
        JObject jsonObject = JObject.Parse(json);
        var products = jsonObject["ProductData"]?.ToObject<List<TheBigGuyProductData>>();
        var remappedProducts = _mapper.Map<List<TheTourGuyModel>>(products);
        // Assign the supplier name and add to the main list
        foreach (var product in remappedProducts)
        {
            product.SupplierName = supplierName;
            product.Destination = destination;
            
        }
        
        return remappedProducts;
    }
}