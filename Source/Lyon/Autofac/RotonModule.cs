using System.Linq;
using Autofac;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Lyon.Autofac;

public sealed class RotonModule : Module
{
    private readonly Context _context;

    public RotonModule(Context context)
    {
        _context = context;
    }
        
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        var cmsf = new ContextMetadataServiceFactory();

        builder.Register(_ => cmsf.Get(_context))
            .As<IContextMetadataService>()
            .AutoActivate()
            .SingleInstance();
            
        builder.RegisterTypes(cmsf.Get(Context.Startup).GetTypes().ToArray())
            .AsImplementedInterfaces()
            .AutoActivate()
            .SingleInstance();
            
        builder.RegisterTypes(cmsf.Get(_context).GetTypes().ToArray())
            .AsImplementedInterfaces()
            .AutoActivate()
            .SingleInstance();
    }
}