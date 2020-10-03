using System.Linq;
using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
            //Configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddScoped<IProductRepository,ProductRepository>();
            // services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddDbContext<StoreContext>(x => 
            x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IConnectionMultiplexer>( c => {
                var configuration = ConfigurationOptions.Parse(
                    _config.GetConnectionString("Redis"), true
                );
                return ConnectionMultiplexer.Connect(configuration);
            });

        // sequence of the below code matters and must be added only after controllers are added. 
            // services.Configure<ApiBehaviorOptions>(options =>
            // {
            //     options.InvalidModelStateResponseFactory = actionContext =>
            //     {
            //         var errors = actionContext.ModelState
            //         .Where(e =>e.Value.Errors.Count >0)
            //         .SelectMany(X =>X.Value.Errors)
            //         .Select(x =>x.ErrorMessage).ToArray();

            //         var errorResponse = new ApiValidationErrorResponse
            //         {
            //             Errors = errors
            //         };

            //     return new BadRequestObjectResult(errorResponse);
            //     };
            // }); 

            services.AddApplicationServices();

            services.AddSwaggerDocumentation();
            // services.AddSwaggerGen( c =>
            //     {
            //         c.SwaggerDoc("v1",new OpenApiInfo{Title = "Skinet API", Version = "v2"}); 
            //     }
            // );   

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy",policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseSwaggerDocumentation();
            
            //app.UseSwagger();
            // app.UseSwaggerUI( c =>
            // {
            //     c.SwaggerEndpoint("/swagger/v1/swagger.json","skinet API v1");
            // });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
