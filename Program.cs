var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Comment this in dev if you saw blank pages
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Default route → Home1/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home1}/{action=Index}/{id?}"
);

// Fixed dev URL
app.Urls.Add("http://localhost:5000");

app.Run();