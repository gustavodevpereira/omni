using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Entities.Users;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Rebus.Bus;
using Rebus.ServiceProvider;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddHttpContextAccessor();

            builder.AddBasicHealthChecks();

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Exemplo: \"Bearer {token}\"",
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
            new string[] {}
             }
                });
            });

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(_ => true)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            builder.Services.AddJwtAuthentication(builder.Configuration);

            // Register message broker service
            builder.Services.AddMessageBroker(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });


            builder.Services.AddScoped<IUserContext, HttpUserContext>();

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserContextBehavior<,>));

            var app = builder.Build();
            
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // Habilitar CORS - política padrão sem restrições
            app.UseCors();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapControllers();

            // Aplicar migrações do Entity Framework
            Log.Information("Applying database migrations if needed...");
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DefaultContext>();
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        Log.Information("Pending migrations found. Applying...");
                        context.Database.Migrate();
                        Log.Information("Migrations applied successfully!");
                    }
                    else
                    {
                        Log.Information("No pending migrations found.");
                    }

                    // Seed data if necessary
                    SeedData(services);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occurred while applying migrations");
                }
            }

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void SeedData(IServiceProvider services)
    {
        try
        {
            var context = services.GetRequiredService<DefaultContext>();
            
            // Seed Users if none exist
            if (!context.Users.Any())
            {
                Log.Information("No users found. Creating seed users...");
                
                var users = new List<User>
                {
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        Email = "admin@example.com",
                        Phone = "1234567890",
                        Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        Role = UserRole.Admin,
                        Status = UserStatus.Active,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "manager",
                        Email = "manager@example.com",
                        Phone = "0987654321",
                        Password = BCrypt.Net.BCrypt.HashPassword("Manager123!"),
                        Role = UserRole.Manager,
                        Status = UserStatus.Active,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "user",
                        Email = "user@example.com",
                        Phone = "5555555555",
                        Password = BCrypt.Net.BCrypt.HashPassword("User123!"),
                        Role = UserRole.Customer,
                        Status = UserStatus.Active,
                        CreatedAt = DateTime.UtcNow
                    }
                };
                
                context.Users.AddRange(users);
                context.SaveChanges();
                Log.Information("Seed users created successfully!");
            }
            
            // Seed Products if none exist
            if (!context.Products.Any())
            {
                Log.Information("No products found. Creating seed products...");
                
                var branchId = Guid.NewGuid();
                var products = new List<Product>
                {
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Brahma 600ml",
                        Description = "Brahma Beer 600ml",
                        Sku = "BRA600",
                        Price = 8.99M,
                        StockQuantity = 100,
                        Category = "Beer",
                        Status = ProductStatus.Active,
                        BranchExternalId = branchId,
                        BranchName = "Ambev Store",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Skol 350ml",
                        Description = "Skol Beer Can 350ml",
                        Sku = "SKL350",
                        Price = 4.50M,
                        StockQuantity = 200,
                        Category = "Beer",
                        Status = ProductStatus.Active,
                        BranchExternalId = branchId,
                        BranchName = "Ambev Store",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Antarctica 1L",
                        Description = "Antarctica Beer Bottle 1L",
                        Sku = "ANT1000",
                        Price = 12.99M,
                        StockQuantity = 50,
                        Category = "Beer",
                        Status = ProductStatus.Active,
                        BranchExternalId = branchId,
                        BranchName = "Ambev Store",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Guaraná Antarctica 2L",
                        Description = "Guaraná Antarctica Soft Drink 2L",
                        Sku = "GUANT2L",
                        Price = 9.99M,
                        StockQuantity = 75,
                        Category = "Soft Drink",
                        Status = ProductStatus.Active,
                        BranchExternalId = branchId,
                        BranchName = "Ambev Store",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = "Stella Artois 330ml",
                        Description = "Stella Artois Beer Can 330ml",
                        Sku = "STA330",
                        Price = 6.99M,
                        StockQuantity = 120,
                        Category = "Beer",
                        Status = ProductStatus.Active,
                        BranchExternalId = branchId,
                        BranchName = "Ambev Store",
                        CreatedAt = DateTime.UtcNow
                    }
                };
                
                context.Products.AddRange(products);
                context.SaveChanges();
                Log.Information("Seed products created successfully!");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while seeding data");
        }
    }
}
