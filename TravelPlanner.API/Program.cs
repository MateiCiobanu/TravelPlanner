using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using TravelPlanner.Application.Common.Mapping;
using TravelPlanner.Application.Services;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Infrastructure.Persistence;
using TravelPlanner.Infrastructure.Repositories;
using TravelPlanner.WebUI;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .AddApplicationPart(typeof(TravelPlanner.WebUI.Controllers.ItineraryViewController).Assembly);
builder.Services.AddAutoMapper(typeof(Mapping).Assembly);

// Repository registration
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITravelerTypeRepository, TravelerTypeRepository>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IItineraryItemRepository, ItineraryItemRepository>();

builder.Services.AddLogging();


builder.Services.AddScoped<ItinerarySuggestionService>();
builder.Services.AddScoped<ItineraryViewModelMapper>();



builder.Services.AddEndpointsApiExplorer();


// Database
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});



builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TravelPlanner API",
        Version = "v1",
        Description = "API documentation for TravelPlanner",
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient<GooglePlacesService>();
builder.Services.AddHttpClient<GooglePlacesService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelPlanner API v1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();  
var webUIPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "TravelPlanner.WebUI", "wwwroot");
if (Directory.Exists(webUIPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(webUIPath),
        RequestPath = ""
    });
}
app.UseRouting();    
app.UseCors("AllowAll");
app.UseAuthorization();


app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();