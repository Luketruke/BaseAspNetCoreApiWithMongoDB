namespace MyBaseProject.Application.Interfaces.ExternalServices
{
    public interface IPusherService
    {
        Task SendMessageAsync(string channel, string eventName, object data);
    }
}
