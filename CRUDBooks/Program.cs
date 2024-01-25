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
            // �������� ������ ����������� �� ����� ������������
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // ���������, ����� �� �������������� �������� ��� ��������� ������
                    ValidateIssuer = true,
                    // ������, �������������� ��������
                    ValidIssuer = AuthOptions.ISSUER,
                    // ����� �� �������������� ����������� ������
                    ValidateAudience = true,
                    // ��������� ����������� ������
                    ValidAudience = AuthOptions.AUDIENCE,
                    // ����� �� �������������� ����� �������������
                    ValidateLifetime = true,
                    // ��������� ����� ������������
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // ��������� ����� ������������
                    ValidateIssuerSigningKey = true,
                };
            });    // ����������� �������������� � ������� jwt-�������
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);    // ��������� �������� ApplicationContext � �������� ������� � ����������
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();

            //����������� ������������ ��������
            builder.Services.AddTransient<IQueryHandler<GetAllBooksQuery, List<Book>>, GetAllBooksQueryHandler>();
            builder.Services.AddTransient<IQueryHandler<GetBookByIdQuery, Book>, GetBookByIdQueryHandler>();
            builder.Services.AddTransient<IQueryHandler<GetBookByISBNQuery, Book>, GetBookByISBNQueryHandler>();
            builder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

            // ����������� ������������ ������
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

                // �������� ������������ Bearer ��������������
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