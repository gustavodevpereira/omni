# Stage 1: Build the application
FROM node:20-alpine AS build
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy the rest of the application
COPY . .

# Build the application
RUN npm run build

# Stage 2: Serve the application
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copy the build output
COPY --from=build /app/dist/frontend/ .

# Copy custom nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80
EXPOSE 80

# Command to run the server
CMD ["nginx", "-g", "daemon off;"] 