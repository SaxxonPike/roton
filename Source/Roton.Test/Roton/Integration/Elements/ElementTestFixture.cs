using NUnit.Framework;
using Roton.Emulation.Data.Impl;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public abstract class ElementTestFixture(Context context) : AllContextIntegrationTestFixture(context)
{
    [SetUp]
    public void __Setup()
    {
        if (ElementList.ObjectId < 0)
        {
            Assert.Inconclusive();
            return;
        }

        Tracer.Attach(TestContext.Out);
    }

    [TearDown]
    public void __TearDown()
    {
        Tracer.Detach(TestContext.Out);
    }
}