using BusinessLogic;
using DataAccess;
using Serilog;

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
builder.Services.AddDataAccess(builder.Configuration);
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
