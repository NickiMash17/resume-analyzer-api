using ResumeAnalyzerAPI.Models;
using System.Text.RegularExpressions;

namespace ResumeAnalyzerAPI.Services
{
    public class NlpService : INlpService
    {
        private readonly string[] _commonTechKeywords = {
            "C#", "C++", "Java", "Python", "JavaScript", "TypeScript", "React", "Angular", "Vue",
            "ASP.NET", ".NET", "Node.js", "Express", "Spring", "Django", "Flask",
            "SQL", "PostgreSQL", "MySQL", "MongoDB", "Redis", "SQL Server", "Oracle",
            "Azure", "AWS", "GCP", "Docker", "Kubernetes", "CI/CD", "Git", "GitHub",
            "REST API", "GraphQL", "Microservices", "Agile", "Scrum", "DevOps",
            "HTML", "CSS", "SASS", "Bootstrap", "Tailwind", "Webpack", "npm", "yarn",
            "Entity Framework", "LINQ", "WPF", "WinForms", "Xamarin", ".NET Core",
            "Machine Learning", "AI", "TensorFlow", "PyTorch", "Data Science"
        };

        private readonly string[] _commonSkills = {
            "Leadership", "Team Management", "Project Management", "Communication",
            "Problem Solving", "Analytical Thinking", "Agile Methodology", "Scrum",
            "Software Development", "Code Review", "Testing", "QA", "Debugging",
            "System Design", "Architecture", "Performance Optimization", "Security"
        };

        public async Task<AnalysisResult> AnalyzeTextAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new AnalysisResult
                {
                    Keywords = Array.Empty<string>(),
                    Entities = Array.Empty<string>(),
                    SentimentScore = 0.0,
                    OverallScore = 0.0,
                    Suggestions = new[] { "Resume appears to be empty or could not be parsed." }
                };
            }

            return await Task.Run(() =>
            {
                var normalizedText = text.ToLowerInvariant();
                
                // Extract keywords
                var keywords = ExtractKeywords(normalizedText);
                
                // Extract entities (companies, roles, etc.)
                var entities = ExtractEntities(text);
                
                // Calculate sentiment score
                var sentimentScore = CalculateSentimentScore(normalizedText);
                
                // Calculate overall score
                var overallScore = CalculateOverallScore(keywords, entities, sentimentScore, text);
                
                // Generate suggestions
                var suggestions = GenerateSuggestions(keywords, entities, sentimentScore, text);

                return new AnalysisResult
                {
                    Keywords = keywords.ToArray(),
                    Entities = entities.ToArray(),
                    SentimentScore = sentimentScore,
                    OverallScore = overallScore,
                    Suggestions = suggestions.ToArray()
                };
            });
        }

        private List<string> ExtractKeywords(string text)
        {
            var foundKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            // Check for technical keywords
            foreach (var keyword in _commonTechKeywords)
            {
                if (text.Contains(keyword.ToLowerInvariant()))
                {
                    foundKeywords.Add(keyword);
                }
            }

            // Check for common skills
            foreach (var skill in _commonSkills)
            {
                if (text.Contains(skill.ToLowerInvariant()))
                {
                    foundKeywords.Add(skill);
                }
            }

            // Extract years of experience patterns
            var experiencePattern = new Regex(@"(\d+)\+?\s*(years?|yrs?)\s*(of\s*)?(experience|exp)", RegexOptions.IgnoreCase);
            var experienceMatches = experiencePattern.Matches(text);
            foreach (Match match in experienceMatches)
            {
                foundKeywords.Add($"{match.Groups[1].Value} years experience");
            }

            return foundKeywords.OrderBy(k => k).ToList();
        }

        private List<string> ExtractEntities(string text)
        {
            var entities = new HashSet<string>();

            // Extract email addresses
            var emailPattern = new Regex(@"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b");
            var emails = emailPattern.Matches(text);
            foreach (Match email in emails)
            {
                entities.Add(email.Value);
            }

            // Extract potential company names (words starting with capital letters, 2+ words)
            var companyPattern = new Regex(@"\b[A-Z][a-z]+(?:\s+[A-Z][a-z]+)+\b");
            var companies = companyPattern.Matches(text);
            foreach (Match company in companies)
            {
                if (company.Value.Length > 5 && !_commonTechKeywords.Contains(company.Value))
                {
                    entities.Add(company.Value);
                }
            }

            // Extract job titles
            var jobTitlePattern = new Regex(@"\b(Senior|Junior|Lead|Principal|Staff|Associate)?\s*(Software|Senior|Junior|Lead|Principal|Staff|Associate)?\s*(Engineer|Developer|Programmer|Architect|Manager|Analyst|Consultant|Specialist)\b", RegexOptions.IgnoreCase);
            var jobTitles = jobTitlePattern.Matches(text);
            foreach (Match title in jobTitles)
            {
                entities.Add(title.Value.Trim());
            }

            return entities.Take(10).ToList(); // Limit to top 10
        }

        private double CalculateSentimentScore(string text)
        {
            var positiveWords = new[] { "achieved", "successful", "excellent", "improved", "led", "managed", "developed", "created", "implemented", "optimized", "exceeded", "awarded", "recognized" };
            var negativeWords = new[] { "failed", "problem", "issue", "error", "difficult", "challenge", "limited", "lack" };

            var words = text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var wordCount = words.Length;
            
            if (wordCount == 0) return 0.5;

            var positiveCount = words.Count(w => positiveWords.Any(p => w.Contains(p)));
            var negativeCount = words.Count(w => negativeWords.Any(n => w.Contains(n)));

            var baseScore = 0.5;
            var positiveBoost = (positiveCount / (double)wordCount) * 0.4;
            var negativePenalty = (negativeCount / (double)wordCount) * 0.3;

            var score = Math.Max(0.0, Math.Min(1.0, baseScore + positiveBoost - negativePenalty));
            
            // Boost score if resume has good structure indicators
            if (text.Contains("experience") && text.Contains("education") && text.Contains("skills"))
            {
                score = Math.Min(1.0, score + 0.1);
            }

            return Math.Round(score, 2);
        }

        private double CalculateOverallScore(List<string> keywords, List<string> entities, double sentimentScore, string text)
        {
            var score = 0.0;

            // Keyword diversity (0-0.3)
            var keywordScore = Math.Min(0.3, keywords.Count / 30.0);
            score += keywordScore;

            // Entity presence (0-0.2)
            var entityScore = Math.Min(0.2, entities.Count / 10.0);
            score += entityScore;

            // Sentiment score (0-0.3)
            score += sentimentScore * 0.3;

            // Resume length and structure (0-0.2)
            var wordCount = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            var lengthScore = wordCount switch
            {
                < 100 => 0.05,
                < 300 => 0.1,
                < 600 => 0.15,
                _ => 0.2
            };
            score += lengthScore;

            return Math.Round(Math.Min(1.0, score), 2);
        }

        private List<string> GenerateSuggestions(List<string> keywords, List<string> entities, double sentimentScore, string text)
        {
            var suggestions = new List<string>();

            if (keywords.Count < 5)
            {
                suggestions.Add("Consider adding more technical skills and keywords relevant to your target role.");
            }

            if (entities.Count < 2)
            {
                suggestions.Add("Include more specific company names, projects, or achievements to strengthen your resume.");
            }

            if (sentimentScore < 0.6)
            {
                suggestions.Add("Use more action verbs and achievement-oriented language to improve the overall tone.");
            }

            var wordCount = text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (wordCount < 200)
            {
                suggestions.Add("Your resume seems brief. Consider adding more details about your experience and accomplishments.");
            }
            else if (wordCount > 1000)
            {
                suggestions.Add("Your resume may be too lengthy. Consider condensing to highlight the most relevant information.");
            }

            if (!text.Contains("experience", StringComparison.OrdinalIgnoreCase))
            {
                suggestions.Add("Make sure to include a clear 'Experience' section with detailed work history.");
            }

            if (!text.Contains("education", StringComparison.OrdinalIgnoreCase))
            {
                suggestions.Add("Include an 'Education' section with your academic qualifications.");
            }

            if (!text.Contains("skill", StringComparison.OrdinalIgnoreCase))
            {
                suggestions.Add("Add a dedicated 'Skills' section to highlight your technical and soft skills.");
            }

            // Check for quantifiable achievements
            var numberPattern = new Regex(@"\d+");
            var hasNumbers = numberPattern.IsMatch(text);
            if (!hasNumbers)
            {
                suggestions.Add("Include quantifiable achievements (e.g., 'increased performance by 30%', 'managed team of 5') to make your resume more impactful.");
            }

            if (suggestions.Count == 0)
            {
                suggestions.Add("Your resume looks good! Consider tailoring it for specific job applications by emphasizing relevant keywords.");
            }

            return suggestions.Take(5).ToList();
        }
    }
} 