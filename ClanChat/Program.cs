using ClanChat.Abstractions.Clan;
using ClanChat.Core.Services;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Repositories;
using ClanChat.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddDbContext<ClanChatDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddScoped<IClanService, ClanService>();
builder.Services.AddScoped<IClanRepository, ClanRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ClanChatDbContext>();
    dbContext.Database.Migrate();
    dbContext.SeedClans();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
