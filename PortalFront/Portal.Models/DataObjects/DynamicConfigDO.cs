using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models.DataObjects
{
    public class DynamicConfigDO : IDynamicConfig
    {
        public bool IsOnlineRegisterAvailable { get; set; }
        public int UpdateIntervalSec { get; set; }
    }
}
