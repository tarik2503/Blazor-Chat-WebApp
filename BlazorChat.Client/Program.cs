

using BlazorChat.Client.Authentication;
using BlazorChat.Client.HubConnectionStart;
using BlazorChat.Client.Pages;
using BlazorChat.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider,CustomAuthenticationStateProvider>();
builder.Services.AddScoped<Session>();
builder.Services.AddScoped<HubConnectionService>();
builder.Services.AddScoped<Chat>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpClient<IChatManager, ChatManager>(client =>
{
    client.BaseAddress = new Uri("http://localhost:7203/");
});
builder.Services.AddHttpClient<IGroupChatManager, GroupChatManager>(client =>
{
    client.BaseAddress = new Uri("http://localhost:7203/");
});
builder.Services.AddHttpClient<IAccountManager, AccountManager>(client =>
{
    client.BaseAddress = new Uri("http://localhost:7203/");
});
builder.Services.AddMudServices(c => 
 { 
     c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
 });


builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.Expiration = TimeSpan.FromMinutes(1);    
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//}).AddCookie(options =>
//{
//    options.LoginPath = "/Account/Login";
//    options.AccessDeniedPath = "/Account/AccessDenied";
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.Run();
