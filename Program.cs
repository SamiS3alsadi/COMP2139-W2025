using Microsoft.EntityFrameworkCore;
using Lap_1.Data;   

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Default route → Home1/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home1}/{action=Index}/{id?}"
);
// Fixed dev URL
app.Urls.Add("http://localhost:5167");

app.Run();