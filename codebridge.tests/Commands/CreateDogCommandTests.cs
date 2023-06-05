using codebridge.api.application.dogs_features;
using codebridge.api.application.dogs_features.exceptions;
using codebridge.tests.Common;

namespace codebridge.tests.Commands;

[Collection(nameof(DogsTestFixture.DogsFeatCollection))]
public class CreateDogCommandTests
{
    public CreateDogCommandTests(DogsTestFixture fixture) => _fixture = fixture;
    private readonly DogsTestFixture _fixture;


    [Fact]
    public async Task CreateDog__WHEN_DogParamsCorrect__Succeeds()
    {
        // arrange
        var validQueryWithCorrectParams = new CreateDogRequest("Test1", "Test1color & Test1color", 12, 32);
        var dogCreator = new CreateDogCommand(_fixture.Context);

        // act
        var response = await dogCreator.Handle(validQueryWithCorrectParams, default);

        // assert
        Assert.NotEqual(Guid.Empty, response.DogId);
    }

    [Fact]
    public async Task CreateDog__WHEN_DogWithSuchNameExists__Fails()
    {
        // arrange
        var inbaseDogRecord = _fixture.Context.Dogs.First();
        string InBaseDogName = inbaseDogRecord.Name;
        
        var repeatedDogRequest = new CreateDogRequest(InBaseDogName, "some color doesnt matter", 123, 123);
        var dogCreator = new CreateDogCommand(_fixture.Context);

        // act
        // assert
        await Assert.ThrowsAsync<SuchNamedDogAlreadyExistException>(
        async () =>
        {
            _ = await dogCreator.Handle(repeatedDogRequest, default);
        });
    }
}
