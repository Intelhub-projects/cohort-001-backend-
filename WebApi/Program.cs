using System.Reflection;
using System.Text;
using IOC;
using IOC.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using RestfulAPI.Filters;
using Configuration = sib_api_v3_sdk.Client.Configuration;

var myAllowSpecialOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
}).UseNLog();

// Add services to the container.
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("ApplicationContext"));
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
   o.TokenLifespan = TimeSpan.FromHours(3));


builder.Services.AddCors(options => options.AddPolicy(myAllowSpecialOrigins, builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = builder.Configuration["JwtTokenSettings:TokenIssuer"],
        //ValidAudience = builder.Configuration["JwtTokenSettings:TokenIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenSettings:TokenKey"]))
    };
    options.RequireHttpsMetadata = false;
});

//builder.Configuration.AddEnvironmentVariables()
//    .AddKeyPerFile()
//     .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MEDPHARM API",
        Version = "v1",
        Description = "MEDPHARM API"
    });
    c.OperationFilter<SwaggerHeaderFilter>();
    c.CustomSchemaIds(x => x.FullName);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MEDPHARM v1"));

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(myAllowSpecialOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
