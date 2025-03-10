/**
 * Development environment configuration
 * Contains values specific to the development environment
 */
export const environment = {
  production: false,
  apiUrl: '/api/'  // O Nginx configurado redirecionará para o host.docker.internal:8080/api/
}; 