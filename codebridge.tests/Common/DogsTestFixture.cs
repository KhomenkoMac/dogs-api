using codebridge.api.Data;

namespace codebridge.tests.Common;

public class DogsTestFixture: IDisposable
{
    public DogsTestFixture()
    {
        Context = DogsContextSeed.Seed();
    }
    
    public AppDbContext Context;

    public void Dispose()
    {
        DogsContextSeed.Destroy(Context);
    }

    [CollectionDefinition("DogsFeatCollection")]
    public class DogsFeatCollection : ICollectionFixture<DogsTestFixture> { }
}