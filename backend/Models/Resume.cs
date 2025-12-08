using System.ComponentModel.DataAnnotations;

namespace ResumeAnalyzerAPI.Models
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
} 