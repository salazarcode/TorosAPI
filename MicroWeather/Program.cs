
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MicroWeather
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


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

            //var key = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportFromPem(File.ReadAllText("public.key"));

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

                    #region JwtEvents
                    options.Events = new JwtBearerEvents
                    {
                        //Aprovecho este evento para restringir si no se trae el audience
                        //Lamentablemente la libreria que lee el token aqui no logra obtener 
                        //el aud que si esta en el token, pero esta dice que no
                        //OnTokenValidated = context =>
                        //{
                        //    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        //    if (claimsIdentity != null)
                        //    {
                        //        var audienceClaim = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Aud)?.Value;
                        //        if (audienceClaim != audience)
                        //        {
                        //            // Si la audiencia no coincide, fallamos la autenticación
                        //            context.Fail("Invalid audience");
                        //        }
                        //    }
                        //    return Task.CompletedTask;
                        //},
                    };
                    #endregion
                });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
