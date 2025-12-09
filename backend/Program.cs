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

// Ensure database is created - RELIABLE APPROACH
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        Console.WriteLine("=== DATABASE INITIALIZATION START ===");
        
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "resumeanalyzer.db");
        Console.WriteLine($"Database path: {dbPath}");
        
        // Delete corrupted/empty database if it exists
        if (File.Exists(dbPath))
        {
            var fileInfo = new FileInfo(dbPath);
            if (fileInfo.Length == 0)
            {
                Console.WriteLine("Deleting empty database file...");
                File.Delete(dbPath);
            }
        }
        
        try
        {
            // Ensure database file exists
            dbContext.Database.EnsureCreated();
            
            // Open connection explicitly
            if (dbContext.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
            {
                dbContext.Database.OpenConnection();
            }
            
            // Always create tables using raw SQL (CREATE TABLE IF NOT EXISTS is safe)
            Console.WriteLine("Creating Users table...");
            dbContext.Database.ExecuteSqlRaw(@"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Email TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    FullName TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
                );
            ");
            Console.WriteLine("✅ Users table created/verified.");
            
            Console.WriteLine("Creating Resumes table...");
            dbContext.Database.ExecuteSqlRaw(@"
                CREATE TABLE IF NOT EXISTS Resumes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FileName TEXT NOT NULL,
                    FilePath TEXT NOT NULL,
                    UploadedBy TEXT NOT NULL,
                    UploadedAt TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
                );
            ");
            Console.WriteLine("✅ Resumes table created/verified.");
            
            // Verify tables exist by querying them
            var userCount = dbContext.Users.Count();
            Console.WriteLine($"✅ Database ready! Current user count: {userCount}");
            
            if (File.Exists(dbPath))
            {
                var fileInfo = new FileInfo(dbPath);
                Console.WriteLine($"✅ Database file size: {fileInfo.Length} bytes");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ ERROR during database initialization:");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"Type: {ex.GetType().Name}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner: {ex.InnerException.Message}");
            }
            throw; // Re-throw to prevent app from starting with broken database
        }
        finally
        {
            // Ensure connection is closed
            try
            {
                if (dbContext.Database.GetDbConnection().State == System.Data.ConnectionState.Open)
                {
                    dbContext.Database.CloseConnection();
                }
            }
            catch { }
        }
        
        Console.WriteLine("=== DATABASE INITIALIZATION END ===");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ CRITICAL ERROR initializing database:");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Type: {ex.GetType().Name}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner: {ex.InnerException.Message}");
    }
    Console.WriteLine($"Stack: {ex.StackTrace}");
    // Don't throw - let the app start but log the error
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
