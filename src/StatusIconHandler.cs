using Microsoft.Extensions.Options;
using SimpleStatusIcons.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStatusIcons {

    public class StatusIconHandler {
        private readonly AppSettings RuntimeConfig;
        private readonly IconHandlerFactory IconFactory;
        public StatusIconHandler(IconHandlerFactory factory, IOptions<AppSettings> appSettings) {
            IconFactory = factory;
            RuntimeConfig = appSettings?.Value;
        }

        public void Run() {
            if (RuntimeConfig?.StatusIcon?.Modes?.Any() != true) {
                return;
            }

            foreach (var config in RuntimeConfig.StatusIcon.Modes) {
                var iconHandler = IconFactory.GetIconHandler(config.Event);
                Task.Run(() => iconHandler.Start(config));
            }
        }
    }
}
