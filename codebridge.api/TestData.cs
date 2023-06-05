using codebridge.api.Data;
using codebridge.api.Data.entities;

namespace codebridge.api;

public class TestData : IHostedService
{
    private readonly IServiceProvider serviceProvider;

    public TestData(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var db = services.GetRequiredService<AppDbContext>();

        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();

        var testDogs = new List<Dog>
        {
            //new Dog { Name = "Neo", Color = "red & amber", TailLength = 22, Weight = 32 },
            //new Dog { Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 }
        };
        var colors = new[]
        {
            "white", "black" , "pink", "red", "brown"
        };

        var rand = new Random();

        for (int i = 0; i < 50; i++)
        {
            testDogs.Add(new Dog { 
                Name = $"Dog{i}", 
                Color = colors[rand.Next(colors.Length)],
                TailLength = rand.Next(1, 10), 
                Weight = rand.Next(1, 30) });
        }

        db.Dogs.AddRange(
            testDogs
        );



        await db.SaveChangesAsync(cancellationToken);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        //throw new NotImplementedException();
        return Task.CompletedTask;
    }
}