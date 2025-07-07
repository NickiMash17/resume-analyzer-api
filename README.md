# Resume Analyzer API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![API](https://img.shields.io/badge/API-RESTful-blue)](https://restfulapi.net/)

An AI-powered RESTful API built with ASP.NET Core 8 for comprehensive resume analysis. Upload PDF or DOCX files and get detailed insights including keyword extraction, sentiment analysis, and improvement suggestions.

## 🚀 Features

- **🔐 JWT Authentication** - Secure user registration and login
- **📄 Multi-format Support** - PDF and DOCX resume uploads
- **🔍 Text Extraction** - Advanced document parsing capabilities
- **🧠 NLP Analysis** - Keyword extraction, sentiment analysis, and scoring
- **💡 AI Suggestions** - Intelligent resume improvement recommendations
- **📊 Swagger Documentation** - Interactive API documentation
- **🔧 Extensible Architecture** - Easy integration with external NLP services
- **💾 Database Integration** - SQLite with Entity Framework Core

## 📋 Table of Contents

- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Authentication](#authentication)
- [Endpoints](#endpoints)
- [File Upload](#file-upload)
- [Response Format](#response-format)
- [Development](#development)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

## 🏁 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- SQLite (included by default)
- Any REST client (Postman, curl, etc.)

### 🛠️ Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/NickiMash17/resume-analyzer-api.git
   cd resume-analyzer-api
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Update configuration** (optional)
   ```bash
   # Edit appsettings.json for custom settings
   cp appsettings.json appsettings.Development.json
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP).

## 📚 API Documentation

### Swagger UI

Interactive API documentation is available at:
```
https://localhost:5001/swagger
```

Features:
- **Try it out** functionality for all endpoints
- **File upload testing** directly in the browser
- **JWT authentication** integration
- **Request/response examples**

### OpenAPI Specification

Raw OpenAPI JSON specification:
```
https://localhost:5001/swagger/v1/swagger.json
```

## 🔐 Authentication

This API uses JWT (JSON Web Tokens) for authentication. Include the token in the `Authorization` header:

```
Authorization: Bearer <your-jwt-token>
```

## 🔗 Endpoints

### Authentication Endpoints

#### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "fullName": "John Doe"
}
```

**Response:**
```json
{
  "success": true,
  "message": "User registered successfully",
  "data": {
    "userId": "12345",
    "email": "user@example.com"
  }
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600,
    "user": {
      "id": "12345",
      "email": "user@example.com",
      "fullName": "John Doe"
    }
  }
}
```

### Analysis Endpoints

#### Analyze Resume
```http
POST /api/analysis/analyze
Authorization: Bearer <jwt-token>
Content-Type: multipart/form-data

File: <resume-file.pdf|docx>
Description: "Updated resume for software engineer position"
```

**Response:**
```json
{
  "success": true,
  "data": {
    "analysisId": "analysis-123",
    "fileName": "resume.pdf",
    "uploadedAt": "2024-07-07T10:30:00Z",
    "keywords": [
      "C#", "ASP.NET Core", "SQL Server", "JavaScript", 
      "React", "Git", "Agile", "API Development"
    ],
    "entities": [
      "Microsoft", "Google", "Software Engineer", 
      "Bachelor's Degree", "Project Manager"
    ],
    "sentimentScore": 0.82,
    "overallScore": 0.87,
    "metrics": {
      "experienceLevel": "Senior",
      "skillsCount": 15,
      "educationLevel": "Bachelor's",
      "keywordDensity": 0.045
    },
    "suggestions": [
      "Add more quantifiable achievements (e.g., 'Increased performance by 40%')",
      "Include leadership experience and team management skills",
      "Add certifications relevant to your field",
      "Highlight soft skills like communication and problem-solving"
    ],
    "sections": {
      "hasContactInfo": true,
      "hasSummary": true,
      "hasExperience": true,
      "hasEducation": true,
      "hasSkills": true,
      "hasProjects": false
    }
  }
}
```

## 📤 File Upload

### Supported Formats
- **PDF** (.pdf)
- **DOCX** (.docx)

### Size Limits
- Maximum file size: **10MB**
- Maximum files per request: **1**

### Upload Examples

#### Using curl
```bash
curl -X POST "https://localhost:5001/api/analysis/analyze" \
  -H "Authorization: Bearer <your-jwt-token>" \
  -F "File=@/path/to/resume.pdf" \
  -F "Description=Software Engineer Resume"
```

#### Using PowerShell
```powershell
$headers = @{ Authorization = "Bearer <your-jwt-token>" }
$form = @{
    File = Get-Item -Path "C:\path\to\resume.pdf"
    Description = "Software Engineer Resume"
}
Invoke-RestMethod -Uri "https://localhost:5001/api/analysis/analyze" `
  -Method Post -Headers $headers -Form $form
```

## 📋 Response Format

All API responses follow a consistent format:

### Success Response
```json
{
  "success": true,
  "data": { /* response data */ },
  "message": "Operation completed successfully"
}
```

### Error Response
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "The uploaded file format is not supported",
    "details": ["Only PDF and DOCX files are allowed"]
  }
}
```

### Common Error Codes
- `VALIDATION_ERROR` - Invalid input data
- `UNAUTHORIZED` - Missing or invalid authentication
- `FILE_TOO_LARGE` - File exceeds size limit
- `UNSUPPORTED_FORMAT` - File format not supported
- `PROCESSING_ERROR` - Error during file processing

## 🛠️ Development

### Configuration

Key configuration options in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=resumeanalyzer.db"
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "ResumeAnalyzer",
    "ExpiryInHours": 24
  },
  "FileUpload": {
    "MaxFileSizeInMB": 10,
    "AllowedExtensions": [".pdf", ".docx"]
  }
}
```

### Adding New NLP Providers

1. Create a new service implementing `INlpService`
2. Register it in `Program.cs`
3. Configure the provider in `appsettings.json`

```csharp
public class CustomNlpService : INlpService
{
    public async Task<AnalysisResult> AnalyzeAsync(string text)
    {
        // Your NLP implementation
    }
}
```

### Database Migrations

Create new migration:
```bash
dotnet ef migrations add YourMigrationName
```

Update database:
```bash
dotnet ef database update
```

## 🔧 Troubleshooting

### Swagger UI Issues

If Swagger UI shows errors like "Unable to render this definition":

1. **Hard refresh** the page (`Ctrl+Shift+R` or `Cmd+Shift+R`)
2. **Try incognito mode** to bypass cache issues
3. **Check browser compatibility** (Chrome recommended)
4. **Verify OpenAPI JSON** at `/swagger/v1/swagger.json`

### File Upload Problems

Common issues and solutions:

| Problem | Solution |
|---------|----------|
| File too large | Check `MaxFileSizeInMB` in configuration |
| Unsupported format | Ensure file is PDF or DOCX |
| Upload timeout | Increase timeout in `appsettings.json` |
| Memory issues | Implement streaming for large files |

### Authentication Issues

- **Token expired**: Re-login to get a new token
- **Invalid token**: Check token format and signing key
- **Missing header**: Ensure `Authorization: Bearer <token>` is included

## 🔮 Future Enhancements

- **Real NLP Integration** - Azure Cognitive Services, OpenAI GPT
- **Advanced Analytics** - Job matching, salary predictions
- **Multiple File Formats** - TXT, RTF, HTML support
- **Cloud Storage** - Azure Blob, AWS S3 integration
- **Batch Processing** - Multiple file analysis
- **Resume Templates** - AI-generated resume suggestions
- **Export Features** - PDF reports, Excel analytics

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with [ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- Document parsing with [iText7](https://itextpdf.com/) and [DocumentFormat.OpenXml](https://github.com/OfficeDev/Open-XML-SDK)
- Authentication with [JWT](https://jwt.io/)
- Documentation with [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

---

<div align="center">
Made with ❤️ by <a href="https://github.com/NickiMash17">NickiMash17</a>
</div>