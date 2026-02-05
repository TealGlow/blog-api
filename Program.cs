

public class Program
{
    public static void Main(string[] args)
    {
        // var builder = WebApplication.CreateBuilder(args);

        // // Add services to the container.
        // builder.Services.AddRazorPages();
        // builder.Services.AddServerSideBlazor();

        // var app = builder.Build();

        // // Configure the HTTP request pipeline.
        // if (!app.Environment.IsDevelopment())
        // {
        //     app.UseExceptionHandler("/Error");
        //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        //     app.UseHsts();
        // }

        // app.UseHttpsRedirection();

        // app.UseStaticFiles();

        // app.UseRouting();

        // app.MapBlazorHub();
        // app.MapFallbackToPage("/_Host");

        // app.Run();

        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while starting the application: {ex.Message}");
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
