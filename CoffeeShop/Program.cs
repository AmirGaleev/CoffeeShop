using CoffeeShop.Models;
using CoffeeShop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register services for dependency injection
builder.Services.AddSingleton<ICoffeeService, CoffeeService>();
builder.Services.Configure<StoreSettings>(
    builder.Configuration.GetSection("StoreSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Custom routes (Занятие 4)
app.MapControllerRoute(
    name: "catalogCategory",
    pattern: "catalog/category/{categoryName}",
    defaults: new { controller = "Catalog", action = "Category" });

app.MapControllerRoute(
    name: "coffeeDetails",
    pattern: "coffee/{id}/{name?}",
    defaults: new { controller = "Catalog", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();