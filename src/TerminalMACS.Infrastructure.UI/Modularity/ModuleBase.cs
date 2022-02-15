using Prism.Ioc;
using Prism.Modularity;
using Unity;

namespace TerminalMACS.Infrastructure.UI.Modularity;

public abstract class ModuleBase : IModule
{
    protected ModuleBase(IUnityContainer container)
    {
        Container = container;
    }

    protected IUnityContainer Container { get; }

    public virtual void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    public virtual void OnInitialized(IContainerProvider containerProvider)
    {
    }
}