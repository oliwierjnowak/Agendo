using Agendo.Server.Persistance;
using Agendo.Server.Services;
using Radzen;
using System.Data;
using System.Data.SqlClient;
using Agendo.AuthAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Hosting.Server;
using System.Reflection;
using Agendo.Server.db;

var builder = WebApplication.CreateBuilder(args);


var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .Build();
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddRadzenComponents();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
var x =Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 
if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+@"\appsettings.json"))
{
    var constring = new ConfigurationBuilder()
     .SetBasePath(builder.Environment.ContentRootPath)
     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
     .Build().GetSection("ConnectionString").Value!;
  
    //builder.Services.AddSingleton<IDbConnection>((sp) => new SqlConnection("Server=localhost,1433;User ID=SA;Password=@Agendooo$1;Trusted_Connection=False;Encrypt=False;"));
    builder.Services.AddSingleton<IDbConnection>((sp) => new SqlConnection(constring));
}
else
{
    builder.Services.AddSingleton<IDbConnection>((sp) => new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;"));
    SqlConnection connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");
    connection.Open();


    var mock = new TestDBMockData { };
    await connection.ExecuteAsync(mock.mockData);
    connection.Close();
}

builder.Services.AddSingleton<IDomainRepository, DomainRepository>();
builder.Services.AddSingleton<IDomainService, DomainService>();

builder.Services.AddSingleton<IShiftRepository, ShiftRepository>();
builder.Services.AddSingleton<IShiftService, ShiftService>();

builder.Services.AddSingleton<IDailyScheduleRepository, DailyScheduleRepository>();
builder.Services.AddSingleton<IDailyScheduleService, DailyScheduleService>();

builder.Services.AddSingleton<IRightsRepository, RightsRepository>();
builder.Services.AddSingleton<IRightsService, RightsService>();

builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
public partial class Program { }