using System.IdentityModel.Tokens.Jwt;
using NeerCore.Logging;
using NeerCore.Logging.Extensions;
using NetHub.Api;
using NetHub.Api.Configuration;
using NetHub.Api.Configuration.Swagger;
using NetHub.Application;
using NetHub.Data.SqlServer;
using NetHub.Infrastructure;
using NLog;
using NLog.Web;
using ILogger = NLog.ILogger;

{
	ILogger logger = LoggerInstaller.InitFromCurrentEnvironment();

	try
	{
		var builder = WebApplication.CreateBuilder(args);
		// builder.Configuration.AddJsonFile("appsettings.Secrets.json");

		ConfigureBuilder(builder);

		var app = builder.Build();
		JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
		ConfigureWebApp(app);

		app.Run();
	}
	catch (Exception e)
	{
		logger.Fatal(e);
	}
	finally
	{
		logger.Info("Application is now stopping");
		LogManager.Shutdown();
	}
}


static void ConfigureBuilder(WebApplicationBuilder builder)
{
	builder.Logging.ConfigureNLogAsDefault();
	builder.Services.AddSqlServerDatabase(builder.Configuration);
	builder.Services.AddApplication(builder.Configuration);
	builder.Services.AddInfrastructure(builder.Configuration);
	builder.Services.AddWebApi(builder.Configuration);

	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();
}

static void ConfigureWebApp(WebApplication app)
{
	app.UseDeveloperExceptionPage();

	if (app.Configuration.GetSwaggerSettings().Enabled)
	{
		app.UseCustomSwagger();
		app.ForceRedirect(from: "/", to: "/swagger");
	}

	app.UseCors("Cors");

	app.UseHttpsRedirection();

	app.UseCustomExceptionHandler();
	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();
}