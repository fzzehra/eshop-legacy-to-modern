using EShop.Modern.Components;
using EShop.Modern.Data;
using EShop.Modern.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDb"))); //Birisi CatalogDbContext isterse, ona appsettings.json'daki bağlantı dizesiyle kurulmuş bir SQL Server bağlantısı ver
builder.Services.AddScoped<ICatalogReadService, CatalogReadService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
