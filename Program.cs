using Microsoft.EntityFrameworkCore;
using Lap_1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add MVC support (controllers + views)
builder.Services.AddControllersWithViews();

// Configure the database context using PostgreSQL (Npgsql)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Use exception page only in production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Basic middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Show a custom "Not Found" page for 404s
app.UseStatusCodePagesWithReExecute("/Home/NotFound");

// Enable routing for Area controllers (e.g., ProjectManagement)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Projects}/{action=Index}/{id?}"
);

// Default route → Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();