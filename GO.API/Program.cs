using Com.Gosol.KNTC.API.Config;
using Com.Gosol.KNTC.API.Formats;
using Com.Gosol.KNTC.BUS.HeThong;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using System.Text.Json.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Look for static files in "wwwroot-custom"
    WebRootPath = "Client"
});


// Add services to the container.


builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
});
builder.Services.AddControllers()
    .AddXmlSerializerFormatters();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });
//services.AddControllers().AddJsonOptions(x =>
//                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// cấu hình cho httpcontext
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(sp => sp.GetRequiredService<HttpContext>().Request);
builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddSession();

//khai báo và thiết lập origin
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
    //options.AddPolicy("AllowOrigin", builder => builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("https://kntcv2dev.gosol.com.vn", "https://kntctest.gosol.com.vn", "https://kntc.vinhphuc.gov.vn/"));

});



// Cấu hình Swagger https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    //thông tin
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Gosol API - created by AnhVH",
        Description = "An ASP.NET 6 Web API for Idics managerment",
        TermsOfService = new Uri("https://gosol.com.vn"),
        Contact = new OpenApiContact
        {
            Name = "Contact",
            Url = new Uri("https://gosol.com.vn")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://gosol.com.vn")
        }
    });
    // add Authen - cho phép nhập Bearer và dùng cho tất cả các api
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    // bắt buộc phải có Bearer
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
//-------------------------------------------------------------------------------------------

//cấu hình đọc dữ liệu trong appsetting.json từ biến IOptions<AppSettings>  trong controller
builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));



//cấu hình jwt---------------------------------------------------------------------------
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.AudienceSecret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
//---------------------------------------------------------------------------------------

// injection các interface --------------------------------------------------------------
builder.Services.AddScoped<ISystemLogBUS, SystemLogBUS>();
builder.Services.AddScoped<ILogHelper, LogHelper>();
// --------------------------------------------------------------------------------------

//build app
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//áp dụng swagger
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
// gán giá trị appsetting cho biên appconnectionstring trong utitis
Com.Gosol.KNTC.Ultilities.SQLHelper.appConnectionStrings = app.Configuration.GetConnectionString("DefaultConnection");
//}
// áp dụng orgin cho biến MyAllowSpecificOrigins đã khai báo ở trên
app.UseCors(MyAllowSpecificOrigins);

app.UseSession();

//cho phép redirect tới https
app.UseHttpsRedirection();

//cho phép sử dụng authen
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

//mapping controller
//app.MapControllers();
app.UseStaticFiles();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    // Add this line
    endpoints.MapFallbackToFile("/index.html");
});


//chạy ứng dụng
app.Run();
