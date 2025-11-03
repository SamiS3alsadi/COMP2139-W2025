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
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Home1/NotFound");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Projects}/{action=Index}/{id?}"
);

// Default route → Home1/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home1}/{action=Index}/{id?}"
);
app.Run();