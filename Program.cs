using BookReviewApi.Data;
using BookReviewApi.Repositories;
using BookReviewApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Needed for the Swagger configuration
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Connection string (comes from environment variables or appsettings)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. JWT authentication configuration
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

// 4. CORS: allow the React app (Vite dev server) to call this API
const string CorsPolicyName = "ReactApp";

// Allowed origins can be overridden through configuration (Cors:AllowedOrigins),
// for example with an environment variable when the frontend is deployed
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                     ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// 5. Dependency injection (DI) registrations
// Books
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

// Users
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Reviews
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// 6. Controllers and Swagger configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger to accept a JWT and show the Authorize button
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BookReviewApi", Version = "v1" });

    // Describe the security scheme (Bearer token)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste only the token. Swagger adds the 'Bearer' prefix automatically."
    });

    // Apply the security requirement to the whole API
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

// 7. Middleware pipeline (the order matters)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS must come before authentication and authorization
app.UseCors(CorsPolicyName);

// Order: authentication (who are you?) first, then authorization (what may you do?)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();