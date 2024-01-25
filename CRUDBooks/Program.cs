using CRUDBooks.Models;
using Microsoft.EntityFrameworkCore;
using CRUDBooks.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CRUDBooks.Properties;
using CRUDBooks.Handlers;
using CRUDBooks.Queries;
using CRUDBooks.Commands;
using Microsoft.OpenApi.Models;
using CRUDBooks.Services;

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
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // указывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = AuthOptions.ISSUER,
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = AuthOptions.AUDIENCE,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
                    // установка ключа безопасности
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });    // подключение аутентификации с помощью jwt-токенов
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);    // добавляем контекст ApplicationContext в качестве сервиса в приложение
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            //Регистрация обработчиков запросов
            builder.Services.AddTransient<IQueryHandler<GetAllBooksQuery, List<Book>>, GetAllBooksQueryHandler>();
            builder.Services.AddTransient<IQueryHandler<GetBookByIdQuery, Book>, GetBookByIdQueryHandler>();
            builder.Services.AddTransient<IQueryHandler<GetBookByISBNQuery, Book>, GetBookByISBNQueryHandler>();
            builder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

            // Регистрация обработчиков команд
            builder.Services.AddTransient<ICommandHandler<AddBookCommand>, AddBookCommandHandler>();
            builder.Services.AddTransient<ICommandHandler<EditBookCommand>, EditBookCommandHandler>();
            builder.Services.AddTransient<ICommandHandler<DeleteBookCommand>, DeleteBookCommandHandler>();
            builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

            builder.Services.AddTransient<IRegistrationService, RegistrationService>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<ITokenService, TokenService>();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRUD API Library", Version = "v1" });

                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath, "CRUDBooks.xml");
                c.IncludeXmlComments(xmlPath);

                // Добавьте конфигурацию Bearer аутентификации
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, new string[] { }
                    }
                });
            });

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