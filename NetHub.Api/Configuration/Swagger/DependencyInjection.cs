using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace NetHub.Api.Configuration.Swagger;

public static class DependencyInjection
{
	public static void AddCustomSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Description = "NetHub Api Swagger",
				Title = "NetHub Api Swagger",
				Version = "1",
			});
			options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
			{
				Name = "Authorization",
				Description = "Please insert JWT with Bearer into field",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http,
				Scheme = JwtBearerDefaults.AuthenticationScheme,
			});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] { }
				}
			});
		});
	}

	public static void UseCustomSwagger(this IApplicationBuilder app)
	{
		var swaggerSettings = app.ApplicationServices.GetRequiredService<IConfiguration>().GetSwaggerSettings();
		var apiProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

		app.UseSwagger();
		app.UseSwaggerUI(options =>
		{
			foreach (ApiVersionDescription description in apiProvider.ApiVersionDescriptions)
			{
				string name = $"{swaggerSettings.Title} {description.GroupName.ToUpper()}";
				string url = $"/swagger/{description.GroupName}/swagger.json";
				options.SwaggerEndpoint(url, name);
			}

			options.DocumentTitle = "Swagger - " + swaggerSettings.Title;
			options.InjectStylesheet("/swagger/custom.css");
			options.InjectJavascript("/swagger/custom.js");
		});

		app.UseReDoc(options =>
		{
			var description = apiProvider.ApiVersionDescriptions[0];
			options.DocumentTitle = $"{swaggerSettings.Title} {description.GroupName.ToUpper()}";
			options.SpecUrl = $"../swagger/{description.GroupName}/swagger.json";
			options.RoutePrefix = "dogs";
			options.HeadContent = "NetHub Api Docs";
		});
	}

	public static void ForceRedirect(this WebApplication app, string from, string to)
	{
		app.MapGet(from, context =>
		{
			context.Response.Redirect(to, true);
			return Task.CompletedTask;
		});
	}

	public static SwaggerConfigurationOptions GetSwaggerSettings(this IConfiguration configuration)
	{
		var settings = new SwaggerConfigurationOptions();
		configuration.Bind("Swagger", settings);
		return settings;
	}
}