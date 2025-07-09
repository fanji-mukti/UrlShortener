namespace Function.Tests.Modules
{
    using Autofac;
    using AutoMapper;
    using UrlShortener.Function.MappingProfiles;

    internal sealed class MappingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c =>
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile(new ShortenedUrlProfile());
                    });

                    return config.CreateMapper();
                })
                .SingleInstance();
        }
    }
}
