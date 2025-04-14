using System.Reflection;
using UludagGroup.Commons;
using UludagGroup.Models.Contexts;
using UludagGroup.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddTransient<Context>();
builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new AreaRoutingConvention());
});

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
    
    app.UseExceptionHandler("/Error"); // 500 ve benzeri hatalar için
    app.UseStatusCodePagesWithReExecute("/Error/{0}");// 404, 403 gibi durum kodlarý için
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    await next();

    // Eðer 404 geldiyse ve endpoint bulunamadýysa
    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Request.Path = "/Error/404";
        await next();
    }
});
app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Admin" }
);
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
        name: "error",
        pattern: "Error/{action=Index}/{id?}",
        defaults: new { controller = "Error" }
    );

app.MapControllers();
app.UseStaticFiles();
app.MapRazorPages();
app.Run();
