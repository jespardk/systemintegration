# systemintegrationcase

## Dashboard

### Screenshot

![plot](./Other/DashScreen1.png)

### How to run
- CD to /systemintegration/Dashboard/Case.Dashboard
- Open the /Properties/launchSettings.json, and add config values
- Run this command: `dotnet watch run`
- This should start a browser window with the blazor Dashboard application.

### Update local launchSettings.json

Add these entries:

`"PowerMeasurementsService.Username": "studerende",`

`"PowerMeasurementsService.Password": "CHANGEME",`

`"PowerMeasurementsService.Url": "ftp://inverter.westeurope.cloudapp.azure.com"`
