using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    }
}