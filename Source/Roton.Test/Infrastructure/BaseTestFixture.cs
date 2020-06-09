using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AutoFixture;
using AutoFixture.Dsl;
using Moq;

namespace Roton.Test.Infrastructure
{
    public abstract class BaseTestFixture
    {
        private Lazy<Fixture> Fixture { get; } = new Lazy<Fixture>(() => new Fixture());

        [DebuggerStepThrough]
        protected Mock<T> Mock<T>(Action<Mock<T>> setup) where T : class
        {
            var mock = new Mock<T>();
            setup(mock);
            return mock;
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
                throw new Exception($"Resource is missing: {fullPath}");
            return result;
        }

        protected byte[] GetResourceFile(string path)
        {
            using var resource = GetResource(path);
            using var reader = new BinaryReader(resource);
            return reader.ReadBytes((int) resource.Length);
        }
    }
}