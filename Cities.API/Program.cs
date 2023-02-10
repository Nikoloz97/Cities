using Cities.API.DBContexts;
using Cities.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
// Downloaded Serilog as nuget package (allows us to log information to a file; useful for production env)
using Serilog;

namespace Cities.API
{
    // Program = starting point of application
    public class Program
    {
        // Main = configures + runs application
        public static void Main(string[] args)
        {
            // Creating a logger that logs to a file path
            Log.Logger = new LoggerConfiguration()
                // Only log for debug level and higher
                .MinimumLevel.Debug()
                .WriteTo.Console()
                // This creates a new logging file each day
                .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


            // ** 1. Web application = built (using WebApplicationBuilder)
            var builder = WebApplication.CreateBuilder(args);

            // Allows us to use Serilog 
            builder.Host.UseSerilog();

            // Add services to the container ("built-in dependency injection container")
            // I.e. inject them wherever needed in our code
            // Adding view as well = not necessary for APIs (the view = in JSON format)
            builder.Services.AddControllers(options =>
            {
                // Sends out error message if user's requested API return format = not supported (406 Not Acceptable code) 
                options.ReturnHttpNotAcceptable = true;

                // Left = Allows patch requests to work 
                // Right = Allows XML format to be supported 
            }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Allows to inject "FileExtensionContentTypeProvider" in other files of code
            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

            // Adding a lifetime service... 
            // Transient = created each time they're requested (for lightweight stuff)
            // Scoped = created once per request
            // Singleton = created first time they're created
            // Allows for dependency injection (see controller)
            // # = compiler directives (from dropdown, debug -> release = activates else statement)
            // i.e. this code = allows us to selectively run "LocalMailService" in debug mode, and "CloudMailService" in release mode
            #if DEBUG
            builder.Services.AddTransient<IMailService, LocalMailService>();
            #else
            builder.Services.AddTransient<IMailService, CloudMailService>();
            #endif

            builder.Services.AddSingleton<CitiesDataStore>();

            // "Registers" CityInfoContext as a DBcontext, so that dependency injection can be applied to it
            builder.Services.AddDbContext<CityInfoContext>(
                // For the parameter, see appsettings.Development.json
                dbContextOptions => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));
            

            var app = builder.Build();

            // ** 2. Configure request pipeline (via adding "middleware")
            // Middleware = Configures the HTTP request pipeline
            // Runs if our environmental variable = set to "development" 
            if (app.Environment.IsDevelopment())
            {
                // These are development middlewares
                // Middlewares = package of code that runs between request <-> response
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Setting up request pipeline:

            // ** 1. Install routing middleware
            app.UseRouting();

            app.UseAuthorization();


            // ** 2. Create an endpoint

            app.UseEndpoints(endpoints =>
            {
                // Configures endpoints without specifying routes (specify routes = via attributes)
                endpoints.MapControllers();
            });
             

            app.Run();
        }
    }
}