using Agendo.Server.Persistance;
using Agendo.Server.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Radzen;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Agendo.AuthAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddSingleton<IDbConnection>((sp) => new SqlConnection(configuration.GetSection("ConnectionString").Value!));

builder.Services.AddSingleton<IDomainRepository, DomainRepository>();
builder.Services.AddSingleton<IDomainService, DomainService>();

builder.Services.AddSingleton<IEmployeeShiftRepository, EmployeeShiftRepository>();
builder.Services.AddSingleton<IEmployeeShiftService, EmployeeShiftService>();

builder.Services.AddSingleton<IDailyScheduleRepository, DailyScheduleRepository>();
builder.Services.AddSingleton<IDailyScheduleService, DailyScheduleService>();

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
