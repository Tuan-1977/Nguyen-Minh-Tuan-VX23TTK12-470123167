using ElectronyatShop.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabaseToServices();
builder.AddDatabaseIdentityToServices();
builder.AddUserRolesToDatabase();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

await app.EnableMigrationsOnStartup();
await app.AddAdminToDb();

// Seed sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ElectronyatShopDbContext>();
    await SeedData.SeedProductsAsync(context, forceReseed: false);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Admin",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();