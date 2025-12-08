# Development Guide

## Getting Started

### Initial Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ai-resume-analyzer
   ```

2. **Install dependencies**
   ```bash
   npm run setup
   ```

3. **Configure environment**
   - Backend: Edit `backend/appsettings.json` (especially JWT key for production)
   - Frontend: Create `frontend/.env` with `VITE_API_URL=http://localhost:5065`

### Running the Application

#### Option 1: Run separately (Recommended for development)

**Terminal 1 - Backend:**
```bash
cd backend
dotnet watch run
```

**Terminal 2 - Frontend:**
```bash
cd frontend
npm run dev
```

#### Option 2: Use npm scripts

```bash
# Terminal 1
npm run dev:backend

# Terminal 2
npm run dev:frontend
```

### Development URLs

- **Frontend**: http://localhost:5173
- **Backend API**: http://localhost:5065
- **Swagger UI**: http://localhost:5065/swagger

## Project Structure

### Backend (`backend/`)

```
backend/
├── Controllers/          # API endpoints
│   ├── AnalysisController.cs
│   └── AuthController.cs
├── Services/            # Business logic
│   ├── AuthService.cs
│   ├── NlpService.cs
│   └── ResumeParserService.cs
├── Models/              # Data models
│   ├── User.cs
│   ├── Resume.cs
│   └── AnalysisResult.cs
├── Data/                # Database context
│   └── AppDbContext.cs
└── Program.cs           # Application entry point
```

### Frontend (`frontend/`)

```
frontend/
├── src/
│   ├── components/      # Reusable components
│   │   └── ProtectedRoute.tsx
│   ├── contexts/        # React contexts
│   │   └── AuthContext.tsx
│   ├── pages/           # Page components
│   │   ├── Login.tsx
│   │   ├── Register.tsx
│   │   └── Dashboard.tsx
│   ├── services/        # API service layer
│   │   └── api.ts
│   ├── App.tsx          # Main app component
│   └── main.tsx         # Entry point
└── package.json
```

## Code Style

### Backend (C#)

- Use PascalCase for public members
- Use camelCase for private fields
- Follow async/await patterns
- Use dependency injection
- Add XML comments for public APIs

### Frontend (TypeScript/React)

- Use functional components with hooks
- Use TypeScript strict mode
- Follow React best practices
- Use Tailwind CSS for styling
- Keep components small and focused

## Testing

### Backend Testing

```bash
cd backend
dotnet test
```

### Frontend Testing

```bash
cd frontend
npm test
```

## Database Migrations

### Create a new migration

```bash
cd backend
dotnet ef migrations add MigrationName
```

### Apply migrations

```bash
dotnet ef database update
```

### Rollback migration

```bash
dotnet ef database update PreviousMigrationName
```

## Building for Production

### Backend

```bash
cd backend
dotnet publish -c Release -o ./publish
```

### Frontend

```bash
cd frontend
npm run build
```

The production build will be in `frontend/dist/`.

## Common Tasks

### Add a new API endpoint

1. Create controller method in `Controllers/`
2. Add route attribute
3. Add service method if needed
4. Update Swagger documentation

### Add a new frontend page

1. Create component in `src/pages/`
2. Add route in `src/App.tsx`
3. Add navigation link if needed

### Add a new service

1. Create interface in `Services/`
2. Implement service class
3. Register in `Program.cs` DI container

## Debugging

### Backend

- Use Visual Studio or VS Code debugger
- Check logs in console output
- Use Swagger UI to test endpoints

### Frontend

- Use React DevTools browser extension
- Check browser console for errors
- Use Network tab to debug API calls

## Environment Variables

### Backend

Edit `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=resumeanalyzer.db"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "AIResumeAnalyzer",
    "Audience": "AIResumeAnalyzerUsers"
  }
}
```

### Frontend

Create `frontend/.env`:

```env
VITE_API_URL=http://localhost:5065
```

## Troubleshooting

### Backend won't start

- Check .NET SDK version: `dotnet --version` (should be 8.x)
- Verify database connection string
- Run `dotnet ef database update`

### Frontend build errors

- Delete `node_modules` and `package-lock.json`
- Run `npm install` again
- Check Node.js version (18+)

### CORS errors

- Verify backend CORS configuration in `Program.cs`
- Check frontend API URL in `.env`
- Ensure backend is running

### Authentication issues

- Check JWT configuration in `appsettings.json`
- Verify token expiration
- Clear browser localStorage

## Contributing

1. Create a feature branch
2. Make your changes
3. Test thoroughly
4. Submit a pull request

## Resources

- [.NET Documentation](https://learn.microsoft.com/dotnet/)
- [React Documentation](https://react.dev/)
- [TypeScript Documentation](https://www.typescriptlang.org/)
- [Tailwind CSS Documentation](https://tailwindcss.com/)

