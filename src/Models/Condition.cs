namespace SimpleStatusIcons.Models {
    public class Condition {
        public IconValidationMode Test { get; set; }
        public string Uri { get; set; }
        public bool? ShouldRetry { get; internal set; }
    }
}
