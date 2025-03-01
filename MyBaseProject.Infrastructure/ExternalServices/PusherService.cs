using Microsoft.Extensions.Options;
using PusherServer;
using MyBaseProject.Application.Interfaces.ExternalServices;
using MyBaseProject.Infrastructure.Extensions.Settings;

namespace MyBaseProject.Infrastructure.ExternalServices
{
    public class PusherService : IPusherService
    {
        private readonly Pusher _pusher;

        public PusherService(IOptions<PusherSettings> options)
        {
            var settings = options.Value ?? throw new ArgumentNullException(nameof(options));

            var pusherOptions = new PusherOptions
            {
                Cluster = settings.Cluster,
                Encrypted = true
            };

            _pusher = new Pusher(settings.AppId, settings.Key, settings.Secret, pusherOptions);
        }

        public async Task SendMessageAsync(string channel, string eventName, object data)
        {
            if (string.IsNullOrEmpty(channel) || string.IsNullOrEmpty(eventName))
                throw new ArgumentException("Channel and event cannot be null or empty.");

            await _pusher.TriggerAsync(channel, eventName, data);
        }
    }
}
