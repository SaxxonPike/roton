using System;
using System.Diagnostics;
using Autofac.Core;
using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;

namespace Roton.Test.Infrastructure;

[TestFixture]
public abstract class UnitTestFixture : BaseTestFixture
{
    [SetUp]
    [DebuggerStepThrough]
    public void __SetUpContext()
    {
        AutoMock = new Lazy<AutoMock>(Autofac.Extras.Moq.AutoMock.GetStrict);
    }
        
    private Lazy<AutoMock> AutoMock { get; set; }

    [DebuggerStepThrough]
    protected Mock<T> MockService<T>(Action<Mock<T>> setup) where T : class
    {
        var mock = AutoMock.Value.Mock<T>();
        setup(mock);
        return mock;
    }

    [DebuggerStepThrough]
    protected T CreateService<T>(params Parameter[] parameters)
    {
        return AutoMock.Value.Create<T>(parameters);
    }
}

public abstract class UnitTestFixture<T> : UnitTestFixture
{
    private Lazy<T> _subject;

    [SetUp]
    public void __SetUpSubject()
    {
        _subject = new Lazy<T>(() => CreateService<T>());
    }

    protected T Subject => _subject.Value;
}