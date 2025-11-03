using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WGU_App_RileyJuniewic.Data.Misc.Events
{
    public class GuidEventArgs : EventArgs
    {
        public Guid Value { get; set; }

        public GuidEventArgs(Guid value)
        {
            Value = value;
        }
    }
}