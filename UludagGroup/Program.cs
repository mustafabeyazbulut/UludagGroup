using System.Reflection;
using UludagGroup.Models.Contexts;
using UludagGroup.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddTransient<Context>();

// tüm repositoriler için tanýmlama
var repositoryTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.BaseType == typeof(BaseRepository));
foreach (var implementation in repositoryTypes)
{
    var serviceInterface = implementation.GetInterfaces().FirstOrDefault();
    if (serviceInterface != null)
    {
        builder.Services.AddScoped(serviceInterface, implementation);
    }
}
// Session servisini ekleyin
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // session süresi
    options.Cookie.HttpOnly = true;  // güvenlik için
    options.Cookie.IsEssential = true;  // cookie'nin gerekli olduðunu belirt
});
var app = builder.Build();
IWebHostEnvironment environment = app.Environment;
environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
app.UseStaticFiles();
app.MapRazorPages();
app.Run();
