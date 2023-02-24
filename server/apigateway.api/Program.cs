using apigateway.api.Extensions;
using Microsoft.OpenApi.Models;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddAuthenticationExtensions(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerForOcelot(builder.Configuration, options => {
    options.GenerateDocsDocsForGatewayItSelf(opt =>
    {
        opt.AddSecurityDefinition(configuration.GetValue<string>("IdentityServer:SecurityScheme"), new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                Password = new OpenApiOAuthFlow()
                {
                    TokenUrl = new Uri(configuration.GetValue<string>("IdentityServer:Authority") + "/connect/token"),
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
        opt.OperationFilter<AuthorizeSwaggerOperationFilter>(new OpenApiSecurityRequirement()
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
                        new List<string>(){ configuration.GetValue<string>("IdentityServer:Name") }
                    }
                });
        opt.OperationFilter<SwaggerFileOperationFilter>();
    });
});

var app = builder.Build();

builder.Configuration
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json", true, true)
    .AddOcelotWithSwaggerSupport((options) =>
    {
        options.Folder = "Routes";
        options.FileOfSwaggerEndPoints = "ocelot.swagger";
    })
    .AddEnvironmentVariables();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";
        //opt.OAuthClientId(configuration.GetValue<string>("IdentityServer:ClientId"));
        //opt.OAuthClientSecret(configuration.GetValue<string>("IdentityServer:ClientSecret"));
        //opt.OAuthAppName(configuration.GetValue<string>("IdentityServer:Name"));
        //opt.OAuthUsePkce();
    });
}

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapSwagger();

app.UseOcelot().Wait();

app.Run();
