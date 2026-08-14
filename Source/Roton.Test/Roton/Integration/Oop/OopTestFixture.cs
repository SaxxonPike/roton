using NUnit.Framework;
using Roton.Emulation.Data.Impl;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Oop;

public abstract class OopTestFixture : AllContextIntegrationTestFixture
{
    public OopTestFixture(Context context) : base(context)
    {
    }

    [SetUp]
    public void __Setup()
    {
        if (ElementList.ObjectId < 0)
            Assert.Inconclusive();
    }
}