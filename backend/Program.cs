using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ResumeAnalyzerAPI.Data;
using ResumeAnalyzerAPI.Services;
using System.Linq;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Ensure absolute path for SQLite
if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("Data Source="))
{
    var dbFileName = connectionString.Split('=').LastOrDefault()?.Trim();
    if (!string.IsNullOrEmpty(dbFileName) && !Path.IsPathRooted(dbFileName))
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), dbFileName);
        connectionString = $"Data Source={fullPath}";
        Console.WriteLine($"Adjusted connection string to use absolute path: {connectionString}");
    }
}
else
{
    Console.WriteLine($"Using connection string as-is: {connectionString}");
}
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IResumeParser, ResumeParserService>();
builder.Services.AddScoped<INlpService, NlpService>();
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Ensure database is created - SIMPLIFIED APPROACH
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        Console.WriteLine("=== DATABASE INITIALIZATION START ===");
        
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "resumeanalyzer.db");
        Console.WriteLine($"Database path: {dbPath}");
        
        // Delete empty/corrupted database
        if (File.Exists(dbPath))
        {
            var fileInfo = new FileInfo(dbPath);
            if (fileInfo.Length == 0)
            {
                Console.WriteLine("Deleting empty database file...");
                File.Delete(dbPath);
            }
        }
        
        // ULTRA-SIMPLE: Just call EnsureCreated and verify it worked
        Console.WriteLine("=== DATABASE INITIALIZATION ===");
        Console.WriteLine($"Database path: {dbPath}");
        
        try
        {
            // Delete empty database if it exists
            if (File.Exists(dbPath))
            {
                var fileInfo = new FileInfo(dbPath);
                if (fileInfo.Length == 0)
                {
                    Console.WriteLine("Deleting empty database file...");
                    File.Delete(dbPath);
                }
            }
            
            // Force create database and tables
            Console.WriteLine("Calling EnsureCreated()...");
            var created = dbContext.Database.EnsureCreated();
            Console.WriteLine($"EnsureCreated returned: {created}");
            
            // Force a connection to ensure database file is written
            Console.WriteLine("Opening database connection...");
            dbContext.Database.OpenConnection();
            Console.WriteLine("Connection opened.");
            
            // Execute a simple query to force table creation
            Console.WriteLine("Executing test query to verify tables exist...");
            try
            {
                var testQuery = dbContext.Database.ExecuteSqlRaw("SELECT COUNT(*) FROM Users");
                Console.WriteLine($"Test query executed: {testQuery}");
            }
            catch (Exception testEx)
            {
                Console.WriteLine($"Test query failed (this is OK if tables don't exist yet): {testEx.Message}");
            }
            
            // Now create tables manually if they don't exist
            Console.WriteLine("Creating tables via raw SQL...");
            dbContext.Database.ExecuteSqlRaw(@"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Email TEXT NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    FullName TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL
                );
            ");
            Console.WriteLine("Users table created.");
            
            dbContext.Database.ExecuteSqlRaw(@"
                CREATE TABLE IF NOT EXISTS Resumes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FileName TEXT NOT NULL,
                    FilePath TEXT NOT NULL,
                    UploadedBy TEXT NOT NULL,
                    UploadedAt TEXT NOT NULL
                );
            ");
            Console.WriteLine("Resumes table created.");
            
            dbContext.Database.CloseConnection();
            Console.WriteLine("Connection closed.");
            
            // Verify
            var userCount = dbContext.Users.Count();
            Console.WriteLine($"✅ SUCCESS! Database ready. User count: {userCount}");
            
            if (File.Exists(dbPath))
            {
                var fileInfo = new FileInfo(dbPath);
                Console.WriteLine($"✅ Database file size: {fileInfo.Length} bytes");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ ERROR: {ex.Message}");
            Console.WriteLine($"Type: {ex.GetType().Name}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner: {ex.InnerException.Message}");
            }
            Console.WriteLine($"Stack: {ex.StackTrace}");
        }
        
        // Verify tables exist
        try
        {
            var userCount = dbContext.Users.Count();
            Console.WriteLine($"Database is ready. Current user count: {userCount}");
            
            if (File.Exists(dbPath))
            {
                var fileInfo = new FileInfo(dbPath);
                Console.WriteLine($"Database file size: {fileInfo.Length} bytes");
            }
        }
        catch (Exception verifyEx)
        {
            Console.WriteLine($"ERROR verifying database: {verifyEx.Message}");
            Console.WriteLine($"This means tables still don't exist!");
        }
        
        Console.WriteLine("=== DATABASE INITIALIZATION END ===");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"CRITICAL ERROR initializing database:");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Type: {ex.GetType().Name}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner: {ex.InnerException.Message}");
    }
    Console.WriteLine($"Stack: {ex.StackTrace}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AI Resume Analyzer API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
