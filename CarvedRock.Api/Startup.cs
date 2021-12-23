using CarvedRock.Api.Data;
using CarvedRock.Api.GraphQL;
using CarvedRock.Api.Repositories;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarvedRock.Api
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<CarvedRockDbContext>(options =>
                options.UseSqlServer(_config["ConnectionStrings:CarvedRock"]));
            services.AddScoped<ProductRepository>();
            services.AddScoped<ProductReviewRepository>();

            services.AddScoped<IDependencyResolver>(s => 
                new FuncDependencyResolver(s.GetRequiredService));

            services.AddScoped<CarvedRockSchema>();
            services.AddSingleton<ReviewMessageService>();

            services.AddGraphQL(o => { o.ExposeExceptions = false; })
                // AddGraphTypes scans assembly for all GraphTypes and registers them automatically
                .AddGraphTypes(ServiceLifetime.Scoped)
                // To get the claims principal object - which represents the user in ASPNETCORE use AddUserContextBuilder
                // Whenever user context is needed somewhere in graphtype, this lambda will be executed
                .AddUserContextBuilder(context => context.User)
                // AddDataLoader: the first time reviews are needed for specific product, the reviews for all products are fetched.
                //  These are stored in cache owned by data loader
                //  For the next products, these will be fetched from the cache
                .AddDataLoader()
                // Add websockets
                .AddWebSockets();

            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, CarvedRockDbContext dbContext)
        {
            app.UseCors(builder => 
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            // Add the websockets middleware and endpoint
            app.UseWebSockets();
            app.UseGraphQLWebSockets<CarvedRockSchema>("/graphql");

            app.UseGraphQL<CarvedRockSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            dbContext.Seed();
        }
    }
}