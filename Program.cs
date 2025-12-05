using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using COMP2139_ICE.Data;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Database (PostgreSQL)
// -----------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// -----------------------------
// Identity (Login / Register)
// -----------------------------
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

// -----------------------------
// MVC + Razor Pages (Identity)
// -----------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IEmailSender, COMP2139_ICE.Services.EmailSender>();
builder.Services.AddRazorPages();

var app = builder.Build();

// -----------------------------
// Middleware
// -----------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // <-- MUST be before Authorization
app.UseAuthorization();

// -----------------------------
// Custom 404 Page
// -----------------------------
app.UseStatusCodePagesWithReExecute("/Home/NotFound");

// -----------------------------
// Area Routing (ProjectManagement)
// -----------------------------
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Projects}/{action=Index}/{id?}"
);

// -----------------------------
// Default Route
// -----------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// -----------------------------
// Identity Razor Pages
// -----------------------------
app.MapRazorPages();

app.Run();