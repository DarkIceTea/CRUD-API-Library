using CRUDBooks.Configuration;
using CRUDBooks.Data;
using CRUDBooks.Extensions;
using CRUDBooks.Middleware;
using CRUDBooks.Repositiries;
using CRUDBooks.Services;
using CRUDBooks.Services.ServiceInterfaces;
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

            services.AddDbContext<DataContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);    // добавляем контекст ApplicationContext в качестве сервиса в приложение
            services.AddTransient<IDataContextInitializer, SeedData>();
            services.AddTransient<MappsterConfiguration, MappsterConfiguration>();

            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            services.AddTransient<IBookRepository, BookRepository>();

            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IBookService, BookService>();

            services.AddCustomSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDataContextInitializer dataContextInitializer, MappsterConfiguration mappsterConfiguration)
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
            mappsterConfiguration.Configure();

            dataContextInitializer.Initialize();

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(@"v1/swagger.json", "CRUD API Library"));

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
