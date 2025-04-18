using System.Reflection;
using UludagGroup.Commons;
using UludagGroup.Models.Contexts;
using UludagGroup.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<Context>();
builder.Services.AddScoped<ImageOperations>();
builder.Services.AddScoped<HashHelper>();
builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new AreaRoutingConvention());
});
builder.Services.AddAuthentication()
    .AddCookie("AdminScheme", options =>
    {
        options.LoginPath = "/Admin/Auth/Login";
        options.LogoutPath = "/Admin/Auth/Logout";
        options.AccessDeniedPath = "/Admin/Auth/AccessDenied";
        options.Cookie.Name = "AdminAuth";
    })
    .AddCookie("FinanceScheme", options =>
    {
        options.LoginPath = "/Finance/Auth/Login";
        options.LogoutPath = "/Finance/Auth/Logout";
        options.AccessDeniedPath = "/Finance/Auth/AccessDenied";
        options.Cookie.Name = "FinanceAuth";
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add("AdminScheme");
        policy.RequireAuthenticatedUser();
    });

    options.AddPolicy("FinancePolicy", policy =>
    {
        policy.AuthenticationSchemes.Add("FinanceScheme");
        policy.RequireAuthenticatedUser();
    });
});

// t�m repositoriler i�in tan�mlama
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
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // session s�resi
    options.Cookie.HttpOnly = true;  // g�venlik i�in
    options.Cookie.IsEssential = true;  // cookie'nin gerekli oldu�unu belirt
});
var app = builder.Build();


IWebHostEnvironment environment = app.Environment;
environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Error"); // 500 ve benzeri hatalar i�in
    app.UseStatusCodePagesWithReExecute("/Error/{0}");// 404, 403 gibi durum kodlar� i�in
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    await next();

    // E�er 404 geldiyse ve endpoint bulunamad�ysa
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
    pattern: "Admin/{controller=Auth}/{action=Login}/{id?}",
    defaults: new { area = "Admin" }
);
app.MapControllerRoute(
    name: "finance",
    pattern: "finance/{controller=Auth}/{action=Login}/{id?}",
    defaults: new { area = "finance" }
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
