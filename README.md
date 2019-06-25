# TicketSJ
## The solution is created according to intership task.
With help of ASP.NET Core Web API Json generated Ticket with Enity sample and randomly filled and send to Azure ServiceBus.  
Windows Service "Receive Message", reads the Ticket from the Azure ServiceBus queue added to Azure Sql and writes logs.  
  
## Settings:  
  
### Project Data:  
 - Generate a unique ticket number: TicketJSWebAPI\TicketJSWebAPI\numbers.dat (via serialization)  
 - WebAPi data for generating: TicketJSWebAPI\TicketJSWebAPI\bin\Debug\netcoreapp2.0\info\  
 - Generated logs Windows Service: *path*\TicketJSWebAPI\TicketSJWindowsService\bin\Debug\Logs\  
  
### Adding and Removing Windows Services  
In CMD with administrator privileges:  
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319  
add: InstallUtil.exe *path*\TicketJSWebAPI\TicketSJWindowsService\bin\Debug\TicketSJWindowsService.exe
remove: InstallUtil.exe -u *path*\TicketJSWebAPI\TicketSJWindowsService\bin\Debug\TicketSJWindowsService.exe

### Adding Model Database in project .NET CORE
In Package Manager:
Scaffold-DbContext "*Your Connection String*" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models


