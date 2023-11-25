using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Web_153502_Tolstoi.API.Data;
using Web_153502_Tolstoi.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.Authority = builder
    .Configuration
    .GetSection("isUri").Value;
    opt.IncludeErrorDetails = true;
    opt.TokenValidationParameters.ValidateAudience = false;
    opt.TokenValidationParameters.ValidateIssuer = false;
    opt.TokenValidationParameters.ValidateLifetime = true;
    opt.TokenValidationParameters.ValidateIssuerSigningKey = false;
    opt.TokenValidationParameters.ValidTypes =
    new[] { "at+jwt" };
});
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
