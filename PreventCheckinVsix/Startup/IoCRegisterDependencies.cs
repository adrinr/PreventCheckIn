using System.Collections.Generic;

using Autofac;

namespace PreventCheckinVsix.Startup
{
    public class IoCRegisterDependencies
    {
        public static void RegisterOtherDependencies(ContainerBuilder builder)
        {
            var modules = new List<Module>
            {
                new PreventCheckin.IoC.IoCModule(),
            };

            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }
        }
    }
}