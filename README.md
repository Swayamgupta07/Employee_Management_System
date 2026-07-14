# 🏢 Employee Management System (HRMS)

A comprehensive, full-stack Human Resource Management System built to streamline employee data, attendance tracking, department management, and administrative workflows. 

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Angular](https://img.shields.io/badge/Angular-20-DD0031?logo=angular)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql)
![Deployment](https://img.shields.io/badge/Live-Vercel%20%7C%20Somee-success)

## 🌐 Live Links

- **Frontend (Live App):** [https://employee-management-system-swayam.vercel.app](https://employee-management-system-swayam.vercel.app)
- **Backend API (Swagger):** [http://employeemanagementswayam.somee.com/index.html](http://employeemanagementswayam.somee.com/index.html)

---

## 🛠️ Technology Stack

### **Frontend**
- **Framework:** Angular 20
- **Styling:** Tailwind CSS + Vanilla CSS (Dynamic UI, Glassmorphism, Micro-animations)
- **State Management & Routing:** RxJS, Angular Router
- **Hosting:** Vercel (with custom proxy configuration for API and Image uploads)

### **Backend**
- **Framework:** .NET 8 (ASP.NET Core Web API)
- **Architecture:** Repository Pattern + Dependency Injection
- **Authentication:** JWT (JSON Web Tokens) & Google Single Sign-On (OAuth 2.0)
- **Hosting:** Somee.com (Windows IIS)

### **Database**
- **Engine:** MySQL (Cloud Hosted via Aiven)
- **ORM:** Entity Framework Core (EF Core)
- **Features:** Case-insensitive configurations (Linux compatibility), Automated Migrations

### **Additional Tools & Services**
- **Email Service:** Gmail SMTP (for OTPs, Password Resets, and Admin Alerts)
- **PDF Generation:** DinkToPdf (wkhtmltopdf wrapper) for exporting Monthly Attendance Reports
- **Security:** BCrypt Password Hashing, JWT role-based claims (Admin, HR, Employee)

---

## ✨ Key Features

1. **Authentication & Security:**
   - Standard Email/Password login.
   - **Google SSO (Single Sign-On)** integration.
   - Email Verification via OTP (One-Time Password).
   - JWT-based Role Authorization (Admin vs Employee permissions).

2. **Admin Approval Workflow:**
   - New users requesting "Admin" roles are placed in a waitlist.
   - Existing Admins can approve or reject these requests from a dedicated dashboard.

3. **Employee & Department Management:**
   - Full CRUD operations for Departments and Employees.
   - Graphical representation of headcount and organization structure.

4. **Attendance Tracking & Reporting:**
   - Mark daily attendance (Present, Absent, Leave).
   - Generate Monthly Attendance Reports and instantly download them as **PDFs**.

5. **Profile Customization:**
   - Upload and crop profile pictures.
   - Pre-built SVG default avatars.

---

## 🚀 Deployment Architecture & Overcoming Challenges

Deploying a complex .NET & Angular application across multiple free-tier cloud providers involved solving several unique challenges:

### 1. Database Case-Sensitivity (Windows vs Linux)
- **Problem:** MySQL on Windows (Local) is case-insensitive, but Aiven MySQL on Linux is strictly case-sensitive. When deployed, EF Core attempted to query capitalized tables (e.g., `Users`), which failed to match the lowercase imported tables (`users`).
- **Solution:** Configured `ApplicationDbContext` to force EF Core to intercept all queries and convert table names to `.ToLowerInvariant()`, ensuring cross-platform stability.

### 2. Free-Tier Outbound Firewalls (Somee)
- **Problem:** Somee.com free tier blocks outbound HTTP requests to external APIs. This caused Google SSO token verification to crash the server with a `500 Internal Server Error`.
- **Solution:** Modified startup scripts and implemented fallback mechanisms. (Note: True Google SSO requires a premium host or Azure free-tier for uninterrupted outbound API calls).

### 3. Cross-Origin Resource Sharing (CORS) & Image Proxies (Vercel)
- **Problem:** Profile pictures were saved on the Somee server, but the Angular frontend (on Vercel) tried to load them locally, resulting in broken image icons.
- **Solution:** Configured `vercel.json` to act as a reverse proxy, successfully rewriting all `/api/(.*)` and `/uploads/(.*)` traffic directly to the Somee backend, bypassing CORS and serving dynamic images flawlessly.

---

## 💻 Local Setup & Development

### Prerequisites
- Node.js & Angular CLI
- .NET 8 SDK
- MySQL Workbench

### 1. Backend Setup
```bash
cd EmployeeManagementAPI
# Restore dependencies
dotnet restore

# Run the API
dotnet run
```
*Note: Make sure to update `appsettings.Development.json` with your local MySQL string if you don't want to hit the live Aiven database during development.*

### 2. Frontend Setup
```bash
cd EmployeeManagementUI
# Install packages
npm install

# Run development server
ng serve
```
*Navigate to `http://localhost:4200/`. The local environment file (`environment.development.ts`) is configured to route to `http://localhost:5228/api` by default.*

---

## 🛡️ API Endpoints Overview

- `POST /api/Auth/login` - Authenticate user
- `POST /api/Auth/register` - Create new account (triggers OTP email)
- `POST /api/Auth/verify-otp` - Verify email address
- `GET /api/Employees` - Fetch all employees
- `GET /api/Departments` - Fetch all departments
- `GET /api/Attendance/monthlyreport` - Generates PDF report

---
*Built with ❤️ for Modern HR Operations.*
