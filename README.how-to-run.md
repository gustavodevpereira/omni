## How to run this project locally:

```bash
>>> cd template
>>> docker compose up -d
```

This will start:
- PostgreSQL database (USED)
- RabbitMQ message broker (USED)
- Redis cache (NOT USED)
- .NET Core backend API (USED)
- Angular frontend application (USED)

# Access the application:
   - Frontend: http://localhost:4200
   - Backend API: http://localhost:8080 - admin account: (username: admin@example.com, password: Admin123!)
   - Swagger UI: http://localhost:8080/swagger
   - RabbitMQ Management: http://localhost:15672 (username: ambev_user, password: strong_password by default)


## How to run the tests:

1. Unit <FRONTEND>:
```bash
cd template
cd frontend
npm install &&
npm run test 
```

2. Coverage <FRONTEND>:
```bash
cd template
cd frontend
npm install &&
npm run coverage
```

OBS: This will:
- Run all the tests using Karma
- Generate a coverage report
- Create a /coverage directory in your project with the detailed HTML reports

3. Unit <BACKEND>:
```bash
cd template
cd backend
dotnet test tests/Ambev.DeveloperEvaluation.Unit/Ambev.DeveloperEvaluation.Unit.csproj
```

4. Coverage <BACKEND>:
```bash
cd template
cd backend
dotnet test tests/Ambev.DeveloperEvaluation.Unit/Ambev.DeveloperEvaluation.Unit.csproj /p:CollectCoverage=true
```
