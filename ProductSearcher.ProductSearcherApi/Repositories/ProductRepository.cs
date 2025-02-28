﻿using System.Collections;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductSearcher;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ProductSearcher.Interfaces;
using ProductSearcher.Models;
using ProductSearcher.Models.Extensions;
using ProductSearcher.Models.Internal;

namespace ProductSearcherApi.Repositories;



public class ProductRepository : IProductRepository
{
    private readonly RestAPIConfiguration _restApiConfiguration;
    private readonly RabbitMqConfiguration _configuration;
    private readonly IRabbitMqExchangeService _exchangeService;
    private readonly ILogger<ProductRepository> _logger;
    private readonly List<TheTourGuyModel> _products;

    public ProductRepository(RestAPIConfiguration restApiConfiguration, RabbitMqConfiguration configuration,IRabbitMqExchangeService exchangeService,ILogger<ProductRepository> logger)
    {
        _restApiConfiguration = restApiConfiguration;
        _configuration = configuration;
        _exchangeService = exchangeService;
        _logger = logger;
        _products = new List<TheTourGuyModel>();
        LoadProducts();
    }

    public void LoadProducts()
    {
        LoadProductsFromFile("TheTourGuyData.json", "TheTourGuy","Mexico");
    }

    private void LoadProductsFromFile(string filePath, string supplierName,string destination)
    {
        _logger.LogInformation($"Loading products from {filePath} to {destination}");
        var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "JsonSources", filePath));
        JObject jsonObject = JObject.Parse(json);
       
        var products = jsonObject["data"]?.ToObject<List<TheTourGuyModel>>();
        _logger.LogInformation($"Loaded {products.Count} products from {filePath} to {destination}");
        // Assign the supplier name and add to the main list
        foreach (var product in products)
        {
            product.SupplierName = supplierName;
            product.Destination = destination;
            _products.Add(product);
        }
    }

    public async Task<IEnumerable<TheTourGuyModel>> SearchProducts(ProductFilter filter)
    {
        if(filter == null)
            throw new ArgumentNullException(nameof(filter));
        List<TheTourGuyModel> result = null;
        try
        {
            var query = _products.TheTourGuyFilter(filter);

            List<Task<IEnumerable<TheTourGuyModel>>> tasks = new List<Task<IEnumerable<TheTourGuyModel>>>();
            result = query.ToList();
            
            foreach (var supplier in _restApiConfiguration.ExternalSupplier)
            {
                tasks.Add(_exchangeService.SearchExternalProducts(supplier, filter));
                
            }
            var resultExternal = await Task.WhenAll(tasks.ToArray());
            foreach (var singleResult in resultExternal)
            {
                result.AddRange(singleResult);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occured while searching products: {ex.Message}");
        }

        return result;
    }


}