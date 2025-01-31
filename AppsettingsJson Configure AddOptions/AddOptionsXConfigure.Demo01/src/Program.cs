using AddOptionsXConfigure.Demo01;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Read from appsettings.json
builder.Services.AddOptions<DatabaseSettings>().BindConfiguration(nameof(DatabaseSettings));

//Read from appsettings.json Too
builder.Services.Configure<DatabaseDefaults>(builder.Configuration.GetSection(nameof(DatabaseDefaults)));

//Set here defaults values
builder.Services.AddOptions<DatabaseUsers>().BindConfiguration(nameof(DatabaseUsers))
    .Configure(x=> x.Users = new List<string>() { "User1", "User2", "User3" });


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/databasesettings-from-options", (IOptionsMonitor<DatabaseSettings> options) =>
{
    return Results.Ok($"Get from appsettings.json using AddOptions ({options.CurrentValue.ConnectionString})");
});

app.MapGet("/databasedefaults-from-options", (IOptionsMonitor<DatabaseDefaults> options) =>
{
    return Results.Ok($"Get from appsettings.json using Configure (DefaultCommandTimeout:{options.CurrentValue.DefaultCommandTimeout},DefaultDatabaseName:{options.CurrentValue.DefaultDatabaseName})");
});

app.MapGet("/databaseusers-from-options", (IOptionsMonitor<DatabaseUsers> options) =>
{
    var lstStr = string.Join(",",options.CurrentValue.Users!.ToArray(), 0, options.CurrentValue.Users.Count);
    return Results.Ok($"Get from default values in code unsing AddOptions ({lstStr})");
});


app.Run();

