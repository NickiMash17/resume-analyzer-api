# Resume Analyzer API

An AI-powered RESTful API built with ASP.NET Core for analyzing resumes (PDF, DOCX).
Features JWT authentication, file upload, text extraction, and mock NLP analysis.

---

## 🚀 Features

- **User Registration & Login** (JWT authentication)
- **Resume Upload & Parsing** (PDF, DOCX)
- **Text Extraction** from resumes
- **Mock NLP Analysis** (keywords, sentiment, suggestions)
- **Swagger/OpenAPI Documentation** (interactive API docs)
- **SQLite Database Integration**
- **Extensible Service Architecture**
- **Minimal Test Controller** for file upload verification

---

## 🛠️ Technologies Used

- **.NET 8 / ASP.NET Core** — Main framework
- **Entity Framework Core** — ORM (with SQLite)
- **Swashbuckle.AspNetCore** — Swagger/OpenAPI docs
- **JWT (JSON Web Tokens)** — Authentication
- **BCrypt.Net-Next** — Password hashing
- **UglyToad.PdfPig** — PDF text extraction
- **DocumentFormat.OpenXml** — DOCX parsing
- **Microsoft.AspNetCore.Http** — File uploads
- **SQLite** — Lightweight database

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

## API Documentation & Testing

### Swagger UI
After running the API, access the interactive documentation at:
```
http://localhost:5065/swagger
```
- Try out endpoints directly in your browser, including file uploads.
- The `/api/analysis/analyze` and `/api/test/upload` endpoints accept file uploads via `multipart/form-data`.

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
`form-data` with:
- `File`: (your PDF or DOCX file)
- `Description`: (optional string)

**Example (curl):**
```bash
curl -X POST "http://localhost:5065/api/analysis/analyze" \
  -H "Authorization: Bearer <JWT_TOKEN>" \
  -F "File=@/path/to/resume.pdf" \
  -F "Description=My latest resume"
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

### 4. Minimal File Upload Test
**POST** `/api/test/upload`
- Use Swagger UI or `curl` to test file upload compatibility.

---

## Troubleshooting Swagger UI
If you see errors like:
> Unable to render this definition
> The provided definition does not specify a valid version field.

**Try the following:**
- Hard refresh the Swagger UI page (`Ctrl+Shift+R` or `Cmd+Shift+R`).
- Open Swagger UI in a private/incognito window.
- Try a different browser (Chrome is recommended).
- Ensure you have no custom Swagger UI files in your project (`wwwroot/swagger`).
- Check the raw OpenAPI JSON at `http://localhost:5065/swagger/v1/swagger.json`—it should start with `"openapi": "3.x.x"`.

---

## Extending
- Replace mock NLP logic in `NlpService` with a real NLP API.
- Add more file type support in `ResumeParserService`.
- Add storage (e.g., Azure Blob) or job description comparison.

---

## License
MIT
