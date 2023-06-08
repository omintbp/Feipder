using Feipder.Data;
using Feipder.Data.Repository;
using Feipder.Entities;
using Microsoft.AspNetCore.Identity;
using Feipder.Entities.Models;
using Feipder.Tools;
using Feipder.Tools.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.ConfigureJWTAuthorization(builder.Configuration);
builder.Services.ConfigureIdentity();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<TokenService, TokenService>();

builder.Services.AddCors();
builder.Services.AddControllers()
   .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

    });

builder.Services.AddDbContext<DataContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.ConfigureSwagger();

#endregion

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin());

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dataContext.Database.EnsureDeleted();
        dataContext.Database.EnsureCreated();
        dataContext.Seed(scope.ServiceProvider.GetRequiredService<IConfiguration>());

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await UsersInitializer.Initialize(userManager, rolesManager);
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
