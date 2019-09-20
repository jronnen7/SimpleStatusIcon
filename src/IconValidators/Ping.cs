using Microsoft.Extensions.Options;
using SimpleStatusIcons.Models;
using System.Net.NetworkInformation;

namespace SimpleStatusIcons {

    public class Ping : IconValidator {
        public readonly IconConfig[] Icons;
        public Ping(IOptions<AppSettings> appSetting) {
            Icons = appSetting?.Value?.StatusIcon?.Icons;
        }
        public IconValidationMode Type { get => IconValidationMode.Ping; }

        public bool IsValid(Condition conditionConfig) {
            if (string.IsNullOrWhiteSpace(conditionConfig?.Uri)) {
                return false;
            }

            return PingHost(conditionConfig.Uri);
        }

        public bool PingHost(string nameOrAddress) {
            bool pingable = false;
            System.Net.NetworkInformation.Ping pinger = null;

            try {
                pinger = new System.Net.NetworkInformation.Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException) {
                // we just want to discard and return false, handle excpetion silently
            }
            finally {
                pinger?.Dispose();
            }

            return pingable;
        }
    }
}
