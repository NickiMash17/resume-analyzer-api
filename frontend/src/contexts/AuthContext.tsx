import { createContext, useContext, useState, useEffect } from 'react';
import type { ReactNode } from 'react';
import { authService } from '../services/api';
import type { LoginData, RegisterData } from '../services/api';

interface User {
  email: string;
  fullName: string;
}

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (data: LoginData) => Promise<void>;
  register: (data: RegisterData) => Promise<void>;
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    try {
      const storedToken = localStorage.getItem('token');
      const storedUser = localStorage.getItem('user');
      
      if (storedToken && storedUser) {
        setToken(storedToken);
        setUser(JSON.parse(storedUser));
      }
    } catch (error) {
      console.error('Error loading stored auth data:', error);
      localStorage.removeItem('token');
      localStorage.removeItem('user');
    }
  }, []);

  const login = async (data: LoginData) => {
    const response = await authService.login(data);
    setToken(response.token);
    
    const userData: User = {
      email: data.email,
      fullName: '',
    };
    
    setUser(userData);
    localStorage.setItem('token', response.token);
    localStorage.setItem('user', JSON.stringify(userData));
  };

  const register = async (data: RegisterData) => {
    await authService.register(data);
    await login({ email: data.email, password: data.password });
  };

  const logout = () => {
    setToken(null);
    setUser(null);
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        login,
        register,
        logout,
        isAuthenticated: !!token,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

