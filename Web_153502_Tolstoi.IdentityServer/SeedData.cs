using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using Web_153502_Tolstoi.IdentityServer.Data;
using Web_153502_Tolstoi.IdentityServer.Models;

namespace Web_153502_Tolstoi.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var userRole = roleMgr.FindByNameAsync("user").Result;
                if (userRole == null)
                {
                    userRole = new IdentityRole
                    {
                        Name = "user",
                        NormalizedName = "user"
                    };

                    var result = roleMgr.CreateAsync(userRole).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("userRole created");
                }

                IdentityResult userRoleResult;
                bool userRoleExists = roleMgr.RoleExistsAsync("user").Result;
                if (!userRoleExists)
                {
                    userRoleResult = roleMgr.CreateAsync(new IdentityRole("user")).Result;
                }

                IdentityResult adminRoleResult;
                bool adminRoleExists = roleMgr.RoleExistsAsync("admin").Result;
                if (!adminRoleExists)
                {
                    adminRoleResult = roleMgr.CreateAsync(new IdentityRole("admin")).Result;
                }


                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var user = userMgr.FindByNameAsync("user@gmail.com").Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "user@gmail.com",
                        Email = "user@gmail.com",
                        EmailConfirmed = true,

                    };
                    var result = userMgr.CreateAsync(user, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "user"),
                            new Claim(JwtClaimTypes.GivenName, "user"),
                            new Claim(JwtClaimTypes.FamilyName, "user"),
                            new Claim(JwtClaimTypes.WebSite, "http://user.com"),
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("user created");
                }
                else
                {
                    Log.Debug("user already exists");
                }
                userMgr.AddToRoleAsync(user, "user");



                var admin = userMgr.FindByNameAsync("admin@gmail.com").Result;
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,

                    };
                    var result = userMgr.CreateAsync(admin, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(admin, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "admin"),
                            new Claim(JwtClaimTypes.GivenName, "admin"),
                            new Claim(JwtClaimTypes.FamilyName, "admin"),
                            new Claim(JwtClaimTypes.WebSite, "http://admin.com"),
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("admin created");
                }
                else
                {
                    Log.Debug("admin already exists");
                }
                userMgr.AddToRoleAsync(admin, "admin");
            }
        }
    }
}