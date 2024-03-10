using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tournament.Data.Context;
using Tournament.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ApplicationContext>(options =>
//                options.UseSqlServer("Server=94.199.202.244;Database=mara9452_turnuvaDb;User Id=turnuva;Password=Ed4b122ff!;TrustServerCertificate=True;Pooling=true:"));

builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseMySQL("Server=94.199.202.234;Database=mara9452_turnuvaMysql;User Id=melih;Password=Ed4b122ff!"));

builder.Services.AddScoped<EmailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
