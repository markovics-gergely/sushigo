using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace apigateway.api.Extensions
{
    /// <summary>
    /// Swagger extensions
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// Add swagger related extensions
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = configuration.GetValue<string>("IdentityServer:Description"), Version = "v1" });

                options.AddSecurityDefinition(configuration.GetValue<string>("IdentityServer:SecurityScheme"), new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow()
                        {
                            TokenUrl = new Uri(configuration.GetValue<string>("IdentityServer:Authority") + "/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { configuration.GetValue<string>("IdentityServer:Name") ?? string.Empty,
                                    configuration.GetValue<string>("IdentityServer:Description") ?? string.Empty }
                            }
                        }
                    },
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer"
                });
                options.OperationFilter<AuthorizeSwaggerOperationFilter>(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = configuration.GetValue<string>("IdentityServer:SecurityScheme"),
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>(){ configuration.GetValue<string>("IdentityServer:Name") ?? string.Empty }
                    }
                });
                options.OperationFilter<SwaggerFileOperationFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthorizeSwaggerOperationFilter : IOperationFilter
    {
        private readonly OpenApiSecurityRequirement requirement;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requirement"></param>
        public AuthorizeSwaggerOperationFilter(OpenApiSecurityRequirement requirement)
        {
            this.requirement = requirement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttribute<AuthorizeAttribute>() != null ||
                context.MethodInfo.DeclaringType?.GetCustomAttribute<AuthorizeAttribute>() != null)
            {
                operation.Security.Add(requirement);
                operation.Responses.TryAdd("401", new OpenApiResponse() { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse() { Description = "Forbidden" });
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadMime = "multipart/form-data";
            if (operation.RequestBody == null || !operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMime, StringComparison.InvariantCultureIgnoreCase)))
                return;

            var fileParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType == typeof(IFormFile));
            if (fileParams == null)
                return;
            operation.RequestBody.Content[fileUploadMime].Schema.Properties =
                fileParams.ToDictionary(k => k.Name ?? string.Empty, v => new OpenApiSchema()
                {
                    Type = "string",
                    Format = "binary"
                });
        }
    }
}
