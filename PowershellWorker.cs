using System.Diagnostics;
using System.Management.Automation;
using System.Reflection;

namespace Refresher;

public static class PowershellWorker
{

    static string CreateUpdateScript(string folderPath, string assemblyName, int processId, string updateLink)
    {

        string script = @$"# We don't need progress bars to consume CPU
$ProgressPreference = 'SilentlyContinue'
# Stopping the current app
$appDll = '{assemblyName}.dll'
Stop-Process -Id {processId} -Force
$appFolder = '{folderPath}'
Set-Location -Path $appFolder
# Source file location
$source = '{updateLink}'
# Destination to save the file (folder of the script)
$updateName = Get-Date -Format 'up_dd_MM_yyyy_HH_mm'
$updateNameFile = $updateName + '_update.zip'
$updateZipPath = Join-Path -Path $appFolder -ChildPath $updateNameFile
# Download the update
Invoke-WebRequest -Uri $source -OutFile $updateZipPath
# Unpack to the subfolder
# $updateFolder = Join-Path {folderPath} -ChildPath $updateName
# New-Item -Path $updateFolder -ItemType Directory
# Classic Unzip (replace with your system)
Expand-Archive -Path $updateZipPath -DestinationPath $appFolder -Force
# Cleaning
Remove-Item -Path $updateZipPath
Start-Process -FilePath 'dotnet' -ArgumentList $appDll";


        return script;

    }


    /// <summary>
    /// Generate the powershell script for an update
    /// </summary>
    /// <param name="updateLink">Link to the zip</param>
    /// <returns>Filepath in the system</returns>
    static async Task<string> CreateUpdateFile(string updateLink)
    {
        // TODO...check update archieve

        var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
        // Current process for shutdown
        var procId = Process.GetCurrentProcess().Id;
        // Root directory of the App
        var directory = AppDomain.CurrentDomain.BaseDirectory;
        // Update script will be here
        var filePath = Path.Combine(directory, "update.ps1");

        await File.WriteAllTextAsync(filePath, CreateUpdateScript(directory, assemblyName, procId, updateLink));

        return filePath;
    }


    static async Task RunScript(string updateLink)
    {
        // create a new hosted PowerShell instance using the default runspace.
        // wrap in a using statement to ensure resources are cleaned up.

        using var ps = PowerShell.Create();
        var filePath = await CreateUpdateFile(updateLink);

        // specify the script code to run.
        ps.AddScript($"Start-Process pwsh {filePath} -NoNewWindow");

        //if (scriptParameters != null)
        //    // specify the parameters to pass into the script.
        //    ps.AddParameters(scriptParameters);

        // execute the script and await the result.
        var pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);

        // print the resulting pipeline objects to the console.
        foreach (var item in pipelineObjects)
        {
            Console.WriteLine(item.BaseObject.ToString());
        }

    }


    public static async Task UpdateAsync(string updateLink)
    {
        Console.WriteLine("Starting an update. The app will shutdown.");
        await RunScript(updateLink);
    }

}
