import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { authService, LoginData, RegisterData } from '../services/api';

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

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    const storedToken = localStorage.getItem('token');
    const storedUser = localStorage.getItem('user');
    
    if (storedToken && storedUser) {
      setToken(storedToken);
      setUser(JSON.parse(storedUser));
    }
  }, []);

  const login = async (data: LoginData) => {
    const response = await authService.login(data);
    setToken(response.token);
    
    // Decode token to get user info (simplified - in production, use jwt-decode)
    const userData: User = {
      email: data.email,
      fullName: '', // Would be decoded from token in production
    };
    
    setUser(userData);
    localStorage.setItem('token', response.token);
    localStorage.setItem('user', JSON.stringify(userData));
  };

  const register = async (data: RegisterData) => {
    await authService.register(data);
    // Auto-login after registration
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

