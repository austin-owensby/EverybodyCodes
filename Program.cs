using EverybodyCodes.Controllers;
using EverybodyCodes.Gateways;
using EverybodyCodes.PuzzleHelper;
using EverybodyCodes.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EverybodyCodes", Version = "v1" });
    c.ParameterFilter<ParameterFilter>();

    string filePath = Path.Combine(AppContext.BaseDirectory, "EverybodyCodes.xml");
    c.IncludeXmlComments(filePath);
});

// Add the gateway as singleton since almost all API calls use it and it sets up a client that we'd like to keep configured
builder.Services.AddSingleton<EverybodyCodesGateway>();

// Adding all services as Transient because on each request, we should only call the service once.
// We could use Singleton for performance improvement on successive calls,
//    but because we want to avoid spamming the server, we'll assume that this performance is negligible.
builder.Services.AddTransient<SolutionService>();
builder.Services.AddTransient<PuzzleHelperService>();

#region Setup Daily Solution Services
// Get a list of assembly types for the whole app
Type[] assemblyTypes = Assembly.GetAssembly(typeof(Program))?.GetTypes() ?? [];

// Get only the types for the classes that inherit from the ISolutionQuestService
IEnumerable<Type> solutionQuestServiceTypes = assemblyTypes.Where(x => !x.IsInterface && x.GetInterface(nameof(ISolutionQuestService)) != null);

// Register each Solution Quest Service class
foreach (Type solutionQuestServiceType in solutionQuestServiceTypes)
{
    // This is not null because of the filter a few lines above
    Type interfaceType = solutionQuestServiceType.GetInterface(nameof(ISolutionQuestService))!;
    
    builder.Services.AddTransient(interfaceType, solutionQuestServiceType);
}
#endregion

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();