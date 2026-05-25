using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Repositories;
using Gymly.Infrastructure;
using Gymly.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<GymlyDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Gymly.Infrastructure")));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<GymlyDbContext>());

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
