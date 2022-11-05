using CryptoInvestmentSimulator;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Landing/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Landing}/{action=Index}/{id?}");

app.Run();
