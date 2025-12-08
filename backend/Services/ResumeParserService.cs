using Microsoft.AspNetCore.Http;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ResumeAnalyzerAPI.Services
{
    public class ResumeParserService : IResumeParser
    {
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string[] _allowedExtensions = { ".pdf", ".docx" };

        public async Task<string> ParseResumeAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty or null");

            if (file.Length > MaxFileSize)
                throw new ArgumentException($"File size exceeds maximum limit of {MaxFileSize / 1024 / 1024}MB");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                throw new NotSupportedException($"File type {extension} is not supported. Only PDF and DOCX files are allowed.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            return extension switch
            {
                ".pdf" => await ParsePdfAsync(stream),
                ".docx" => await ParseDocxAsync(stream),
                _ => throw new NotSupportedException($"Unsupported file type: {extension}")
            };
        }

        private async Task<string> ParsePdfAsync(Stream stream)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var document = PdfDocument.Open(stream);
                    var textBuilder = new System.Text.StringBuilder();

                    foreach (var page in document.GetPages())
                    {
                        var pageText = page.Text;
                        textBuilder.AppendLine(pageText);
                    }

                    return textBuilder.ToString().Trim();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to parse PDF: {ex.Message}", ex);
                }
            });
        }

        private async Task<string> ParseDocxAsync(Stream stream)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var wordDocument = WordprocessingDocument.Open(stream, false);
                    var body = wordDocument.MainDocumentPart?.Document?.Body;

                    if (body == null)
                        return string.Empty;

                    var textBuilder = new System.Text.StringBuilder();

                    foreach (var paragraph in body.Elements<Paragraph>())
                    {
                        var paragraphText = string.Join(" ", paragraph.Elements<Run>()
                            .SelectMany(r => r.Elements<Text>())
                            .Select(t => t.Text));

                        if (!string.IsNullOrWhiteSpace(paragraphText))
                        {
                            textBuilder.AppendLine(paragraphText);
                        }
                    }

                    return textBuilder.ToString().Trim();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to parse DOCX: {ex.Message}", ex);
                }
            });
        }
    }
} 