using BrockAllen.MembershipReboot;
using ServerHost.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerHost.App_Start
{
    public class MembershipRebootConfig
    {
        public static MembershipRebootConfiguration<CustomUser> Create()
        {
            var settings = SecuritySettings.Instance;

            var config = new MembershipRebootConfiguration<CustomUser>(settings);

            return config;
        }
    }
}