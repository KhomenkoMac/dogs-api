namespace codebridge.api.application.dogs_features;

using codebridge.api.application.common.Mappings;
using codebridge.api.application.exceptions;
using codebridge.api.Data;
using codebridge.api.Data.entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

public record class GetDogsRequest(
    string? Attribute, 
    string? Order,
    [Range(1, int.MaxValue)] int? PageNumber,
    [Range(1, int.MaxValue)] int? PageSize) 
    : IRequest<GetDogsResponse>
{
    public struct Ordering
    {
        public const string Ascending = "asc";
        public const string Descending = "desc";
    }
}

public record class GetDogsResponse(IList<DogViewModel> Dogs);

public record class DogViewModel(string Name, string Color, int TailLength, int Weight);

public class GetSortedDogsQuery : IRequestHandler<GetDogsRequest, GetDogsResponse>
{
    private readonly AppDbContext _context;

    private static readonly Dictionary<string, dynamic> OrderFunctions =
    new()
    {
        { "name", (Expression<Func<Dog, string>>)(x => x.Name) },
        { "weight",  (Expression<Func<Dog, int>>)(x => x.Weight) },
        { "taillength",   (Expression<Func<Dog, int>>)(x => x.TailLength) }
    };

    public GetSortedDogsQuery(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<GetDogsResponse> Handle(GetDogsRequest request, CancellationToken cancellationToken)
    {
        var allDogs = _context.Dogs.AsQueryable();

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            allDogs = allDogs
                .Skip((request.PageNumber.Value! - 1) * request.PageSize.Value)
                .Take(request.PageSize.Value);
        }

        if (request.Attribute is not null && request.Order is not null)
        {
            // searching for such attribute to order by
            _ = typeof(Dog).GetProperty(request.Attribute!, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new NoSuchSortingAttributeException(); // with reflection

            var orderingAttribute = OrderFunctions[request.Attribute];

            if (request.Order == GetDogsRequest.Ordering.Ascending)
            {
                allDogs = Queryable.OrderBy(allDogs, orderingAttribute);
            }
            else if (request.Order == GetDogsRequest.Ordering.Descending)
            {
                allDogs = Queryable.OrderByDescending(allDogs, orderingAttribute);
            }
        }

        var response = (await allDogs.ToListAsync(cancellationToken)).ProjectToViewModel();
        return new GetDogsResponse(response);
    }
}
