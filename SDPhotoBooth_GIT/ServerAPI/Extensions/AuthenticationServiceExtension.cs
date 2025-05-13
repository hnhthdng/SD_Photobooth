using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
namespace ServerAPI.Extensions
{
    public static class AuthenticationServiceExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateActor = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new RsaSecurityKey(KeyHelper.GetPublicKey(configuration["Jwt:PublicKey"])),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = async context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        var errorDetail = new
                        {
                            status = 401,
                            title = "Authentication Failed",
                            detail = context.Exception.Message
                        };

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetail));
                    },

                    OnTokenValidated = async context =>
                    {
                        var token = context.SecurityToken as JwtSecurityToken;

                        if (token != null)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, token.Subject),
                                new Claim(JwtRegisteredClaimNames.Jti, token.Id)
                            };

                            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                            context.Principal = new ClaimsPrincipal(identity);
                        }

                        await Task.CompletedTask;
                    },

                    OnChallenge = async context =>
                    {
                        if (context.HttpContext.Request.Headers["Accept"].ToString().Contains("text/html"))
                        {
                            context.Response.Redirect("/Identity/Login?returnUrl=" + Uri.EscapeDataString(context.Request.Path));
                        }
                        else
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                status = 401,
                                title = "Unauthorized",
                                detail = "You are not authorized to access this resource."
                            }));
                        }
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            status = 403,
                            title = "Forbidden",
                            detail = "You are not authorized to access this resource."
                        }));
                    },

                    OnMessageReceived = async context =>
                    {
                        context.Token = context.Request.Cookies["access_token"];
                    }
                };
            });

            return services;
        }
    }

}













/*
namespace ServerAPI.Extensions
{
    public static class AuthenticationServiceExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateActor = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new RsaSecurityKey(KeyHelper.GetPublicKey(configuration["Jwt:PublicKey"])),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = async context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        var errorDetail = new
                        {
                            status = 401,
                            title = "Authentication Failed",
                            detail = context.Exception.Message
                        };

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetail));
                    },

                    OnChallenge = async context =>
                    {
                        if (context.HttpContext.Request.Headers["Accept"].ToString().Contains("text/html"))
                        {
                            context.Response.Redirect("/Identity/Login?returnUrl=" + Uri.EscapeDataString(context.Request.Path));
                        }
                        else
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                status = 401,
                                title = "Unauthorized",
                                detail = "You are not authorized to access this resource."
                            }));
                        }
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            status = 403,
                            title = "Forbidden",
                            detail = "You are not authorized to access this resource."
                        }));
                    },

                    OnMessageReceived = async context =>
                    {
                        context.Token = context.Request.Cookies["access_token"];
                    },

                    OnTokenValidated = async context =>
                    {
                        var token = context.SecurityToken as JwtSecurityToken;
                        if (token != null)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, token.Subject),
                                new Claim(JwtRegisteredClaimNames.Jti, token.Id)
                            };

                            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                            context.Principal = new ClaimsPrincipal(identity);
                        }

                        await Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
*/