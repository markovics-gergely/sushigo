using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace apigateway.api.Extensions
{
    public static class AuthenticationExtension
    {
        public static void AddAuthenticationExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = configuration.GetValue<string>("Authentication:AuthorityDocker");
                    options.Audience = configuration.GetValue<string>("Authentication:AuthorityDocker") + "/resources";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["token"];
                            if (string.IsNullOrEmpty(accessToken) == false)
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>())
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                    builder.SetIsOriginAllowed((host) => true)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });
        }
    }
}
