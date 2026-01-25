namespace Todo.FunctionalTests.Infrastructure;

/// <summary>
/// Custom factory to create an in-memory API instance for E2E tests
/// </summary>
public class TodoApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext that uses SQL Server
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TodoContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Add DbContext with InMemory database for testing
            services.AddDbContext<TodoContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // Ensure the database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<TodoContext>();

            db.Database.EnsureCreated();
        });

        // Configure testing environment
        builder.UseEnvironment("Testing");
    }
}
