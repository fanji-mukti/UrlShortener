namespace Function.Tests.Modules
{
    using Autofac;
    using UrlShortener.Function;

    internal sealed class FunctionTriggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<UrlShorternerHttpTrigger>();

            builder
                .RegisterType<CosmosDbTrigger>();
        }
    }
}
