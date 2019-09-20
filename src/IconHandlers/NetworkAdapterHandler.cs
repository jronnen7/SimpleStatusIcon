using Microsoft.Extensions.Options;
using SimpleStatusIcons.Models;
using System.Net.NetworkInformation;

namespace SimpleStatusIcons {
    /// <summary>
    /// Resolve Icon conditions when Network Adapter Changes Occur
    /// </summary>
    public class NetworkAdapterIconHandler : BaseIconHandler {
        public NetworkAdapterIconHandler(IOptions<AppSettings> appSetting, IconValidatorFactory factory, IconDisplayContext displayContext) : base(appSetting?.Value?.StatusIcon?.Icons, factory, displayContext) {
        }

        public override IconHandlerMode Type { get => IconHandlerMode.NetworkAdapterChanged; }

        public override void Start(RunMode mode) {
            // call at startup for first time config
            Handle();

            // Add event handling
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler((sender, e) => Handle());
        }
    }
}
