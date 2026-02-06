
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

        // MongoDB configuration
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
        if (!collectionNames.Contains("Users"))
        {
            database.CreateCollection("Users");
        }

        //JWT
        Configuration["JwtSettings:SecretKey"] = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "default_secret_key";
        Configuration["JwtSettings:Issuer"] = Configuration["JwtSettings:Issuer"] ?? "YourApiIssuer";
        Configuration["JwtSettings:Audience"] = Configuration["JwtSettings:Audience"] ?? "YourApiClient";
        Configuration["JwtSettings:AccessTokenExpirationMinutes"] = Configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "60";



        services.AddScoped<IPostRepository, PostRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IBlogService, BlogService>()
                .AddScoped<IUserService, UserService>();
    
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        DotNetEnv.Env.Load();

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