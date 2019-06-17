using Autofac;

namespace P72CommonLib.Utilities
{
    public interface IAutofacModule
    {
        void Register(ContainerBuilder container);
    }
}
