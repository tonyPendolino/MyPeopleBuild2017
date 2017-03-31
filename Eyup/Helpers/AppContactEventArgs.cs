using Eyup.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyup.Helpers
{
    public class AppContactEventArgs : EventArgs
    {
        public AppContact AppContact { get; set; }

        public AppContactEventArgs() { }

        public AppContactEventArgs(AppContact appContact)
        {
            AppContact = appContact;
        }
    }
}
