using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Eyup.Model
{
    public class NavigationParameter
    {
        public string Scheme { get; set; }
        public string ContactRemoteIds { get; set; }
        public AppContact AppContact { get; set; }
        public ShareTargetActivatedEventArgs ShareTargetActivatedEventArgs { get; set; }
    }
}
