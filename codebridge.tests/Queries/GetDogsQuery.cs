using codebridge.api.application.dogs_features;
using codebridge.api.application.exceptions;
using codebridge.tests.Common;

namespace codebridge.tests.Queries;

[Collection(nameof(DogsTestFixture.DogsFeatCollection))]
public class GetDogsQueryTest
{
    private readonly DogsTestFixture _fixture;

    public GetDogsQueryTest(DogsTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task DogsList_WhenCorrectParams_SucceedsAsync()
    {
        // arrange
        var query = new GetSortedDogsQuery(_fixture.Context);
        const string validAttributeForOrdering = "weight";
        const string validOrder = GetDogsRequest.Ordering.Descending;

        const int PageSize = 10;
        const int PageNumber = 2;
        var simpleRequest = new GetDogsRequest(validAttributeForOrdering, validOrder, PageNumber, PageSize);
        // act
        var successfulResponse = await query.Handle(simpleRequest, default);

        // assert
        Assert.Equal(PageSize, successfulResponse.Dogs.Count);
    }

    [Fact]
     public async Task GetDogsList__WHEN_DogHasNoSuchPropertyForOrdering__Fails()
    {
        // arrrange
        const string inexistentPropertyForOrdering = "height";
        const string validOrdering = GetDogsRequest.Ordering.Descending;
        var validQueryWithCorrectParams = new GetDogsRequest(inexistentPropertyForOrdering, validOrdering, null, null);

        var query = new GetSortedDogsQuery(_fixture.Context);

        // act
        // assert
        await Assert.ThrowsAsync<NoSuchSortingAttributeException>(
        async () =>
        {
            _ = await query.Handle(validQueryWithCorrectParams, default);
        });

    }
}
