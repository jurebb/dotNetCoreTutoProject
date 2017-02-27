# dotNetCoreTutoProject
following a pluralsight (shawnwildermuth) tutorial
-when running on a different local machine:
  -change the connection string to the corresponding one in SQL Server Object Explorer (Visual15)
  -in case of IIS errors (unable to connect to IIS express) do the following:  
                -run VS in admin mode
                -erase the applicationhost file in root/.vs(hidden)
                -change the port number (project>properties)
                -re-run
  -run the following commands in cmd in the project root (TheWorld>): 
                                                                -dotnet ef migrations add InitialDatabase
                                                                -dotnet ef database update
