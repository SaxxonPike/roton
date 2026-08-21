using NUnit.Framework;

namespace Roton.Test.Infrastructure;

[TestFixture]
public abstract class SuperContextTestFixture() : ContextTestFixture(Context.Super);