using CookStack.Api.Data;
using CookStack.Api.Features.Recipes;
using CookStack.Api.Features.ShoppingList;
using Microsoft.EntityFrameworkCore;

var corsPolicyName = "ClientApp";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IShoppingListService, ShoppingListService>();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
        policy =>
        {
            policy
            .WithOrigins(
                "http://localhost:5067",
                "https://localhost:7067"
                )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});


var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
