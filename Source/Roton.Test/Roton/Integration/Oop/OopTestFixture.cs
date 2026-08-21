using NUnit.Framework;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Oop;

public abstract class OopTestFixture(Context context) : AllContextTestFixture(context)
{
    [SetUp]
    public void __Setup()
    {
        EnableTracer();
        if (Elements.ObjectId < 0)
            Assert.Inconclusive();
    }

    [TearDown]
    public void __TearDown()
    {
        DisableTracer();
    }
}