using BookReviewApi.Data;
using BookReviewApi.Repositories;
using BookReviewApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // ضفنا دي عشان إعدادات Swagger
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// 4. Dependency Injection (DI) Registration
// تسجيل الـ Books
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

// تسجيل الـ Users
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// تسجيل الـ Reviews (الجديد)
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// 5. Controllers & Swagger Configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// إعداد Swagger ليقبل الـ JWT ويظهر زرار الـ Authorize
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BookReviewApi", Version = "v1" });

    // تعريف شكل الـ Security (Bearer Token)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "دخل التوكن بتاعك هنا بالشكل ده: Bearer {your_token}"
    });

    // تطبيق الـ Security على كل الـ API
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// 6. Middleware Pipeline (الترتيب مهم جداً)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// الترتيب: التحقق من الهوية (Authentication) ثم الصلاحيات (Authorization)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();