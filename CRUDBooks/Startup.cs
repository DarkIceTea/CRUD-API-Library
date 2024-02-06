using CRUDBooks.Data;
using CRUDBooks.Extensions;
using CRUDBooks.Repositiries;
using CRUDBooks.Services;
using Microsoft.EntityFrameworkCore;

namespace CRUDBooks
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddAuthorization();
            services.AddCustomAuthentication();

            services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));    // добавляем контекст ApplicationContext в качестве сервиса в приложение
            services.AddTransient<IDataContextInitializer, SeedData>();

            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            services.AddTransient<IBookRepository, BookRepository>();

            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddCustomSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDataContextInitializer dataContextInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            dataContextInitializer.Initialize();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(@"v1/swagger.json", "CRUD API Library"));

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
