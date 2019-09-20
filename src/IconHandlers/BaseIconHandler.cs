using SimpleStatusIcons.Models;
using System;
using System.Linq;
using System.Threading;

namespace SimpleStatusIcons {
    public abstract class BaseIconHandler : IconHandler {
        public abstract IconHandlerMode Type { get; }
        public abstract void Start(RunMode mode);

        protected readonly IconConfig[] Icons;
        protected readonly IconValidatorFactory IconConditionFactory;
        protected readonly IconDisplayContext IconDisplay;
        protected BaseIconHandler(IconConfig[] icons, IconValidatorFactory iconConditionFactory, IconDisplayContext iconDisplay) {
            Icons = icons;
            IconConditionFactory = iconConditionFactory;
            IconDisplay = iconDisplay;
        }

        private static object SyncRootIconConfig = new object();
        private static IconConfig IconConfig {
            get {
                return _IconConfig;
            }
            set {
                if (value == _IconConfig) {
                    return;
                }
                lock (SyncRootIconConfig) {
                    if (value != _IconConfig) {
                        _IconConfig = value;
                    }
                }
            }
        }
        private static IconConfig _IconConfig;

        private static DateTime LastRun = default;
        private const int debounceMs = 5000;
        private static object SyncRootLastRun = new object();
        protected void Handle() {
            var now = DateTime.UtcNow;
            var waitUntil = LastRun.AddMilliseconds(debounceMs);
            if (waitUntil >= now) {
                return;
            }
            lock (SyncRootLastRun) {
                waitUntil = LastRun.AddMilliseconds(debounceMs);
                if (waitUntil >= now) {
                    return;
                }
                else {
                    LastRun = now;
                }
            }

            // we want to run everything at the end of the debounce time
            Thread.Sleep(debounceMs);
            Run();
        }
        protected void Run() {
            if (Icons?.Any() != true) {
                return;
            }

            Action<IconConfig> setIcon = (IconConfig iconConfig) => {
                if (iconConfig == IconConfig) {
                    return;
                }
                IconConfig = iconConfig;
                IconDisplay.SetIcon(iconConfig);
            };

            foreach (var iconConfig in Icons) {
                // null condition we use as a fallback, only the last item in config should have a null condition
                if (iconConfig.Condition == null) {
                    setIcon(iconConfig);
                    break;
                }

                var conditionResolver = IconConditionFactory.GetConditionResolver(iconConfig.Condition.Test);
                var isValid = conditionResolver.IsValid(iconConfig.Condition);
                if(!isValid && iconConfig.Condition.ShouldRetry == true) {
                    isValid = conditionResolver.IsValid(iconConfig.Condition);
                }
                if (isValid) {
                    setIcon(iconConfig);
                    break;
                }
            }
        }
    }
}
