using SimpleStatusIcons.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimpleStatusIcons {
    public class IconValidatorFactory {
        public List<IconValidator> validators = new List<IconValidator>();
        public IconValidatorFactory(List<IconValidator> validators) {
            this.validators = validators;
        }

        public IconValidator GetConditionResolver(IconValidationMode type) {
            return validators.FirstOrDefault(z => z.Type == type);
        }
    }
}
