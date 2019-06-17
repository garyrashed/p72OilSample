
using Autofac;
using P72CommonLib.Entities;
using P72CommonLib.Utilities;

namespace P72CommonLib.Lib
{

    internal class P72CommonLibModule : IAutofacModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<DayCounter>().AsImplementedInterfaces().SingleInstance();
        }
    }
}
