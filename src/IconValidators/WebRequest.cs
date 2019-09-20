using Microsoft.Extensions.Options;
using SimpleStatusIcons.Models;
using System.Net;

namespace SimpleStatusIcons {

    public class WebRequest : IconValidator {
        public readonly IconConfig[] Icons;
        public WebRequest(IOptions<AppSettings> appSetting) {
            Icons = appSetting?.Value?.StatusIcon?.Icons;
        }
        public IconValidationMode Type { get => IconValidationMode.WebRequest; }

        public bool IsValid(Condition conditionConfig) {
            if (string.IsNullOrWhiteSpace(conditionConfig?.Uri)) {
                return false;
            }

            return HttpGet(conditionConfig.Uri);
        }

        private bool HttpGet(string nameOrAddress) {
            bool httpResponseOk = false;
            var request = (HttpWebRequest)HttpWebRequest.Create(nameOrAddress);

            var response = (HttpWebResponse)request.GetResponse();
            httpResponseOk = IsOkStatusCode((int)response.StatusCode);

            return httpResponseOk;
        }

        private bool IsOkStatusCode(int statusCode) {
            return statusCode >= 200 && statusCode < 300;
        }
    }
}
