using Microsoft.EntityFrameworkCore;
using ResumeAnalyzerAPI.Models;

namespace ResumeAnalyzerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Resume> Resumes { get; set; }
    }
} 