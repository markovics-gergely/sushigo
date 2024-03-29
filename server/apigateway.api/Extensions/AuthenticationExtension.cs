﻿using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace apigateway.api.Extensions
{
    /// <summary>
    /// Extensions for authentication
    /// </summary>
    public static class AuthenticationExtension
    {
        /// <summary>
        /// Add authentication related extensions
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
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
                    options.Authority = configuration.GetValue<string>("IdentityServer:AuthorityDocker");
                    options.Audience = configuration.GetValue<string>("IdentityServer:AuthorityDocker") + "/resources";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            else
                            {
                                accessToken = context.Request.Query["access_token"];
                                if (!string.IsNullOrEmpty(accessToken))
                                {
                                    context.Token = accessToken;
                                }
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new string[] { })
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

        /// <summary>
        /// Query authorization to header
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static async Task AuthQueryStringToHeader(HttpContext context, Func<Task> next)
        {
            if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]) && context.Request.QueryString.HasValue)
            {
                var accessToken = context.Request.Query["token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + accessToken);
                }
                else
                {
                    accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Request.Headers.Add("Authorization", "Bearer " + accessToken);
                    }
                }
            }
            await next.Invoke();
        }
    }
}
