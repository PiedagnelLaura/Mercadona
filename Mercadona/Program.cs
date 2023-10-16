using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Mercadona.Models;
using Mercadona.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



//Add Entity Framework
builder.Services.AddDbContext<MercadonaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MercadonaConnection"))
    .UseLazyLoadingProxies());

builder.Services.AddDefaultIdentity<MercadonaUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MercadonaDbContext>();

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<MercadonaDbContext>();


//!var mvcBuilder = builder.Services.AddRazorPages();
//builder.Services.AddTransient<IDataGenerator, UserService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<MercadonaDbContext>();
    context.Database.EnsureCreated();
    // DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
