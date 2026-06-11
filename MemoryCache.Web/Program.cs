using BusinessLogic;
using DataAccess;
using Serilog;
using Microsoft.EntityFrameworkCore;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.Seq("http://localhost:5341", apiKey: null)
	.CreateLogger(); 
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Services.AddMemoryCache();
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//	options.Configuration = "localhost:6379";
//	options.InstanceName = "ProductApp_"; 
//});
var config = new ConfigurationBuilder()
	.AddInMemoryCollection(new Dictionary<string, string?>
	{
		["ConnectionStrings:DefaultConnection"] = "Host=dpg-d8lc9028qa3s73a1lavg-a.oregon-postgres.render.com;Port=5432;Database=cachedb_y461;Username=cachedb_y461_user;Password=0enjGJ8NoSl0SvlSiRw0CLPccgZBNoh9;SSL Mode=Require;Trust Server Certificate=true"
	})
	.Build();

builder.Services.AddDataAccess(config);
builder.Services.AddBusinessLogic();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Use(async (context, next) =>
{
	if (context.Request.Path == "/")
	{
		context.Response.Redirect("/swagger");
		return;
	}
	await next();
});
app.Run();
