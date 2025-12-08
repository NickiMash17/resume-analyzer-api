import { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { analysisService, AnalysisResult } from '../services/api';
import { Upload, FileText, LogOut, CheckCircle, AlertCircle, TrendingUp, Lightbulb, Award } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

export const Dashboard = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [file, setFile] = useState<File | null>(null);
  const [description, setDescription] = useState('');
  const [loading, setLoading] = useState(false);
  const [analysisResult, setAnalysisResult] = useState<AnalysisResult | null>(null);
  const [error, setError] = useState('');

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const selectedFile = e.target.files[0];
      const maxSize = 5 * 1024 * 1024; // 5MB
      
      if (selectedFile.size > maxSize) {
        setError('File size exceeds 5MB limit');
        return;
      }

      const extension = selectedFile.name.split('.').pop()?.toLowerCase();
      if (extension !== 'pdf' && extension !== 'docx') {
        setError('Only PDF and DOCX files are supported');
        return;
      }

      setFile(selectedFile);
      setError('');
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!file) {
      setError('Please select a file');
      return;
    }

    setLoading(true);
    setError('');
    setAnalysisResult(null);

    try {
      const result = await analysisService.analyzeResume(file, description);
      if (result.success && result.data) {
        setAnalysisResult(result.data);
      } else {
        setError(result.error || 'Analysis failed');
      }
    } catch (err: any) {
      setError(err.response?.data?.error || 'Failed to analyze resume. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const formatFileSize = (bytes: number) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow-sm border-b border-gray-200">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
          <div className="flex items-center justify-between">
            <div>
              <h1 className="text-2xl font-bold text-gray-900">AI Resume Analyzer</h1>
              <p className="text-sm text-gray-600">Welcome back, {user?.fullName || user?.email}</p>
            </div>
            <button
              onClick={handleLogout}
              className="btn-secondary flex items-center gap-2"
            >
              <LogOut className="w-4 h-4" />
              Logout
            </button>
          </div>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Upload Section */}
          <div className="lg:col-span-2">
            <div className="card">
              <h2 className="text-2xl font-bold text-gray-900 mb-6 flex items-center gap-2">
                <Upload className="w-6 h-6" />
                Upload Resume
              </h2>

              <form onSubmit={handleSubmit} className="space-y-6">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Resume File (PDF or DOCX, max 5MB)
                  </label>
                  <div className="mt-1 flex justify-center px-6 pt-5 pb-6 border-2 border-gray-300 border-dashed rounded-lg hover:border-primary-400 transition-colors">
                    <div className="space-y-1 text-center">
                      <FileText className="mx-auto h-12 w-12 text-gray-400" />
                      <div className="flex text-sm text-gray-600">
                        <label className="relative cursor-pointer rounded-md font-medium text-primary-600 hover:text-primary-500">
                          <span>Upload a file</span>
                          <input
                            type="file"
                            className="sr-only"
                            accept=".pdf,.docx"
                            onChange={handleFileChange}
                            disabled={loading}
                          />
                        </label>
                        <p className="pl-1">or drag and drop</p>
                      </div>
                      <p className="text-xs text-gray-500">PDF, DOCX up to 5MB</p>
                      {file && (
                        <p className="text-sm text-gray-700 mt-2">
                          Selected: {file.name} ({formatFileSize(file.size)})
                        </p>
                      )}
                    </div>
                  </div>
                </div>

                <div>
                  <label htmlFor="description" className="block text-sm font-medium text-gray-700 mb-2">
                    Job Description (Optional)
                  </label>
                  <textarea
                    id="description"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    className="input-field"
                    rows={4}
                    placeholder="Paste the job description here for targeted analysis..."
                  />
                </div>

                {error && (
                  <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg flex items-center gap-2">
                    <AlertCircle className="w-5 h-5" />
                    {error}
                  </div>
                )}

                <button
                  type="submit"
                  disabled={loading || !file}
                  className="btn-primary w-full flex items-center justify-center gap-2"
                >
                  {loading ? (
                    'Analyzing...'
                  ) : (
                    <>
                      <TrendingUp className="w-5 h-5" />
                      Analyze Resume
                    </>
                  )}
                </button>
              </form>
            </div>
          </div>

          {/* Results Section */}
          <div className="lg:col-span-1">
            {analysisResult ? (
              <div className="space-y-6">
                {/* Overall Score */}
                <div className="card bg-gradient-to-br from-primary-500 to-primary-600 text-white">
                  <div className="flex items-center justify-between mb-4">
                    <h3 className="text-lg font-semibold">Overall Score</h3>
                    <Award className="w-6 h-6" />
                  </div>
                  <div className="text-4xl font-bold mb-2">
                    {Math.round(analysisResult.overallScore * 100)}%
                  </div>
                  <div className="text-primary-100 text-sm">
                    Sentiment: {Math.round(analysisResult.sentimentScore * 100)}%
                  </div>
                </div>

                {/* Keywords */}
                <div className="card">
                  <h3 className="text-lg font-semibold text-gray-900 mb-4 flex items-center gap-2">
                    <CheckCircle className="w-5 h-5 text-primary-600" />
                    Keywords Found
                  </h3>
                  <div className="flex flex-wrap gap-2">
                    {analysisResult.keywords.map((keyword, index) => (
                      <span
                        key={index}
                        className="px-3 py-1 bg-primary-100 text-primary-800 rounded-full text-sm font-medium"
                      >
                        {keyword}
                      </span>
                    ))}
                  </div>
                </div>

                {/* Entities */}
                {analysisResult.entities.length > 0 && (
                  <div className="card">
                    <h3 className="text-lg font-semibold text-gray-900 mb-4">Entities</h3>
                    <div className="space-y-2">
                      {analysisResult.entities.map((entity, index) => (
                        <div key={index} className="text-sm text-gray-700 bg-gray-50 px-3 py-2 rounded">
                          {entity}
                        </div>
                      ))}
                    </div>
                  </div>
                )}

                {/* Suggestions */}
                <div className="card">
                  <h3 className="text-lg font-semibold text-gray-900 mb-4 flex items-center gap-2">
                    <Lightbulb className="w-5 h-5 text-yellow-500" />
                    Suggestions
                  </h3>
                  <ul className="space-y-3">
                    {analysisResult.suggestions.map((suggestion, index) => (
                      <li key={index} className="flex items-start gap-2 text-sm text-gray-700">
                        <span className="text-primary-600 mt-1">•</span>
                        <span>{suggestion}</span>
                      </li>
                    ))}
                  </ul>
                </div>
              </div>
            ) : (
              <div className="card text-center text-gray-500">
                <FileText className="w-16 h-16 mx-auto mb-4 text-gray-300" />
                <p>Upload a resume to see analysis results here</p>
              </div>
            )}
          </div>
        </div>
      </main>
    </div>
  );
};

