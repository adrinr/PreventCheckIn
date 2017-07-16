using Autofac;

using PreventCheckin.Configurations.Implementations;
using PreventCheckin.Configurations.Interfaces;
using PreventCheckin.Services.Implementations;
using PreventCheckin.Services.Interfaces;

namespace PreventCheckin.IoC
{
    public class IoCModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PreventCheckinService>().As<IPreventCheckinService>();
            builder.RegisterType<PreventCheckinConfiguration>().As<IPreventCheckinConfiguration>();
        }
    }
}