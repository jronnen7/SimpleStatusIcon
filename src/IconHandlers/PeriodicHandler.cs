using Microsoft.Extensions.Options;
using SimpleStatusIcons.Models;
using System.Threading;

namespace SimpleStatusIcons {
    /// <summary>
    /// Resolve Icon conditions when Network Adapter Changes Occur
    /// </summary>
    public class PeriodicHandler : BaseIconHandler {
        public PeriodicHandler(IOptions<AppSettings> appSetting, IconValidatorFactory factory, IconDisplayContext displayContext) : base(appSetting?.Value?.StatusIcon?.Icons, factory, displayContext) {
        }

        public override IconHandlerMode Type { get => IconHandlerMode.Periodic; }
        private static Timer Timer;

        public override void Start(RunMode mode) {
            int.TryParse(mode.Frequency, out int msBeforeRepeat);
            Timer = new System.Threading.Timer(
                callback: new TimerCallback((timerState)=> { Handle(); }),
                state: null,
                dueTime: 0,
                period: msBeforeRepeat);
        }
    }
}
