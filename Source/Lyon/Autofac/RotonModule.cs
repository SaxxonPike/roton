using System.Linq;
using System.Reflection;
using Autofac;
using Roton;
using Module = Autofac.Module;

namespace Lyon.Autofac;

public sealed class RotonModule(Context context, params Assembly[] additionalAssemblies) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        // Each concrete type must have all its services registered at the same time
        // so that AutoFac knows that they all refer to the same instance.

        var map = RotonServices.Get(context, additionalAssemblies)
            .GroupBy(s => s.Implementation);

        foreach (var serviceGroup in map)
            builder.RegisterType(serviceGroup.Key)
                .As([.. serviceGroup.Select(sg => sg.Service)])
                .AutoActivate()
                .SingleInstance();
    }
}