
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
        if (!collectionNames.Contains("Credentials"))
        {
            database.CreateCollection("Credentials");
        }
        if (!collectionNames.Contains("Tokens"))
        {
            database.CreateCollection("Tokens");
        }

        //JWT
        Configuration["JwtSettings:SecretKey"] = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "default_secret_key";
        Configuration["JwtSettings:Issuer"] = Configuration["JwtSettings:Issuer"] ?? "YourApiIssuer";
        Configuration["JwtSettings:Audience"] = Configuration["JwtSettings:Audience"] ?? "YourApiClient";
        Configuration["JwtSettings:AccessTokenExpirationMinutes"] = Configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "60";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // Set to true in production
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"] ?? "default_secret_key"))
            };
        });

        services.AddAuthorization();

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICredentialsRepository, CredentialsRepository>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IAuthService, AuthService>();

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

        app.UseAuthentication(); // Must be before app.UseAuthorization()
        app.UseAuthorization();

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