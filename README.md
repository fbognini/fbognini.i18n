# fbognini.i18n.Dashboard

If you're using MVC, please register "Area ControllerRoute" and "Default ControllerRoute" before `app.UseI18nDashboard()`.
For example:

```
var app = builder.Build();

// Additional middlewares

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    
if (app.Environment.IsDevelopment())
{
    app.UseI18nDashboard();
}

app.MapRazorPages();

app.Run();
 ```
