using Autofac;
using P72CommonLib.Utilities;
using P72OilTest.Configuration;
using P72OilTest.Domain.Entities;
using P72OilTest.Services;


namespace P72OilTest.Application
{
    internal class P72OilTestModule : IAutofacModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<OilDrillService>().AsImplementedInterfaces();
            builder.RegisterType<OilServiceConfig>().AsImplementedInterfaces();
            builder.RegisterType<WellManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DrillFactory>().AsImplementedInterfaces();
            builder.RegisterType<DrillManager>().AsImplementedInterfaces().SingleInstance();

        }
    }
}
