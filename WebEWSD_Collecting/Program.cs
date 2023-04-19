using Microsoft.EntityFrameworkCore;
using EWSD_Collecting.Utility;
using EWSD_Collecting.DataAccess.Data;
using EWSD_Collecting.DataAccess.Repository;
using EWSD_Collecting.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using EWSD_Collecting.Utility.Mail;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Identity.UI.Services;
using EWSD_Collecting.Models;
using EWSD_Collecting.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);
var mailsettings = builder.Configuration.GetSection("MailSettings");
var services = builder.Services;
var configuration = builder.Configuration;
builder.Services.AddResponseCaching();

services.AddAuthentication()
.AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
}); ;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDefaultIdentity<IdentityUser>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.Configure<MailSettings>(mailsettings);
builder.Services.AddScoped<ISendMailService, SendMailService>();

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
app.UseResponseCaching();

app.UseRouting();
SeedDatabase();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseStatusCodePagesWithRedirects("/Error/{0}"); //This middleware uses StatusCodePagesWithRedirects to redirect to a specified URL when an HTTP status code error is encountered.
app.UseEndpoints(endpoints => {

    endpoints.MapGet("/testmail", async context => {

        // Lấy dịch vụ sendmailservice
        var sendmailservice = context.RequestServices.GetService<ISendMailService>();

        MailContent content = new MailContent
        {
            To = "hiepplmgbs16019@fpt.edu.vn",
            Subject = "Test",
            Body = "<p><strong>Hello</strong></p>"
        };

        await sendmailservice.SendMail(content);
        await context.Response.WriteAsync("Send mail");
    });
});
app.Run();
void SeedDatabase()
{
    using(var scope= app.Services.CreateScope())
    {
        var dbInitializer = scope .ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
