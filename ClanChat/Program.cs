using ClanChat.Abstractions.Clan;
using ClanChat.Abstractions.Message;
using ClanChat.Abstractions.User;
using ClanChat.Core.Services;
using ClanChat.Data.DbConfigurations;
using ClanChat.Data.Entities;
using ClanChat.Data.Repositories;
using ClanChat.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

builder.Services.AddDbContext<ClanChatDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);

builder.Services.AddAutoMapper(typeof(MapProfile));



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5291")
              .AllowAnyHeader()
              .AllowAnyMethod()
                        .AllowCredentials();  // Разрешаем отправку cookie

});
});

builder.Services.AddIdentity<UserEntity, IdentityRole<Guid>>(options => {
options.Password.RequireDigit = false;          
options.Password.RequireLowercase = false;     
options.Password.RequireUppercase = false;       
options.Password.RequireNonAlphanumeric = false; 
options.Password.RequiredLength = 1;            
options.Password.RequiredUniqueChars = 1;       })
    .AddEntityFrameworkStores<ClanChatDbContext>()
    ;

builder.Services.AddSignalR();

builder.Services.AddScoped<IClanService, ClanService>();
builder.Services.AddScoped<IClanRepository, ClanRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddHttpContextAccessor();


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
app.UseCors("AllowLocalhost");
app.UseRouting();
app.MapHub<MessageHub>("/messageHub");


app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
