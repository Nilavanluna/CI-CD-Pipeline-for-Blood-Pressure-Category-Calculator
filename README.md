# CI/CD Pipeline for Blood Pressure Category Calculator

![CI Pipeline](https://github.com/Nilavanluna/CI-CD-Pipeline-for-Blood-Pressure-Category-Calculator/actions/workflows/ci.yml/badge.svg)
![CD Pipeline](https://github.com/Nilavanluna/CI-CD-Pipeline-for-Blood-Pressure-Category-Calculator/actions/workflows/cd.yml/badge.svg)

## Project Overview

A fully automated CI/CD pipeline for a Blood Pressure Category Calculator built with ASP.NET Core Razor Pages. This project was developed as part of the M.Sc. in DevOps at TU Dublin, Tallaght Campus.

The application takes systolic and diastolic blood pressure readings and categorises them as Low, Ideal, Pre-High, or High Blood Pressure. It also provides a health recommendation alongside the category.

## Live Application

| Environment | URL |
|---|---|
| Production | https://bp-prod-nilavan-g4dsfqatf4b5g5hh.switzerlandnorth-01.azurewebsites.net |
| Staging | https://bp-staging-nilavan-epaebkbed7gah2ac.switzerlandnorth-01.azurewebsites.net |
| QA | https://bp-qa-nilavan-febzavhrbuamh3g8.switzerlandnorth-01.azurewebsites.net |

## Application Features

- Blood pressure category calculation (Low, Ideal, Pre-High, High)
- Input validation (systolic must exceed diastolic, values within valid ranges)
- Health recommendation message for each category
- Application Insights telemetry tracking

## Blood Pressure Categories

| Category | Systolic (mmHg) | Diastolic (mmHg) |
|---|---|---|
| Low Blood Pressure | Below 90 | Below 60 |
| Ideal Blood Pressure | 90 – 119 | 60 – 79 |
| Pre-High Blood Pressure | 120 – 139 | 80 – 89 |
| High Blood Pressure | 140 or above | 90 or above |

## Tech Stack

| Layer | Technology |
|---|---|
| Application | ASP.NET Core 9 Razor Pages |
| Unit Testing | xUnit + FluentAssertions |
| BDD Testing | SpecFlow |
| Code Coverage | Coverlet + ReportGenerator |
| Static Analysis | SonarCloud |
| Dependency Scanning | Snyk |
| Secret Scanning | Gitleaks |
| E2E Testing | Playwright |
| Performance Testing | k6 |
| Security Testing | OWASP ZAP |
| CI/CD | GitHub Actions |
| Hosting | Azure App Service |
| Telemetry | Application Insights |

## CI/CD Pipeline

### CI Pipeline
Triggers on every push and pull request to master and feature branches.
```
Build → Unit Tests (91% coverage) → BDD Tests → SonarCloud → Snyk → Gitleaks
```

### CD Pipeline
Triggers automatically after CI passes on master. Requires manual approval for production.
```
Deploy QA → E2E Tests → Deploy Staging → Performance Tests + ZAP Scan → [Manual Approval] → Deploy Production → Health Check
```

## Test Coverage

| Test Type | Tool | Count | Status |
|---|---|---|---|
| Unit Tests | xUnit | 29 | ✅ Passing |
| BDD Scenarios | SpecFlow | 7 | ✅ Passing |
| E2E Tests | Playwright | 8 | ✅ Passing |
| Performance Tests | k6 | 1 load test | ✅ Passing |
| Security Scan | OWASP ZAP | Baseline scan | ✅ Passing |

## Getting Started

### Prerequisites
- .NET 9 SDK
- Node.js 20+
- Git

### Run Locally
```bash
git clone https://github.com/Nilavanluna/CI-CD-Pipeline-for-Blood-Pressure-Category-Calculator.git
cd CI-CD-Pipeline-for-Blood-Pressure-Category-Calculator/BPCalculator
dotnet run
```

Open http://localhost:5000 in your browser.

### Run Unit Tests
```bash
cd BPCalculator.Tests
dotnet test
```

### Run BDD Tests
```bash
cd BPCalculator.BDDTests
dotnet test
```

### Run E2E Tests

Start the app first, then in a second terminal:
```bash
cd e2e-tests
npx playwright test
```

### Run Performance Tests
```bash
k6 run performance-tests/bp-load-test.js
```

## Branching Strategy

This project uses the Git Feature Branch workflow:

- `master` — production-ready code, protected branch
- `feature/*` — individual feature branches, merged via Pull Requests

All changes go through a Pull Request. CI runs automatically on every PR.

## Security

| Tool | Purpose | Stage |
|---|---|---|
| Gitleaks | Secret scanning | CI |
| Snyk | Dependency vulnerability scanning | CI |
| SonarCloud | Static code analysis | CI |
| OWASP ZAP | Dynamic application security testing | CD |
| Azure HTTPS | Transport security | All environments |

## Deployment Strategy

Blue/Green deployment is simulated using two Azure App Service instances:

- **Staging** acts as the Green environment — all tests run here first
- **Production** acts as the Blue environment — only receives traffic after staging is verified
- Manual approval gate protects production from unverified deployments
- Post-deployment health check verifies production is responding correctly

## Telemetry

Application Insights tracks:
- Custom event `BPCalculated` on every calculation
- Systolic and diastolic metric values
- Request rates and response times
- Exception tracking

## Module Information

- **Module:** Continuous Software Deployment
- **Lecturer:** Gary Clynch
- **Institution:** TU Dublin, Tallaght Campus
- **Programme:** M.Sc. in Computing in Development Operations (DevOps)
- **Academic Year:** 2025/2026