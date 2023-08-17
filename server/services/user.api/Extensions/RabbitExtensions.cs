using MassTransit;
using user.bll.Infrastructure.Consumers;

namespace user.api.Extensions
{
    /// <summary>
    /// Helper class for RabbitMQ
    /// </summary>
    public static class RabbitExtensions
    {
        /// <summary>
        /// Add services for mass transit
        /// </summary>
        /// <param name="services"></param>
        public static void AddRabbitExtensions(this IServiceCollection services)
        {
            services.AddMassTransit(options =>
            {
                options.AddConsumer<DeckBoughtConsumer>();
                options.AddConsumer<LobbyJoinedConsumer>();
                options.AddConsumer<GameJoinedConsumer>();
                options.SetKebabCaseEndpointNameFormatter();

                options.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                        h.ConfigureBatchPublish(c =>
                        {
                            c.Enabled = true;
                            c.Timeout = TimeSpan.FromMilliseconds(2);
                        });
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
