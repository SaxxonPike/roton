using NUnit.Framework;

namespace Roton.Test.Infrastructure;

[TestFixture]
public abstract class SuperContextBaseIntegrationTestFixture() : ContextBaseIntegrationTestFixture(Context.Super);