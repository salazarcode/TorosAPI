
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

            var key = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = false,

                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    #region JwtEvents
                    // Todo este bloque contiene eventos del ciclo de validación del JWT
                    // necesarios para depurar lo que viene dentro de él
                    /*
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogError("Authentication failed. Message: " + context.Exception.Message);
                            logger.LogError("Authentication failed. Inner exception message: " + context.Exception.InnerException?.Message);
                            return Task.CompletedTask;
                        },

                        OnMessageReceived = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                            if (!string.IsNullOrEmpty(token))
                            {
                                var handler = new JwtSecurityTokenHandler();
                                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                                if (jsonToken != null)
                                {
                                    logger.LogInformation($"Token Audience: {jsonToken.Audiences.FirstOrDefault()}");
                                    logger.LogInformation($"Token Claims: {string.Join(", ", jsonToken.Claims.Select(c => $"{c.Type}: {c.Value}"))}");
                                }
                            }

                            return Task.CompletedTask;

                        },
                        OnTokenValidated = context =>
                        {
                            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                            if (claimsIdentity != null)
                            {
                                var audienceClaim = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Aud)?.Value;
                                if (audienceClaim != audience)
                                {
                                    // Si la audiencia no coincide, fallamos la autenticación
                                    context.Fail("Invalid audience");
                                }
                            }
                            return Task.CompletedTask;
                        },
                    };
                    */
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

            app.Use(async (context, next) =>
            {
                // Log the incoming request's authorization header
                var authHeader = context.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(authHeader))
                {
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation($"Authorization Header: {authHeader}");
                }

                // Call the next middleware in the pipeline
                await next.Invoke();
            });

            app.UseCors("AllowEverything");  

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
