using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace apigateway.api.Extensions
{
    public static class SwaggerExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = configuration.GetValue<string>("IdentityServer:Description"), Version = "v1" });

                options.AddSecurityDefinition(configuration.GetValue<string>("Authentication:SecurityScheme"), new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow()
                        {
                            TokenUrl = new Uri(configuration.GetValue<string>("Authentication:Authority") + "/connect/token"),
                            Scopes = new Dictionary<string, string> {
                                { configuration.GetValue<string>("IdentityServer:Name"),
                                    configuration.GetValue<string>("IdentityServer:Description") }
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
                                Id = configuration.GetValue<string>("Authentication:SecurityScheme"),
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>(){ configuration.GetValue<string>("IdentityServer:Name") }
                    }
                });
                options.OperationFilter<SwaggerFileOperationFilter>();

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }

    public class AuthorizeSwaggerOperationFilter : IOperationFilter
    {
        private readonly OpenApiSecurityRequirement requirement;

        public AuthorizeSwaggerOperationFilter(OpenApiSecurityRequirement requirement)
        {
            this.requirement = requirement;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttribute<AuthorizeAttribute>() != null ||
                context.MethodInfo.DeclaringType.GetCustomAttribute<AuthorizeAttribute>() != null)
            {
                operation.Security.Add(requirement);
                operation.Responses.TryAdd("401", new OpenApiResponse() { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse() { Description = "Forbidden" });
            }
        }
    }

    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadMime = "multipart/form-data";
            if (operation.RequestBody == null || !operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMime, StringComparison.InvariantCultureIgnoreCase)))
                return;

            var fileParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType == typeof(IFormFile));
            operation.RequestBody.Content[fileUploadMime].Schema.Properties =
                fileParams.ToDictionary(k => k.Name, v => new OpenApiSchema()
                {
                    Type = "string",
                    Format = "binary"
                });
        }
    }
}
