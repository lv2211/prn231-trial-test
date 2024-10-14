using Microsoft.EntityFrameworkCore;
using PE.Core;
using PE.Core.Commons;
using PE.Core.Contracts;
using PE.Infrastructure;
using PE.Infrastructure.Databases;
using PE.Infrastructure.Repositories;
using PE.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    opt.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    opt.JsonSerializerOptions.WriteIndented = true;
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IFootballClubRepository, FootballClubRepository>();
builder.Services.AddScoped<IFootballPlayerRepository, FootballPlayerRepository>();

builder.Services.AddTransient(typeof(TokenService));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFootballPlayerService, FootballPlayerService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
