using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Web_153502_Tolstoi.API.Data;
using Microsoft.Extensions.Configuration;
using WEB_153502_Tolstoi.Services.ApiData;
using System.Text.Json;
using System.Text.Json.Serialization;
using WEB_153502_Tolstoi.Services.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Web_153502_Tolstoi.API.Services;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<UriData>(builder.Configuration.GetSection("UriData"));
builder.Services
    .AddHttpClient<ICategoryService, ApiCategoryService>();
builder.Services
    .AddHttpClient<IGameService, ApiGameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapAreaControllerRoute(
    name: "Admin_area",
    areaName: "Admin",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
