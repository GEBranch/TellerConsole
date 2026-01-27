This program as the same functionality as the Teller Console but with the following additional features:
1. Changed the Database to SqlServer. I used (local) as it comes default with Visual Studio. To view
the tables, you will need to load SSMS (Sql Server Management Studio) and connect to (local).
2. Added AutoMapper to reduce the amount of code needed to map between DTOs and Domain Models.
3. Changed Serilog to log to a file instead of the console.
3. Added Dependency Injection for AutoMapper.
3. Added Dependency Injection for Deposit and Withdraw Services.

To see the current Teller Console application, please download it from Main and look
at in Visual Studio. There are files in Github that should not be there as they are no
longer part of this project. Please ignore them. I cant seem to get rid
of them.