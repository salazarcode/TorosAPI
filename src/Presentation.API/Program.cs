using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Domain.Core.Interfaces;
using Infrastructure.Mapper.AutoMapper.Maps;
using Infrastructure.Repository.EF.Contexts;
using Infrastructure.Repository.EF;

namespace Presentation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DevLocal")));

            // Registro de AutoMapper con los perfiles
            builder.Services.AddAutoMapper(typeof(EfToDomainProfile));

            builder.Services.AddSingleton(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DevLocal"));
                return new DatabaseContextFactory(optionsBuilder.Options);
            });


            builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowEverything",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            // Cargar configuración de JWT desde los secretos
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportFromPem(File.ReadAllText("Keys/public.key"));

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.UseSecurityTokenValidators = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey(rsa),

                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = false,
                        ValidAudience = audience,

                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomBPMS API", Version = "v1" });

                // Indicar que int? es nullable
                c.MapType<int?>(() => new OpenApiSchema { Type = "integer", Format = "int32", Nullable = true });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowEverything");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();


            app.Run();
        }
    }
}
