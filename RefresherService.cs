namespace Refresher;

public interface IRefresher
{
    Task UpdateAsync(string updateLink);
}

public class RefresherService : IRefresher
{
    /// <summary>
    /// Start an update (current proccess will shutdown)
    /// </summary>
    /// <param name="updateLink">Path to the Zip Archieve</param>
    public async Task UpdateAsync(string updateLink)
    {
        await PowershellWorker.UpdateAsync(updateLink);
    }
}