using Microsoft.EntityFrameworkCore;
using Project.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(opts => {
    opts.IdleTimeout = TimeSpan.FromMinutes(120);
    opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddDistributedMemoryCache();

// Regis Transient
builder.Services.AddTransient<IHomeResponsitory, HomeResponsitory>();
builder.Services.AddTransient<ICartReponsitory, CartResponsitory>();
builder.Services.AddTransient<IProductResponsitory, ProductResponsitory>();
builder.Services.AddTransient<IUserResponsitory, UserResponsitory>();
builder.Services.AddTransient<ICategoryResponsitory, CategoryResponsitory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "User",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();