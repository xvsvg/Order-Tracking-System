using Xunit;

namespace Test.Core.Fixtures;

[CollectionDefinition(nameof(CoreDatabaseCollectionFixture))]
public class CoreDatabaseCollectionFixture : ICollectionFixture<CoreDatabaseFixture>
{
}