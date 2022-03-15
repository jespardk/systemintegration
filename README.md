# systemintegrationcase

## Dashboard

### Screenshot

![plot](./Other/DashScreen1.png)

## How to run

1. Ensure .Net 6.0 SDK is installed.
1. Open a command line
1. CD to /systemintegration/Dashboard/Case.Dashboard
2. Setup configuration
   - Create a copy of the file `appsettings.Sample.json`, ensure the name is precisely = `appsettings.Secrets.json`.
   - Fill out the secrets in file.
5. Run this command: `dotnet watch run`. 
   - This should start a browser window with the blazor Dashboard application.


   <br>

   Note: Launching directly from within Visual Studio will not work.