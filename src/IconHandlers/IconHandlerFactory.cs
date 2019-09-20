using SimpleStatusIcons.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimpleStatusIcons {
    public class IconHandlerFactory {
        public List<IconHandler> handlers = new List<IconHandler>();
        public IconHandlerFactory(List<IconHandler> handlers) {
            this.handlers = handlers;
        }

        public IconHandler GetIconHandler(IconHandlerMode type) {
            return handlers.FirstOrDefault(z => z.Type == type);
        }
    }
}
