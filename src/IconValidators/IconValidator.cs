using SimpleStatusIcons.Models;

namespace SimpleStatusIcons {
    public interface IconValidator {
        IconValidationMode Type { get; }
        bool IsValid(Condition conditionConfig);
    }
}
