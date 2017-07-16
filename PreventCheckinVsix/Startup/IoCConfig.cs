using Autofac;
using Autofac.Extras.CommonServiceLocator;

using Microsoft.Practices.ServiceLocation;

namespace PreventCheckinVsix.Startup
{
    public class IoCConfig
    {
        public static IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            IoCRegisterDependencies.RegisterOtherDependencies(builder);

            var container = builder.Build();

            SetServiceLocator(container);

            return container;
        }

        private static void SetServiceLocator(IContainer container)
        {
            // Set the service locator to an AutofacServiceLocator.
            var serviceLocator = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }
    }
}