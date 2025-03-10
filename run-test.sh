#!/bin/bash

# Script to run all tests with coverage reports
# Run from project root directory

echo "==============================================="
echo "Starting full test suite execution with coverage"
echo "==============================================="

# 1. Run backend tests
echo -e "\n\n🧪 Running backend tests...\n"
cd backend
dotnet test
backend_test_result=$?

if [ $backend_test_result -eq 0 ]; then
  echo -e "\n✅ Backend tests passed successfully!"
else
  echo -e "\n❌ Backend tests failed with exit code $backend_test_result"
fi

# 2. Generate backend coverage report
echo -e "\n\n📊 Generating backend coverage report...\n"
dotnet test --collect:"XPlat Code Coverage"
backend_coverage_result=$?

if [ $backend_coverage_result -eq 0 ]; then
  echo -e "\n✅ Backend coverage report generated successfully!"
  
  # Check if ReportGenerator is installed
  if ! command -v reportgenerator &> /dev/null; then
    echo "📦 Installing ReportGenerator tool..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
  fi
  
  echo "🔄 Converting coverage data to HTML report..."
  reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
  echo "📁 Backend coverage report available at: $(pwd)/coverage-report/index.html"
else
  echo -e "\n❌ Backend coverage generation failed with exit code $backend_coverage_result"
fi

# 3. Run frontend tests in headless mode
echo -e "\n\n🧪 Running frontend tests (headless mode)...\n"
cd ../frontend
npx ng test --no-watch --browsers=ChromeHeadless
frontend_test_result=$?

if [ $frontend_test_result -eq 0 ]; then
  echo -e "\n✅ Frontend tests passed successfully!"
else
  echo -e "\n❌ Frontend tests failed with exit code $frontend_test_result"
fi

# 4. Generate frontend coverage report
echo -e "\n\n📊 Generating frontend coverage report...\n"
npx ng test --no-watch --code-coverage --browsers=ChromeHeadless
frontend_coverage_result=$?

if [ $frontend_coverage_result -eq 0 ]; then
  echo -e "\n✅ Frontend coverage report generated successfully!"
  echo "📁 Frontend coverage report available at: $(pwd)/coverage/index.html"
else
  echo -e "\n❌ Frontend coverage generation failed with exit code $frontend_coverage_result"
fi

# Return to root directory
cd ..

# Final summary
echo -e "\n\n==============================================="
echo "Test execution summary:"
echo "==============================================="
echo "Backend tests: $([ $backend_test_result -eq 0 ] && echo "✅ PASSED" || echo "❌ FAILED")"
echo "Backend coverage: $([ $backend_coverage_result -eq 0 ] && echo "✅ GENERATED" || echo "❌ FAILED")"
echo "Frontend tests: $([ $frontend_test_result -eq 0 ] && echo "✅ PASSED" || echo "❌ FAILED")"
echo "Frontend coverage: $([ $frontend_coverage_result -eq 0 ] && echo "✅ GENERATED" || echo "❌ FAILED")"
echo "==============================================="

# Open reports automatically if all succeeded
if [ $backend_coverage_result -eq 0 ] && [ $frontend_coverage_result -eq 0 ]; then
  echo -e "\nWould you like to open the coverage reports now? (y/n)"
  read -r response
  if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
    # Detect OS and open reports accordingly
    if [[ "$OSTYPE" == "darwin"* ]]; then
      # macOS
      open backend/coverage-report/index.html
      open frontend/coverage/index.html
    elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
      # Linux
      xdg-open backend/coverage-report/index.html
      xdg-open frontend/coverage/index.html
    elif [[ "$OSTYPE" == "msys" || "$OSTYPE" == "cygwin" || "$OSTYPE" == "win32" ]]; then
      # Windows
      start backend/coverage-report/index.html
      start frontend/coverage/index.html
    fi
  fi
fi

# Final exit code
if [ $backend_test_result -eq 0 ] && [ $backend_coverage_result -eq 0 ] && [ $frontend_test_result -eq 0 ] && [ $frontend_coverage_result -eq 0 ]; then
  echo -e "\n✅ All processes completed successfully!"
  exit 0
else
  echo -e "\n❌ Some processes failed! Check the logs above for details."
  exit 1
fi