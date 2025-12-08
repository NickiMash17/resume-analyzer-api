import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5065';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add token to requests if available
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Handle token expiration
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export interface RegisterData {
  email: string;
  password: string;
  fullName: string;
}

export interface LoginData {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  expiresIn: number;
}

export interface AnalysisResult {
  keywords: string[];
  entities: string[];
  sentimentScore: number;
  overallScore: number;
  suggestions: string[];
}

export interface AnalysisResponse {
  success: boolean;
  data: AnalysisResult;
  fileName?: string;
  fileSize?: number;
  processedAt?: string;
  error?: string;
}

export const authService = {
  register: async (data: RegisterData): Promise<{ success: boolean }> => {
    const response = await api.post('/api/auth/register', data);
    return response.data;
  },

  login: async (data: LoginData): Promise<AuthResponse> => {
    const response = await api.post('/api/auth/login', data);
    return response.data;
  },
};

export const analysisService = {
  analyzeResume: async (file: File, description?: string): Promise<AnalysisResponse> => {
    const formData = new FormData();
    formData.append('File', file);
    if (description) {
      formData.append('Description', description);
    }

    const response = await api.post<AnalysisResponse>('/api/analysis/analyze', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });

    return response.data;
  },
};

export default api;

