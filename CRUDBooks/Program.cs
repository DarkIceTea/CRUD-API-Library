using Microsoft.EntityFrameworkCore;
using CRUDBooks.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CRUDBooks.Properties;
using Microsoft.OpenApi.Models;
using CRUDBooks.Services;
using CRUDBooks.Repositiries;
using CRUDBooks.Extensions;

namespace CRUDBooks
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // получаем строку подключения из файла конфигурации
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddAuthorization();

            builder.Services.AddCustomAuthentication();
            
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);    // добавляем контекст ApplicationContext в качестве сервиса в приложение
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
            builder.Services.AddTransient<IBookCommandRepository, BookRepository>();
            builder.Services.AddTransient<IBookQueryRepository, BookRepository>();

            builder.Services.AddTransient<IRegistrationService, RegistrationService>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<ITokenService, TokenService>();

            builder.Services.AddCustomSwagger();

            var app = builder.Build();

            app.UseDeveloperExceptionPage();
            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint(@"v1/swagger.json", "CRUD API Library"));

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run();
        }
    }
}