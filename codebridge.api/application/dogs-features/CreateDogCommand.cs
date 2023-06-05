using codebridge.api.application.common.Mappings;
using codebridge.api.application.dogs_features.exceptions;
using codebridge.api.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace codebridge.api.application.dogs_features;

public record class CreateDogRequest(
    string Name,string Color, 
    [Range(0, int.MaxValue)] int TailLength, 
    [Range(0, int.MaxValue)] int Weight
) : IRequest<CreateDogResponse>;

public record class CreateDogResponse(Guid DogId);

public class CreateDogCommand : IRequestHandler<CreateDogRequest, CreateDogResponse>
{
    private readonly AppDbContext _context;

    public CreateDogCommand(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<CreateDogResponse> Handle(CreateDogRequest request, CancellationToken cancellationToken)
    {
        var newDog = request.ToEntity();
        

        try
        {
            _ = await _context.Dogs.AddAsync(newDog, cancellationToken);
            _ = await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new SuchNamedDogAlreadyExistException(newDog.Name);
        }
        return new CreateDogResponse(newDog.DogId); // TODO
    }
}
