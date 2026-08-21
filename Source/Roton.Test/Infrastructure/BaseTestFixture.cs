using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Dsl;
using JetBrains.Annotations;
using Moq;
using NUnit.Framework;

namespace Roton.Test.Infrastructure;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[Parallelizable(ParallelScope.All)]
[PublicAPI]
public abstract class BaseTestFixture
{
    private Lazy<IFixture> Fixture { get; } = new(() => new Fixture()
        .Customize(new AutoMoqCustomization()));

    [DebuggerStepThrough]
    protected Mock<T> Freeze<T>(Action<Mock<T>>? setup = null) where T : class
    {
        var mock = Fixture.Value.Freeze<Mock<T>>();
        setup?.Invoke(mock);
        return mock;
    }
    
    [DebuggerStepThrough]
    protected T Inject<T>(T instance)
    {
        Fixture.Value.Inject(instance);
        return instance;
    }

    [DebuggerStepThrough]
    protected T Create<T>() => Fixture.Value.Create<T>();

    [DebuggerStepThrough]
    protected IEnumerable<T> CreateMany<T>() => Fixture.Value.CreateMany<T>();

    [DebuggerStepThrough]
    protected IEnumerable<T> CreateMany<T>(int count) => Fixture.Value.CreateMany<T>(count);

    [DebuggerStepThrough]
    protected ICustomizationComposer<T> Build<T>() => Fixture.Value.Build<T>();

    protected Stream GetResource(string path)
    {
        var assembly = GetType().Assembly;
        var fullPath = $"{assembly.GetName().Name}.Resources.{path}";
        var result = assembly.GetManifestResourceStream(fullPath);
        if (result == null)
            throw new RotonException($"Resource is missing: {fullPath}");
        return result;
    }

    protected byte[] GetResourceFile(string path)
    {
        using var resource = GetResource(path);
        using var reader = new BinaryReader(resource);
        return reader.ReadBytes((int)resource.Length);
    }
}