using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using PE.Core;
using PE.Core.Commons;
using PE.Core.Contracts;
using PE.Infrastructure;
using PE.Infrastructure.Databases;
using PE.Infrastructure.Repositories;
using PE.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// OData configuration
var odModelBuilder = new ODataConventionModelBuilder();
var playerEntitySet = odModelBuilder.EntitySet<FootballPlayer>("Players");

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    opt.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    opt.JsonSerializerOptions.WriteIndented = true;
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
}).AddOData(opt =>
{
    opt.Select()
    .Filter()
    .Count()
    .OrderBy()
    .Expand()
    .SetMaxTop(100)
    .AddRouteComponents("odata", odModelBuilder.GetEdmModel());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Practical Exam PRN231", Version = "v1" });

    // Add JWT Bearer token authorization
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IFootballClubRepository, FootballClubRepository>();
builder.Services.AddScoped<IFootballPlayerRepository, FootballPlayerRepository>();

builder.Services.AddTransient(typeof(TokenService));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFootballPlayerService, FootballPlayerService>();

builder.Services.AddAuthentication(options =>
{
    // Set the default authentication scheme to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],  // Your issuer
        ValidAudience = builder.Configuration["JwtSettings:Audience"],  // Your audience
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])) // Secret key
    };
});

// Add Authorization services
builder.Services.AddAuthorization(options =>
{
    // Administration: 1, Staff: 2
    options.AddPolicy("ReadPolicy", policy => policy.RequireRole("1", "2")); 
    options.AddPolicy("OtherPolicy", policy => policy.RequireRole("1")); 
});

builder.Services.AddDbContext<EnglishPremierLeague2024DbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"), sqlOptions =>
    {
        sqlOptions.CommandTimeout(120);
    });
});

builder.Services.AddAutoMapper(typeof(MappingProfileExtension).Assembly);
// Options pattern: https://learn.microsoft.com/en-us/dotnet/core/extensions/options
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("PRN231FE", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Practical Exam PRN231 v1");
    });
}

app.UseCors("PRN231FE");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
