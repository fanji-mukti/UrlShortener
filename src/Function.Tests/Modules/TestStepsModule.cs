namespace Function.Tests.Modules
{
    using Autofac;

    internal class TestStepsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register test steps or any other test-specific services here.
            // This is a placeholder for future test step registrations.
            // Example: builder.RegisterType<MyTestStep>().As<IMyTestStep>();
            builder
                .RegisterType<UrlShorternerHttpTriggerSteps>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CosmosDbTriggerSteps>()
                .InstancePerLifetimeScope();
        }
    }
}
