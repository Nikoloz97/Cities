namespace Cities.API
{
    // Program = starting point of application
    public class Program
    {
        // Main = configures + runs application
        public static void Main(string[] args)
        {
            // ** 1. Web application = built (using WebApplicationBuilder)
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container ("built-in dependency injection container")
            // I.e. inject them wherever needed in our code
            // Adding view as well = not necessary for APIs (the view = in JSON format)
            builder.Services.AddControllers(options =>
            {
                // Sends out error message if user's requested API return format = not supported (406 Not Acceptable code) 
                options.ReturnHttpNotAcceptable = true;

                // Allows XML format to be supported 
            }).AddXmlDataContractSerializerFormatters();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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