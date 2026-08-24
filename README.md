# DreamBank 2.0

DreamBank 2.0 is a web application built with ASP.NET / Visual Studio, backed by a SQL Server database. This README walks you through setting up the database, configuring the application, and running it locally.

---

## Table of Contents

- [Prerequisites](#prerequisites)
- [1. Enable FILESTREAM on SQL Server](#1-enable-filestream-on-sql-server)
- [2. Restore the DreamBank Database](#2-restore-the-dreambank-database)
- [3. Create a SQL Server Login](#3-create-a-sql-server-login)
- [4. Open the Project in Visual Studio](#4-open-the-project-in-visual-studio)
- [5. Configure the Connection String](#5-configure-the-connection-string)
- [6. Run the Application](#6-run-the-application)
- [Default Admin Credentials](#default-admin-credentials)
- [Troubleshooting](#troubleshooting)
- [Repository](#repository)

---

## Prerequisites

Before you begin, make sure you have the following installed:

- **SQL Server** (Developer or Express edition, 2016+) with **SQL Server Management Studio (SSMS)**
- **Visual Studio** (2019 or later recommended) with the ASP.NET / web development workload installed
- **.NET SDK** matching the project's target framework
- The `DreamBank.bak` database backup file
- Git (to clone the repository)

---

## 1. Enable FILESTREAM on SQL Server

The DreamBank database uses FILESTREAM to store data, so this feature must be enabled **before** restoring the backup.

1. Open **SQL Server Configuration Manager**.
2. Select **SQL Server Services** in the left pane.
3. Right-click your SQL Server instance (e.g. `SQL Server (MSSQLSERVER)`) and choose **Properties**.
4. Go to the **FILESTREAM** tab and check:
   - ✅ Enable FILESTREAM for Transact-SQL access
   - ✅ Enable FILESTREAM for file I/O access (recommended)
5. Click **Apply**, then restart the SQL Server service for changes to take effect.
6. In **SSMS**, confirm the feature is enabled by running:

   ```sql
   EXEC sp_configure filestream_access_level;
   ```

   If it returns `0`, enable it at the engine level too:

   ```sql
   EXEC sp_configure filestream_access_level, 2;
   RECONFIGURE;
   ```

---

## 2. Restore the DreamBank Database

1. Copy `DreamBank.bak` to a location accessible by your SQL Server instance (e.g. the default `Backup` folder).
2. In **SSMS**, right-click **Databases** → **Restore Database...**
3. Choose **Device**, then browse to and select `DreamBank.bak`.
4. Under **Files**, verify the database name is set to `DreamBank` and confirm the file paths are valid for your machine.
5. Click **OK** to restore.

Alternatively, restore via T-SQL:

```sql
RESTORE DATABASE [DreamBank]
FROM DISK = N'C:\Path\To\DreamBank.bak'
WITH MOVE 'DreamBank_Data' TO 'C:\Path\To\Data\DreamBank.mdf',
     MOVE 'DreamBank_Log' TO 'C:\Path\To\Data\DreamBank_log.ldf',
     MOVE 'DreamBank_FSData' TO 'C:\Path\To\Data\DreamBank_FSData',
     REPLACE;
```

> **Note:** Adjust the logical file names and paths above to match what's actually in your `.bak` file. You can inspect them first with:
> ```sql
> RESTORE FILELISTONLY FROM DISK = N'C:\Path\To\DreamBank.bak';
> ```

---

## 3. Create a SQL Server Login

Create a SQL login that the application will use to connect to the database. Run the following in a new query window on your SQL Server instance:

```sql
CREATE LOGIN [admin] WITH PASSWORD = N'admin';
ALTER SERVER ROLE [sysadmin] ADD MEMBER [admin];
```

> ⚠️ **Security note:** This creates a login with `sysadmin` privileges and a weak password, which is fine for **local development only**. Never use these settings in a staging or production environment — instead, create a login scoped only to the `DreamBank` database with the minimum permissions it needs, and use a strong password.

Also confirm your SQL Server instance has **Mixed Mode Authentication** (SQL Server and Windows Authentication) enabled, otherwise SQL logins like `admin` won't be able to connect:

1. Right-click the server in SSMS → **Properties** → **Security**.
2. Select **SQL Server and Windows Authentication mode**.
3. Restart the SQL Server service.

---

## 4. Open the Project in Visual Studio

1. Clone the repository:

   ```bash
   git clone https://github.com/oviahsanhabib/Dream-Bank-2.0.git
   ```

2. Open the solution file (`.sln`) in Visual Studio.
3. Restore/upgrade the project's NuGet dependencies:
   - Right-click the **Solution** in Solution Explorer → **Restore NuGet Packages**, or
   - Go to **Tools → NuGet Package Manager → Manage NuGet Packages for Solution** and update any outdated packages.
4. Build the solution (**Build → Build Solution**) to confirm everything compiles cleanly.

---

## 5. Configure the Connection String

Open `appsettings.json` in the project and update the `DefaultConnection` string to match your local SQL Server setup:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost;Initial Catalog=DreamBank;User Id=admin;Password=admin"
  }
}
```

- `Data Source` — your SQL Server instance name (e.g. `localhost`, `localhost\SQLEXPRESS`, or a remote server name).
- `Initial Catalog` — the database name (`DreamBank`, as restored in step 2).
- `User Id` / `Password` — the SQL login created in step 3.

---

## 6. Run the Application

1. In Visual Studio, set the web project as the **Startup Project** if it isn't already.
2. Press **F5** (or click **IIS Express** / the run button) to build and launch the application.
3. Your default browser should open to the application's home page.

---

## Default Admin Credentials

Use the following credentials to log in as an administrator:

| Field    | Value             |
|----------|-------------------|
| Username | `admin@gmail.com` |
| Password | `admin`           |

> ⚠️ Change this password immediately if you deploy the application beyond your local machine.

---

## Troubleshooting

| Issue | Possible Fix |
|---|---|
| `RESTORE DATABASE` fails with a FILESTREAM error | Confirm FILESTREAM is enabled at both the OS/service level and via `sp_configure` (see step 1). |
| Login `admin` fails to connect | Verify SQL Server is set to **Mixed Mode Authentication** and the SQL Server Browser/service is running. |
| Application can't connect to the database | Double-check the `Data Source`, `Initial Catalog`, and credentials in `appsettings.json`, and that the SQL Server instance is reachable. |
| NuGet restore errors | Ensure Visual Studio has internet access, or check `nuget.config` for the correct package source. |
| Build errors after restore | Make sure the .NET SDK version installed matches the project's target framework. |

---

## Repository

GitHub: [oviahsanhabib/Dream-Bank-2.0](https://github.com/oviahsanhabib/Dream-Bank-2.0)

---
