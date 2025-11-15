using CoffeeShop.Services;
using CoffeeShop.Data;
using CoffeeShop.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Настройка сессий
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "CoffeeShop.Session";
});

// Добавить IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register services for dependency injection
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<ICoffeeService, CoffeeService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IVisitCounterService, VisitCounterService>();
// Register services for dependency injection
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<ICoffeeService, CoffeeService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IVisitCounterService, VisitCounterService>();
builder.Services.Configure<StoreSettings>(
    builder.Configuration.GetSection("StoreSettings"));

// Для API: Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Для CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Подключение сессий
app.UseSession();

// CORS middleware
app.UseCors("AllowAll");

app.UseAuthorization();

// Для API: Swagger в dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom routes
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