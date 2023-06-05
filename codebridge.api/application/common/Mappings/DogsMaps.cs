using codebridge.api.application.dogs_features;
using codebridge.api.Data.entities;

namespace codebridge.api.application.common.Mappings;

public static class DogsMaps
{
    public static Dog ToEntity(this CreateDogRequest request)
    {
        return new Dog
        {
            Name = request.Name,
            Color = request.Color,
            TailLength = request.TailLength,
            Weight = request.Weight,
        };
    }

    public static DogViewModel ToViewModel(this Dog entity) 
        => new (entity.Name, entity.Color, entity.TailLength, entity.Weight);

    public static IList<DogViewModel> ProjectToViewModel(this IEnumerable<Dog> entities)
        => entities.Select(x => ToViewModel(x)).ToList();
}
