using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Eyup.Services
{
    public class NavigationHelperService
    {
        private static NavigationHelperService current;

        public static NavigationHelperService Current => current ?? (current = new NavigationHelperService());

        public string GetProtocolScheme(IActivatedEventArgs parameter)
        {
            string scheme = string.Empty;
            if (parameter is ProtocolActivatedEventArgs)
            {
                Dictionary<string, string> activationKeyPairs = new Dictionary<string, string>();
                var args = parameter as ProtocolActivatedEventArgs;
                scheme = args?.Uri.Scheme;
            }

            //example: ms-ipmessaging, ms-contact-profile, ...
            return scheme;
        }

        public string GetContactRemoteIds(IActivatedEventArgs parameter)
        {
            if (parameter is ProtocolActivatedEventArgs)
            {
                Dictionary<string, string> activationKeyPairs = new Dictionary<string, string>();
                var args = parameter as ProtocolActivatedEventArgs;
                var scheme = args?.Uri.Scheme;

                var queryArgs = args?.Uri.Query.Replace("?", "").Split('&');
                foreach (var queryArg in queryArgs)
                {
                    var keyPair = queryArg.Split('=');
                    if (keyPair.Count() == 2)
                    {
                        activationKeyPairs.Add(keyPair[0], keyPair[1]);
                    }
                }

                if (activationKeyPairs.ContainsKey("ContactRemoteIds"))
                {
                    return activationKeyPairs["ContactRemoteIds"];
                }
            }

            return string.Empty;
        }
    }
}
