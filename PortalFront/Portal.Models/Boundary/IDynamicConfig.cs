using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public interface IDynamicConfig
    {
        bool IsOnlineRegisterAvailable { get; }
        int UpdateIntervalSec { get; }
    }
}
