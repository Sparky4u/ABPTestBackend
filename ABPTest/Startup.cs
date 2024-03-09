using ABPTest.Data;
using ABPTest.Data.Repositories;
using ABPTest.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ABPTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthorization(); 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ABPTest", Version = "v1" });
            });
            services.AddDbContext<ExperimentContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IExperimentDatabaseRepository, ExperimentDatabaseRepository>();
            services.AddScoped<IExperimentService, ExperimentService>();
        }

    
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ABPTest v1");
            });
        }

    }
}
