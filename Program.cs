//namespace Mom_Project.Filters;
//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var builder = WebApplication.CreateBuilder(args);

//        // Add services to the container.
//        builder.Services.AddControllersWithViews();

//        //Login  
//        builder.Services.AddDistributedMemoryCache();
//        builder.Services.AddHttpContextAccessor();
//        builder.Services.AddSession();
//        builder.Services.AddControllersWithViews();

//        var app = builder.Build();

//        // Configure the HTTP request pipeline.
//        if (!app.Environment.IsDevelopment())
//        {
//            app.UseExceptionHandler("/Home/Error");
//        }
//        app.UseRouting();

//        app.UseSession();
//        app.UseHttpsRedirection();
//        app.UseStaticFiles();
//        app.UseRouting();
//        app.UseAuthorization();
//        app.MapStaticAssets();
//        app.MapControllerRoute(
//            name: "default",
//            pattern: "{controller=Home}/{action=Index}/{id?}")
//            .WithStaticAssets();

//        app.Run();
//    }
//}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

app.Run();
