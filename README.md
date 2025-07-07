# Resume Analyzer API

An AI-powered RESTful API built with ASP.NET Core for analyzing resumes (PDF, DOCX).  
Features JWT authentication, file upload, text extraction, and mock NLP analysis.

---

## Features

- **User Registration & Login** (JWT authentication)
- **Resume Upload** (PDF, DOCX)
- **Text Extraction** from resumes
- **Mock NLP Analysis** (keywords, sentiment, suggestions)
- **Extensible** for real NLP and storage integrations

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQLite (default, or change in `appsettings.json`)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/NickiMash17/resume-analyzer-api.git
   cd resume-analyzer-api
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the API**
   ```bash
   dotnet run
   ```

---

## API Usage

### 1. Register a User

**POST** `/api/auth/register`  
**Body:**
```json
{
  "email": "user@example.com",
  "password": "YourPassword123!",
  "fullName": "User Name"
}
```

### 2. Log In

**POST** `/api/auth/login`  
**Body:**
```json
{
  "email": "user@example.com",
  "password": "YourPassword123!"
}
```
**Response:**  
```json
{
  "token": "<JWT_TOKEN>",
  "expiresIn": 3600
}
```

### 3. Analyze a Resume

**POST** `/api/analysis/analyze`  
**Headers:**  
`Authorization: Bearer <JWT_TOKEN>`

**Body:**  
`form-data` with a key named `file` and a PDF or DOCX file as the value.

**Example (curl):**
```bash
curl -X POST "http://localhost:5000/api/analysis/analyze" \
  -H "Authorization: Bearer <JWT_TOKEN>" \
  -F "file=@/path/to/resume.pdf"
```

**Response:**
```json
{
  "success": true,
  "data": {
    "keywords": ["C#", "ASP.NET", "SQL"],
    "entities": ["Microsoft", "Developer"],
    "sentimentScore": 0.8,
    "overallScore": 0.85,
    "suggestions": [
      "Add more leadership experience",
      "Highlight teamwork skills"
    ]
  }
}
```

---

## Extending

- Replace mock NLP logic in `NlpService` with a real NLP API.
- Add more file type support in `ResumeParserService`.
- Add storage (e.g., Azure Blob) or job description comparison.

---

## License

MIT