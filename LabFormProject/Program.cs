using LabFormProject.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Required for session
builder.Services.AddDistributedMemoryCache();

// Session with options
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolDbConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.MapGet("/", async context =>
{
    var sessionToken = context.Session.GetString("token");
    var sessionUsername = context.Session.GetString("username");
    var sessionId = context.Session.GetString("session_id");

    var cookieToken = context.Request.Cookies["token"];
    var cookieUsername = context.Request.Cookies["username"];
    var cookieSessionId = context.Request.Cookies["session_id"];

     var isAutanthenticated = sessionToken == cookieToken &&
            sessionUsername == cookieUsername &&
            sessionId == cookieSessionId;
            
    if (context.Request.Cookies.ContainsKey("username"))
        context.Response.Redirect("/Index");
    else
        context.Response.Redirect("/Login");
});

app.MapRazorPages();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
