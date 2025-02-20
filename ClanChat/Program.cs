using ClanChat.Abstractions.Clan;
using ClanChat.Abstractions.User;
using ClanChat.Core.Services;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Entities;
using ClanChat.Data.Repositories;
using ClanChat.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClanChatDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddIdentity<UserEntity, IdentityRole<Guid>>(options => {
options.Password.RequireDigit = false;          
options.Password.RequireLowercase = false;     
options.Password.RequireUppercase = false;       
options.Password.RequireNonAlphanumeric = false; 
options.Password.RequiredLength = 1;            
options.Password.RequiredUniqueChars = 1;       })
    .AddEntityFrameworkStores<ClanChatDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<IClanService, ClanService>();
builder.Services.AddScoped<IClanRepository, ClanRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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
