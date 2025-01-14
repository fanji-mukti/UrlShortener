namespace UrlShortener.Function.Configurations
{
    using Autofac;
    using EnsureThat;
    using UrlShortener.Core.Services;
    using UrlShortener.Core.Utilities;

    /// <summary>
    /// Represents a module for registering URL shortener service dependencies in the Autofac container.
    /// </summary>
    internal sealed class UrlShortenerServiceModule : Module
    {
        private readonly UrlShortenerServiceConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlShortenerServiceModule"/> class with the specified URL shortener service configuration.
        /// </summary>
        /// <param name="configuration">The configuration settings for the URL shortener service.</param>
        public UrlShortenerServiceModule(UrlShortenerServiceConfiguration configuration)
        {
            _configuration = EnsureArg.IsNotNull(configuration, nameof(configuration));
        }

        /// <summary>
        /// Adds registrations to the Autofac container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<DefaultTimeProvider>()
                .As<ITimeProvider>()
                .SingleInstance();

            builder
                .Register(context => new SnowflakeIdGenerator(_configuration.DataCenterId, _configuration.WorkerId, context.Resolve<ITimeProvider>()))
                .As<IIdGenerator>()
                .SingleInstance();

            builder
                .RegisterType<CustomBaseEncoder>()
                .As<IEncoder>()
                .SingleInstance();

            builder
                .RegisterType<UrlShortenerService>()
                .As<IUrlShortenerService>()
                .InstancePerDependency();
        }
    }
}
