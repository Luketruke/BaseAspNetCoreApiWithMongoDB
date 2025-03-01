using Newtonsoft.Json.Converters;
using MyBaseProject.API.Extensions;
using MyBaseProject.API.Middleware;
using MyBaseProject.Application.Extensions;
using MyBaseProject.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

DotNetEnv.Env.Load();

builder.Host.AddLoggingConfiguration();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddApplicationLayer();
builder.Services.AddValidationServices();
builder.Services.AddApplicationServices();
builder.Services.AddApplicationAndInfrastructure(configuration);
builder.Services.AddCorsPolicy(configuration);
builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddSwaggerDocumentation(configuration);
builder.Services.AddApiServices();
builder.Services.AddMongoDb(configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseSwaggerDocumentation();
app.UseRouting();
app.UseCorsPolicy(configuration);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
