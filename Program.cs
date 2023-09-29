using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopNShop.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    await DbSeeder.SeedDefaultData(scope.ServiceProvider);

//}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Add a route for the admin area
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller=Admin}/{action=Index}/{id?}",
    defaults: new { area = "Admin" }
);

// Set up a custom route for the admin dashboard
app.Map("/admin", adminApp =>
{
    adminApp.UseRouting();
    adminApp.UseAuthorization();
    adminApp.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "admin-dashboard",
            pattern: "{controller=Admin}/{action=Index}/{id?}",
            defaults: new { area = "Admin" }
        );
    });
});
app.MapRazorPages();

app.Run();
