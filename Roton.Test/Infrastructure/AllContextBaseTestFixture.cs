using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Infrastructure
{
    [TestFixture(Context.Original)]
    [TestFixture(Context.Super)]
    public abstract class AllContextBaseTestFixture : ContextBaseTestFixture
    {
        protected AllContextBaseTestFixture(Context context) : base(context)
        {
        }
    }
}