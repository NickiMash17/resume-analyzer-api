namespace ResumeAnalyzerAPI.Models
{
    public class AnalysisResult
    {
        public string[] Keywords { get; set; }
        public string[] Entities { get; set; }
        public double SentimentScore { get; set; }
        public double OverallScore { get; set; }
        public string[] Suggestions { get; set; }
    }
} 