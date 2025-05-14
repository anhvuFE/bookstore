using LibraryCore.Models;
using LibraryCore.Repositories;
using LibraryCore.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Role", "admin"));
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "MyCookieAuthenticationScheme";
    options.DefaultChallengeScheme = "MyCookieAuthenticationScheme";
}).AddCookie("MyCookieAuthenticationScheme", options =>
{
    options.LoginPath = "/Auth/Login"; // Đường dẫn đến trang đăng nhập
    options.AccessDeniedPath = "/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian tồn tại của cookie (tuỳ chọn)
    options.SlidingExpiration = true; // Cho phép gia hạn thời gian tồn tại khi người dùng tương tác (tuỳ chọn)
});

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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.UseSession();

app.Run();
