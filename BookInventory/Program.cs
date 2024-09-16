using BookInventory.BusinessLogicAcessLayer.Configurations;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccessLayer.Database;
using BookInventory.DataAccessLayer.Repository;
using BookInventory.LogicAcessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateLogger();


builder.Host.UseSerilog();

// Configuration for database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connecting to the database {connectionString}!");

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = """Standard Authorization header using the Bearer scheme. Example: "bearer {token}" """,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                       .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

// Configure policies and register policy services using PolicyConfiguration
PolicyConfiguration.ConfigurePolicies(builder.Services);

builder.Services.AddHttpContextAccessor();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureServices();
builder.Services.ConfigureRepositorys();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();