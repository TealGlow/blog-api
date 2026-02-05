
using MongoDB.Driver;

/// <summary>
/// Startup class for configuring services and the app's request pipeline.
/// </summary>
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddRazorPages();
        _ = services.AddServerSideBlazor();
        var mongoClient = new MongoClient(Configuration["MongoDb:ConnectionString"]);
        services.AddSingleton(mongoClient);
        services.AddScoped(provider =>
            mongoClient.GetDatabase(Configuration["MongoDb:DatabaseName"]));

        // Create Posts collection if it doesn't exist
        var database = mongoClient.GetDatabase(Configuration["MongoDb:DatabaseName"]);
        var collectionNames = database.ListCollectionNames().ToList();
        if (!collectionNames.Contains("Posts"))
        {
            database.CreateCollection("Posts");
        }

        services.AddScoped<IPostRepository<PostDto>, PostRepository>();
        services.AddScoped<BlogManager>();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        _ = app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        });
    }
}