# Ambev Developer Evaluation Project

SETUP: 
```bash
cd template
docker-compose up -d
```

This will start:
- PostgreSQL database
- RabbitMQ message broker
- Redis cache
- .NET Core backend API
- Angular frontend application

# Access the application:
   - Frontend: http://localhost:4200
   - Backend API: http://localhost:8080
   - Swagger UI: http://localhost:8080/swagger
   - RabbitMQ Management: http://localhost:15672 (username: ambev_user, password: strong_password by default)
