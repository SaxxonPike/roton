using NUnit.Framework;

namespace Roton.Test.Infrastructure;

[TestFixture]
public abstract class OriginalContextBaseIntegrationTestFixture() : ContextBaseIntegrationTestFixture(Context.Original);