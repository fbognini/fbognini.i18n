using fbognini.i18n;
using fbognini.i18n.Dashboard;
using fbognini.i18n.Dashboard.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddI18N(builder.Configuration);
builder.Services.AddI18nDashboardServices();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


await app.InitializeI18N();

app.UseRequestLocalizationI18N();

var dashboardOptions = new DashboardOptions()
{
};
app.UseI18nDashboard(options: dashboardOptions);

app.UseAuthorization();

app.MapRazorPages();

app.Run();
