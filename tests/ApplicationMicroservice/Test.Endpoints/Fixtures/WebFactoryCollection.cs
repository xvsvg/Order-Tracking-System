using Xunit;

namespace Test.Endpoints.Fixtures;

[CollectionDefinition(nameof(WebFactoryCollection))]
public class WebFactoryCollection : ICollectionFixture<WebFactory>
{
}