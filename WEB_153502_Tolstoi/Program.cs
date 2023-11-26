using WEB_153502_Tolstoi.Logs;
using WEB_153502_Tolstoi.Services.ApiData;
using System.Text.Json;
using System.Text.Json.Serialization;
using WEB_153502_Tolstoi.Services.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Web_153502_Tolstoi.API.Services;
using Microsoft.AspNetCore.Builder;
using Web_153502_Tolstoi.Domain.Entities;
using WEB_153502_Tolstoi.Services.CartServices;
using Serilog;
using Serilog.Events;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<UriData>(builder.Configuration.GetSection("UriData"));
builder.Services
    .AddHttpClient<ICategoryService, ApiCategoryService>();
builder.Services
    .AddHttpClient<IGameService, ApiGameService>();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookie")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority =
    builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
    options.ClientId =
    builder.Configuration["InteractiveServiceSettings:ClientId"];
    options.ClientSecret =
    builder.Configuration["InteractiveServiceSettings:ClientSecret"];
    // Получить Claims пользователя
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ResponseType = "code";
    options.ResponseMode = "query";
    options.SaveTokens = true;
});

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning).CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseSerilogRequestLogging(opts =>
{
    opts.GetLevel = WEB_153502_Tolstoi.Logs.LogHelper.ExcludeHealthChecks;
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.MapRazorPages();
app.MapRazorPages().RequireAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapAreaControllerRoute(
    name: "Admin_area",
    areaName: "Admin",
    pattern: "{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
