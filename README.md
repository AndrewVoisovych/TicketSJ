# TicketSJ
## The solution is created according to intership task.
Accordingly to model, generated ticket (JSON format) randomly filled up with data and sent to Azure ServiceBus using ASP.NET Core Web API. Windows Service "Receive Message", gets the Ticket from the Azure ServiceBus queue, adds to Azure Sql and writes logs. 

## Video work and presentation
http://bit.ly/30Ik5yW


## Settings:  
  
### Project Data:  
 - Generate a unique ticket number: SoftJourn.Ticket.WebAPI\numbers.dat (via serialization)  
 - WebAPi data for generating: SoftJourn.Ticket.WebAPI\bin\Debug\netcoreapp2.0\info\  
 - Generated logs Windows Service: *path*\SoftJourn.Ticket.WindowsService\TicketSJWindowsService\bin\Debug\Logs\  
  
### Adding and Removing Windows Services  
In CMD with administrator privileges:  
> cd C:\Windows\Microsoft.NET\Framework\v4.0.30319  
add: 
> InstallUtil.exe *path*\SoftJourn.Ticket.WindowsService\bin\Debug\WindowsService.exe
remove:  
> InstallUtil.exe -u *path*\TicketSJWSoftJourn.Ticket.WindowsServiceindowsService\bin\Debug\WindowsService.exe

### Adding Model Database in project .NET CORE
In Package Manager:
> Scaffold-DbContext "*Your Connection String*" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models


