# Refresher
![refresher](https://static.wikia.nocookie.net/dotaunderlords_gamepedia_en/images/2/27/Refresher_orb_icon.png/revision/latest/scale-to-width-down/108?cb=20190708110333)

This service will generate an update script for your app and run it via PowerShell Core from the app itself.

# Quick Start
1. Add nuget to your project

`Install-Package Refresher`

2. Add service to the app

`builder.Services.AddSingleton<RefresherService>();`

3. Inject the service or invoke it directly

C# (service)

```
app.MapGet("/update", async () =>
{
    var provider = builder.Services.BuildServiceProvider();
    var updater = provider.GetService<RefresherService>();
    await updater.UpdateAsync("https://example.com/update1.zip");
    return Results.Ok();
});
```

F# (direct, minimal API)

```
app.MapGet("/update", Func<IResult>(fun () ->

       task {
            let updater = new RefresherService()
            updater.UpdateAsync("https://example.com/update1.zip")  |> ignore
       }  |> ignore

       Results.Ok()

    )) |> ignore
```

4. Invoke it via `http://yourapp.com/update`

## How it works
1. Your app will generate an update script in folder.
2. Your app will invoke PowerShell CLI.
3. PowerShell will shutdown your app, download and unpack the update, replace old files with new files.
4. The update script will start your app again.

## Requirements
PowerShell Core on target machine (> 7.2)
https://docs.microsoft.com/en-us/powershell/scripting/install/install-ubuntu?view=powershell-7.2

First start of your app via `nohup dotnet app.dll > /dev/null 2>&1 &`

Correct user rights for the main app and the folders.

## Why PowerShell instead of some custom update daemon
1. PowerShell is available on all platforms.
2. Your app generates and starts the update process.
3. No overhead for additional process.

## Future
- [ ] Fully functional background service with manifest files
- [ ] PowerShell checks for errors
- [ ] Function to create minimal updates

## Tested
- [x] Ubuntu 20.04.4 LTS
- [x] Windows 11 (10.0.22000.0)
