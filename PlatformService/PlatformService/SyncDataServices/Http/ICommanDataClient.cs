using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommanDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto platform);
    }
}
