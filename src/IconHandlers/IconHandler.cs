using SimpleStatusIcons.Models;

namespace SimpleStatusIcons {
    public interface IconHandler {
        IconHandlerMode Type { get; }
        void Start(RunMode mode);
    }
}
