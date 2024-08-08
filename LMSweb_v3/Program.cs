using LMSweb_v3.Configs;
using LMSweb_v3.Services;
using LMSwebDB.Data;
using LMSwebDB.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseKestrel(options =>
    {
        // 設定最大請求主體大小為100MB
        options.Limits.MaxRequestBodySize = 100 * 1024 * 1024;
    });

    //測試用
    builder.Services.Configure<AzureOpenAIConfig>(c =>
    {
        c.Endpoint = builder.Configuration["AzureOpenAI:Endpoint"];
        c.APIKey = builder.Configuration["AzureOpenAI:APIKey"];
        c.GPTModel = builder.Configuration["AzureOpenAI:GPTModel"];
        c.Gpt16kModel = builder.Configuration["AzureOpenAI:Gpt16kModel"];
        c.EmbeddingModel = builder.Configuration["AzureOpenAI:EmbeddingModel"];
        c.GPT4Model = builder.Configuration["AzureOpenAI:GPT4Model"];
    });

    // 加入資料庫服務到DI容器中，測試用
    builder.Services.AddDbContext<DbContext, LMSContext>(
        // 設定資料庫連線字串
        // 記得利用管理使用者秘密功能將連線字串加入使用者秘密中
        options => options.UseSqlServer(builder.Configuration["LMSContext"])
    );
}
else
{
    builder.Services.Configure<AzureOpenAIConfig>(c =>
    {
        c.Endpoint = builder.Configuration.GetConnectionString("AzureOpenAI:Endpoint");
        c.APIKey = builder.Configuration.GetConnectionString("AzureOpenAI:APIKey");
        c.GPTModel = builder.Configuration.GetConnectionString("AzureOpenAI:GPTModel");
        c.Gpt16kModel = builder.Configuration.GetConnectionString("AzureOpenAI:Gpt16kModel");
        c.EmbeddingModel = builder.Configuration.GetConnectionString("AzureOpenAI:EmbeddingModel");
        c.GPT4Model = builder.Configuration.GetConnectionString("AzureOpenAI:GPT4Model");
    });

    // 加入資料庫服務到DI容器中
    builder.Services.AddDbContext<DbContext, LMSContext>(

        // 設定資料庫連線字串
        // 記得利用管理使用者秘密功能將連線字串加入使用者秘密中
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("LMSContext"))
    );
}

//加入Cookie Policy服務到DI容器中
builder.Services.AddCookiePolicy(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict; // 設定最小同站規則
    options.HttpOnly = HttpOnlyPolicy.Always; // 設定HttpOnly
    options.Secure = CookieSecurePolicy.Always; // 設定安全性
});

//加入Cookie認證服務到DI容器中
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme) // 設定Cookie認證方案
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login"; // 登入路徑
        options.AccessDeniedPath = "/Home/Error"; // 拒絕存取路徑
        options.LogoutPath = "/Home/Logout"; // 登出路徑
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie過期時間
    }
);

builder.Services.AddScoped<LMSRepository>();
builder.Services.AddScoped<FileProcessService>();
builder.Services.AddScoped<FileOperationService>();
builder.Services.AddScoped<StudentManagementSercices>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<AzureOpenAIService>();
builder.Services.AddScoped<ChatService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
