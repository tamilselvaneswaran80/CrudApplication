using Crud_application.Data;
using Crud_application.Services;
using Curd_application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
//using static Curd_application.Services.StudentService;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Crud Api",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// JWT Authentication
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("THIS_IS_MY_SUPER_SECRET_KEY_FOR_JWT_TOKEN_123456"))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Student.View", policy =>
    policy.RequireAssertion(c => c.User.Claims.Any(x =>
        x.Type == "Permission" && x.Value == "Student.View")));

    options.AddPolicy("Student.Create", policy =>
        policy.RequireAssertion(c =>c.User.Claims.Any(x => 
        x.Type == "Permission" && x.Value == "Student.Create")));

    options.AddPolicy("Student.Edit", policy =>
        policy.RequireAssertion(c => c.User.Claims.Any(x =>
        x.Type == "Permission" && x.Value == "Student.Edit")));

    options.AddPolicy("Student.Delete", policy =>
        policy.RequireAssertion(c => c.User.Claims.Any(x =>
        x.Type == "Permission" && x.Value == "Student.Delete")));
});
// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
builder.Services.AddSignalR();
builder.Services.AddScoped<ICrudService, CrudService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

// Static file serving
app.UseDefaultFiles();   // looks for index.html by default
app.UseStaticFiles();    // serves files from wwwroot
app.MapHub<EmployeeHub>("/employeeHub");
app.MapControllers();
app.Run();
