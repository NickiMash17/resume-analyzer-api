# 🎯 Resume Analyzer API

<div align="center">
  
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![API](https://img.shields.io/badge/API-RESTful-blue)](https://restfulapi.net/)
[![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen)](https://github.com/NickiMash17/resume-analyzer-api)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](CONTRIBUTING.md)

*Transform resume analysis with AI-powered insights*

**[📖 Documentation](#-api-documentation--testing) • [🚀 Quick Start](#-quick-start) • [🔧 API Reference](#-api-reference) • [🤝 Contributing](#-contributing)**

</div>

---

## 💡 Overview

The **Resume Analyzer API** is a sophisticated, AI-powered solution that revolutionizes how organizations process and analyze resumes. Built with modern .NET 8 architecture, this RESTful API seamlessly extracts, analyzes, and provides actionable insights from resume documents.

### 🎨 What Makes It Special

- **🤖 Intelligent Analysis**: Advanced NLP processing for keyword extraction and sentiment analysis
- **📄 Universal Format Support**: Handles PDF and DOCX files with ease
- **🔐 Enterprise Security**: JWT-based authentication with BCrypt password hashing
- **⚡ Performance Optimized**: Lightweight SQLite database with Entity Framework Core
- **🎯 Developer Friendly**: Interactive Swagger documentation and intuitive API design
- **🏗️ Scalable Architecture**: Clean, extensible service-oriented design

---

## 🚀 Features

<table>
<tr>
<td width="50%">

### 🔐 Authentication & Security
- **JWT Token-based Auth** — Secure user sessions
- **BCrypt Password Hashing** — Industry-standard security
- **Role-based Authorization** — Granular access control

### 📄 Document Processing
- **Multi-format Support** — PDF & DOCX compatibility
- **Intelligent Text Extraction** — Clean, structured content parsing
- **File Upload Validation** — Size and type restrictions

</td>
<td width="50%">

### 🧠 AI-Powered Analysis
- **Keyword Extraction** — Identify key skills and technologies
- **Sentiment Analysis** — Gauge resume tone and confidence
- **Smart Suggestions** — Actionable improvement recommendations

### 🛠️ Developer Experience
- **Interactive Documentation** — Swagger/OpenAPI integration
- **RESTful Design** — Clean, predictable endpoints
- **Comprehensive Testing** — Built-in test controllers

</td>
</tr>
</table>

---

## 🛠️ Technology Stack

<div align="center">

| Category | Technology | Purpose |
|----------|------------|---------|
| **Core Framework** | .NET 8 / ASP.NET Core | High-performance web API |
| **Database** | Entity Framework Core + SQLite | Lightweight ORM solution |
| **Authentication** | JWT + BCrypt.Net-Next | Secure user management |
| **Documentation** | Swashbuckle.AspNetCore | Interactive API docs |
| **File Processing** | UglyToad.PdfPig + DocumentFormat.OpenXml | Document parsing |
| **NLP Engine** | Custom Service (Extensible) | Text analysis & insights |

</div>

---

## 🚀 Quick Start

### 📋 Prerequisites

Ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (Latest LTS)
- Git for version control
- Your favorite IDE (Visual Studio, VS Code, or JetBrains Rider)

### ⚡ Installation

```bash
# 1. Clone the repository
git clone https://github.com/NickiMash17/resume-analyzer-api.git
cd resume-analyzer-api

# 2. Restore NuGet packages
dotnet restore

# 3. Set up the database
dotnet ef database update

# 4. Launch the API
dotnet run
```

### 🎉 Verify Installation

Open your browser and navigate to:
```
http://localhost:5065/swagger
```

You should see the interactive API documentation! 🎊

---

## 📖 API Documentation & Testing

### 🌐 Swagger UI

Experience the API through our interactive documentation:

```
🔗 http://localhost:5065/swagger
```

**Features:**
- 🔧 **Try It Out** — Test endpoints directly in your browser
- 📤 **File Upload Testing** — Drag & drop resume files
- 🔐 **Authentication Testing** — JWT token management
- 📊 **Response Visualization** — Real-time API responses

### 🧪 Testing Endpoints

The API includes dedicated test controllers for development:
- `/api/test/upload` — Verify file upload functionality
- Health check endpoints for monitoring

---

## 🔧 API Reference

### 👤 Authentication Flow

<details>
<summary><strong>POST</strong> <code>/api/auth/register</code> — Create New User</summary>

**Request Body:**
```json
{
  "email": "developer@example.com",
  "password": "SecurePassword123!",
  "fullName": "John Developer"
}
```

**Response:**
```json
{
  "success": true,
  "message": "User registered successfully",
  "userId": "abc123"
}
```
</details>

<details>
<summary><strong>POST</strong> <code>/api/auth/login</code> — User Authentication</summary>

**Request Body:**
```json
{
  "email": "developer@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```
</details>

### 📊 Resume Analysis

<details>
<summary><strong>POST</strong> <code>/api/analysis/analyze</code> — Analyze Resume</summary>

**Headers:**
```
Authorization: Bearer <JWT_TOKEN>
Content-Type: multipart/form-data
```

**Form Data:**
- `File`: Resume file (PDF/DOCX, max 5MB)
- `Description`: Optional context (string)

**cURL Example:**
```bash
curl -X POST "http://localhost:5065/api/analysis/analyze" \
  -H "Authorization: Bearer <JWT_TOKEN>" \
  -F "File=@resume.pdf" \
  -F "Description=Senior Developer Position"
```

**Response:**
```json
{
  "success": true,
  "data": {
    "keywords": ["C#", "ASP.NET", "SQL", "Azure", "React"],
    "entities": ["Microsoft", "Google", "Senior Developer"],
    "sentimentScore": 0.85,
    "overallScore": 0.92,
    "suggestions": [
      "Consider adding cloud platform certifications",
      "Highlight leadership and mentoring experience",
      "Include specific project metrics and achievements"
    ],
    "analysisId": "analysis_abc123",
    "processedAt": "2024-07-08T14:30:00Z"
  }
}
```
</details>

---

## 🔍 Error Handling

The API uses consistent error responses:

```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "File size exceeds maximum limit of 5MB",
    "details": ["File must be PDF or DOCX format"]
  }
}
```

**Common Error Codes:**
- `AUTHENTICATION_FAILED` — Invalid credentials
- `UNAUTHORIZED` — Missing or invalid JWT token
- `VALIDATION_ERROR` — Request validation failure
- `FILE_PROCESSING_ERROR` — Document parsing issues

---

## 🚨 Troubleshooting

### Swagger UI Issues

If you encounter rendering problems:

1. **Hard Refresh**: `Ctrl+Shift+R` (Windows/Linux) or `Cmd+Shift+R` (Mac)
2. **Private Browser**: Try incognito/private mode
3. **Different Browser**: Chrome is recommended
4. **Clear Cache**: Remove browser cache and cookies
5. **Check Raw JSON**: Visit `http://localhost:5065/swagger/v1/swagger.json`

### Common Issues

<details>
<summary><strong>Database Connection Error</strong></summary>

**Problem:** SQLite database not found
**Solution:** Run `dotnet ef database update`
</details>

<details>
<summary><strong>File Upload Fails</strong></summary>

**Problem:** File upload returns 400 error
**Solution:** Check file size (<5MB) and format (PDF/DOCX only)
</details>

<details>
<summary><strong>JWT Token Expired</strong></summary>

**Problem:** 401 Unauthorized after some time
**Solution:** Refresh token via `/api/auth/login`
</details>

---

## 🔮 Roadmap & Extensions

### 🎯 Planned Features

- [ ] **Advanced NLP Integration** — OpenAI GPT or Azure Cognitive Services
- [ ] **Batch Processing** — Multiple resume analysis
- [ ] **Job Matching** — Resume-to-job description compatibility
- [ ] **Export Features** — PDF reports and Excel exports
- [ ] **Cloud Storage** — Azure Blob or AWS S3 integration
- [ ] **Real-time Notifications** — WebSocket-based updates
- [ ] **Advanced Analytics** — Dashboard and reporting

### 🔧 Extension Points

```csharp
// Example: Custom NLP Service Implementation
public class OpenAINlpService : INlpService
{
    public async Task<NlpResult> AnalyzeAsync(string text)
    {
        // Your custom NLP logic here
        // Integrate with OpenAI, Azure Cognitive Services, etc.
    }
}
```

---

## 📊 Performance & Scaling

### 🚀 Optimization Tips

- **Database Indexing**: Add indexes for frequently queried fields
- **Caching**: Implement Redis for API response caching
- **File Storage**: Move to cloud storage for production
- **Background Processing**: Use Hangfire for long-running tasks

### 📈 Monitoring

Consider adding:
- **Application Insights** for telemetry
- **Serilog** for structured logging
- **Health Check** endpoints
- **Metrics** collection

---

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details.

### 🐛 Reporting Issues

Found a bug? [Create an issue](https://github.com/NickiMash17/resume-analyzer-api/issues) with:
- Clear description
- Steps to reproduce
- Expected vs actual behavior
- Environment details

### 💡 Feature Requests

Have an idea? [Open a discussion](https://github.com/NickiMash17/resume-analyzer-api/discussions) first!

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- **UglyToad.PdfPig** team for excellent PDF processing
- **OpenXML SDK** contributors
- **.NET Community** for continuous innovation
- **Open Source Community** for inspiration and collaboration

---

<div align="center">
  <h3>🌟 Star this project if you find it helpful!</h3>
  
  **Made with ❤️ and ☕ by [NickiMash17](https://github.com/NickiMash17)**
  
  *Transforming resume analysis, one API call at a time*

  [![GitHub stars](https://img.shields.io/github/stars/NickiMash17/resume-analyzer-api?style=social)](https://github.com/NickiMash17/resume-analyzer-api)
  [![GitHub forks](https://img.shields.io/github/forks/NickiMash17/resume-analyzer-api?style=social)](https://github.com/NickiMash17/resume-analyzer-api)
  [![Follow on GitHub](https://img.shields.io/github/followers/NickiMash17?style=social)](https://github.com/NickiMash17)
</div>