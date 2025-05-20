# 📈 Live Stock Dashboard

A real-time stock trading simulation app where investors can create accounts, add stocks, and trade with other investors — just like a real trading platform.

Built using **React.js**, **ASP.NET Core Web API**, **Azure Cosmos DB**, **SignalR**, and **.NET Worker Services**, this project is ideal for demonstrating full-stack cloud-integrated architecture.

---

## 🚀 Features

- 🔐 User signup and login
- 📊 Real-time stock price updates (SignalR)
- 📈 Home dashboard with live prices
- 📃 Orders page showing current-day trades
- 🧾 Add and manage stock assets
- ⚙️ Worker service simulates dynamic price changes
- 🏗️ Infrastructure project to initialize DB and load master data

---

## 🧰 Tech Stack

| Layer       | Technology                                           |
|-------------|------------------------------------------------------|
| Frontend    | React.js, Axios, React Router, Redux, SignalR Client |
| Backend     | ASP.NET Core Web API, .NET Worker Service            |
| Database    | Azure Cosmos DB Emulator                             |
| Real-time   | SignalR                                              |
| Infra Setup | .NET Console App                                     |

---

## 📁 Folder Structure
<pre>
stock-live-dashboard/<br>
    client/ # React.js frontend<br>
        ... # Login, Dashboard, Orders, etc.<br>
    server/ # .NET backend<br>
        StockLiveDashboard/ # Web API project<br>
        StockLiveDashboard.WorkerService/ # Background service for price updates<br>
        StockLiveDashboard.Infrastructure/ # Master data + Cosmos DB setup<br>
    README.md # You're reading it!<br>
</pre>

---

## ⚙️ Setup Instructions

### Prerequisites

- Node.js + npm
- .NET 8 SDK
- Azure Cosmos DB Emulator
- Git

---

### 1️⃣ Clone the Repo

```bash
git clone https://github.com/saurabhk2323/stock-live-dashboard.git
cd stock-live-dashboard
```

---

### 2️⃣ Run Infra Setup

```bash
cd server/StockLiveDashboard.Infrastructure
dotnet run
```
Initializes Cosmos DB and loads master data.

---

### 3️⃣ Start Backend Services
# API
```bash
cd ../StockLiveDashboard
dotnet run --urls="http://localhost:5000"
```

---

# Worker (Simulates stock price updates)
```bash
cd ../StockLiveDashboard.WorkerService
dotnet run --urls="http://localhost:5001"
```
SignalR Hub: http://localhost:5001/stockshub

---

### 4️⃣ Start Frontend
```bash
cd ../../client
npm install
npm start
```
Visit: http://localhost:3000

---

## 🧪 Sample Environment
The repository includes a safe appsettings.Development.json for API and worker projects.

No secrets or keys are exposed.

---

## 📸 Screenshots
  ### 📷 Screenshots to be added here soon. Stay tuned!

---

## 🧑‍💻 Developed By
Made with ❤️ by Saurabh
GitHub: @saurabhk2323

---

## 📄 License
This project is licensed under the MIT License.
Feel free to fork, use, and contribute!


---


