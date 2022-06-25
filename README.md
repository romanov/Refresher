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
4. Ivoke it via `http://yourapp.com/update`

## Requirements
PowerShell Core on target machine.
https://docs.microsoft.com/en-us/powershell/scripting/install/install-ubuntu?view=powershell-7.2

## Tested
[+] Ubuntu 20.04.4 LTS
