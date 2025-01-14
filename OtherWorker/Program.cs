﻿// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using TheBigGuyWorker.ExternalSourceRepositories;

namespace TheBigGuyWorker
{
    using Microsoft.Extensions.Hosting;
    using ProductSearcher.BasicWorker;
    using ProductSearcher.Interfaces;
    using ProductSearcher.Models;

    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<RabbitMqConfiguration>();
                    services.AddSingleton<IRabbitMqService, RabbitMqService>();
                    services.AddSingleton<IExternalRepository, OtherRepository>();
                    services.AddAutoMapper(typeof(Program));
                })
                .Build();
            await host.RunAsync();
        }
    }
}