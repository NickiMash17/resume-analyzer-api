# 🎯 AI Resume Analyzer

<div align="center">

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-19-61DAFB?logo=react&logoColor=white)](https://react.dev/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.9-3178C6?logo=typescript&logoColor=white)](https://www.typescriptlang.org/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

*Professional AI-powered resume analysis platform with modern web interface*

**[📖 Documentation](#-documentation) • [🚀 Quick Start](#-quick-start) • [🏗️ Architecture](#️-architecture) • [🤝 Contributing](#-contributing)**

</div>

---

## 💡 Overview

**AI Resume Analyzer** is a full-stack application that provides intelligent resume analysis using advanced NLP techniques. The platform features a modern React frontend and a robust .NET 8 API backend, delivering actionable insights to help job seekers optimize their resumes.

### 🎨 Key Features

- **🤖 Intelligent Analysis**: Advanced NLP processing for keyword extraction, sentiment analysis, and actionable suggestions
- **📄 Multi-Format Support**: Handles PDF and DOCX resume files seamlessly
- **🎨 Modern UI**: Beautiful, responsive React frontend with Tailwind CSS
- **🔐 Secure Authentication**: JWT-based authentication with BCrypt password hashing
- **⚡ High Performance**: Optimized backend with Entity Framework Core and SQLite
- **📊 Detailed Insights**: Comprehensive analysis including keywords, entities, scores, and improvement suggestions
- **🛡️ Production Ready**: Professional architecture with error handling, validation, and CORS configuration

---

## 🏗️ Architecture

### Project Structure

```
ai-resume-analyzer/
├── backend/                 # .NET 8 Web API
│   ├── Controllers/         # API endpoints
│   ├── Services/           # Business logic
│   ├── Models/             # Data models
│   ├── Data/               # Database context
│   └── Migrations/         # EF Core migrations
│
├── frontend/               # React + TypeScript + Vite
│   ├── src/
│   │   ├── components/     # Reusable components
│   │   ├── contexts/       # React contexts (Auth)
│   │   ├── pages/          # Page components
│   │   ├── services/       # API service layer
│   │   └── App.tsx         # Main app component
│   └── package.json
│
└── README.md               # This file
```

### Technology Stack

#### Backend
- **.NET 8** - Modern C# web framework
- **Entity Framework Core** - ORM for database operations
- **SQLite** - Lightweight database
- **JWT Bearer** - Authentication
- **Swagger/OpenAPI** - API documentation
- **UglyToad.PdfPig** - PDF parsing
- **DocumentFormat.OpenXml** - DOCX parsing

#### Frontend
- **React 19** - UI library
- **TypeScript** - Type-safe JavaScript
- **Vite** - Build tool and dev server
- **React Router** - Client-side routing
- **Axios** - HTTP client
- **Tailwind CSS** - Utility-first CSS framework
- **Lucide React** - Icon library

---

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) (Latest LTS)
- [Node.js](https://nodejs.org/) 18+ and npm
- Git for version control

### Installation

#### 1. Clone the Repository

```bash
git clone <repository-url>
cd ai-resume-analyzer
```

#### 2. Backend Setup

```bash
cd backend

# Restore NuGet packages
dotnet restore

# Set up the database
dotnet ef database update

# Run the API (default: http://localhost:5065)
dotnet run
```

The API will be available at `http://localhost:5065` with Swagger UI at `http://localhost:5065/swagger`.

#### 3. Frontend Setup

```bash
cd frontend

# Install dependencies
npm install

# Start development server (default: http://localhost:5173)
npm run dev
```

The frontend will be available at `http://localhost:5173`.

### 🎉 Verify Installation

1. **Backend**: Navigate to `http://localhost:5065/swagger` - you should see the API documentation
2. **Frontend**: Navigate to `http://localhost:5173` - you should see the login page

---

## 📖 Documentation

### API Endpoints

#### Authentication

- `POST /api/auth/register` - Register a new user
  ```json
  {
    "email": "user@example.com",
    "password": "SecurePassword123!",
    "fullName": "John Doe"
  }
  ```

- `POST /api/auth/login` - Authenticate user
  ```json
  {
    "email": "user@example.com",
    "password": "SecurePassword123!"
  }
  ```

#### Resume Analysis

- `POST /api/analysis/analyze` - Analyze resume (requires authentication)
  - **Headers**: `Authorization: Bearer <JWT_TOKEN>`
  - **Content-Type**: `multipart/form-data`
  - **Body**:
    - `File`: Resume file (PDF/DOCX, max 5MB)
    - `Description`: Optional job description

### Frontend Routes

- `/login` - Login page
- `/register` - Registration page
- `/dashboard` - Main dashboard (protected)

---

## 🔧 Configuration

### Backend Configuration

Edit `backend/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=resumeanalyzer.db"
  },
  "Jwt": {
    "Key": "your_super_secret_key_here_change_in_production",
    "Issuer": "AIResumeAnalyzer",
    "Audience": "AIResumeAnalyzerUsers"
  }
}
```

### Frontend Configuration

Create `frontend/.env`:

```env
VITE_API_URL=http://localhost:5065
```

---

## 🧪 Development

### Running in Development Mode

1. **Backend**: `dotnet watch run` (auto-reloads on changes)
2. **Frontend**: `npm run dev` (hot module replacement)

### Building for Production

#### Backend
```bash
cd backend
dotnet publish -c Release -o ./publish
```

#### Frontend
```bash
cd frontend
npm run build
```

The production build will be in `frontend/dist/`.

---

## 🎯 Features in Detail

### Resume Analysis

The platform analyzes resumes and provides:

1. **Keywords Extraction**: Identifies technical skills, technologies, and relevant keywords
2. **Entity Recognition**: Extracts companies, job titles, and important entities
3. **Sentiment Analysis**: Evaluates the tone and confidence level of the resume
4. **Overall Score**: Comprehensive score based on multiple factors
5. **Actionable Suggestions**: Specific recommendations to improve the resume

### Security

- JWT-based authentication with configurable expiration
- BCrypt password hashing (industry standard)
- CORS configuration for frontend integration
- Input validation and error handling
- Secure file upload with size and type restrictions

---

## 🚨 Troubleshooting

### Common Issues

**Backend won't start**
- Ensure .NET 8 SDK is installed: `dotnet --version`
- Check database connection string in `appsettings.json`
- Run `dotnet ef database update` to create the database

**Frontend can't connect to API**
- Verify backend is running on `http://localhost:5065`
- Check `VITE_API_URL` in frontend `.env` file
- Ensure CORS is properly configured in backend

**File upload fails**
- Check file size (max 5MB)
- Ensure file format is PDF or DOCX
- Verify authentication token is valid

---

## 🔮 Roadmap

### Planned Features

- [ ] Advanced NLP integration (OpenAI GPT, Azure Cognitive Services)
- [ ] Batch resume processing
- [ ] Job description matching
- [ ] PDF report generation
- [ ] Resume history and comparison
- [ ] Cloud storage integration (Azure Blob, AWS S3)
- [ ] Real-time analysis progress updates
- [ ] Advanced analytics dashboard
- [ ] Multi-language support

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Development Guidelines

1. Follow C# coding conventions for backend
2. Use TypeScript strict mode for frontend
3. Write meaningful commit messages
4. Add tests for new features
5. Update documentation as needed

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- **UglyToad.PdfPig** - Excellent PDF processing library
- **OpenXML SDK** - DOCX parsing capabilities
- **.NET Community** - Continuous innovation
- **React Team** - Amazing UI library
- **Tailwind CSS** - Beautiful utility-first CSS

---

<div align="center">

**Made with ❤️ and ☕**

*Transforming resume analysis with AI-powered insights*

[![GitHub stars](https://img.shields.io/github/stars/yourusername/ai-resume-analyzer?style=social)](https://github.com/yourusername/ai-resume-analyzer)
[![GitHub forks](https://img.shields.io/github/forks/yourusername/ai-resume-analyzer?style=social)](https://github.com/yourusername/ai-resume-analyzer)

</div>

