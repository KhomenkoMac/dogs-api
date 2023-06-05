using codebridge.api.Data;
using codebridge.api.Data.entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace codebridge.tests.Common;

internal static class DogsContextSeed
{
    private static SqliteConnection? connection = null;

    public static AppDbContext Seed()
    {
        var connectionString = "Data Source=InMemoryBase;Mode=Memory;Cache=Shared";
        connection = new SqliteConnection(connectionString);

        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();

        var rand = new Random();
        var testDogs = new List<Dog>();
        var colors = new[] { "white", "black", "pink", "red", "brown" };
        
        for (int i = 0; i < 50; i++)
        {
            testDogs.Add(new Dog
            {
                Name = $"Dog{i}",
                Color = colors[rand.Next(colors.Length)],
                TailLength = rand.Next(1, 10),
                Weight = rand.Next(1, 30)
            });
        }
        context.Dogs.AddRange(
        testDogs
        );

        context.SaveChanges();

        return context;
    }

    public static void Destroy(AppDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}
