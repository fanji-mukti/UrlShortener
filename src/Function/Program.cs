using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrlShortener.Function.Configurations;
using UrlShortener.Function.Modules;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        var cosmosDbConfig = Configuration.GetCosmosDbConfiguration();
        var urlShortenerServiceConfig = Configuration.GetUrlShortenerServiceConfiguration();

        builder
            .RegisterModule(new RepositoryModule(cosmosDbConfig))
            .RegisterModule(new UrlShortenerServiceModule(urlShortenerServiceConfig));
    })
    .Build();

host.Run();
