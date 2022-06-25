# Refresher
This service will generate an update script for your app and run it via PowerShell Core.

# Quick Start
1. Add nuget to your project

`Install-Package Refresher -Version 1.0.0`

2. Add service to the app

`builder.Services.AddSingleton<IRefresher>();`

3. Inject the service or get it directly
```
app.MapGet("/update", async () =>
{
    var provider = builder.Services.BuildServiceProvider();
    var updater = provider.GetService<IRefresher>();
    await updater.UpdateAsync("https://example.com/update1.zip");
    return Results.Ok();
});
```
4. Invoke it via `http://yourapp.com/update`

## How it works
1. Your app will generate an update script in folder.
2. Your app will invoke PowerShell CLI.
3. PowerShell will shutdown your app, download and unpack the update, replace old files with new files.
4. The update script will start your app again.

## Requirements
PowerShell Core on target machine.
https://docs.microsoft.com/en-us/powershell/scripting/install/install-ubuntu?view=powershell-7.2

Firs start of your app via `nohup dotnet app.dll > /dev/null 2>&1 &`

## Future
- [ ] Fully functional background service
- [ ] PowerShell checks for errors
- [ ] Function to create minimal updates

## Tested
[+] Ubuntu 20.04.4 LTS
