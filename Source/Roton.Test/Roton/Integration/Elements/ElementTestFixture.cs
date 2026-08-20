using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public abstract class ElementTestFixture(Context context) : AllContextIntegrationTestFixture(context)
{
    [SetUp]
    public void __Setup()
    {
        if (Elements.ObjectId < 0)
        {
            Assert.Inconclusive();
            return;
        }

        EnableTracer();
    }

    [TearDown]
    public void __TearDown()
    {
        DisableTracer();
    }
}