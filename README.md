# TicketSJ
The solution is created according to intership task.\n
-With help of ASP.NET Core Web API Json generated Ticket with Enity sample and randomly filled and send to Azure ServiceBus.\n
-Windows Service reads the Ticket from the Azure ServiceBus queue  added to Azure Sql and writes logs.\n
-Settings:\n
\n
Project Data:\n
 - Generate a unique ticket number: numbers.dat (via serialization)\n
 - WebAPi data for generating: bin\Debug\netcoreapp2.0\info\\n
 - Generated logs Windows Service: \bin\Debug\Logs\\n

Adding and Removing Windows Services
In CMD with administrator privileges:
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319 
add: InstallUtil.exe *fullpath*.exe
remove: InstallUtil.exe -u *fullpath*.exe

Adding Model Database in project .NET CORE
In Package Manager:
Scaffold-DbContext "*Your Connection String*" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models


